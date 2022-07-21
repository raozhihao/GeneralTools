using System;
using System.Windows;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 图像尺寸数据
    /// </summary>
    public class ImageCutRectEventArgs : EventArgs
    {
        /// <summary>
        /// 截取到的图像像素大小
        /// </summary>
        public Int32Rect CutPixelRect { get; set; }

        /// <summary>
        /// 是否向下传递
        /// </summary>
        public bool HandleToNext { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ImageCutRectEventArgs(Int32Rect rect, bool next = true)
        {
            this.CutPixelRect = rect;
            this.HandleToNext = next;
        }
    }
}
