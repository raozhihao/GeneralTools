using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.General.WPFHelper.DiagramDesigner.Models;

using SimpleDiagram.BlockVIewModels;

namespace SimpleDiagram.Blocks
{
    public abstract class TrueFalseBaseBlock : BaseBlock
    {
        public override void OnCreateInCanvas()
        {
            base.OnCreateInCanvas();
            this.ConnectorThumbs[Direction.Left].Visibility =
               System.Windows.Visibility.Collapsed;

            this.SetCanConnectSinkDirections(new System.Collections.Generic.Dictionary<Direction, Direction[]>()
            {
                { Direction.Bottom,new Direction[] {Direction.Top}},
                { Direction.Right,new Direction[] {Direction.Top}},

            });

            this.ConnectorThumbs[Direction.Bottom].ConnectorType = ConnectorType.OnlySource;
            this.ConnectorThumbs[Direction.Top].ConnectorType = ConnectorType.OnlySink;
        }

        public override void SetProperty()
        {
            base.SetProperty();
            if (this.SinkItems.Count == 0)
            {
                return;
            }
            var ifData = this.BlockViewModel as TrueFalseBaseBlockViewModel;

            //找出正确的连接线
            var trueConnection = this.ParentCanvas.Children.OfType<Connection>().FirstOrDefault(s => s.SourceThumb == this.ConnectorThumbs[Direction.Bottom]);
            if (trueConnection != null)
                ifData.TrueViewModel = trueConnection.SinkBlock.DataContext as BaseBlockViewModel;

            //找出错误的连接线
            var falseConnection = this.ParentCanvas.Children.OfType<Connection>().FirstOrDefault(s => s.SourceThumb == this.ConnectorThumbs[Direction.Right]);
            if (falseConnection != null)
                ifData.FalseViewModel = falseConnection.SinkBlock.DataContext as BaseBlockViewModel;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!this.IsInCanvas) return;
            base.OnRender(drawingContext);
            var trueFormat = new FormattedText("True", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, System.Windows.Media.Brushes.Black, 1);
            drawingContext.DrawText(trueFormat, new Point(this.DesiredSize.Width / 2 + 10, this.DesiredSize.Height));

            var falseFormat = new FormattedText("False", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 10, System.Windows.Media.Brushes.Black, 1);
            drawingContext.DrawText(falseFormat, new Point(this.DesiredSize.Width + 5, this.DesiredSize.Height / 2 - 20));
        }

        public override void OnRemoveConnection(Connection connection)
        {
            var ifData = this.BlockViewModel as TrueFalseBaseBlockViewModel;
            //找出是哪边的删除的
            var direction = connection.SourceThumb.Direction;
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
