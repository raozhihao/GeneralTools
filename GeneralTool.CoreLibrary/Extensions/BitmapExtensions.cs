using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using GeneralTool.CoreLibrary.Win32;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ConvertImageToBase64(this Image file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.Save(memoryStream, file.RawFormat);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Image ConvertBase64ToImage(this string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                return Image.FromStream(ms, true);
            }
        }

        /// <summary>
        /// 将句柄转为24位图
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Bitmap To24BitmapByIntPtr(this IntPtr handle, Size size)
        {
            var len = size.Width * 3 * size.Height;
            var ptr = Marshal.AllocHGlobal(len);
            Win32Helper.CopyMemory(ptr, handle, len);
            return new Bitmap(size.Width, size.Height, size.Width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, ptr);
        }

        /// <summary>
        /// 将句柄转为24位图
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="width"></param>
        /// <param name="height">height</param>
        /// <returns></returns>
        public static Bitmap To24BitmapByIntPtr(this IntPtr handle, int width, int height)
            => handle.To24BitmapByIntPtr(new Size(width, height));
    }
}
