using System;
using System.Collections.Generic;
using System.Linq;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.Maths
{
    /// <summary>
    /// 生成各种图形坐标集合
    /// </summary>
    public static class Shapes
    {
        /// <summary>
        /// 生成正弦曲线
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<System.Windows.Point> Sinusoidal(System.Windows.Point startPoint, System.Windows.Point endPoint, int num)
        {
            //先将起点与终点旋转到原图坐标点 

            System.Windows.Rect rect = new System.Windows.Rect(startPoint, endPoint);

            double width = rect.Width;
            double height = rect.Height;
            double top = rect.Top;
            double left = rect.Left;

            List<System.Windows.Point> points = new List<System.Windows.Point>();
            //绘制
            for (double x = 0; x < width; x += 3)
            {
                double y = (height / 2) * (Math.Cos(2 * Math.PI * num / width * x) + 1) + top;
                double x1 = x + left;
                double y1 = y;
                points.Add(new System.Windows.Point(x1, y1));
            }

            return points;
        }

        /// <summary>
        /// 生成等腰三角形
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static List<System.Windows.Point> GetThreeDeg(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            System.Windows.Point p1 = new System.Windows.Point((endPoint.X - startPoint.X) / 2f + startPoint.X, startPoint.Y);
            System.Windows.Point p2 = new System.Windows.Point(startPoint.X, endPoint.Y);
            System.Windows.Point p3 = new System.Windows.Point(endPoint.X, endPoint.Y);
            return new List<System.Windows.Point>() { p1, p2, p3, p1 };
        }

        /// <summary>
        /// 获取五角星
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static List<System.Windows.Point> GetPentagram(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {

            Tuple<System.Windows.Point, System.Windows.Point, double> p = GetCircileCenterPoint(startPoint, endPoint);

            double cx = p.Item1.X;
            double cy = p.Item1.Y;
            double cr = p.Item3;
            System.Windows.Point p1 = new System.Windows.Point(cx, cy - cr);
            System.Windows.Point p2 = new System.Windows.Point(cx - cr * Math.Sin(0.2 * Math.PI), cy + cr * Math.Cos(Math.PI * 0.2));
            System.Windows.Point p3 = new System.Windows.Point(cx + cr * Math.Sin(0.4 * Math.PI), cy - cr * Math.Cos(Math.PI * 0.4));
            System.Windows.Point p4 = new System.Windows.Point(cx - cr * Math.Sin(0.4 * Math.PI), cy - cr * Math.Cos(Math.PI * 0.4));
            System.Windows.Point p5 = new System.Windows.Point(cx + cr * Math.Sin(0.2 * Math.PI), cy + cr * Math.Cos(Math.PI * 0.2));
            return new List<System.Windows.Point>()
            {
                p1,p2,p3,p4,p5,p1
            };
        }

        /// <summary>
        /// 获取S形
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static List<System.Windows.Point> GetSArcPoints(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            System.Windows.Rect rectLarge = new System.Windows.Rect(startPoint, endPoint);
            //s形要分两半
            System.Windows.Size size = new System.Windows.Size(rectLarge.Width, rectLarge.Height / 2);
            System.Windows.Rect topRect = new System.Windows.Rect(rectLarge.TopLeft, size);
            System.Windows.Rect bottomRect = new System.Windows.Rect(new System.Windows.Point(rectLarge.Left, rectLarge.Top + rectLarge.Height / 2), size);

            System.Windows.Point center = new System.Windows.Point(topRect.Left + topRect.Width / 2, topRect.Top + topRect.Height / 2);
            //获取数据
            double yDis = topRect.Height / 2.0;
            double xDis = topRect.Width / 2.0;
            List<System.Windows.Point> points = CutCirclePoints(xDis, yDis, center, 90, 360, 90);

            points.RemoveAt(points.Count - 1);
            //S形,下方
            center = new System.Windows.Point(bottomRect.Left + bottomRect.Width / 2, bottomRect.Top + bottomRect.Height / 2);
            yDis = bottomRect.Height / 2.0;
            xDis = bottomRect.Width / 2.0;
            List<System.Windows.Point> c2 = CutCirclePoints(xDis, yDis, center, 180, -90, 90);
            points.AddRange(c2);

            return points;
        }

        /// <summary>
        /// 桃心爱心
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static List<System.Windows.Point> HeartArcPoints(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            #region 再次更新

            System.Windows.Rect rect = new System.Windows.Rect(startPoint, endPoint);
            //获取横半径
            int d = (int)rect.Width;
            int r = d / 2;//rect.Width;
            int a = 1;//此值影响顶上的两个半球，值越大，半球越高
            int l = -r;

            List<System.Windows.Point> leftTop = new List<System.Windows.Point>();
            List<System.Windows.Point> leftBottom = new List<System.Windows.Point>();

            double x;

            //起点绘制坐标
            System.Windows.Point startDrawPoint = new System.Windows.Point(rect.Left, rect.Top + rect.Height / 2);

            System.Windows.Point prevTopPoint = default(System.Windows.Point);
            //两点之间最小像素,是在最顶端点与旁边点之间的距离
            double skip = 0.1;

            double skipDis = 1.0;
            System.Windows.Point top1Point = GetHeartPoint(r + skipDis, d, a, -d, startDrawPoint);

            double skipMin = top1Point.Distance(top1Point);//两点之间间隔的最小像素

            //两点之间最大像素,是起始点与第2点之间的距离
            System.Windows.Point sp = GetHeartPoint(0, d, a, -d, startDrawPoint);
            System.Windows.Point sp1 = GetHeartPoint(skipDis, d, a, -d, startDrawPoint);

            double skipMax = sp.Distance(sp1);
            //Console.WriteLine("===========min : {0},max : {1}", skipMin, skipMax);

            System.Windows.Point prevBottomPoint = default(System.Windows.Point);
            for (x = 0; x <= d; x += skip)
            {
                double xtmp = x;
                System.Windows.Point currTop = GetHeartPoint(xtmp, r, a, l, startDrawPoint, true);

                currTop = new System.Windows.Point(currTop.X, currTop.Y);

                if (prevTopPoint.X == 0 && prevTopPoint.Y == 0)
                {
                    leftTop.Add(currTop);
                    prevTopPoint = currTop;
                }
                else
                {
                    double dis = prevTopPoint.Distance(currTop);
                    //Console.WriteLine(dis);
                    //修正两点之间的距离,在符合距离之中的点位进行写入
                    if (dis > skipMin && dis < skipMax)
                    {
                        leftTop.Add(currTop);

                        prevTopPoint = currTop;
                    }
                    else
                    {
                        // Console.WriteLine("Skip index - [{0}] , distance : [{1}]", x, dis);
                    }
                }

                System.Windows.Point currBottom = GetHeartPoint(xtmp, r, a, l, startDrawPoint, false);
                if (prevBottomPoint.X == 0 && prevBottomPoint.Y == 0)
                {
                    leftBottom.Add(currBottom);

                    prevBottomPoint = currBottom;
                }
                else
                {
                    double dis = prevBottomPoint.Distance(currBottom);
                    //修正两点之间的距离,在符合距离之中的点位进行写入
                    if (dis > skipMin && dis < skipMax)
                    {
                        leftBottom.Add(currBottom);

                        prevBottomPoint = currBottom;
                    }
                    else
                    {
                        // Console.WriteLine("Skip index - [{0}] , distance : [{1}]", x, dis);
                    }
                }
            }

            //将上部最后一个点加上
            System.Windows.Point topLast = GetHeartPoint(d, r, a, l, startDrawPoint, true);
            System.Windows.Point topLast2 = leftTop.Last();
            double distance = topLast.Distance(topLast2);
            if (topLast != topLast2 && distance < skipMax && distance > skipMin)
            {
                leftTop.Add(topLast);
            }
            //相同的点去掉,因为刚好上半部和下半部分是相反的,所以要查看两个连接节点是否一致,下面还得反转一下下方的点

            if (leftBottom.Last() == leftTop.Last())
            {
                leftTop.RemoveAt(0);
            }

            leftBottom.Reverse();
            List<System.Windows.Point> points = new List<System.Windows.Point>();

            points.AddRange(leftTop);
            points.AddRange(leftBottom);

            //Console.WriteLine(points.Count);

            return points;

            #endregion
        }

        private static System.Windows.Point GetHeartPoint(double x, double r, double a, double l, System.Windows.Point topLeft, bool isY1 = true)
        {
            double hh = (a * 2 + 0.5);//此值影响心形下的尖尖，值越小，尖尖就越长
            double tmmTop = Math.Pow(x + l, 2);
            double y = isY1
                ? -(a * Math.Sqrt(r * Math.Sqrt(tmmTop) - tmmTop))
                : -(-a * (r / hh) * Math.Sqrt(Math.Sqrt(r) - Math.Sqrt(Math.Abs(x + l))));
            x += topLeft.X;
            y += topLeft.Y;
            return new System.Windows.Point(x, y);

        }

        /// <summary>
        /// 获取三次贝赛尔曲线
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static List<System.Windows.Point> ThreeArcPoints(System.Windows.Point startPoint, System.Windows.Point endPoint, System.Windows.Point s1, System.Windows.Point s2)
        {
            List<System.Windows.Point> list = new List<System.Windows.Point>();
            for (double i = 0; i <= 1; i += 0.03)
            {
                System.Windows.Point p = ThreeBezier(i, startPoint, endPoint, s1, s2);
                list.Add(p);
            }
            return list;
        }

        private static System.Windows.Point ThreeBezier(double t, System.Windows.Point sp, System.Windows.Point ep, System.Windows.Point cp1, System.Windows.Point cp2)
        {
            double x1 = sp.X;
            double y1 = sp.Y;
            double x2 = ep.X;
            double y2 = ep.Y;
            double cx1 = cp1.X;
            double cy1 = cp1.Y;
            double cx2 = cp2.X;
            double cy2 = cp2.Y;

            double x =
                x1 * (1 - t) * (1 - t) * (1 - t) +
                3 * cx1 * t * (1 - t) * (1 - t) +
                3 * cx2 * t * t * (1 - t) +
                x2 * t * t * t;

            double y =
                y1 * (1 - t) * (1 - t) * (1 - t) +
                3 * cy1 * t * (1 - t) * (1 - t) +
                3 * cy2 * t * t * (1 - t) +
                y2 * t * t * t;

            return new System.Windows.Point(x, y);
        }

        /// <summary>
        /// 生成圆弧
        /// </summary>
        /// <param name="xDistance">x轴向上的半长</param>
        /// <param name="yDistance">y轴向上的半长</param>
        /// <param name="centerPoint">中心点坐标</param>
        /// <param name="startDeg">开始绘制的角度,从下中开始为0度,以逆时针旋转,如果此值比endDeg小,则为顺时针旋转</param>
        /// <param name="endDeg">结束绘制的角度</param>
        /// <param name="pointCount">总共生成的点数</param>
        public static List<System.Windows.Point> CutCirclePoints(double xDistance, double yDistance, System.Windows.Point centerPoint, double startDeg = 0, double endDeg = 360, int pointCount = 90)
        {
            //绘制路径是从下中到右中
            double radians = Math.PI / 180; //弧度

            double sd = startDeg;
            double ed = endDeg;
            double skip = Math.Abs(sd - ed) / pointCount;
            double ox = centerPoint.X;
            double oy = centerPoint.Y;

            //反向绘制,从大到小去绘
            bool reveser = sd > ed;

            List<System.Windows.Point> points = new List<System.Windows.Point>();
            //填充xy坐标
            if (reveser)
            {
                double tmp = sd;
                sd = ed;
                ed = tmp;

                for (double i = ed; i >= sd; i -= skip)
                {
                    //此处为xy的绝对坐标
                    double x = ox + xDistance * Math.Sin(radians * i);
                    double y = oy + yDistance * Math.Cos(radians * i);
                    ////此处为xy的相对坐标
                    points.Add(new System.Windows.Point(x, y));
                }
            }
            else
            {
                for (double i = sd; i <= ed; i += skip)
                {
                    double x = ox + xDistance * Math.Sin(radians * i);
                    double y = oy + yDistance * Math.Cos(radians * i);

                    points.Add(new System.Windows.Point(x, y));
                }
            }

            //查看有无闭合
            if (points.Last() != points[0] && ed - sd >= 360)
            {
                points.Add(points[0]);
            }

            return points;
        }

        /// <summary>
        /// 根据圆外接矩形两点求取
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns>圆心点,终点,半径长度</returns>
        public static Tuple<System.Windows.Point, System.Windows.Point, double> GetCircileCenterPoint(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            //根据宽度来生成圆对应的坐标
            System.Windows.Rect rect = new System.Windows.Rect(startPoint, endPoint);

            //半径长度,直径/2
            double r = rect.Width / 2;
            //圆心坐标
            double centerPointX = rect.Location.X + r;
            //找出圆心的Y坐标
            double centerPointY = rect.Location.Y + r;
            //返回圆心坐标，圆上的一个点
            return new Tuple<System.Windows.Point, System.Windows.Point, double>(new System.Windows.Point(centerPointX, centerPointY), new System.Windows.Point(centerPointX, centerPointY - r), r);
        }

    }
}
