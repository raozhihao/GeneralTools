using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes;

namespace SimpleDiagram.Shapes
{
    public class HeartShape : BaseShape
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        public HeartShape(Rect rect)
        {
            this.rect = rect;
        }

        private Rect rect;
        /// <summary>
        /// 矩形大小
        /// </summary>
        public Rect Rect
        {
            get => rect;
            set
            {
                UpdateShape(ParseToPoints(value).ToList());
                rect = value;
            }
        }

        private ObservableCollection<Point> ParseToPoints(Rect value)
        {
            PixelPoints.Clear();
            PixelPoints.Add(value.Location);
            PixelPoints.Add(value.TopRight);
            PixelPoints.Add(value.BottomRight);
            PixelPoints.Add(value.BottomLeft);
            return PixelPoints;
        }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            _ = ParseToPoints(rect);
            Rect canvasRect = ParseToCanvasRect(rect);
            //创建一个心形
            List<Point> points = CreateHeart(canvasRect);
            DrawHeart(points);
        }

        private static List<Point> CreateHeart(Rect rect)
        {
            //获取横半径
            double r = rect.Width / 2;//rect.Width;
            _ = Math.Pow(Math.Sqrt(r), 2.5);
            int a = 1;//此值影响顶上的两个半球，值越大，半球越高
            double l = -r;

            List<Point> leftTop = new List<Point>();
            List<Point> leftBottom = new List<Point>();

            double x;

            double rScale = r;
            // 左上半圆
            for (x = rScale; x >= 0; x--)
            {
                double xtmp = x;
                Point curr = GetHeartPoint(xtmp, r, a, l, rect.TopLeft, true);

                curr = new Point(curr.X, curr.Y);
                leftTop.Add(curr);
            }

            //左下部分
            for (x = 0; x <= rScale; x++)
            {
                double xtmp = x;
                Point curr = GetHeartPoint(xtmp, r, a, l, rect.TopLeft, false);
                curr = new Point(curr.X, curr.Y);
                leftBottom.Add(curr);
            }

            if (leftBottom.Last().X != leftTop.First().X)
            {
                leftBottom.Add(GetHeartPoint(rScale, r, a, l, rect.TopLeft, false));
            }
            List<Point> points = new List<Point>();

            points.AddRange(leftTop);
            points.AddRange(leftBottom);

            //中间x
            double mx = leftTop[0].X;
            //左半部分倒序
            for (int i = points.Count - 1; i >= 0; i--)
            {
                Point p = points[i];
                points.Add(new Point(mx - p.X + mx, p.Y));
            }

            Console.WriteLine(points.Count);

            return points;
        }

        private void DrawHeart(List<Point> points)
        {
            Point first = points[0];
            StringBuilder buidler = new StringBuilder($"M{first}");

            for (int i = 1; i < points.Count; i++)
            {
                _ = buidler.Append($"L{points[i]}");
            }

            Geometry geo = Geometry.Parse(buidler.ToString());
            Path.Data = geo;
        }

        private static Point GetHeartPoint(double x, double r, double a, double l, Point topLeft, bool isY1 = true)
        {
            double hh = (a * 2) + 0.5;//此值影响心形下的尖尖，值越小，尖尖就越长
            double tmmTop = Math.Pow(x + l, 2);
            double y = isY1
                ? -(a * Math.Sqrt((r * Math.Sqrt(tmmTop)) - tmmTop))
                : -(-a * (r / hh) * Math.Sqrt(Math.Sqrt(r) - Math.Sqrt(Math.Abs(x + l))));
            x += topLeft.X;
            y += topLeft.Y;
            return new Point(x, y);

        }

        private Rect ParseToCanvasRect(Rect rect)
        {
            Point lt = rect.TopLeft;
            Point rb = rect.BottomRight;
            lt = ImageView.TranslateToCanvasPoint(lt);
            rb = ImageView.TranslateToCanvasPoint(rb);
            return new Rect(lt, rb);
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            //更新图像
            //创建一个心形
            Rect rect = new Rect(canvasPoints[0], canvasPoints[2]);

            //创建一个心形
            List<Point> points = CreateHeart(rect);
            DrawHeart(points);
        }

    }
}
