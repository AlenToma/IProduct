using IProduct.Modules.Library;
using IProduct.Modules.Library.Custom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IProduct.Modules
{
    public static class Actions
    {
        /// <summary>
        /// Resize and save an image to fit under width and height like a canvas keeping things proportional
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="thumbImagePath"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public static byte[] GenerateThumbImage(byte[] image, int newWidth, int newHeight)
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
    }
}
