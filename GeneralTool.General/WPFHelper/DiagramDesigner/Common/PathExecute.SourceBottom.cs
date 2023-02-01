using System.Collections.Generic;
using System.Windows;

using GeneralTool.General.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Common
{
    partial class PathExecute
    {
        private void CreateSourceBottomPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            switch (sinkInfo.Direction)
            {
                case Direction.Top:
                    this.CreateBottomTopPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Bottom:
                    this.CreateBottomBottomPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Right:
                    this.CreateBottomRightPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Left:
                    this.CreateBottomLeftPoints(sourceInfo, sinkInfo, points);
                    break;
            }
        }

        private void CreateBottomBottomPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //源在上,目标在下
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    var x = sinkInfo.Point.X + sinkInfo.Size.Width / 2 + 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    var x = sinkInfo.Point.X - sinkInfo.Size.Width / 2 - 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
            else
            {
                //源在下,目标在上
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    var x = sinkInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
        }

        private void CreateBottomRightPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //源在上,目标在下
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                }
            }
            else
            {
                //源在下,目标在上
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    var x = sinkInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
        }

        private void CreateBottomLeftPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //源在上,目标在下
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
                }
            }
            else
            {
                //源在下,目标在上
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    var x = sourceInfo.Point.X + sourceInfo.Size.Width / 2 + 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
        }

        private void CreateBottomTopPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            //源在上,目标在下
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //目标在源的下方
                points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
            }
            else
            {
                //目标在上
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在源左方,左上方
                    var x = sinkInfo.Point.X - sinkInfo.Size.Width / 2 - 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在源右方,右上方
                    var x = sourceInfo.Point.X + sourceInfo.Size.Width / 2 + 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
        }
    }
}
