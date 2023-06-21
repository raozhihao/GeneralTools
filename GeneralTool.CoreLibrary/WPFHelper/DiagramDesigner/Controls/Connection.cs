using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Adorners;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Common;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls
{
    /// <summary>
    /// 连接线
    /// </summary>
    public class Connection : Control, IDisposable
    {
        static Connection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
            FrameworkPropertyMetadata metaData = new FrameworkPropertyMetadata()
            {
                AffectsRender = true,
                BindsTwoWayByDefault = true,
                SubPropertiesDoNotAffectRender = true,
                Inherits = true,
            };
            //此依赖属性主要用于绘制线点
            PathGeometryGroupProperty = DependencyProperty.Register(nameof(PathGeometryGroup), typeof(GeometryGroup), typeof(Connection), metaData);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty PathGeometryGroupProperty;

        /// <summary>
        /// 
        /// </summary>
        public GeometryGroup PathGeometryGroup
        {
            get => GetValue(PathGeometryGroupProperty) as GeometryGroup;
            set => SetValue(PathGeometryGroupProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(Connection), new PropertyMetadata(Brushes.DarkGoldenrod));

        /// <summary>
        /// 线条颜色
        /// </summary>
        public Brush Stroke
        {
            get => GetValue(StrokeProperty) as Brush;
            set => SetValue(StrokeProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(Connection), new PropertyMetadata(3.0));
        /// <summary>
        /// 线条粗细
        /// </summary>
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        /// <summary>
        /// 源点
        /// </summary>
        public ConnectorThumb SourceThumb { get; set; }
        /// <summary>
        /// 目标点
        /// </summary>
        public ConnectorThumb SinkThumb { get; set; }

        /// <summary>
        /// 画布
        /// </summary>
        public DesignerCanvas ParentCanvas { get; private set; }
        /// <summary>
        /// 源块
        /// </summary>
        public BlockItem SourceBlock { get; set; }
        /// <summary>
        /// 目录块
        /// </summary>
        public BlockItem SinkBlock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsStart { get; }

        /// <summary>
        /// 
        /// </summary>
        public Connection(ConnectorThumb sourceThumb, ConnectorThumb sinkThumb)
        {
            SourceThumb = sourceThumb;
            SinkThumb = sinkThumb;
            SourceBlock = sourceThumb.SourceBlock;
            SinkBlock = sinkThumb.SourceBlock;
            SourceBlock.LayoutUpdated += SourceBlock_LayoutUpdated;
            ParentCanvas = SourceBlock.ParentCanvas;
            PathGeometryGroup = new GeometryGroup();
            AddHandler(UIElement.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
            sourceThumb.SinkBlock = sinkThumb.SourceBlock;
            UpdatePath();
        }

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed && connector != null)
            {
                if (!IsMouseCaptured)
                {
                    _ = CaptureMouse();
                }

                connector.End = e.GetPosition(SourceBlock);

                e.Handled = true;
            }
            //this.Focus();

            base.OnPreviewMouseMove(e);
        }

        private ConnectionAdorner connector;

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (e.Source != this) return;
            if (ParentCanvas != null)
            {
                if (!ParentCanvas.CanOperation)
                {
                    return;
                }
            }

            ParentCanvas.SetTop(this);
            _ = CaptureMouse();
            SourceBlock.ConnectorVisibility = SinkBlock.ConnectorVisibility = Visibility.Collapsed;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(SourceBlock);
                Point startPoint = SourceThumb.TranslatePoint(new Point(SourceThumb.ActualWidth / 2, SourceThumb.ActualHeight / 2), SourceBlock);
                SourceBlock.ConnectorVisibility = Visibility.Visible;
                //this.ParentCanvas.ShowItemConnector();
                connector = new ConnectionAdorner(SourceBlock, startPoint, SourceThumb);
                ParentCanvas.ClearSection();
                layer.Add(connector);
                e.Handled = true;
                return;
            }
            //
        }

        private void Layer_KeyDown(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            if (e.Source != this) return;
            ReleaseMouseCapture();
            if (connector != null)
            {
                //查看是否在另一个点上
                ConnectorThumb sinkThumb = ParentCanvas.HitConnectorItem(e);
                if (sinkThumb != null)
                {
                    //判断目标点是否可作为终点使用
                    if (sinkThumb != null && (sinkThumb.ConnectorType == ConnectorType.None || sinkThumb.ConnectorType == ConnectorType.OnlySink))
                    {
                        //读取可连接的目标数量
                        //  var count = this.SourceBlock.ParentCanvas.Children.OfType<Connection>().Where(c => c.SinkThumb == sinkThumb).Count();
                        if (SourceThumb.Direction != sinkThumb.Direction)
                        {
                            //找到目标的点了
                            //if (!sinkThumb.SourceBlock.Equals(this.SourceBlock) && sinkThumb.SinkCount != count)
                            if (SourceThumb.GetCanConnectThumbs(sinkThumb.Direction))
                            {
                                //一个源只能对应一个相同的目标点,两个块之间不能互连
                                //当前块是否已经拥有目标点了,以及当前目标点是否作为源时是否已经拥有当前块做为目标点

                                //获取目标点的中间点

                                //中间点在canvas中的位置

                                //相对于当前源的位置

                                Dispose();

                                //清除当前连接的点
                                _ = SourceBlock.SinkItems.Remove(SinkBlock);
                                // this.SinkBlock.SourceItems.Remove(this.SourceBlock);
                                ParentCanvas.RemoveConnection(this);

                                //在当前块的目标集合中添加目标块
                                SourceBlock.AddSinkItem(sinkThumb.SourceBlock);
                                //在当前目标块的源集合中添加当前块
                                //  sinkThumb.SourceBlock.AddSourceItem(this.SourceBlock);

                                ParentCanvas.AddConnection(new Connection(SourceThumb, sinkThumb));
                            }
                        }

                    }
                }
                AdornerLayer.GetAdornerLayer(SourceBlock).Remove(connector);
                //this.ParentCanvas.CollapsedItemConnector();
            }
            _ = Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            LayoutUpdated -= SourceBlock_LayoutUpdated;
            if (SourceBlock != null)
            {
                SourceBlock.LayoutUpdated -= SourceBlock_LayoutUpdated;
                _ = SourceBlock.SinkItems.Remove(SinkBlock);

                //this.SinkBlock.SourceItems.Remove(this.SourceBlock);
            }
            if (connector != null)
            {
                AdornerLayer.GetAdornerLayer(SourceBlock).Remove(connector);
            }

        }

        private void SourceBlock_LayoutUpdated(object sender, EventArgs e)
        {
            UpdatePath();
        }

        private void UpdatePath()
        {
            #region MyRegion

            Point p1 = GetPoint(SourceThumb);// this.SourceThumb.TranslatePoint(new Point(this.SourceThumb.Width / 2, this.SourceThumb.Height / 2), this.ParentCanvas);
            Point p2 = GetExpand(p1, SourceThumb.Direction);

            Point p6 = GetPoint(SinkThumb);//this.SinkThumb.TranslatePoint(new Point(this.SinkThumb.Width / 2, this.SinkThumb.Height / 2), this.ParentCanvas);
            Point p5 = GetExpand(p6, SinkThumb.Direction);

            System.Collections.Generic.List<Point> points = PathExecute.Execute.GetPoints(new ConnectorInfo()
            {
                Direction = SourceThumb.Direction,
                Point = p2,
                Size = SourceBlock.DesiredSize
            },
            new ConnectorInfo()
            {
                Direction = SinkThumb.Direction,
                Size = SinkBlock.DesiredSize,
                Point = p5
            });

            PathGeometryGroup.Children.Clear();

            StreamGeometry stream = new StreamGeometry();
            using (StreamGeometryContext context = stream.Open())
            {
                context.BeginFigure(p1, true, false);
                context.PolyLineTo(points, true, false);
                context.LineTo(p6, true, false);
            }

            PathGeometryGroup.Children.Add(stream);

            #endregion

            DrawArrow(p6, SinkThumb.Direction);
            ParentCanvas.SetLine(this);
        }

        private Point GetPoint(ConnectorThumb thumb)
        {
            switch (thumb.Direction)
            {
                case Direction.Top:
                    return thumb.TranslatePoint(new Point(thumb.ActualWidth / 2, 0), ParentCanvas);
                case Direction.Bottom:
                    return thumb.TranslatePoint(new Point(thumb.ActualWidth / 2, thumb.ActualHeight), ParentCanvas);
                case Direction.Right:
                    return thumb.TranslatePoint(new Point(thumb.ActualWidth, thumb.ActualHeight / 2), ParentCanvas);
                case Direction.Left:
                    return thumb.TranslatePoint(new Point(0, thumb.ActualHeight / 2), ParentCanvas);

            }

            return default;
        }

        private void DrawArrow(Point startPoint, Direction direction)
        {
            PathFigure figure = new PathFigure
            {
                IsClosed = true,
                IsFilled = true,
                StartPoint = startPoint
            };
            switch (direction)
            {
                case Direction.Top:
                    figure.Segments.Add(new LineSegment(new Point(startPoint.X + 2.5, startPoint.Y - 5), true));
                    figure.Segments.Add(new LineSegment(new Point(startPoint.X - 2.5, startPoint.Y - 5), true));
                    break;
                case Direction.Bottom:
                    figure.Segments.Add(new LineSegment(new Point(startPoint.X + 2.5, startPoint.Y + 5), true));
                    figure.Segments.Add(new LineSegment(new Point(startPoint.X - 2.5, startPoint.Y + 5), true));
                    break;
                case Direction.Right:
                    figure.Segments.Add(new LineSegment(new Point(startPoint.X + 5, startPoint.Y + 2.5), true));
                    figure.Segments.Add(new LineSegment(new Point(startPoint.X + 5, startPoint.Y - 2.5), true));
                    break;
                case Direction.Left:
                    figure.Segments.Add(new LineSegment(new Point(startPoint.X - 5, startPoint.Y + 2.5), true));
                    figure.Segments.Add(new LineSegment(new Point(startPoint.X - 5, startPoint.Y - 2.5), true));
                    break;
            }
            PathGeometry polygon = new PathGeometry();
            polygon.Figures.Add(figure);
            PathGeometryGroup.Children.Add(polygon);
        }

        private Point GetExpand(Point point, Direction direction)
        {
            //外扩点
            switch (direction)
            {
                case Direction.Top:
                    point.Y -= 10;
                    break;
                case Direction.Bottom:
                    point.Y += 10;
                    break;
                case Direction.Right:
                    point.X += 10;
                    break;
                case Direction.Left:
                    point.X -= 10;
                    break;
            }
            return point;
        }

    }

}
