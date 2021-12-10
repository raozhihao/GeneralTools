using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using GeneralTool.General.Enums;
using GeneralTool.General.ExceptionHelper;

namespace GeneralTool.General.WPFHelper
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
            try
            {
                encoder.Frames.Add(BitmapFrame.Create(source));
                FileStream file = new FileStream(path, FileMode.Create);
                encoder.Save(file);
                file.Close();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.GetInnerExceptionMessage());
                return false;
            }
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
            BitmapSource cutSource = null;
            if (source is BitmapImage img)
                cutSource = img.GetChooseRectImageSouce(rect);
            else if (source is BitmapFrame frame)
                cutSource = frame.GetChooseRectImageSouce(rect);

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


        #endregion Public 方法
    }
}