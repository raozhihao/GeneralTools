using System.Collections.Generic;

namespace GeneralTool.General.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class PointExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Drawing.Point ToDrawPoint(this System.Windows.Point point)
            => new System.Drawing.Point((int)point.X, (int)point.Y);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Drawing.Point ToDrawPoint(this System.Drawing.PointF point)
            => new System.Drawing.Point((int)point.X, (int)point.Y);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<System.Drawing.Point> ToDrawPoints(this IEnumerable<System.Windows.Point> points)
        {
            foreach (var item in points)
            {
                yield return item.ToDrawPoint();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<System.Drawing.Point> ToDrawPoints(this IEnumerable<System.Drawing.PointF> points)
        {
            foreach (var item in points)
            {
                yield return item.ToDrawPoint();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Drawing.PointF ToDrawPointF(this System.Windows.Point point)
            => new System.Drawing.PointF((float)point.X, ((float)point.Y));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Drawing.PointF ToDrawPointF(this System.Drawing.Point point)
            => new System.Drawing.PointF(point.X, point.Y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<System.Drawing.PointF> ToDrawPointFs(this IEnumerable<System.Windows.Point> points)
        {
            foreach (var item in points)
            {
                yield return item.ToDrawPointF();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<System.Drawing.PointF> ToDrawPointFs(this IEnumerable<System.Drawing.Point> points)
        {
            foreach (var item in points)
            {
                yield return item.ToDrawPointF();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Windows.Point ToWindowPoint(this System.Drawing.Point point)
            => new System.Windows.Point(point.X, point.Y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static System.Windows.Point ToWindowPoint(this System.Drawing.PointF point)
            => new System.Windows.Point(point.X, point.Y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<System.Windows.Point> ToWindowPoints(this IEnumerable<System.Drawing.Point> points)
        {
            foreach (var item in points)
            {
                yield return item.ToWindowPoint();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<System.Windows.Point> ToWindowPoints(this IEnumerable<System.Drawing.PointF> points)
        {
            foreach (var item in points)
            {
                yield return item.ToWindowPoint();
            }
        }
    }
}
