using EntityWorker.Core.Attributes;
using EntityWorker.Core.FastDeepCloner;
using EntityWorker.Core.Helper;
using IProduct.Modules.Attributes;
using IProduct.Modules.Library.Custom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IProduct.Modules
{
    public static class Actions
    {
        /// <summary>
        /// Return embedded file from IProduct.Modules.SQL
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetSql(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"IProduct.Modules.SQL.{fileName}";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        /// <summary>
        /// return Json Action Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="JsonFormatting"></param>
        /// <returns></returns>
        public static CallbackJsonResult ViewResult<T>(this T o)
        {
            return new CallbackJsonResult(o);
        }

        /// <summary>
        /// Validate the object and return error if it fail validation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<string> ValidateRequireField<T>(T item)
        {
            var type = item is Type ? item.GetType() : typeof(T);
            var validatePropertiesOnly = item is Type;
            var errorList = new List<string>();
            foreach (var prop in DeepCloner.GetFastDeepClonerProperties(type).Where(x =>
             (!x.ContainAttribute<JsonDocument>() && !x.ContainAttribute<XmlDocument>() && !x.ContainAttribute<ExcludeFromAbstract>() && x.CanReadWrite) && (x.ContainAttribute<Required>() || !x.IsInternalType)))
            {
                string e = null;
                var required = prop.GetCustomAttribute<Required>();
                var stringLength = prop.GetCustomAttribute<StringLength>();
                var reqExp = prop.GetCustomAttribute<Regex>();
                var value = validatePropertiesOnly ? DeepCloner.CreateInstance(prop.PropertyType) : prop.GetValue(item);
                if (prop.IsInternalType)
                {
                    if ((e = required.Validate(value, prop)) != null)
                        errorList.Add(e);
                    else if (stringLength != null && (e = stringLength.Validate(value, prop)) != null)
                        errorList.Add(e);
                    else if (reqExp != null && (e = reqExp.Validate(value, prop)) != null)
                        errorList.Add(e);
                }
                else
                {
                    if (prop.PropertyType.GetActualType() != prop.PropertyType) // Its an IList then
                    {
                        var list = value as IList;
                        if (list?.Count > 0 || validatePropertiesOnly)
                        {
                            if (list?.Count <= 0)
                                errorList.AddRange(ValidateRequireField(prop.PropertyType.GetActualType()));
                            else
                                foreach (var tItem in list)
                                    errorList.AddRange(ValidateRequireField(tItem));
                        }
                        else
                        {
                            if (value != null || validatePropertiesOnly)
                                errorList.AddRange(ValidateRequireField(value ?? prop.PropertyType));
                        }
                    }
                }
            }

            return errorList;
        }

        /// <summary>
        /// Load the ApplicationCredentials. 
        /// </summary>
        /// <param name="signInApplication"></param>
        /// <returns></returns>
        public static ApplicationCredentials LoadCredentials(SignInApplication signInApplication)
        {
            var path = Path.Combine(ConfigurationManager.AppSettings["Credentials"], signInApplication.ToString());
            if (File.Exists(path))
                return File.ReadAllText(path).FromJson<ApplicationCredentials>();
            return null;
        }

        enum ImageFileType
        {
            Undefined, // Unknown, None, etc. whatever you like
            Jpeg,
            Jpg = Jpeg,
            Png,
            MemoryBmp,
            Bmp,
            Emf,
            Wmf,
            Gif,
            Tiff,
            Exif,
            Icon,
            TiF

        }

        /// <summary>
        /// Validate if path is a supported Image
        /// </summary>
        /// <param name="file"></param>
        /// <trueValidation>Image.FromFile will be included  </trueValidation>
        /// <returns></returns>
        public static bool IsImage(string file, bool trueValidation = false)
        {

            var formates = Enum.GetNames(typeof(ImageFileType));
            var valid = formates.Any(x => file.ToLower().Contains(x.ToLower()));
            if (valid && trueValidation)
            {
                try
                {
                    Image.FromFile(file);
                }
                catch
                {
                    valid = !valid;
                }
            }
            return valid;

        }

        /// <summary>
        /// Validate if path is a supported Image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsImage(byte[] file)
        {
            try
            {
                using (var ms = new MemoryStream(file))
                    new Bitmap(ms);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Resize and save an image to fit under width and height like a canvas keeping things proportional
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="thumbImagePath"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public static byte[] GenerateThumbImage(byte[] image, int newWidth, int newHeight)
        {
            try
            {
                Bitmap srcBmp;
                using (var ms = new MemoryStream(image))
                    srcBmp = new Bitmap(ms);
                float ratio = 1;
                float minSize = Math.Min(newHeight, newHeight);

                if (srcBmp.Width > srcBmp.Height)
                {
                    ratio = minSize / (float)srcBmp.Width;
                }
                else
                {
                    ratio = minSize / (float)srcBmp.Height;
                }

                SizeF newSize = new SizeF(srcBmp.Width * ratio, srcBmp.Height * ratio);
                Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);

                using (Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        target.Save(memoryStream, ImageFormat.Jpeg);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch
            {
                return image;
            }
        }
    }
}
