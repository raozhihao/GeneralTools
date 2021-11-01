using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 图像事件参数
    /// </summary>
    public class ImageEventArgs : EventArgs
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="sucess">
        /// </param>
        /// <param name="msg">
        /// </param>
        public ImageEventArgs(BitmapSource source, bool sucess, string msg)
        {
            this.Source = source;
            this.Sucess = sucess;
            this.ErroMsg = msg;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErroMsg { get; set; }

        /// <summary>
        /// 图像
        /// </summary>
        public BitmapSource Source { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Sucess { get; set; }

        /// <summary>
        /// 位置信息
        /// </summary>
        public Int32Rect Int32Rect { get; set; }

        #endregion Public 属性
    }
}