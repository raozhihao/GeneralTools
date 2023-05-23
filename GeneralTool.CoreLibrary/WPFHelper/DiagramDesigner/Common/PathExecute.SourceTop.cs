using System.Collections.Generic;
using System.Windows;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Common
{
    partial class PathExecute
    {
        private void CreateSourceTopPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            //源点在上方
            switch (sinkInfo.Direction)
            {
                case Direction.Top:
                    //两者都在上方,查看两者的高度
                    CreateTopTop(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Bottom:
                    CreateTopBottom(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Right:
                    CreateTopRight(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Left:
                    CreateTopLeft(sourceInfo, sinkInfo, points);
                    break;
            }
        }

        private void CreateTopLeft(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            //源在上,目标在下方
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //目标点在源点的下方,则查看目标点是否在右方,为右下方
                var p2X = sourceInfo.Size.Width / 2 + sourceInfo.Point.X;
                if (p2X < (sinkInfo.Size.Width / 2 + sinkInfo.Point.X))
                {
                    //且在源点右方,则为下右
                    double x = p2X + 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));

                    if (sinkInfo.Point.X > x)
                    {
                        points.Add(new Point(x, sinkInfo.Point.Y));
                    }
                    else
                    {
                        var tmpY = sinkInfo.Point.Y - sinkInfo.Size.Height / 2 - 5;
                        points.Add(new Point(x, tmpY));
                        points.Add(new Point(sinkInfo.Point.X, tmpY));
                    }
                }
                else
                {
                    //在源点左方,则为下左,源向左
                    if (sinkInfo.Point.X < (sourceInfo.Point.X - sourceInfo.Size.Width / 2))
                    {
                        points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                        points.Add(new Point(sinkInfo.Point.X, sinkInfo.Point.Y));
                    }
                    else
                    {
                        var tmpX = sourceInfo.Point.X - sourceInfo.Size.Width / 2 - 5;
                        points.Add(new Point(tmpX, sourceInfo.Point.Y));
                        points.Add(new Point(tmpX, sinkInfo.Point.Y));
                    }
                }
            }
            else
            {
                //源在下,目标在上方,查看目标在源的x轴方向
                if (sourceInfo.Point.X < sinkInfo.Point.X)
                {
                    //目标在右上方
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在左上方
                    points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));

                }
            }
        }

        private void CreateTopRight(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            //源在上
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //目标在下
                var tmpSourceX = sourceInfo.Point.X - sourceInfo.Size.Width / 2;
                if (tmpSourceX > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(tmpSourceX - 5, sourceInfo.Point.Y));
                    points.Add(new Point(tmpSourceX - 5, sinkInfo.Point.Y));
                }
                else
                {
                    //目标在右
                    tmpSourceX = sourceInfo.Point.X + sourceInfo.Size.Width / 2;

                    if (tmpSourceX > sinkInfo.Point.X)
                    {
                        points.Add(new Point(tmpSourceX + 5, sourceInfo.Point.Y));
                        points.Add(new Point(tmpSourceX + 5, sinkInfo.Point.Y));
                    }
                    else
                    {
                        points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                    }
                }
            }
            else
            {
                //目标在上
                if (sourceInfo.Point.X > sinkInfo.Point.X)
                {
                    //目标在左
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
                }
                else
                {
                    points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
                }
            }
        }

        private void CreateTopBottom(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            //源在上,目标在下
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //目标在源的下方
                var tmpSinkX = (sinkInfo.Point.X - sinkInfo.Size.Width / 2);
                if (sourceInfo.Point.X > tmpSinkX)
                {
                    //且目标处于源的左方
                    var tmpSourceX = (sourceInfo.Point.X - sourceInfo.Size.Width / 2);
                    if (tmpSourceX < tmpSinkX)
                    {
                        var tmpX = tmpSourceX - 5;
                        points.Add(new Point(tmpX, sourceInfo.Point.Y));
                        points.Add(new Point(tmpX, sinkInfo.Point.Y));
                    }
                    else
                    {
                        points.Add(new Point(tmpSinkX - 5, sourceInfo.Point.Y));
                        points.Add(new Point(tmpSinkX - 5, sinkInfo.Point.Y));
                    }
                }
                else
                {
                    //目标在右方
                    var tmpSourceX = sourceInfo.Point.X + sourceInfo.Size.Width / 2;
                    double x;
                    //在右方且源块右边比目标块左边要大
                    if (tmpSourceX > tmpSinkX)
                    {
                        x = sinkInfo.Point.X + sinkInfo.Size.Width / 2 + 5;
                        points.Add(new Point(x, sourceInfo.Point.Y));
                        points.Add(new Point(x, sinkInfo.Point.Y));
                    }
                    else
                    {
                        x = tmpSourceX + 5;
                        points.Add(new Point(x, sourceInfo.Point.Y));
                        points.Add(new Point(x, sinkInfo.Point.Y));
                    }
                }
            }
            else
            {
                //目标在上
                points.Add(new Point(sinkInfo.Point.X, sourceInfo.Point.Y));
            }
        }

        private void CreateTopTop(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            if (sourceInfo.Point.Y < sinkInfo.Point.Y)
            {
                //目标点在源点下方
                var p2X = sourceInfo.Size.Width / 2 + sourceInfo.Point.X;
                double x;
                if (p2X < (sinkInfo.Size.Width / 2 + sinkInfo.Point.X))
                {
                    //且在源点右方,则为下右
                    x = p2X + 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));
                }
                else
                {
                    //目标点在源左方,则为下左,则线条往左前进
                    x = sourceInfo.Point.X - sourceInfo.Size.Width / 2 - 5;
                    points.Add(new Point(x, sourceInfo.Point.Y));
                    points.Add(new Point(x, sinkInfo.Point.Y));

                }
            }
            else
            {
                //目标点在源点上方
                //判断目标点是在右还是左,则判断目标点所处的x坐标是否大于
                var tmpX = sinkInfo.Point.X - sinkInfo.Size.Width / 2;
                if (sourceInfo.Point.X > tmpX)
                {
                    //目标点在左上方

                    points.Add(new Point(tmpX - 5, sourceInfo.Point.Y));
                    points.Add(new Point(tmpX - 5, sinkInfo.Point.Y));
                }
                else
                {
                    //目标点在右上方
                    points.Add(new Point(sourceInfo.Point.X, sinkInfo.Point.Y));
                }

            }
        }
    }
}
