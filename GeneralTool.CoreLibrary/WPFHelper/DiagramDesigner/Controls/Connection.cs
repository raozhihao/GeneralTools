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
            var metaData = new FrameworkPropertyMetadata()
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
            get => this.GetValue(PathGeometryGroupProperty) as GeometryGroup;
            set => this.SetValue(PathGeometryGroupProperty, value);
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
            get => this.GetValue(StrokeProperty) as Brush;
            set => this.SetValue(StrokeProperty, value);
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
            get => (double)this.GetValue(StrokeThicknessProperty);
            set => this.SetValue(StrokeThicknessProperty, value);
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
            this.SourceThumb = sourceThumb;
            this.SinkThumb = sinkThumb;
            this.SourceBlock = sourceThumb.SourceBlock;
            this.SinkBlock = sinkThumb.SourceBlock;
            this.SourceBlock.LayoutUpdated += SourceBlock_LayoutUpdated;
            this.ParentCanvas = this.SourceBlock.ParentCanvas;
            this.PathGeometryGroup = new GeometryGroup();
            this.AddHandler(UIElement.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
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
            this.ParentCanvas.SetTop(this);

            if (e.LeftButton == MouseButtonState.Pressed && this.connector != null)
            {
                if (!this.IsMouseCaptured)
                {
                    this.CaptureMouse();
                }

                this.connector.End = e.GetPosition(this.SourceBlock);

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
            if (this.ParentCanvas != null)
            {
                if (!this.ParentCanvas.CanOperation)
                {
                    return;
                }
            }

            this.CaptureMouse();
            this.SourceBlock.ConnectorVisibility = this.SinkBlock.ConnectorVisibility = Visibility.Collapsed;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var layer = AdornerLayer.GetAdornerLayer(this.SourceBlock);
                var startPoint = this.SourceThumb.TranslatePoint(new Point(this.SourceThumb.ActualWidth / 2, this.SourceThumb.ActualHeight / 2), this.SourceBlock);
                this.SourceBlock.ConnectorVisibility = Visibility.Visible;
                //this.ParentCanvas.ShowItemConnector();
                this.connector = new ConnectionAdorner(this.SourceBlock, startPoint, this.SourceThumb);
                this.ParentCanvas.ClearSection();
                layer.Add(this.connector);
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
            this.ReleaseMouseCapture();
            if (this.connector != null)
            {
                //查看是否在另一个点上
                var sinkThumb = this.ParentCanvas.HitConnectorItem(e);
                if (sinkThumb != null)
                {
                    //判断目标点是否可作为终点使用
                    if (sinkThumb != null && (sinkThumb.ConnectorType == ConnectorType.None || sinkThumb.ConnectorType == ConnectorType.OnlySink))
                    {
                        //读取可连接的目标数量
                        //  var count = this.SourceBlock.ParentCanvas.Children.OfType<Connection>().Where(c => c.SinkThumb == sinkThumb).Count();
                        if (this.SourceThumb.Direction != sinkThumb.Direction)
                        {
                            //找到目标的点了
                            //if (!sinkThumb.SourceBlock.Equals(this.SourceBlock) && sinkThumb.SinkCount != count)
                            if (this.SourceThumb.GetCanConnectThumbs(sinkThumb.Direction))
                            {
                                //一个源只能对应一个相同的目标点,两个块之间不能互连
                                //当前块是否已经拥有目标点了,以及当前目标点是否作为源时是否已经拥有当前块做为目标点

                                //获取目标点的中间点

                                //中间点在canvas中的位置

                                //相对于当前源的位置

                                this.Dispose();

                                //清除当前连接的点
                                this.SourceBlock.SinkItems.Remove(this.SinkBlock);
                                // this.SinkBlock.SourceItems.Remove(this.SourceBlock);
                                this.ParentCanvas.RemoveConnection(this);

                                //在当前块的目标集合中添加目标块
                                this.SourceBlock.AddSinkItem(sinkThumb.SourceBlock);
                                //在当前目标块的源集合中添加当前块
                                //  sinkThumb.SourceBlock.AddSourceItem(this.SourceBlock);

                                ParentCanvas.AddConnection(new Connection(this.SourceThumb, sinkThumb));
                            }
                        }

                    }
                }
                AdornerLayer.GetAdornerLayer(this.SourceBlock).Remove(this.connector);
                //this.ParentCanvas.CollapsedItemConnector();
            }
            this.Focus();
        }



        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.LayoutUpdated -= SourceBlock_LayoutUpdated;
            if (this.SourceBlock != null)
            {
                this.SourceBlock.LayoutUpdated -= SourceBlock_LayoutUpdated;
                this.SourceBlock.SinkItems.Remove(this.SinkBlock);

                //this.SinkBlock.SourceItems.Remove(this.SourceBlock);
            }
            if (this.connector != null)
            {
                AdornerLayer.GetAdornerLayer(this.SourceBlock).Remove(this.connector);
            }

        }

        private void SourceBlock_LayoutUpdated(object sender, EventArgs e)
        {
            this.UpdatePath();
        }

        private void UpdatePath()
        {
            #region MyRegion

            var p1 = this.GetPoint(this.SourceThumb);// this.SourceThumb.TranslatePoint(new Point(this.SourceThumb.Width / 2, this.SourceThumb.Height / 2), this.ParentCanvas);
            var p2 = GetExpand(p1, this.SourceThumb.Direction);

            var p6 = this.GetPoint(this.SinkThumb);//this.SinkThumb.TranslatePoint(new Point(this.SinkThumb.Width / 2, this.SinkThumb.Height / 2), this.ParentCanvas);
            var p5 = GetExpand(p6, this.SinkThumb.Direction);

            var points = PathExecute.Execute.GetPoints(new ConnectorInfo()
            {
                Direction = this.SourceThumb.Direction,
                Point = p2,
                Size = this.SourceBlock.DesiredSize
            },
            new ConnectorInfo()
            {
                Direction = this.SinkThumb.Direction,
                Size = this.SinkBlock.DesiredSize,
                Point = p5
            });

            this.PathGeometryGroup.Children.Clear();

            var stream = new StreamGeometry();
            using (var context = stream.Open())
            {
                context.BeginFigure(p1, true, false);
                context.PolyLineTo(points, true, false);
                context.LineTo(p6, true, false);
            }

            this.PathGeometryGroup.Children.Add(stream);

            #endregion

            DrawArrow(p6, this.SinkThumb.Direction);
            this.ParentCanvas.SetLine(this);
        }

        private Point GetPoint(ConnectorThumb thumb)
        {
            //this.SourceThumb.TranslatePoint(new Point(this.SourceThumb.Width / 2, this.SourceThumb.Height / 2), this.ParentCanvas);
            switch (thumb.Direction)
            {
                case Direction.Top:
                    return thumb.TranslatePoint(new Point(thumb.ActualWidth / 2, 0), this.ParentCanvas);
                case Direction.Bottom:
                    return thumb.TranslatePoint(new Point(thumb.ActualWidth / 2, thumb.ActualHeight), this.ParentCanvas);
                case Direction.Right:
                    return thumb.TranslatePoint(new Point(thumb.ActualWidth, thumb.ActualHeight / 2), this.ParentCanvas);
                case Direction.Left:
                    return thumb.TranslatePoint(new Point(0, thumb.ActualHeight / 2), this.ParentCanvas);
            }
            return default;
        }

        private void DrawArrow(Point startPoint, Direction direction)
        {
            var figure = new PathFigure
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
            var polygon = new PathGeometry();
            polygon.Figures.Add(figure);
            this.PathGeometryGroup.Children.Add(polygon);
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
