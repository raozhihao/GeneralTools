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
        public HeartShape( Rect rect) 
        {
            this.rect = rect;
        }



        private Rect rect;
        /// <summary>
        /// 矩形大小
        /// </summary>
        public Rect Rect
        {
            get => this.rect;
            set
            {
                this.UpdateShape(this.ParseToPoints(value).ToList());
                this.rect = value;
            }
        }

        private ObservableCollection<Point> ParseToPoints(Rect value)
        {
            this.PixelPoints.Clear();
            this.PixelPoints.Add(value.Location);
            this.PixelPoints.Add(value.TopRight);
            this.PixelPoints.Add(value.BottomRight);
            this.PixelPoints.Add(value.BottomLeft);
            return this.PixelPoints;
        }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            this.ParseToPoints(this.rect);
            var canvasRect = this.ParseToCanvasRect(this.rect);
            //创建一个心形
            var points = CreateHeart(canvasRect);
            this.DrawHeart(points);
        }

        private static List<Point> CreateHeart(Rect rect)
        {
            //获取横半径
            var r = rect.Width / 2;//rect.Width;
            var h = Math.Pow(Math.Sqrt(r), 2.5);
            var a = 1;//此值影响顶上的两个半球，值越大，半球越高
            var l = -r;

            var leftTop = new List<Point>();
            var leftBottom = new List<Point>();

            double x;

            var rScale = r;
            // 左上半圆
            for (x = rScale; x >= 0; x--)
            {
                var xtmp = x;
                var curr = GetHeartPoint(xtmp, r, a, l, rect.TopLeft, true);

                curr = new Point(curr.X, curr.Y);
                leftTop.Add(curr);
            }

            //左下部分
            for (x = 0; x <= rScale; x++)
            {
                var xtmp = x;
                var curr = GetHeartPoint(xtmp, r, a, l, rect.TopLeft, false);
                curr = new Point(curr.X, curr.Y);
                leftBottom.Add(curr);
            }

            if (leftBottom.Last().X != leftTop.First().X)
            {
                leftBottom.Add(GetHeartPoint(rScale, r, a, l, rect.TopLeft, false));
            }
            var points = new List<Point>();

            points.AddRange(leftTop);
            points.AddRange(leftBottom);

            //中间x
            var mx = leftTop[0].X;
            //左半部分倒序
            for (int i = points.Count - 1; i >= 0; i--)
            {
                var p = points[i];
                points.Add(new Point(mx - p.X + mx, p.Y));
            }

            Console.WriteLine(points.Count);

            return points;
        }

        private void DrawHeart(List<Point> points)
        {
            var first = points[0];
            var buidler = new StringBuilder($"M{first}");

            for (int i = 1; i < points.Count; i++)
            {
                buidler.Append($"L{points[i]}");
            }

            var geo = Geometry.Parse(buidler.ToString());
            this.Path.Data = geo;
        }

        private static Point GetHeartPoint(double x, double r, double a, double l, Point topLeft, bool isY1 = true)
        {
            var hh = (a * 2 + 0.5);//此值影响心形下的尖尖，值越小，尖尖就越长
            var tmmTop = Math.Pow(x + l, 2);
            double y;
            if (isY1)
                y = -(a * Math.Sqrt(r * Math.Sqrt(tmmTop) - tmmTop));
            else
                y = -(-a * (r / hh) * Math.Sqrt(Math.Sqrt(r) - Math.Sqrt(Math.Abs(x + l))));

            x += topLeft.X;
            y += topLeft.Y;
            return new Point(x, y);

        }

        private Rect ParseToCanvasRect(Rect rect)
        {
            var lt = rect.TopLeft;
            var rb = rect.BottomRight;
            lt = this.ImageView.TranslateToCanvasPoint(lt);
            rb = this.ImageView.TranslateToCanvasPoint(rb);
            return new Rect(lt, rb);
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            //更新图像
            //创建一个心形
            var rect = new Rect(canvasPoints[0], canvasPoints[2]);

            //创建一个心形
            var points = CreateHeart(rect);
            this.DrawHeart(points);
        }

    }
}
