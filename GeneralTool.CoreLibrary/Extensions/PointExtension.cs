using System;
using System.Collections.Generic;

namespace GeneralTool.CoreLibrary.Extensions
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

        /// <summary>
        /// 以两点做线,求线外一个点与该线段的一个垂直点坐标
        /// </summary>
        /// <param name="x0">起点x</param>
        /// <param name="y0">起点y</param>
        /// <param name="x1">终点x</param>
        /// <param name="y1">终点y</param>
        /// <param name="x2">线段外x</param>
        /// <param name="y2">线段外y</param>
        /// <returns></returns>
        public static System.Windows.Point VerticalLinePoint(double x0, double y0, double x1, double y1, double x2, double y2)
        {
            var k = (y1 - y0 == 0 ? 1 : y1 - y0) / (x1 - x0 == 0 ? 1 : x1 - x0);
            var x = (y2 - y1 + k * x1 + x2 / k) / (k + 1.0 / k);
            var y = y1 + k * (x - x1);
            if (double.IsNaN(x) || double.IsNaN(y)) return new System.Windows.Point(x2, y2);
            return new System.Windows.Point(x, y);
        }

        /// <summary>
        /// 以两点做线,求线外一个点与该线段的一个垂直点坐标
        /// </summary>
        public static System.Windows.Point VerticalLinePoint(this System.Windows.Point p1, System.Windows.Point p2, System.Windows.Point lineOutPoint)
        => VerticalLinePoint(p1.X, p1.Y, p2.X, p2.Y, lineOutPoint.X, lineOutPoint.Y);

        /// <summary>
        /// 以当前线段作夹角,求另一条线段的终眯坐标
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static System.Windows.Point LineDirectionPoint(double x1, double y1, double x2, double y2, double deg)
        {

            double dx = x2 - x1;
            double dy = y2 - y1;
            var angle = Math.Atan2(dy, dx);
            var degrees = angle * 180.0 / Math.PI;

            deg = degrees + deg;

            var d = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));


            var theta_rad = Math.PI / 180.0 * deg;
            //# 计算待求点的坐标
            var x4 = x1 + d * Math.Cos(theta_rad);
            var y4 = y1 + d * Math.Sin(theta_rad);
            return new System.Windows.Point(x4, y4);
        }

        /// <summary>
        /// 获取箭头
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="arrowAngle"></param>
        /// <param name="arrowLength"></param>
        /// <returns></returns>
        public static System.Windows.Point[] GetArrow(System.Windows.Point startPoint, System.Windows.Point endPoint, double arrowAngle = Math.PI / 12, double arrowLength = 20)
        {
            double x1 = startPoint.X;
            double y1 = startPoint.Y;
            double x2 = endPoint.X;
            double y2 = endPoint.Y;
            var point2 = new System.Windows.Point(x2, y2);     // 箭头终点
            double angleOri = Math.Atan((y2 - y1) / (x2 - x1));      // 起始点线段夹角
            double angleDown = angleOri - arrowAngle;   // 箭头扩张角度
            double angleUp = angleOri + arrowAngle;     // 箭头扩张角度
            int directionFlag = (x2 > x1) ? -1 : 1;     // 方向标识
            double x3 = x2 + ((directionFlag * arrowLength) * Math.Cos(angleDown));   // 箭头第三个点的坐标
            double y3 = y2 + ((directionFlag * arrowLength) * Math.Sin(angleDown));

            double x4 = x2 + ((directionFlag * arrowLength) * Math.Cos(angleUp));     // 箭头第四个点的坐标
            double y4 = y2 + ((directionFlag * arrowLength) * Math.Sin(angleUp));
            var point3 = new System.Windows.Point(x3, y3);   // 箭头第三个点
            var point4 = new System.Windows.Point(x4, y4);   // 箭头第四个点
            var points = new System.Windows.Point[] { point2, point3, point4, point2 };   // 多边形，起点 --> 终点 --> 第三点 --> 第四点 --> 终点
            return points;
        }



        /// <summary>
        /// 获取中心点
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static System.Windows.Point GetCenterPoint(this System.Windows.Rect rect)
        {
            return new System.Windows.Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
        }
    }
}
