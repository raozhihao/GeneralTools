using System.Windows;

namespace GeneralTool.General.WPFHelper
{
    /// <summary>
    /// 
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// 转换成Int类型
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point ToIntPoint(this Point point)
        {
            return new Point((int)point.X, (int)point.Y);
        }
    }
}
