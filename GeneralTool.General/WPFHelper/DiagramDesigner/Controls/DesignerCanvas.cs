using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using GeneralTool.General.WPFHelper.DiagramDesigner.Adorners;
using GeneralTool.General.WPFHelper.DiagramDesigner.Common;
using GeneralTool.General.WPFHelper.DiagramDesigner.Models;
using GeneralTool.General.WPFHelper.DiagramDesigner.Thumbs;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Controls
{
    public partial class DesignerCanvas : Canvas
    {
        /// <summary>
        /// 
        /// </summary>
        public DesignerCanvas()
        {
            this.PreviewMouseWheel += DesignerCanvas_PreviewMouseWheel;

            MiddleController.Controller.OnDragBlockItemEvent += Controller_OnDragBlockItemEvent;

            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
            this.Loaded += DesignerCanvas_Loaded;
        }

        private ScrollViewer ParentScrollView;
        private void DesignerCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.ParentScrollView = this.GetUIParentCore() as ScrollViewer;
        }


        /// <summary>
        /// 
        /// </summary>
        public List<BlockItem> Removes { get; private set; } = new List<BlockItem>();

        private DragObject currentObj;

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                var block = this.Children.OfType<BlockItem>().Where(b => b.IsSelected).FirstOrDefault();
                if (block != null)
                {
                    this.currentObj = block.GetDragObject();
                    this.currentObj.DragItem = block;
                    this.currentObj.IsDoubleClickAdd = true;
                }
            }
            else if (e.Key == Key.V && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                if (this.currentObj != null)
                {
                    this.AddDragObject(this.currentObj, null);
                }
            }
            else if (e.Key == Key.Delete)
            {
                if (this.CanOperation)
                {
                    this.RemoveList();
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public event Func<List<BlockItem>, bool> RemoveItemsEvent;
        private void RemoveList()
        {
            //多选
            var items = this.Children.OfType<BlockItem>().Where(b => b.IsSelected).ToList();

            if (this.RemoveItemsEvent != null && items.Count > 0)
            {
                var re = this.RemoveItemsEvent(items);
                if (!re)
                    return;
            }
            this.RemoveBlocks(items.ToArray());
            this.ClearSection();

            //单个
            var hit = this.InputHitTest(Mouse.GetPosition(this));

            if (hit == null)
            {
                return;
            }


            while (hit != null)
            {
                if (hit is Connection t)
                {
                    this.RemoveConnection(t);
                    break;
                }
                else if (hit is BlockItem b)
                {
                    this.RemoveBlocks(b);
                    break;
                }

                if (!(hit is FrameworkElement tmp))
                    break;
                hit = tmp.TemplatedParent as IInputElement;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveBlocks(params BlockItem[] blocks)
        {
            #region new

            var sourceItems = new List<BlockItem>();//源集合
            var sinkItems = new List<BlockItem>();//目标集合
            //循环集合
            foreach (var item in blocks)
            {
                if (item.IsStart) continue;

                // sourceItems.AddRange(item.SourceItems);
                sinkItems.AddRange(item.SinkItems);
                //删除自身
                this.RemoveItem(item);
            }

            BlockItem previaSource = null;
            foreach (var item in sourceItems)
            {
                if (this.Children.Contains(item))
                {
                    previaSource = item;
                    break;
                }
            }

            if (previaSource == null)
                return;
            BlockItem nextSource = null;
            foreach (var item in sinkItems)
            {
                if (this.Children.Contains(item))
                {
                    nextSource = item;
                    break;
                }
            }
            if (nextSource == null) return;

            if (nextSource == previaSource) return;
            previaSource.AddSinkItem(nextSource);
            // nextSource.AddSourceItem(prevSource);
            var connection = new Connection(previaSource.ConnectorThumbs[Direction.Bottom], nextSource.ConnectorThumbs[Direction.Top]);
            this.AddConnection(connection);
            #endregion
        }



        #region 附加/依赖属性
        /// <summary>
        /// 
        /// </summary>

        public static readonly DependencyProperty MinScaleValueProperty = DependencyProperty.Register(nameof(MinScaleValue), typeof(double), typeof(DesignerCanvas), new PropertyMetadata(0.5));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MaxScaleValueProperty = DependencyProperty.Register(nameof(MaxScaleValue), typeof(double), typeof(DesignerCanvas), new PropertyMetadata(5.0));




        #endregion

        #region 属性


        /// <summary>
        /// 缩放的最小值
        /// </summary>
        public double MinScaleValue
        {
            get => Convert.ToDouble(this.GetValue(MinScaleValueProperty));
            set => this.SetValue(MinScaleValueProperty, value);
        }

        /// <summary>
        /// 缩放的最大值
        /// </summary>
        public double MaxScaleValue
        {
            get => Convert.ToDouble(this.GetValue(MaxScaleValueProperty));
            set => this.SetValue(MaxScaleValueProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public SelectionService Selection => new SelectionService();

        /// <summary>
        /// 
        /// </summary>
        public string LayoutId { get; set; }
        #endregion

        #region 私有方法

        private Point? rubberbandSelectionStartPoint;

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            Focus();
            if (e.Source == this)
            {
                this.rubberbandSelectionStartPoint = new Point?(e.GetPosition(this));
            }
        }

        private Point? rightMouseDownStartPoint;
        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
            this.rightMouseDownStartPoint = e.GetPosition(this);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (e.Source == this && this.rubberbandSelectionStartPoint.HasValue && e.LeftButton == MouseButtonState.Pressed)
            {
                // create rubberband adorner
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    RubberbandAdorner adorner = new RubberbandAdorner(this, rubberbandSelectionStartPoint);
                    if (adorner != null)
                    {
                        adornerLayer.Add(adorner);
                    }
                }
                e.Handled = true;
            }
            else if (e.RightButton == MouseButtonState.Pressed && this.rightMouseDownStartPoint != null)
            {
                var curr = e.GetPosition(this);
                var v = curr - this.rightMouseDownStartPoint.Value;
                if (this.ParentScrollView != null)
                {
                    this.ParentScrollView.ScrollToVerticalOffset(this.ParentScrollView.VerticalOffset - v.Y);
                    this.ParentScrollView.ScrollToHorizontalOffset(this.ParentScrollView.HorizontalOffset - v.X);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (e.Source == this)
            {
                this.rubberbandSelectionStartPoint = null;
                this.ReleaseMouseCapture();
                e.Handled = true;
            }
            this.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonUp(e);
            this.ClearSection();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearSection()
        {
            foreach (var item in this.Children.OfType<BlockItem>())
            {
                item.IsSelected = false;
            }
        }


        internal void RaiseSelectionChanged()
        {
            this.ReleaseMouseCapture();
            this.rubberbandSelectionStartPoint = null;
            this.Focus();
        }

        void DesignerCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //处理缩放

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                //if (this.LayoutTransform is ScaleTransform scaleTransform)
                //{
                //    var scale = e.Delta * 0.001 + scaleTransform.ScaleX;
                //    scale = scale < 0.05 ? 0.05 : scale;
                //    scale = scale > 5 ? 5 : scale;

                //    scaleTransform.ScaleX = scale;
                //    scaleTransform.ScaleY = scale;

                //    e.Handled = true;
                //    ScaleChanged?.Invoke(scale * 100);

                //}

                if (this.LayoutTransform is TransformGroup group)
                {
                    var scaleTransform = group.Children[0] as ScaleTransform;

                    //Point zoomCenter = e.GetPosition(this);//参数必须是this.mCanvas, e.GetPosition和RenderTransform有关?
                    //var translate = group.Children[1] as TranslateTransform;
                    //Point pt = this.LayoutTransform.Inverse.Transform(zoomCenter);
                    //translate.X = (zoomCenter.X - pt.X) * scaleTransform.ScaleX;
                    //translate.Y = (zoomCenter.Y - pt.Y) * scaleTransform.ScaleY;

                    var scale = e.Delta * 0.001 + scaleTransform.ScaleX;
                    scale = scale < 0.05 ? 0.05 : scale;
                    scale = scale > 5 ? 5 : scale;

                    scaleTransform.ScaleX = scale;
                    scaleTransform.ScaleY = scale;

                    e.Handled = true;
                    ScaleChanged?.Invoke(scale * 100);



                }
            }

        }
        // }

        internal ConnectorThumb HitConnectorItem(MouseButtonEventArgs e)
        {
            var dest = this.InputHitTest(e.GetPosition(this));
            if (dest is System.Windows.Shapes.Ellipse el)
            {
                if (el.TemplatedParent is ConnectorThumb thumbTmp)
                {
                    return thumbTmp;
                }
            }
            //else if (dest is System.Windows.Shapes.Shape s)
            //{
            //    if (s.TemplatedParent is Connection c)
            //    {
            //        return c.SinkThumb;
            //    }
            //}
            else if (dest is Border borderTmp)
            {
                if (borderTmp.Name == "ThumbBorder")
                {
                    return borderTmp.TemplatedParent as ConnectorThumb;
                }
            }

            return null;

            ////((dest as FrameworkElement).TemplatedParent as FrameworkElement).TemplatedParent
            //BlockItem item = null;
            //FrameworkElement fe = dest as FrameworkElement;
            //while (item == null)
            //{
            //    if (fe is BlockItem b)
            //    {
            //        item = b;
            //        break;
            //    }
            //    else
            //    {
            //        if (fe == null) break;
            //        if (fe.TemplatedParent is FrameworkElement f)
            //            fe = f;
            //        if (fe is DesignerCanvas)
            //            break;
            //    }
            //}

            //if (item == null)
            //    return null;

            ////查看当前的点在当前的块的哪个区域
            //var blockPoint = this.TranslatePoint(e.GetPosition(this), item);
            //if (blockPoint.X < 0 || blockPoint.Y < 0)
            //    return null;

            //if (blockPoint.X > item.DesiredSize.Width || blockPoint.Y > item.DesiredSize.Height)
            //    return null;

            ////查看是否在左边区域
            //var thumb = this.GetThumb(item, Direction.Left, blockPoint);
            //if (thumb == null)
            //    thumb = this.GetThumb(item, Direction.Top, blockPoint);
            //if (thumb == null)
            //    thumb = this.GetThumb(item, Direction.Right, blockPoint);
            //if (thumb == null)
            //    thumb = this.GetThumb(item, Direction.Bottom, blockPoint);

            //return thumb;
        }


        private void Controller_OnDragBlockItemEvent(DragObject dragObject)
        {
            AddDragObject(dragObject, null);
        }



        /// <summary>
        /// 
        /// </summary>
        public void RemoveConnection(Connection connection)
        {
            connection.Dispose();
            connection.SourceBlock.RemoveConnection(connection);
            this.Children.Remove(connection);
            connection.SourceThumb.SinkBlock = null;
        }


        /// <summary>
        /// 
        /// </summary>
        public void AddConnection(Connection connection)
        {
            connection.SourceBlock.AddConnection(connection);
            this.Children.Add(connection);
        }


        /// <summary>
        /// 复制时事件
        /// </summary>
        public event EventHandler<BlockCopyArgs> CopyEvent;


        /// <summary>
        /// 
        /// </summary>
        public void AddDragObject(DragObject dragObject, DragEventArgs e)
        {
            if (dragObject == null)
                return;
            if (!dragObject.CanRepeatToCanvas)
            {
                if (this.Children.OfType<BlockItem>().FirstOrDefault(t => t.GetType() == dragObject.DragType) != null)
                {
                    return;
                }
            }

            var block = Activator.CreateInstance(dragObject.DragType) as BlockItem;
            block.FontSize = dragObject.FontSize;
            block.Background = dragObject.BackGround;
            block.Foreground = dragObject.ForceGround;
            block.Padding = dragObject.Padding;
            block.Content = dragObject.Content;
            block.Header = dragObject.Header;
            block.IsStart = dragObject.IsStart;

            //设置位置
            Connection connection = null;

            this.Children.Add(block);
            block.IsInCanvas = true;
            block.ApplyTemplate();
            AddTempelte(block);
            block.Apply();
            var last = this.HitSourceBlock(e);
            if (last == null)
            {
                Point point = default;
                if (e != null)
                    point = e.GetPosition(this);
                Canvas.SetLeft(block, point.X);
                Canvas.SetTop(block, point.Y);
            }
            else
            {
                var lastX = Canvas.GetLeft(last.Item1);
                var lastY = Canvas.GetTop(last.Item1);
                var lastWidth = last.Item1.ActualWidth;
                var lastHeight = last.Item1.ActualHeight;
                //将新的放在上一个的正下方
                var lastBottomMiddlePoint = last.Item1.TranslatePoint(new Point(0, lastHeight), this);
                var newX = lastBottomMiddlePoint.X;
                if (newX <= 0)
                {
                    newX = MoveThumb.MinLeft;
                }
                var left = newX;

                var newY = lastBottomMiddlePoint.Y + 30;
                var top = newY;
                if (!dragObject.IsDoubleClickAdd)
                {

                    Point point = default;
                    if (e != null)
                        point = e.GetPosition(this);
                    left = point.X;
                    top = point.Y;
                }

                left = left < MoveThumb.MinLeft ? MoveThumb.MinLeft : left;
                top = top < MoveThumb.MinTop ? MoveThumb.MinTop : top;

                if (last.Item2.Direction == Direction.Bottom)
                {
                    top += 35;
                }
                if (last.Item2.Direction == Direction.Right)
                {
                    left += 15;
                }

                Canvas.SetLeft(block, left);
                Canvas.SetTop(block, top);

                //创建连接线
                var sourceThumb = last.Item2;//last.ConnectorThumbs[Direction.Bottom];
                var sinkThumb = block.ConnectorThumbs[Direction.Top];
                var bottomConnector = block.ConnectorThumbs[Direction.Bottom];
                if (sinkThumb.Visibility == Visibility.Visible
                    && sourceThumb.Visibility == Visibility.Visible)
                {
                    last.Item1.ConnectorVisibility
                        = block.ConnectorVisibility
                        = Visibility.Visible;

                    sinkThumb.SourceBlock = block;

                    sourceThumb.SourceBlock = last.Item1;
                    //放上去后,检查当前放置的连接点有没有占用情况,如果有,则将其挤开,没有,则正常连接
                    //获取当前的放置到的块的目标块
                    var lastSinkBlock = last.Item1.SinkItems.FirstOrDefault();
                    //如果目标块存在
                    if (lastSinkBlock != null && last.Item3)
                    {
                        //找出连接线
                        var oldConnection = this.Children.OfType<Connection>().FirstOrDefault(c => c.SourceBlock == last.Item1 && c.SinkBlock == lastSinkBlock);
                        //查看其源连接点是否与当前连接点一致
                        if (oldConnection != null && last.Item2 == oldConnection.SourceThumb)
                        {
                            //如果一致,则将其挤到后面
                            //去除旧有连接线
                            this.RemoveConnection(oldConnection);

                            //先添加新的连接线,此线为新放上的块与要放上的块的连接线
                            connection = new Connection(sourceThumb, sinkThumb);
                            this.Children.Add(connection);
                            //设置目标点
                            last.Item1.SinkItems.Add(block);
                            // block.SourceItems.Add(last.Item1);
                            block.AddConnection(connection);

                            //添加旧目标点与当前块的连接线
                            //获取旧的目标点连接点
                            var oldSinkThumb = oldConnection.SinkThumb;

                            //获取当前块的对应连接点
                            var newSourceThumb = block.ConnectorThumbs[last.Item2.Direction];
                            newSourceThumb.SourceBlock = block;
                            //添加
                            connection = new Connection(newSourceThumb, oldSinkThumb);
                            this.Children.Add(connection);
                            //设置目标点
                            block.SinkItems.Add(oldSinkThumb.SourceBlock);
                            //oldSinkThumb.SourceBlock.SourceItems.Add(block);
                            //this.SetLine(connection);
                        }
                        else
                        {
                            //不存在,则直接添加
                            connection = new Connection(sourceThumb, sinkThumb);
                            this.Children.Add(connection);
                            //设置目标点
                            last.Item1.SinkItems.Add(block);
                            // block.SourceItems.Add(last.Item1);
                        }

                    }
                    else
                    {
                        //不存在,则直接添加
                        connection = new Connection(sourceThumb, sinkThumb);
                        this.Children.Add(connection);
                        //设置目标点
                        last.Item1.SinkItems.Add(block);
                        //block.SourceItems.Add(last.Item1);
                    }


                }
            }

            if (this.CopyEvent != null)
            {
                var copyValue = new BlockCopyArgs() { DragItem = dragObject,DestBlock=block };
                this.CopyEvent(this, copyValue);
                if (copyValue.Handle)
                {
                    return;
                }
            }
            block.OnDragToCanvas(connection);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddTempelte(BlockItem block)
        {
            var temptele = new ResourceDictionary { Source = new Uri("pack://application:,,,/GeneralTool.General;component/WPFHelper/DiagramDesigner/Resources/CommonTemplate.xaml", UriKind.RelativeOrAbsolute) };

            if (temptele != null)
            {
                var resizeTempelte = temptele["ResizeControl"] as ControlTemplate;
                if (block.Template.FindName("PART_Resize", block) is Control resize)
                {
                    resize.Template = resizeTempelte;
                    resize.ApplyTemplate();
                }


                var connectorTempelte = temptele["RectConnectorControlTemplete"] as ControlTemplate;
                if (block.Template.FindName("PART_Connectors", block) is Control connector)
                {
                    connector.Template = connectorTempelte;
                    connector.ApplyTemplate();
                }

            }

        }


        /// <summary>
        /// 
        /// </summary>
        public void SetLine(Connection connection)
        {
            #region 以Bottom为下标

            var sourceThumb = connection.SourceThumb;
            if (sourceThumb.Direction == Direction.Bottom)
            {
                connection.Stroke = Brushes.Green;
            }
            else
                connection.Stroke = Brushes.DarkGoldenrod;


            #endregion
        }


        private Tuple<BlockItem, ConnectorThumb, bool> HitSourceBlock(DragEventArgs e)
        {
            BlockItem item = default;
            var re = false;
            if (e == null)
            {
                var block = this.GetBlockLastOrDefault();
                return new Tuple<BlockItem, ConnectorThumb, bool>(block, block.ConnectorThumbs[Direction.Bottom], re);
            }
            var point = e.GetPosition(this);
            var obj = this.InputHitTest(point);
            var fram = obj as FrameworkElement;
            ConnectorThumb thumb = null;
            while (fram != null)
            {
                if (fram is ConnectorThumb t)
                {
                    item = t.SourceBlock;
                    thumb = t;
                    re = true;
                    break;
                }
                else if (fram is BlockItem b)
                {
                    item = b;
                    thumb = b.ConnectorThumbs[Direction.Bottom];
                    re = false;
                    break;
                }

                fram = fram.TemplatedParent as FrameworkElement;
            }

            if (item == null)
            {
                item = this.GetBlockLastOrDefault();
                // item = this.Children.OfType<BlockItem>().LastOrDefault();
                thumb = item.ConnectorThumbs[Direction.Bottom];
                re = false;
            }
            return new Tuple<BlockItem, ConnectorThumb, bool>(item, thumb, re);
        }

        private BlockItem GetBlockLastOrDefault()
        {
            var blocks = this.Children.OfType<BlockItem>();
            var first = blocks.FirstOrDefault(s => s.ConnectorThumbs[Direction.Bottom].Visibility == Visibility.Visible && s.ConnectorThumbs[Direction.Bottom].SinkBlock == null);

            if (first != null) return first;
            //直接先获取最后一个
            var last = blocks.ElementAt(blocks.Count() - 2);
            if (last != null)
            {
                if (last.SinkItems.Count == 0)
                {
                    //如果最后一个的下一个不为空的话,则直接返回,避免下面的死循环
                    return last;
                }
            }


            var count = blocks.Count();
            var start = blocks.FirstOrDefault(f => f.IsStart);
            do
            {
                //查看是否已有
                if (count == 0)
                {
                    start = last;
                    break;
                }
                if (start == null)
                {
                    break;
                }

                var tmp = start.SinkItems.FirstOrDefault();
                if (tmp == null)
                {
                    break;
                }

                count--;
                start = tmp;
            } while (true);


            return start;
        }



        #endregion

        #region 事件

        /// <summary>
        /// 
        /// </summary>
        public event Action<double> ScaleChanged;

        #endregion

        #region 公共方法



        /// <summary>
        /// 添加块
        /// </summary>
        public void AddItem(BlockItem item, bool isDrop)
        {
            this.Children.Add(item);

            item.IsInCanvas = true;
            if (!isDrop)
                item.OnAddItem();

        }

        /// <summary>
        /// 清除所有块
        /// </summary>
        public void ClearChildren()
        {
            var removeList = new List<BlockItem>();
            foreach (var item in this.Children)
            {
                if (item is BlockItem b)
                {
                    removeList.Add(b);
                }
            }
            foreach (var item in removeList)
            {
                this.RemoveItem(item);
                item.IsInCanvas = false;
                item.ContentVisibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetTop(UIElement element)
        {

            foreach (UIElement item in this.Children)
            {

                if (item is BlockItem || item.Equals(element))
                {
                    Canvas.SetZIndex(item, 91);

                }
                else
                {
                    Canvas.SetZIndex(item, 1);

                }

            }
        }


        /// <summary>
        /// 移除块
        /// </summary>
        /// <param name="blockItem"></param>
        public void RemoveItem(BlockItem blockItem)
        {
            if (blockItem.IsStart)
                return;
            if (blockItem != null)
            {
                this.Removes.Add(blockItem);
                var removeList = new List<Connection>();
                //移除其上的线条,作为源的,作为目标的
                foreach (var item in this.Children)
                {
                    if (item is Connection c)
                    {
                        if (c.SourceBlock == blockItem)
                        {
                            removeList.Add(c);
                        }
                        else if (c.SinkBlock == blockItem)
                        {
                            removeList.Add(c);
                        }
                    }

                }

                foreach (var item in removeList)
                {
                    item.Dispose();
                    this.RemoveConnection(item);
                }
                blockItem.Delete();
                this.Children.Remove(blockItem);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void DragArrary()
        {
            if (this.DragEndArrary)
            {
                this.Arrary();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Arrary()
        {
            this.Array(this.Children.OfType<BlockItem>());
        }

        private double minLeft = 30;
        private readonly double minTop = 30;
        private const double MARGIN = 40;
        /// <summary>
        /// 
        /// </summary>
        public void Array(IEnumerable<BlockItem> blockItems)
        {
            this.minLeft = 30;
            var list = blockItems.ToList();
            while (true)
            {
                // 返回所有放空的块(就是没有顶点连接块)
                var result = this.LayoutBlocks(list);
                list = result.Item1;
                if (list.Count == 0)
                    break;
                this.minLeft = result.Item2;
            }

            this.InvalidateMeasure();
            this.InvalidateVisual();
        }

        private Tuple<List<BlockItem>, double> LayoutBlocks(List<BlockItem> blockItems)
        {

            var caches = new List<BlockItem>(blockItems);
            //找出第一列
            var colBlocks = new List<BlockItem>();
            var cur = blockItems.FirstOrDefault(s => s.IsStart) ?? blockItems.FirstOrDefault();

            //找出最下边的块,第一列已排序完成
            var nextResult = this.GetNextItems(cur, caches, minLeft, minTop);
            var nextBlocks = nextResult.Item1;
            var rMaxRight = nextResult.Item2;
            if (nextBlocks.Count != 0)
            {

                var right = nextResult.Item2;

                while (true)
                {
                    var startTop = minTop;//开始的高度,一轮完成后
                    if (nextBlocks.Count == 0) break;
                    //循环其块,找出所有的右节点
                    var blocks = new List<BlockItem>();
                    var curMaxRight = right;//当前列的最大右边距
                    foreach (var block in nextBlocks)
                    {
                        var rightNext = block.ConnectorThumbs[Direction.Right].SinkBlock;
                        if (rightNext != null && caches.Contains(rightNext))
                        {
                            caches.Remove(rightNext);
                            blocks.Add(rightNext);
                            ////设置坐标
                            //Canvas.SetLeft(rightNext, right);
                            //Canvas.SetTop(rightNext, startTop);
                            //设置其下方的

                            var result = this.GetNextItems(rightNext, caches, right, startTop);
                            if (result.Item1.Count > 0)
                            {
                                result.Item1.RemoveAt(0);//把第一个去掉,会将当前项又加入
                            }
                            startTop = startTop + rightNext.ActualHeight + MARGIN;
                            //再次设置Top
                            if (result.Item3 > startTop)
                                startTop = result.Item3;

                            var nextRight = right + rightNext.ActualWidth + MARGIN;
                            if (nextRight > curMaxRight)
                                curMaxRight = nextRight;

                            blocks.AddRange(result.Item1);
                            if (curMaxRight < result.Item2)
                                curMaxRight = result.Item2;
                        }
                    }
                    nextBlocks = blocks;
                    right = curMaxRight;
                    rMaxRight = right;
                }

            }

            return new Tuple<List<BlockItem>, double>(caches, rMaxRight);
        }

        /// <summary>
        /// 根据当前的块,找出其下节点的所有块,并设置其位置
        /// </summary>
        /// <returns>返回找到的块,右边距,上边距</returns>
        private Tuple<List<BlockItem>, double, double> GetNextItems(BlockItem cur, List<BlockItem> caches, double left, double top)
        {
            var nextColBlocks = new List<BlockItem>();
            var right = left;
            while (true)
            {
                if (cur == null)
                    break;
                nextColBlocks.Add(cur);
                caches.Remove(cur);

                Canvas.SetLeft(cur, left);
                Canvas.SetTop(cur, top);

                var curLeft = Canvas.GetLeft(cur);
                var curRight = curLeft + cur.ActualWidth + MARGIN;//当前块的宽度
                if (right < curRight)
                    right = curRight;//更新最大宽度

                top = top + cur.ActualHeight + MARGIN;

                var next = cur.ConnectorThumbs[Direction.Bottom].SinkBlock;
                if (next != null && (!nextColBlocks.Contains(next) && caches.Contains(next)))
                {
                    nextColBlocks.Add(next);
                    caches.Remove(next);
                    //设置当前的右边距
                    if (next.ActualWidth < cur.ActualWidth)
                    {
                        left = cur.ActualWidth + left - next.ActualWidth;
                    }
                    Canvas.SetLeft(next, left);
                    Canvas.SetTop(next, top);
                    top += next.ActualHeight + MARGIN;//设置下一个块的上边距

                    curRight = left + next.ActualWidth + MARGIN;//当前块的宽度
                    if (right < curRight)
                        right = curRight;//更新最大宽度

                    next = next.ConnectorThumbs[Direction.Bottom].SinkBlock;
                    if (!caches.Contains(next)) break;
                    cur = next;
                }
                else
                    break;
            }
            return new Tuple<List<BlockItem>, double, double>(nextColBlocks, right, top);
        }

        ///// <summary>
        ///// 根据当前的块,找出其右节点的所有块
        ///// </summary>
        ///// <param name="curItem"></param>
        ///// <param name="left"></param>
        ///// <param name="top"></param>
        ///// <returns></returns>
        //private List<BlockItem> GetRightItems(BlockItem curItem, double left, double top)
        //{

        //}



        #endregion

        #region 重写

        /// <summary>
        /// 
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {

            Size size = new Size();

            foreach (UIElement element in this.InternalChildren)
            {

                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 10 : left;
                top = double.IsNaN(top) ? 10 : top;

                //measure desired size for each child
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 50;
            size.Height += 50;

            //size.Width = size.Width > this.ActualWidth ? size.Width : this.ActualWidth;
            //size.Height = size.Height > this.ActualHeight ? size.Height : this.ActualHeight;

            return size;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //Trace.WriteLine("ArrangeOverride -> " + arrangeSize);
            return base.ArrangeOverride(arrangeSize);
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            var dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
            AddDragObject(dragObject, e);

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewDrop(DragEventArgs e)
        {
            base.OnPreviewDrop(e);
        }

        #endregion
    }
}
