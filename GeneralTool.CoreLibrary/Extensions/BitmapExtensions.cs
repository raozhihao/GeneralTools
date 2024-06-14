using System;
using System.Drawing;
using System.Drawing.Imaging;
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

        /// <summary>
        /// 将相机数据复制到句柄中
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="handle"></param>
        public static void CopyBitmapToIntPtr(this Bitmap bitmap, IntPtr handle)
        {
            using (var data = new BitmapDataEx(bitmap))
            {
                data.FillToPtr(handle);
            }
        }
    }

    /// <summary>
    /// 位图数据扩展类
    /// </summary>
    public class BitmapDataEx : IDisposable
    {
        private bool disposedValue;

        private Bitmap bitmap;
        private bool autoDisposeBitmap;
        private BitmapData data;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="map">图像对象</param>
        /// <param name="autoDisposeBitmap">是否在 <see cref="Dispose()"/> 时自动释放图片对象</param>
        public BitmapDataEx(Bitmap map, bool autoDisposeBitmap = false)
        {
            this.bitmap = map;
            this.autoDisposeBitmap = autoDisposeBitmap;
        }

        /// <summary>
        /// 获取位图数据对象
        /// </summary>
        public BitmapData Data
        {
            get
            {
                if (this.data == null)
                    this.data = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
                return this.data;
            }
        }

        /// <summary>
        /// 获取位图指针对象
        /// </summary>
        public IntPtr Ptr => this.Data.Scan0;

        /// <summary>
        /// 获取位图步幅
        /// </summary>
        public int Stride => this.Data.Stride;

        /// <summary>
        /// 获取位图的字节流
        /// </summary>
        public byte[] Buffer
        {
            get
            {
                var bytes = new byte[this.Data.Stride * this.bitmap.Height];
                Marshal.Copy(this.Ptr, bytes, 0, bytes.Length);
                return bytes;
            }
        }

        /// <summary>
        /// 图像大小
        /// </summary>
        public int Size => this.Data.Height * this.Data.Stride;


        /// <summary>
        /// 往图片中设置字节数组
        /// </summary>
        /// <param name="buffer"></param>
        public void FillByBuffer(byte[] buffer)
        {
            Marshal.Copy(buffer, 0, this.Ptr, buffer.Length);
        }

        /// <summary>
        /// 将句柄中的图片信息填充到图片中
        /// </summary>
        /// <param name="dataPointer"></param>
        /// <param name="size"></param>
        public void FillByPtr(IntPtr dataPointer, int size = -1)
        {
            if (size < 1) size = this.Size;
            Win32Helper.CopyMemory(this.Ptr, dataPointer, size);
        }

        /// <summary>
        /// 将图片中的信息填充到句柄中
        /// </summary>
        /// <param name="toPointer"></param>
        public void FillToPtr(IntPtr toPointer)
        {
            Win32Helper.CopyMemory(toPointer, this.Ptr, this.Size);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    if (this.data != null)
                    {
                        this.bitmap.UnlockBits(this.data);
                    }

                    if (this.autoDisposeBitmap)
                        this.bitmap?.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~BitmapDataEx()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
