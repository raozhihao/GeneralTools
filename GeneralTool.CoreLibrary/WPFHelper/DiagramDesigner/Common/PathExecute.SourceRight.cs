using System.Collections.Generic;
using System.Windows;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Common
{
    partial class PathExecute
    {

        private void CreateSourceRightPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            switch (sinkInfo.Direction)
            {
                case Direction.Top:
                    this.CreateRightTopPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Bottom:
                    this.CreateRightBottomPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Right:
                    this.CreateRightRightPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Left:
                    this.CreateRightLeftPoints(sourceInfo, sinkInfo, points);
                    break;
            }
        }

        private void CreateRightLeftPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //源在上,目标在下
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    var y = sourceInfo.Point.Y + sourceInfo.Size.Height / 2 + 5;
                    points.Add(new Point(sourceInfo.Point.X, y));
                    points.Add(new Point(sinkInfo.Point.X, y));
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
                    var y = sourceInfo.Point.Y - sourceInfo.Size.Height / 2 - 5;
                    points.Add(new Point(sourceInfo.Point.X, y));
                    points.Add(new Point(sinkInfo.Point.X, y));
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
        private void CreateRightRightPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //源在上,目标在下
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
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
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
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
        private void CreateRightBottomPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //源在上,目标在下
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    var x = sourceInfo.Point.X;
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
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    var x = sourceInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
        }
        private void CreateRightTopPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //源在上,目标在下
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在右
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
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
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    var x = sourceInfo.Point.X;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
            }
        }
    }
}
