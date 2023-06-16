using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.WPFHelper.Extensions
{
    /// <summary>
    /// Bitmap扩展类
    /// </summary>
    public static class BitmapExtensions
    {
        #region Public 方法

        /// <summary>
        /// 将 System.Drawing.Bitmap 位图文件转为 BitmapImage
        /// </summary>
        /// <param name="srcImg">
        /// 原始位图文件
        /// </param>
        /// <param name="scale">
        /// 缩放比例
        /// </param>
        /// <returns>
        /// </returns>
        public static BitmapImage ToBitmapImage(this System.Drawing.Bitmap srcImg, int scale = 1)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            srcImg.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.DecodePixelWidth = srcImg.Width / (scale < 1 ? 1 : scale);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;

            image.EndInit();
            image.Freeze();
            srcImg.Dispose();
            stream.Dispose();
            return image;
        }

        /// <summary>
        /// 将Bitmap图片写入到WriteableBitmap中
        /// </summary>
        /// <param name="srcBitmap">
        /// </param>
        /// <param name="writeable">
        /// </param>
        /// <param name="dispatcher">
        /// </param>
        /// <param name="size">
        /// </param>
        /// <param name="pixelFormat">
        /// </param>
        public static void WriteToWriteableBitmap(this System.Drawing.Bitmap srcBitmap, WriteableBitmap writeable, System.Windows.Threading.Dispatcher dispatcher = null, int size = 4, PixelFormat pixelFormat = PixelFormat.Format32bppArgb)
        {
            if (srcBitmap == null || writeable == null)
            {
                return;
            }
            int picWidth = srcBitmap.Width;
            int picHeight = srcBitmap.Height;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, picWidth, picHeight);

            byte[] buffer = new byte[picWidth * picHeight * size];
            try
            {
                //锁bp到内存开始进行复制
                BitmapData bmpData = srcBitmap.LockBits(rect, ImageLockMode.ReadWrite, pixelFormat);
                IntPtr ptrBmp = bmpData.Scan0;

                //若要改变图片格式，请自行计算，以8为一个单位，8位乘以1,16位乘以2,32位乘以3 ·····依次类推
                //int picSize = picWidth * picHeight * 3;
                //pRrgaByte = new byte[picSize];
                //从内存指针中复制元素到数组
                Marshal.Copy(ptrBmp, buffer, 0, buffer.Length);

                //解锁释放内存
                srcBitmap.UnlockBits(bmpData);

                if (dispatcher == null)
                    WriteTo(writeable, buffer, picWidth, picHeight);
                else
                {
                    dispatcher.Invoke(new Action(() =>
                    {
                        WriteTo(writeable, buffer, picWidth, picHeight);
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"图片复制到内存失败:{ex.GetInnerExceptionMessage()}");
            }
            finally
            {
                srcBitmap.Dispose();
                if (buffer != null)
                {
                    Array.Clear(buffer, 0, buffer.Length);
                    buffer = null;
                }
            }
        }

        #endregion Public 方法

        #region Private 方法

        private static void WriteTo(WriteableBitmap writeable, byte[] buffer, int picWidth, int picHeight)
        {
            //锁住内存
            try
            {
                writeable.Lock();

                Marshal.Copy(buffer, 0, writeable.BackBuffer, buffer.Length);

                //指定更改位图的区域
                writeable.AddDirtyRect(new System.Windows.Int32Rect(0, 0, picWidth, picHeight));
                writeable.Unlock();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.GetInnerExceptionMessage());
            }
        }

        #endregion Private 方法
    }
}