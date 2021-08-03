using GeneralTool.General.Enums;
using System;
using System.IO;
using System.Windows.Media.Imaging;

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
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion Public 方法
    }
}