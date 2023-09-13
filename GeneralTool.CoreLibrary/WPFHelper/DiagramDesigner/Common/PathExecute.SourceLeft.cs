using System.Collections.Generic;
using System.Windows;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Common
{
    public partial class PathExecute
    {

        private void CreateSourceLeftPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            switch (sinkInfo.Direction)
            {
                case Direction.Top:
                    CreateLeftTopPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Bottom:
                    CreateLeftBottomPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Right:
                    CreateLeftRightPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Left:
                    CreateLeftLeftPoints(sourceInfo, sinkInfo, points);
                    break;
            }
        }

        private void CreateLeftBottomPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //源在上,目标在下
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    double x = sinkInfo.Point.X - (sinkInfo.Size.Width / 2) - 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    double x = sourceInfo.Point.X;
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
                    double x = sinkInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    double y = sourceInfo.Point.Y - (sourceInfo.Size.Height / 2) - 5;
                    points.Add(new Point(sourceInfo.Point.X, y));
                    points.Add(new Point(sinkInfo.Point.X, y));
                }
            }
        }
        private void CreateLeftRightPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
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
                    double y = sourceInfo.Point.Y + (sourceInfo.Size.Height / 2) + 5;
                    points.Add(new Point(sourceInfo.Point.X, y));
                    points.Add(new Point(sinkInfo.Point.X, y));
                }
            }
            else
            {
                //源在下,目标在上
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    double x = sinkInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    double y = sourceInfo.Point.Y - (sourceInfo.Size.Height / 2) - 5;
                    points.Add(new Point(sourceInfo.Point.X, y));
                    points.Add(new Point(sinkInfo.Point.X, y));
                }
            }
        }
        private void CreateLeftLeftPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
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
                    double x = sinkInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    double x = sourceInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
        }

        private void CreateLeftTopPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
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
                    //目标在右
                    double x = sinkInfo.Point.X - (sinkInfo.Size.Width / 2) - 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在左
                    double x = sourceInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
        }
    }
}
