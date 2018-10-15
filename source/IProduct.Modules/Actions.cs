using EntityWorker.Core.Helper;
using IProduct.Modules.Library.Custom;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            using(Stream stream = assembly.GetManifestResourceStream(resourceName))
            using(StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }


        /// <summary>
        /// Load the ApplicationCredentials. 
        /// </summary>
        /// <param name="signInApplication"></param>
        /// <returns></returns>
        public static ApplicationCredentials LoadCredentials(SignInApplication signInApplication)
        {
            var path = Path.Combine(ConfigurationManager.AppSettings["Credentials"], signInApplication.ToString());
            if(File.Exists(path))
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
            if(valid && trueValidation)
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
                using(var ms = new MemoryStream(file))
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
                using(var ms = new MemoryStream(image))
                    srcBmp = new Bitmap(ms);
                float ratio = 1;
                float minSize = Math.Min(newHeight, newHeight);

                if(srcBmp.Width > srcBmp.Height)
                {
                    ratio = minSize / (float)srcBmp.Width;
                }
                else
                {
                    ratio = minSize / (float)srcBmp.Height;
                }

                SizeF newSize = new SizeF(srcBmp.Width * ratio, srcBmp.Height * ratio);
                Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);

                using(Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);

                    using(MemoryStream memoryStream = new MemoryStream())
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
