using System;
using System.Windows;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 图像尺寸数据
    /// </summary>
    public class ImageCutRectEventArgs : EventArgs
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        public ImageCutRectEventArgs(Int32Rect rect, bool next = true)
        {
            this.CutPixelRect = rect;
            this.HandleToNext = next;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 截取到的图像像素大小
        /// </summary>
        public Int32Rect CutPixelRect { get; set; }

        /// <summary>
        /// 是否向下传递
        /// </summary>
        public bool HandleToNext { get; set; }

        #endregion Public 属性
    }
}