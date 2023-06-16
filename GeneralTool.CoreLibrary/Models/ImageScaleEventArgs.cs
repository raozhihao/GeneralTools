using System;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 在图像缩放时的数据
    /// </summary>
    public class ImageScaleEventArgs : EventArgs
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="scale">
        /// </param>
        public ImageScaleEventArgs(double scale)
        {
            ScaleValue = scale;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 当前缩放比例
        /// </summary>
        public double ScaleValue { get; set; }

        #endregion Public 属性
    }
}