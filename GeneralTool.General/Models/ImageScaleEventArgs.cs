using System;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 在图像缩放时的数据
    /// </summary>
    public class ImageScaleEventArgs : EventArgs
    {
        /// <summary>
        /// 当前缩放比例
        /// </summary>
        public double ScaleValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        public ImageScaleEventArgs(double scale)
        {
            this.ScaleValue = scale;
        }
    }
}
