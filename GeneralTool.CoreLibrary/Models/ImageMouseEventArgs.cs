using System.Windows;
using System.Windows.Input;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 鼠标数据
    /// </summary>
    public class ImageMouseEventArgs : MouseEventArgs
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="mouse">
        /// 鼠标设备
        /// </param>
        /// <param name="timestamp">
        /// </param>
        /// <param name="stylusDevice">
        /// </param>
        /// <param name="point">
        /// </param>
        /// <param name="canvasPoint"></param>
        public ImageMouseEventArgs(MouseDevice mouse, int timestamp, StylusDevice stylusDevice, Point point, Point canvasPoint) : base(mouse, timestamp, stylusDevice)
        {
            this.CurrentPixelPoint = point;
            this.CanvasPoint = canvasPoint;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 当前像素坐标点
        /// </summary>
        public Point CurrentPixelPoint { get; set; }

        /// <summary>
        /// 在画布上的坐标点
        /// </summary>
        public Point CanvasPoint { get; set; }

        #endregion Public 属性
    }
}