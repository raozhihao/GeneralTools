using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using GeneralTool.CoreLibrary.Enums;

namespace GeneralTool.CoreLibrary.WPFHelper
{
    /// <summary>
    /// </summary>
    public static class BitmapSouceExtension
    {
        #region Public 方法

        /// <summary>
        /// 保存图像到本地
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="path">
        /// </param>
        /// <param name="encoderEnum">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool SaveBitmapSouce(this BitmapSource source, string path, BitmapEncoderEnum encoderEnum = BitmapEncoderEnum.Jpeg)
        {
            BitmapEncoder encoder = null;
            switch (encoderEnum)
            {
                case BitmapEncoderEnum.Jpeg:
                    encoder = new JpegBitmapEncoder();
                    break;

                case BitmapEncoderEnum.Png:
                    encoder = new PngBitmapEncoder();
                    break;

                case BitmapEncoderEnum.Bmp:
                    encoder = new BmpBitmapEncoder();
                    break;

                case BitmapEncoderEnum.Gif:
                    encoder = new GifBitmapEncoder();
                    break;

                case BitmapEncoderEnum.Tiff:
                    encoder = new TiffBitmapEncoder();
                    break;

                case BitmapEncoderEnum.Wmp:
                    encoder = new WmpBitmapEncoder();
                    break;
            }
            encoder.Frames.Add(BitmapFrame.Create(source));
            FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            encoder.Save(file);
            file.Close();
            return true;
        }

        /// <summary>
        /// 将指定范围中的图像保存到本地
        /// </summary>
        /// <param name="source">要截取的原图</param>
        /// <param name="rect">范围</param>
        /// <param name="path">保存到的本地路径</param>
        /// <param name="encoderEnum">图像解码</param>
        /// <returns></returns>
        public static bool SaveBitmapSouce(this ImageSource source, Int32Rect rect, string path, BitmapEncoderEnum encoderEnum = BitmapEncoderEnum.Jpeg)
        {
            var cutSource = source.GetChooseRectImageSouce(rect);

            return cutSource.SaveBitmapSouce(path, encoderEnum);
        }
        /// <summary>
        /// 将指定范围中的图像保存到本地
        /// </summary>
        /// <param name="source">要截取的原图</param>
        /// <param name="rect">范围</param>
        /// <param name="path">保存到的本地路径</param>
        /// <param name="encoderEnum">图像解码</param>
        /// <returns></returns>
        public static bool SaveBitmapSouce(this BitmapFrame source, Int32Rect rect, string path, BitmapEncoderEnum encoderEnum = BitmapEncoderEnum.Jpeg)
        {
            var cutSource = source.GetChooseRectImageSouce(rect);
            return cutSource.SaveBitmapSouce(path, encoderEnum);
        }

        /// <summary>
        /// 将指定范围中的图像保存到本地
        /// </summary>
        /// <param name="source">要截取的原图</param>
        /// <param name="rect">范围</param>
        /// <param name="path">保存到的本地路径</param>
        /// <param name="encoderEnum">图像解码</param>
        /// <returns></returns>
        public static bool SaveBitmapSouce(this BitmapImage source, Int32Rect rect, string path, BitmapEncoderEnum encoderEnum = BitmapEncoderEnum.Jpeg)
        {
            var cutSource = source.GetChooseRectImageSouce(rect);
            return cutSource.SaveBitmapSouce(path, encoderEnum);
        }

        /// <summary>
        /// 获取当前截取到的范围内的图片
        /// </summary>
        /// <param name="source">要截取的原图</param>
        /// <param name="rect">范围</param>
        /// <returns></returns>
        public static BitmapSource GetChooseRectImageSouce(this ImageSource source, Int32Rect rect)
        {
            if (source is BitmapSource b)
            {
                var stride = b.Format.BitsPerPixel * rect.Width / 8;
                var data = new byte[rect.Height * stride];
                b.CopyPixels(rect, data, stride, 0);
                var newSource = BitmapSource.Create(rect.Width, rect.Height, b.DpiX, b.DpiY, b.Format, b.Palette, data, stride);
                return newSource;
            }
            return null;
        }

        /// <summary>
        /// 获取当前截取到的范围内的图片
        /// </summary>
        /// <param name="source">要截取的原图</param>
        /// <param name="rect">范围</param>
        /// <returns></returns>
        public static BitmapSource GetChooseRectImageSouce(this BitmapFrame source, Int32Rect rect)
        {
            var stride = source.Format.BitsPerPixel * rect.Width / 8;
            var data = new byte[rect.Height * stride];
            source.CopyPixels(rect, data, stride, 0);
            var newSource = BitmapSource.Create(rect.Width, rect.Height, source.DpiX, source.DpiY, source.Format, source.Palette, data, stride);
            return newSource;
        }

        /// <summary>
        /// 获取当前截取到的范围内的图片
        /// </summary>
        /// <param name="source">要截取的原图</param>
        /// <param name="rect">范围</param>
        /// <returns></returns>
        public static BitmapSource GetChooseRectImageSouce(this BitmapImage source, Int32Rect rect)
        {
            var stride = source.Format.BitsPerPixel * rect.Width / 8;
            var data = new byte[rect.Height * stride];
            source.CopyPixels(rect, data, stride, 0);
            var newSource = BitmapSource.Create(rect.Width, rect.Height, source.DpiX, source.DpiY, source.Format, source.Palette, data, stride);
            return newSource;
        }

        /// <summary>
        /// 将Bitmap写入到writable中
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="writeable"></param>
        /// <param name="reload">是否重新加载</param>
        public static void WriteBitmap(this System.Drawing.Bitmap bitmap, ref WriteableBitmap writeable, bool reload)
        {

            BitmapPalette palette = null;
            System.Windows.Media.PixelFormat format = PixelFormats.Bgr24;
            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                if (writeable == null || writeable.Format != PixelFormats.Indexed8)
                {
                    var colors = new Color[256];
                    for (byte i = 0; i < byte.MaxValue; i++)
                    {
                        colors[i] = Color.FromRgb(i, i, i);
                    }
                    palette = new BitmapPalette(colors);
                    reload = true;
                }
                format = PixelFormats.Indexed8;
            }
            else
            {
                switch (bitmap.PixelFormat)
                {

                    case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
                        format = PixelFormats.Bgr32;
                        break;

                }
            }


            if (reload || writeable == null || writeable.Format != format)
                writeable = new WriteableBitmap(bitmap.Width, bitmap.Height, 96, 96, format, palette);

            var data = bitmap.LockBits(new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), bitmap.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
            try
            {
                writeable.WritePixels(new Int32Rect(0, 0, data.Width, data.Height), data.Scan0, data.Height * data.Stride, data.Stride, 0, 0);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        /// <summary>
        /// 将Bitmap写入到writable中
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="writeable"></param>
        public static void WriteBitmap(this string imagePath, ref WriteableBitmap writeable)
        {
            using (var map = new System.Drawing.Bitmap(imagePath))
            {
                WriteBitmap(map, ref writeable, true);
            }
        }

        #endregion Public 方法
    }
}