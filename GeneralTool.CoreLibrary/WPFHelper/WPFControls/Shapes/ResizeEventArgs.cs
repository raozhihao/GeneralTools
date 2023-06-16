using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes
{
    /// <summary>
    /// 更新大小时的附加属性
    /// </summary>
    public class ResizeEventArgs : EventArgs
    {
        /// <summary>
        /// 外包矩形位置大小
        /// </summary>
        public Rect PixelBounds
        {
            get
            {
                if (PixelPoints == null || PixelPoints.Count <= 1)
                    return default;

                Point first = PixelPoints[0];
                StringBuilder builder = new StringBuilder($"M{first}");
                foreach (Point item in PixelPoints)
                {
                    _ = builder.Append($"L{item}");
                }

                return Geometry.Parse(builder.ToString()).Bounds;
            }
        }

        /// <summary>
        /// 更新后的像素坐标集合
        /// </summary>
        public List<Point> PixelPoints { get; set; }

        /// <summary>
        /// 指示是否已经处理,为true时不再处理后续动作
        /// </summary>
        public bool Handled { get; set; }
    }
}
