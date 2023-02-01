using System.Collections.Generic;
using System.Windows;

using GeneralTool.General.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PathExecute
    {
        /// <summary>
        /// 
        /// </summary>
        public static PathExecute Execute => new PathExecute();

        /// <summary>
        /// 
        /// </summary>
        public List<Point> GetPoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo)
        {
            var points = new List<Point>
            {
                sourceInfo.Point//起点
            };

            CreatePoints(sourceInfo, sinkInfo, points);
            points.Add(sinkInfo.Point);
            return points;
        }

        private void CreatePoints(ConnectorInfo sourceInfo, ConnectorInfo sinkInfo, List<Point> points)
        {
            //根据起点终点来判断线路走向
            switch (sourceInfo.Direction)
            {
                case Direction.Top:
                    CreateSourceTopPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Bottom:
                    CreateSourceBottomPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Right:
                    CreateSourceRightPoints(sourceInfo, sinkInfo, points);
                    break;
                case Direction.Left:
                    CreateSourceLeftPoints(sourceInfo, sinkInfo, points);
                    break;
            }


        }

    }
}
