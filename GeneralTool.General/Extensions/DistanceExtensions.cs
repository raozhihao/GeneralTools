using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralTool.General.Extensions
{
    /// <summary>
    /// 距离扩展
    /// </summary>
    public static class DistanceExtensions
    {
        /// <summary>
        /// 求两个点之间的距离
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double Distance(double x1, double y1, double x2, double y2)
        => Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));

        /// <summary>
        /// 求两个点之间的距离
        /// </summary>
        public static double Distance(this System.Drawing.Point point1, System.Drawing.Point point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);

        /// <summary>
        /// 求两个点之间的距离
        /// </summary>
        public static double Distance(this System.Drawing.Point point1, System.Drawing.PointF point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);


        /// <summary>
        /// 求两个点之间的距离
        /// </summary>
        public static double Distance(this System.Drawing.Point point1, System.Windows.Point point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);


        /// <summary>
        /// 求两个点之间的距离
        /// </summary>
        public static double Distance(this System.Drawing.PointF point1, System.Drawing.PointF point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);


        /// <summary>
        /// 求两个点之间的距离
        /// </summary>
        public static double Distance(this System.Drawing.PointF point1, System.Drawing.Point point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);

        /// <summary>
        /// 求两个点之间的距离
        /// </summary>

        public static double Distance(this System.Drawing.PointF point1, System.Windows.Point point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);

        /// <summary>
        /// 求两个点之间的距离
        /// </summary>

        public static double Distance(this System.Windows.Point point1, System.Windows.Point point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);

        /// <summary>
        /// 求两个点之间的距离
        /// </summary>
        public static double Distance(this System.Windows.Point point1, System.Drawing.PointF point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);

        /// <summary>
        /// 求两个点之间的距离
        /// </summary>
        public static double Distance(this System.Windows.Point point1, System.Drawing.Point point2)
            => Distance(point1.X, point1.Y, point2.X, point2.Y);


        /// <summary>
        /// 求一组点之间的距离
        /// </summary>
        public static double Distance(this IEnumerable<System.Drawing.Point> points)
        {
            var sum = 0d;
            var index = 0;
            var first = default(System.Drawing.Point);
            foreach (var point in points)
            {
                if (index == 0)
                {
                    first = point;
                    index++;
                    continue;
                }

                sum += first.Distance(point);
                first = point;
            }
            return sum;
        }

        /// <summary>
        /// 求一组点之间的距离
        /// </summary>
        public static double Distance(this IEnumerable<System.Drawing.PointF> points)
        {
            var sum = 0d;
            var index = 0;
            var first = default(System.Drawing.PointF);
            foreach (var point in points)
            {
                if (index == 0)
                {
                    first = point;
                    index++;
                    continue;
                }

                sum += first.Distance(point);
                first = point;
            }
            return sum;
        }

        /// <summary>
        /// 求一组点之间的距离
        /// </summary>
        public static double Distance(this IEnumerable<System.Windows.Point> points)
        {
            var sum = 0d;
            var index = 0;
            var first = default(System.Windows.Point);
            foreach (var point in points)
            {
                if (index == 0)
                {
                    first = point;
                    index++;
                    continue;
                }

                sum += first.Distance(point);
                first = point;
            }
            return sum;
        }

        /// <summary>
        /// 获取最大的距离
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double MaxDistance(this IList<System.Windows.Point> points)
        {
            if (points == null || points.Count <= 1) return 0;
            var prev = points.First();
            var disMax = 0d;
            for (int i = 1; i < points.Count; i++)
            {
                var curr = points[i];
                var disTmp = curr.Distance(prev);
                if (disTmp > disMax)
                {
                    disMax = disTmp;
                }
                prev = curr;
            }
            return disMax;
        }

        /// <summary>
        /// 获取最小的距离
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double MinDistance(this IList<System.Windows.Point> points)
        {
            if (points == null || points.Count <= 1) return 0;
            var prev = points.First();
            var disMin = 99990d;
            for (int i = 1; i < points.Count; i++)
            {
                var curr = points[i];
                var disTmp = curr.Distance(prev);
                if (disTmp < disMin)
                {
                    disMin = disTmp;
                }
                prev = curr;
            }
            return disMin;
        }

    }
}
