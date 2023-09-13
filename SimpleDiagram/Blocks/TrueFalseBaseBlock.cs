using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

using SimpleDiagram.BlockVIewModels;

namespace SimpleDiagram.Blocks
{
    public abstract class TrueFalseBaseBlock : BaseBlock
    {
        public override void OnCreateInCanvas()
        {
            base.OnCreateInCanvas();
            ConnectorThumbs[Direction.Left].Visibility =
               System.Windows.Visibility.Collapsed;

            SetCanConnectSinkDirections(new System.Collections.Generic.Dictionary<Direction, Direction[]>()
            {
                { Direction.Bottom,new Direction[] {Direction.Top}},
                { Direction.Right,new Direction[] {Direction.Top}},

            });

            ConnectorThumbs[Direction.Bottom].ConnectorType = ConnectorType.OnlySource;
            ConnectorThumbs[Direction.Top].ConnectorType = ConnectorType.OnlySink;
        }

        public override void SetProperty()
        {
            base.SetProperty();
            if (SinkItems.Count == 0)
            {
                return;
            }
            TrueFalseBaseBlockViewModel ifData = BlockViewModel as TrueFalseBaseBlockViewModel;

            //找出正确的连接线
            Connection trueConnection = ParentCanvas.Children.OfType<Connection>().FirstOrDefault(s => s.SourceThumb == ConnectorThumbs[Direction.Bottom]);
            if (trueConnection != null)
                ifData.TrueViewModel = trueConnection.SinkBlock.DataContext as BaseBlockViewModel;

            //找出错误的连接线
            Connection falseConnection = ParentCanvas.Children.OfType<Connection>().FirstOrDefault(s => s.SourceThumb == ConnectorThumbs[Direction.Right]);
            if (falseConnection != null)
                ifData.FalseViewModel = falseConnection.SinkBlock.DataContext as BaseBlockViewModel;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!IsInCanvas) return;
            base.OnRender(drawingContext);
            FormattedText trueFormat = new FormattedText("True", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, System.Windows.Media.Brushes.Black, 1);
            drawingContext.DrawText(trueFormat, new Point((DesiredSize.Width / 2) + 10, DesiredSize.Height));

            FormattedText falseFormat = new FormattedText("False", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, System.Windows.Media.Brushes.Black, 1);
            drawingContext.DrawText(falseFormat, new Point(DesiredSize.Width + 5, (DesiredSize.Height / 2) - 20));
        }

        public override void OnRemoveConnection(Connection connection)
        {
            TrueFalseBaseBlockViewModel ifData = BlockViewModel as TrueFalseBaseBlockViewModel;
            //找出是哪边的删除的
            Direction direction = connection.SourceThumb.Direction;
            switch (direction)
            {
                case Direction.Top:
                    break;
                case Direction.Bottom:
                    //true的
                    ifData.TrueViewModel = null;
                    break;
                case Direction.Right:
                    //False
                    ifData.FalseViewModel = null;
                    break;
                case Direction.Left:
                    break;
            }
            base.OnRemoveConnection(connection);
        }
    }
}
