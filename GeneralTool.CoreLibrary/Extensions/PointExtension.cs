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
            foreach (System.Windows.Point item in points)
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
            foreach (System.Drawing.PointF item in points)
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
            foreach (System.Windows.Point item in points)
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
            foreach (System.Drawing.Point item in points)
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
            foreach (System.Drawing.Point item in points)
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
            foreach (System.Drawing.PointF item in points)
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
            double k = (y1 - y0 == 0 ? 1 : y1 - y0) / (x1 - x0 == 0 ? 1 : x1 - x0);
            double x = (y2 - y1 + k * x1 + x2 / k) / (k + 1.0 / k);
            double y = y1 + k * (x - x1);
            return double.IsNaN(x) || double.IsNaN(y) ? new System.Windows.Point(x2, y2) : new System.Windows.Point(x, y);
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
            double angle = Math.Atan2(dy, dx);
            double degrees = angle * 180.0 / Math.PI;

            deg = degrees + deg;

            double d = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));

            double theta_rad = Math.PI / 180.0 * deg;
            //# 计算待求点的坐标
            double x4 = x1 + d * Math.Cos(theta_rad);
            double y4 = y1 + d * Math.Sin(theta_rad);
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
            System.Windows.Point point2 = new System.Windows.Point(x2, y2);     // 箭头终点
            double angleOri = Math.Atan((y2 - y1) / (x2 - x1));      // 起始点线段夹角
            double angleDown = angleOri - arrowAngle;   // 箭头扩张角度
            double angleUp = angleOri + arrowAngle;     // 箭头扩张角度
            int directionFlag = (x2 > x1) ? -1 : 1;     // 方向标识
            double x3 = x2 + ((directionFlag * arrowLength) * Math.Cos(angleDown));   // 箭头第三个点的坐标
            double y3 = y2 + ((directionFlag * arrowLength) * Math.Sin(angleDown));

            double x4 = x2 + ((directionFlag * arrowLength) * Math.Cos(angleUp));     // 箭头第四个点的坐标
            double y4 = y2 + ((directionFlag * arrowLength) * Math.Sin(angleUp));
            System.Windows.Point point3 = new System.Windows.Point(x3, y3);   // 箭头第三个点
            System.Windows.Point point4 = new System.Windows.Point(x4, y4);   // 箭头第四个点
            System.Windows.Point[] points = new System.Windows.Point[] { point2, point3, point4, point2 };   // 多边形，起点 --> 终点 --> 第三点 --> 第四点 --> 终点
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

        /// <summary>
        /// 将坐标进行旋转(顺时针方向,如果是正负90度,则宽高应当是正好相反)
        /// </summary>
        /// <param name="rotateDeg">要旋转的角度(顺时针方向)</param>
        /// <param name="currentWidth">当前宽度</param>
        /// <param name="currentHeight">当前高度</param>
        /// <param name="rotatedWidth">旋转后宽度</param>
        /// <param name="rotatedHeight">旋转后高度</param>
        /// <returns></returns>
        public static Tuple<double, double> RotatePoint(double posX, double posY, int rotateDeg, int currentWidth, int currentHeight, int rotatedWidth, int rotatedHeight)
        {
            if (currentWidth == 0 || currentHeight == 0)
            {
                //不能为零旋转
                currentWidth = rotatedHeight;
                currentHeight = rotatedWidth;
            }
            //旋转坐标
            double x0 = posX - currentWidth / 2;
            double y0 = currentHeight / 2 - posY;

            double cosNum = Math.Round(Math.Cos(rotateDeg * 1.0 / 180 * Math.PI), 5);

            double sinNum = Math.Round(Math.Sin(rotateDeg * 1.0 / 180 * Math.PI), 5);

            double x1 = x0 * cosNum + y0 * sinNum;
            double y1 = -x0 * sinNum + y0 * cosNum;

            double x = rotatedWidth / 2 + x1;
            double y = rotatedHeight / 2 - y1;
            return new Tuple<double, double>(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rotateDeg"></param>
        /// <param name="currentWidth"></param>
        /// <param name="currentHeight"></param>
        /// <param name="rotatedWidth"></param>
        /// <param name="rotatedHeight"></param>
        /// <returns></returns>
        public static System.Windows.Point RotatePoint(this System.Windows.Point point, int rotateDeg, int currentWidth, int currentHeight, int rotatedWidth, int rotatedHeight)
        {
            Tuple<double, double> result = RotatePoint(point.X, point.Y, rotateDeg, currentWidth, currentHeight, rotatedWidth, rotatedHeight);
            return new System.Windows.Point(result.Item1, result.Item2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rotateDeg"></param>
        /// <param name="currSize"></param>
        /// <param name="rotateSize"></param>
        /// <returns></returns>
        public static System.Windows.Point RotatePoint(this System.Windows.Point point, int rotateDeg, System.Windows.Size currSize, System.Windows.Size rotateSize)
        => point.RotatePoint(rotateDeg, currSize.Width.ToInt32(), currSize.Height.ToInt32(), rotateSize.Width.ToInt32(), rotateSize.Height.ToInt32());
    }
}
