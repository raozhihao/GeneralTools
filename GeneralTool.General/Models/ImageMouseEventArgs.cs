using System.Windows;
using System.Windows.Input;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 鼠标数据
    /// </summary>
    public class ImageMouseEventArgs : MouseEventArgs
    {
        /// <summary>
        /// 当前像素坐标点
        /// </summary>
        public Point CurrentPixelPoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouse">鼠标设备</param>
        /// <param name="timestamp"></param>
        /// <param name="stylusDevice"></param>
        /// <param name="point"></param>
        public ImageMouseEventArgs(MouseDevice mouse, int timestamp, StylusDevice stylusDevice, Point point) : base(mouse, timestamp, stylusDevice)
        {
            this.CurrentPixelPoint = point;
        }
    }
}
