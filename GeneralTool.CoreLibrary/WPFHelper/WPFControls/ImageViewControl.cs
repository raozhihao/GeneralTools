using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GeneralTool.CoreLibrary.Enums;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls
{
    /// <summary>
    /// 图片绘制类型
    /// </summary>
    [Flags]
    public enum ImageDrawType
    {
        /// <summary>
        /// 圆
        /// </summary>
        Ellipse,

        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle,
        /// <summary>
        /// 
        /// </summary>
        Line
    }

    /// <summary>
    /// 工具箱按钮集合
    /// </summary>
    public class ContextItemCollection : FreezableCollection<UIElement>
    {
        #region Protected 方法

        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore() => new ToolButtonCollection();

        /// <inheritdoc/>
        protected override bool FreezeCore(bool isChecking) => !isChecking;

        #endregion Protected 方法
    }

    /// <summary>
    /// 展示图片,可放大缩小,移动,选取(暂时不支持在选取状态下缩放,以及最控件尺寸发生变更时自适应)
    /// </summary>
    [StyleTypedProperty(Property = nameof(ToolExpanderStyle), StyleTargetType = typeof(Expander))]
    [StyleTypedProperty(Property = nameof(SliderStyle), StyleTargetType = typeof(Slider))]
    [StyleTypedProperty(Property = nameof(ToolCutButtonStyle), StyleTargetType = typeof(ToggleButton))]
    [StyleTypedProperty(Property = nameof(MenuOkStyle), StyleTargetType = typeof(Button))]
    [StyleTypedProperty(Property = nameof(MenuCancelStyle), StyleTargetType = typeof(Button))]
    public partial class ImageViewControl : Control
    {

        #region Public 方法

        /// <summary>
        /// 获取工具按钮
        /// </summary>
        /// <param name="control">
        /// </param>
        /// <returns>
        /// </returns>
        public static ContextItemCollection GetContextMenus(ImageViewControl control)
        {
            ContextItemCollection c = new ContextItemCollection();
            if (control.Img.ContextMenu == null)
            {
                return c;
            }
            foreach (MenuItem item in control.Img.ContextMenu.Items)
            {
                c.Add(item);
            }
            return c;
        }

        /// <summary>
        /// 获取工具按钮
        /// </summary>
        /// <param name="control">
        /// </param>
        /// <returns>
        /// </returns>
        public static ToolButtonCollection GetToolButtons(ImageViewControl control)
        {
            ToolButtonCollection c = new ToolButtonCollection();
            foreach (ButtonBase item in control.stackTools.Children)
            {
                c.Add(item);
            }
            return c;
        }

        /// <summary>
        /// 设置工具按钮
        /// </summary>
        /// <param name="control">
        /// </param>
        /// <param name="items">
        /// </param>
        public static void SetContextMenus(ImageViewControl control, ContextMenu items) => control.SetToolMenus(items);

        /// <summary>
        /// 设置工具按钮
        /// </summary>
        /// <param name="control">
        /// </param>
        /// <param name="buttons">
        /// </param>
        public static void SetToolButtons(ImageViewControl control, ToolButtonCollection buttons) => control.SetToolButtons(buttons);

        /// <summary>
        /// 停止截取框的操作
        /// </summary>
        public void EndCutRect() => CutPanel.Visibility = Visibility.Collapsed;

        /// <summary>
        /// 清除所有指定类型的图形
        /// </summary>
        /// <param name="drawType">
        /// </param>
        public void ClearAll(ImageDrawType drawType = ImageDrawType.Ellipse | ImageDrawType.Rectangle | ImageDrawType.Line)
        {
            List<UIElement> list = new List<UIElement>();
            foreach (UIElement item in ImageCanvas.Children)
            {
                if (item is Ellipse)
                {
                    if (drawType.HasFlag(ImageDrawType.Ellipse))
                        list.Add(item);
                }

                if (item is Rectangle rect)
                {
                    if (drawType.HasFlag(ImageDrawType.Rectangle))
                        list.Add(rect);
                }

                if (item is Line)
                    if (drawType.HasFlag(ImageDrawType.Line))
                        list.Add(item);
            }

            foreach (UIElement item in list)
            {
                ImageCanvas.Children.Remove(item);
            }
            list.Clear();
        }

        /// <summary>
        /// 清除点
        /// </summary>
        /// <param name="pixelPoint">
        /// </param>
        public void ClearPoint(Point pixelPoint)
        {
            UIElement el = null;
            foreach (UIElement item in ImageCanvas.Children)
            {
                if (item is Ellipse ellipse)
                {
                    if (ellipse.Tag == null)
                    {
                        continue;
                    }
                    if (pixelPoint == (Point)ellipse.Tag)
                    {
                        el = ellipse;
                        break;
                    }
                }
            }

            if (el != null)
            {
                ImageCanvas.Children.Remove(el);
            }
        }

        /// <summary>
        /// 清除指定的矩形框
        /// </summary>
        /// <param name="rect">
        /// </param>
        public void ClearRect(Int32Rect rect)
        {
            List<UIElement> list = new List<UIElement>();
            foreach (UIElement item in ImageCanvas.Children)
            {
                if (item is Rectangle r)
                {
                    if ((Int32Rect)r.Tag == rect)
                        list.Add(r);
                }
            }

            foreach (UIElement item in list)
            {
                ImageCanvas.Children.Remove(item);
            }
            list.Clear();
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="drawRect">
        /// 绘制区域
        /// </param>
        /// <param name="stroke">
        /// 线条画刷
        /// </param>
        /// <param name="fill">
        /// 填充画刷
        /// </param>
        /// <param name="thickness">
        /// 线条宽度
        /// </param>
        public Rectangle DrawRect(Int32Rect drawRect, Brush stroke, Brush fill, double thickness = 1)
        {
            if (!CanImageDraw) return null;
            Point leftPoint = new Point(drawRect.X, drawRect.Y);
            Point pos = TranslateToCanvasPoint(leftPoint);
            Point pixelRightTop = new Point(drawRect.Width + leftPoint.X, leftPoint.Y);
            Point rightTop = TranslateToCanvasPoint(pixelRightTop);

            Point pixelLeftBottom = new Point(leftPoint.X, drawRect.Height + leftPoint.Y);
            Point leftBoom = TranslateToCanvasPoint(pixelLeftBottom);

            double width = rightTop.X - pos.X;
            double height = leftBoom.Y - pos.Y;

            Rectangle rect = new Rectangle
            {
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = thickness / Slider.Value,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = width,
                Height = height,
                Visibility = Visibility.Visible
            };
            _ = ImageCanvas.Children.Add(rect);

            if (double.IsInfinity(pos.X) || double.IsInfinity(pos.Y))
            {
                return null;
            }
            rect.Tag = drawRect;

            Canvas.SetLeft(rect, pos.X);
            Canvas.SetTop(rect, pos.Y);
            return rect;
        }

        /// <summary>
        /// 绘制线
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="stroke"></param>
        /// <param name="fill"></param>
        /// <param name="thickness"></param>
        public Line DrawLine(Point p1, Point p2, Brush stroke, Brush fill, double thickness = 1)
        {
            if (!CanImageDraw) return null;

            p1 = TranslateToCanvasPoint(p1);
            p2 = TranslateToCanvasPoint(p2);
            Line line = new Line()
            {
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = thickness,
                X1 = p1.X,
                X2 = p2.X,
                Y1 = p1.Y,
                Y2 = p2.Y
            };

            //Canvas.SetLeft(line, p1.X);
            //Canvas.SetTop(line, p1.Y);
            _ = ImageCanvas.Children.Add(line);
            return line;
        }

        private readonly List<Path> addUielements = new List<Path>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public void RemoveElement(Path element)
        {
            ImageCanvas.Children.Remove(element);
            _ = addUielements.Remove(element);
        }

        /// <summary>
        /// 删除自定义图形
        /// </summary>
        /// <param name="shape"></param>
        public void RemoveCustomeShape(BaseShape shape)
        {
            RemoveElement(shape.Path);
            _ = CustomeShapes.Remove(shape);
            shape.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public void AddElement(Path element)
        {
            _ = ImageCanvas.Children.Add(element);
            addUielements.Add(element);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearElements()
        {
            foreach (Path item in addUielements)
            {
                ImageCanvas.Children.Remove(item);
            }
            addUielements.Clear();
        }

        /// <summary>
        /// 获取到当前的截取范围
        /// </summary>
        /// <returns>
        /// </returns>
        public Int32Rect GetChooseRect()
        {
            Point recStartPoint = CutRectangle.TranslatePoint(new Point(), Img);

            double x = recStartPoint.X * w;
            double y = recStartPoint.Y * h;

            //将其取整
            double width = Math.Ceiling(CutRectangle.Width * w);
            double height = Math.Ceiling(CutRectangle.Height * h);
            //var width = Math.Ceiling(this.CutRectangle.Width);
            //var height = Math.Ceiling(this.CutRectangle.Height);

            Int32Rect rect = new Int32Rect((int)x, (int)y, (int)width, (int)height);
            return rect;
        }

        /// <summary>
        /// 保存指定范围内的图片
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="path"></param>
        /// <param name="encoder"></param>
        public void SaveCutRectBitmap(Int32Rect rect, string path, BitmapEncoderEnum encoder)
        {
            _ = ImageSource.SaveBitmapSouce(rect, path, encoder);
        }

        /// <summary>
        /// 获取当前截取到的范围内的图片
        /// </summary>
        /// <returns>
        /// </returns>
        public BitmapSource GetChooseRectImageSouce()
        {
            Int32Rect rect = GetChooseRect();
            BitmapSource source = Img.Source as BitmapSource;

            int stride = source.Format.BitsPerPixel * rect.Width / 8;
            byte[] data = new byte[rect.Height * stride];
            source.CopyPixels(rect, data, stride, 0);
            BitmapSource newSource = BitmapSource.Create(rect.Width, rect.Height, source.DpiX, source.DpiY, source.Format, source.Palette, data, stride);
            return newSource;
        }

        /// <summary>
        /// 相当于Load
        /// </summary>
        public override void OnApplyTemplate()
        {
            stackTools = GetTemplateChild(nameof(stackTools)) as StackPanel;
            CutRectButton = GetTemplateChild(nameof(CutRectButton)) as ToggleButton;
            CutRectButton.Click += CutArrClick;

            ImageBox = GetTemplateChild(nameof(ImageBox)) as Grid;
            GridBox = GetTemplateChild(nameof(GridBox)) as Grid;
            ImageScroll = GetTemplateChild(nameof(ImageScroll)) as ScrollViewer;

            ImageCanvas = GetTemplateChild(nameof(ImageCanvas)) as Canvas;
            ImageCanvas.SizeChanged += ImageCanvas_SizeChanged;
            ImageCanvas.MouseWheel += Control_MouseWheel;
            ImageCanvas.MouseDown += Canvas_MouseDown;
            ImageCanvas.MouseUp += Canvas_MouseUp;
            ImageCanvas.MouseLeave += Canvas_MouseLeave;
            ImageCanvas.MouseMove += Canvas_MouseMove;

            CutPanel = GetTemplateChild(nameof(CutPanel)) as StackPanel;
            cutTrans = GetTemplateChild(nameof(cutTrans)) as TranslateTransform;
            CutRectangle = GetTemplateChild(nameof(CutRectangle)) as Rectangle;
            CutRectangle.MouseDown += CutRectangle_MouseDown;

            StackMenu = GetTemplateChild(nameof(StackMenu)) as StackPanel;
            MenuOk = GetTemplateChild(nameof(MenuOk)) as Button;
            MenuOk.Click += CutRectButton_Click;

            MenuCancel = GetTemplateChild(nameof(MenuCancel)) as Button;
            MenuCancel.Click += CutTempCancelClick;

            Img = GetTemplateChild(nameof(Img)) as Image;
            Img.MouseDown += Img_MouseDown;
            Img.PreviewMouseMove += Img_MouseMove;
            Img.MouseUp += Img_MouseUp;
            Img.SizeChanged += Img_SizeChanged;

            ImageSource = writeable;
            ImageViewBox = GetTemplateChild(nameof(ImageViewBox)) as Viewbox;

            Grid contentGrid = GetTemplateChild("ContentGrid") as Grid;
            contentGrid.MouseUp += ContentGrid_MouseUp;

            Slider = GetTemplateChild(nameof(Slider)) as Slider;

            ToolExpander = GetTemplateChild(nameof(ToolExpander)) as Expander;

            group = (TransformGroup)ImageCanvas.RenderTransform;
            scaleTrans = group.Children[0] as ScaleTransform;

            //创建绑定,在XAML中绑定会提示错误,虽然不会报错
            Binding aBinding = new Binding
            {
                ElementName = nameof(Slider), // name of the slider
                Path = new PropertyPath("Value")
            };

            _ = BindingOperations.SetBinding(
                scaleTrans, // the ScaleTransform to bind to
                ScaleTransform.ScaleXProperty,
                aBinding);

            _ = BindingOperations.SetBinding(
                scaleTrans, // the ScaleTransform to bind to
                ScaleTransform.ScaleYProperty,
                aBinding);

            tt = group.Children[3] as TranslateTransform;

            SetButtons();

            InitStyles();
            if (ImageSource != null && CutPanelMaxSize.IsEmpty)
            {
                CutPanelMaxSize = new Size(ImageSource.Width, ImageSource.Height);
            }
            CanMoveImage = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> CanvasSizeChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> ImageSizeChangedEvent;
        private void Img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ImageSource == null) return;

            ImageSizeChangedEvent?.Invoke(sender, e);

            //更改了尺寸后,为了不变截取的框的位置,要重新调整
            if (CutPanel.Visibility == Visibility.Collapsed) return;
        }

        /// <summary>
        /// 重置ImageSource的各项值
        /// </summary>
        public void ResertImageSource()
        {
            cutTrans.X = cutTrans.Y = 0;
            Slider.Value = 1;
            tt.X = tt.Y = 0;

            //重置了Img后重新计算比例
            PixelTrans pos = GetPixelTrans;
            System.Diagnostics.Trace.WriteLine(pos);
        }

        /// <summary>
        /// 通知开始执行绘制文本框截取操作
        /// </summary>
        public void SendMouseCutRectStart()
        {
            if (!CanImageDraw) return;
            CutRectButton.IsChecked = true;
            CutArrClick(CutRectButton, null);
        }

        /// <summary>
        /// 向画布设置点
        /// </summary>
        /// <param name="pixelPoint">
        /// </param>
        /// <param name="brush">
        /// </param>
        /// <param name="r">
        /// </param>
        public Ellipse SetPoint(Point pixelPoint, Brush brush, double r = 4)
        {
            if (!CanImageDraw) return null;
            Ellipse myEllipse = new Ellipse
            {
                Stroke = brush,
                Fill = brush,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = r,
                Height = r
            };
            _ = ImageCanvas.Children.Add(myEllipse);
            Point pos = TranslateToCanvasPoint(pixelPoint);
            if (double.IsInfinity(pos.X) || double.IsInfinity(pos.Y))
            {
                return null;
            }
            myEllipse.Tag = pixelPoint;
            Canvas.SetLeft(myEllipse, pos.X - r / 2);
            Canvas.SetTop(myEllipse, pos.Y - r / 2);
            return myEllipse;
        }

        /// <summary>
        /// 添加自定义图形
        /// </summary>
        /// <param name="shape"></param>
        public void AddCustomeShape(BaseShape shape)
        {
            shape.StrokeThickness = shape.Path.StrokeThickness;
            shape.Init(this);
            shape.CreateShape();
            AddElement(shape.Path);
            CustomeShapes.Add(shape);

            shape.UpdateScaleSize(ImageScale);
        }

        private void ImageCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasSizeChanged?.Invoke(sender, e);
        }

        #endregion Public 方法

        #region Internal 方法

        /// <summary>
        /// 设置按钮
        /// </summary>
        internal void SetButtons()
        {
            if (toolButtons != null)
            {
                foreach (UIElement item in toolButtons)
                {
                    _ = stackTools.Children.Add(item);
                }
            }

            Img.ContextMenu = toolMenus;
        }

        internal void SetToolButtons(IEnumerable<UIElement> elements) => toolButtons = elements;

        internal void SetToolMenus(ContextMenu elements) => toolMenus = elements;

        #endregion Internal 方法

        #region Private 方法

        private static void ImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void MenuItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ContextMenu list = e.NewValue as ContextMenu;
                ImageViewControl c = d as ImageViewControl;
                SetContextMenus(c, list);
            }
        }

        private static void ToolButtonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ToolButtonCollection list = e.NewValue as ToolButtonCollection;
                ImageViewControl c = d as ImageViewControl;
                SetToolButtons(c, list);
            }
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(Img);
            bool re = point.X < 0 || point.Y < 0;

            if (e.LeftButton == MouseButtonState.Pressed && !re)
            {
                ////记录鼠标按下时的坐标
                //this.StartPoint = e.GetPosition(ImageCanvas);
                //清除截取框的移动轨迹,否则下一次截图将会受其影响
                if (CutPanel.Visibility == Visibility.Collapsed)
                {
                    cutTrans.X = cutTrans.Y = 0;
                }
            }
            else
            {
                //重置StartPoint
            }

            isDrag = true;
            curretnMouseDownPoint = e.GetPosition(ImageViewBox);
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            isDrag = false;
            Canvas_MouseUp(sender, new MouseButtonEventArgs(e.MouseDevice, 0, MouseButton.Left));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //获取当前鼠标坐标
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    ResizeAndDragCutRectangle(e);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine($"重新设置截取框大小出错:{ex.GetInnerExceptionMessage()}");
                }
                if (CutPanel.Visibility == Visibility.Visible)
                    CutPanelVisibleChanged?.Invoke(sender, new ImageCutRectEventArgs(GetChooseRect()));
            }
            else
            {
                ShowSizeInCutRectangleCursors();
            }

            if (e.LeftButton == MouseButtonState.Pressed && cuting && IsImageMouseDown && CanImageDraw)
            {
                DrawRectangle(e);
            }
            else
            {
                if (isDrag && CanMoveImage)
                {
                    DragCanvas(e);
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (cuting && IsImageMouseDown)
            {
                Int32Rect rect = GetChooseRect();
                if (rect.Width > 0 && rect.Height > 0)
                {
                    StackMenu.Visibility = Visibility.Visible;
                    CutRectangle.Fill = new SolidColorBrush(Color.FromArgb(30, 2, 3, 4));
                    cuting = false;
                }
                else
                {
                    //this.CutTempCancelClick(null, null);
                }

            }

            //判断当前截图框的位置
            ResizeCutPanel();
            if (CutRectangle.Visibility == Visibility.Visible)
                CutPanelVisibleChanged?.Invoke(sender, new ImageCutRectEventArgs(GetChooseRect()));
            isDrag = false;
            Cursor = null;
            dragPoint = new Point();
            IsImageMouseDown = false;
        }

        private void ClearRectangle()
        {
            CutPanel.Visibility = Visibility.Collapsed;
            cuting = false;

            //清空所有画出来的框
            CutRectangle.Width = 0;
            CutRectangle.Height = 0;

            cutTrans.X = 0;
            cutTrans.Y = 0;
        }

        private void ContentGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Control_MouseWheel(object sender, MouseWheelEventArgs e) => WheelScale(e.GetPosition(ImageScroll), e.Delta * 1.0);

        private void CutArrClick(object sender, RoutedEventArgs e)
        {
            //截取
            if (sender is ToggleButton toggle)
            {
                if (!CanImageDraw)
                {
                    toggle.IsChecked = false;
                    return;
                }
                cuting = toggle.IsChecked.Value;
                CutPanel.Visibility = cuting ? Visibility.Visible : Visibility.Collapsed;
                CutRectangle.Visibility = cuting ? Visibility.Visible : Visibility.Collapsed;
                CutRectangle.Width = CutRectangle.Height = 0;
                StackMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void CutRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                CutTempCancelClick(null, null);
                return;
            }

            if (e.ClickCount >= 2 && DoubleClickRaiseImage)
            {
                CutRectButton_Click(MenuOk, null);
                return;
            }
            if (Cursor != null)
            {
                return;
            }
            Cursor = Cursors.Hand;
            dragPoint = e.GetPosition(Img);
        }

        private void CutRectButton_Click(object sender, RoutedEventArgs e)
        {
            Int32Rect rect = GetChooseRect();
            CutPanel.Visibility = Visibility.Collapsed;
            ImageCutRectEventArgs handler = new ImageCutRectEventArgs(rect, true);
            CutImageEvent?.Invoke(sender, handler);
            BitmapSource source = null;
            bool sucess = false;
            string msg = "";
            if (handler.HandleToNext)
            {
                try
                {
                    source = GetChooseRectImageSouce();
                    sucess = true;
                }
                catch (Exception ex)
                {
                    msg = ex.GetInnerExceptionMessage();
                }

            }

            CutRectButton.IsChecked = false;
            isDrag = false;
            ClearRectangle();
            if (handler.HandleToNext)
            {
                CutImageDownEvent?.Invoke(sender, new ImageEventArgs(source, sucess, msg) { Int32Rect = rect });
            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        public event EventHandler CutMenuCancelEvent;
        private void CutTempCancelClick(object sender, RoutedEventArgs e)
        {
            RaiseCancelCutImage();
            CutMenuCancelEvent?.Invoke(sender, e);
        }

        /// <summary>
        /// 取消截图操作
        /// </summary>
        public void RaiseCancelCutImage()
        {
            foreach (UIElement item in stackTools.Children)
            {
                if (item is ToggleButton button)
                {
                    button.IsChecked = false;
                }
            }

            isDrag = false;
            ClearRectangle();
        }

        /// <summary>
        /// 拖拽画布
        /// </summary>
        /// <param name="e">
        /// </param>
        private void DragCanvas(MouseEventArgs e)
        {
            if (CutPanel.Visibility == Visibility.Visible)
            {
                //处于截图状态则不让其可以平移(如果不限制,则用户在截取图片后再移动画布处理起来比较麻烦)
                return;
            }

            Point p = e.GetPosition(ImageViewBox);
            Point topPoint = ImageCanvas.TranslatePoint(new Point(0, 0), ImageViewBox);
            Point bottomPoint = ImageCanvas.TranslatePoint(new Point(ImageCanvas.ActualWidth, ImageCanvas.ActualHeight), ImageViewBox);

            double moveX = p.X - curretnMouseDownPoint.X;
            double moveY = p.Y - curretnMouseDownPoint.Y;

            //向上向下移动条件判断（会有一点点的小偏移，如果想更精确的控制，那么分向上和向下两种情况，并判断边距）
            if ((moveY < 0 && bottomPoint.Y > ImageViewBox.ActualHeight) || (moveY > 0 && topPoint.Y < 0))
            {
                tt.Y += p.Y - curretnMouseDownPoint.Y;
                curretnMouseDownPoint.Y = p.Y;
            }

            //向左向右移动条件判断
            if ((moveX < 0 && bottomPoint.X > ImageViewBox.ActualWidth) || (moveX > 0 && topPoint.X < 0))
            {
                tt.X += p.X - curretnMouseDownPoint.X;
                curretnMouseDownPoint.X = p.X;
            }
        }

        /// <summary>
        /// 绘制截取框
        /// </summary>
        /// <param name="e">
        /// </param>
        private void DrawRectangle(MouseEventArgs e)
        {
            if (ImageSource == null)
                return;
            //鼠标当前的point
            Point Endpoint = e.GetPosition(Img);

            //清空所有画出来的框
            CutRectangle.Width = CutRectangle.Height = 0;

            //转换成基于Image控件的坐标
            width = Endpoint.X - StartPoint.X - 1;
            height = Endpoint.Y - StartPoint.Y;

            //用来记录左上角坐标
            Point StartLeft = new Point();

            //如果从左往右画
            if (Endpoint.X > StartPoint.X)
            {
                width = Endpoint.X - StartPoint.X;
                StartLeft.X = StartPoint.X;

                //如果从上往下画
                if (Endpoint.Y > StartPoint.Y)
                {
                    height = Endpoint.Y - StartPoint.Y;
                    StartLeft.Y = StartPoint.Y;
                }
                else
                {
                    height = StartPoint.Y - Endpoint.Y;
                    StartLeft.Y = StartPoint.Y - height;
                }
            }
            else
            {
                width = StartPoint.X - Endpoint.X;
                StartLeft.X = Endpoint.X;
                if (Endpoint.Y > StartPoint.Y)
                {
                    height = Endpoint.Y - StartPoint.Y;
                    StartLeft.Y = Endpoint.Y - height;
                }
                else
                {
                    height = StartPoint.Y - Endpoint.Y;
                    StartLeft.Y = Endpoint.Y;
                }
            }

            double scale = 1 / Slider.Value;
            CutRectangle.StrokeThickness = scale;
            StackMenu.RenderTransform = new ScaleTransform(scale * 2, scale * 2);

            Size minSize = CutPanelMinSize;
            Size maxSize = CutPanelMaxSize;
            //转换完后的宽高
            width *= w;
            height *= h;

            if (minSize != Size.Empty)
            {
                width = width < minSize.Width ? minSize.Width : width;
                height = height < minSize.Height ? minSize.Height : height;
            }

            if (maxSize != Size.Empty)
            {
                width = width > maxSize.Width ? maxSize.Width : width;

                height = height > maxSize.Height ? maxSize.Height : height;
            }

            width /= w;
            height /= h;
            CutRectangle.Width = width;
            CutRectangle.Height = height;
            StartLeft = Img.TranslatePoint(StartLeft, ImageCanvas);
            Canvas.SetLeft(CutPanel, StartLeft.X);
            Canvas.SetTop(CutPanel, StartLeft.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Point GetCurrentPixelPoint(MouseEventArgs e)
        {
            BitmapSource ims = (BitmapSource)Img.Source;

            Point mousePoint = e.GetPosition(Img);
            CurrentMouseDownPoint = mousePoint;

            w = ims.PixelWidth / Img.ActualWidth;
            double pixelMousePositionX = mousePoint.X * w;

            h = ims.PixelHeight / Img.ActualHeight;

            double pixelMousePositionY = mousePoint.Y * h;

            CurrentMouseDownPixelPoint = new Point(pixelMousePositionX, pixelMousePositionY);
            return CurrentMouseDownPixelPoint;
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //记录鼠标按下时的坐标
            StartPoint = e.GetPosition(Img);
            CurrentMouseDownPixelPoint = GetCurrentPixelPoint(e);
            CurrentMouseDownPoint = StartPoint;
            IsImageMouseDown = true;
            ImageMouseDownEvent?.Invoke(this, e);
        }

        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = GetCurrentPixelPoint(e);
            Point canvasPoint = e.GetPosition(ImageCanvas);
            ImageMouseMoveEvent?.Invoke(new ImageMouseEventArgs(e.MouseDevice, e.Timestamp, e.StylusDevice, point, canvasPoint));

            // this.Canvas_MouseMove(this.Img, e); this.lbPosition.Text = point.X +
            // Environment.NewLine + point.Y;
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void InitStyles()
        {
            //获取样式表
            ResourceDictionary dic = new ResourceDictionary { Source = new Uri("pack://application:,,,/GeneralTool.CoreLibrary;component/Themes/BaseStyle.xaml", UriKind.Absolute) };

            Style styleToolCutButton = (Style)dic["CutButtonIconFontStyle"];

            //初始化截图按钮
            if (ToolCutButtonStyle == null)
                ToolCutButtonStyle = styleToolCutButton;

            //初始化Header
            if (ToolExpanderStyle == null)
            {
                ToolExpander.Header = "工具";
            }

            Style styleMenuOk = (Style)dic["MenuOkStyle"];

            //查看OK按钮有无内容
            if (MenuOkStyle == null)
                MenuOkStyle = styleMenuOk;

            Style styleMenuCancel = (Style)dic["MenuCancelStyle"];

            //查看Cancel按钮有无内容
            if (MenuCancelStyle == null)
                MenuCancelStyle = styleMenuCancel;
        }

        /// <summary>
        /// 拖拽截取图形
        /// </summary>
        /// <param name="e">
        /// </param>
        private void ResizeAndDragCutRectangle(MouseEventArgs e)
        {
            //获取鼠标当前的点
            Point mousePoint = e.GetPosition(Img);

            //获取当前方框在canvas中的终点
            Point recEndPoint = CutRectangle.TranslatePoint(new Point(CutRectangle.Width, CutRectangle.Height), Img);
            //获取当前方框在canvas中的起点
            Point recStartPoint = CutRectangle.TranslatePoint(new Point(0, 0), Img);

            //判断当前鼠标开状
            if (Cursor == Cursors.SizeWE && drawType == DrawType.Right)//在右边拉
            {
                if (mousePoint.X < recEndPoint.X && CutRectangle.Width < 10 / Slider.Value)
                {
                    return;
                }

                double dis = mousePoint.X - recEndPoint.X;
                if (dis + CutRectangle.Width < 10 / Slider.Value)
                    return;

                //左右方拉取,重新赋值宽度
                CutRectangle.Width += dis;
            }
            else if (Cursor == Cursors.SizeWE && drawType == DrawType.Left)//在左边拉
            {
                if (mousePoint.X < recStartPoint.X && CutRectangle.Width < 10 / Slider.Value)
                {
                    return;
                }

                //在左边左右拉取,重新赋左上角坐标
                double dis = recStartPoint.X - mousePoint.X;
                if (dis + CutRectangle.Width < 10 / Slider.Value)
                    return;
                cutTrans.X += mousePoint.X - recStartPoint.X;
                //
                CutRectangle.Width += dis;
            }
            else if (Cursor == Cursors.SizeNS && drawType == DrawType.Bottom)//在下边拉
            {
                if (mousePoint.Y < recEndPoint.Y && CutRectangle.Height < 10 / Slider.Value)
                {
                    return;
                }

                //上下拉取,重新赋值高度
                double dis = mousePoint.Y - recEndPoint.Y;
                if (dis + CutRectangle.Height < 10 / Slider.Value)
                    return;
                CutRectangle.Height += dis;
            }
            else if (Cursor == Cursors.SizeNS && drawType == DrawType.Top)//在上边拉
            {
                if (mousePoint.Y < recEndPoint.Y && CutRectangle.Height < 10 / Slider.Value)
                    return;

                //在上方上下拉取,重新赋左上角坐标
                double dis = recStartPoint.Y - mousePoint.Y;
                if (dis + CutRectangle.Height < 10 / Slider.Value)
                    return;
                cutTrans.Y += mousePoint.Y - recStartPoint.Y; ;
                //
                CutRectangle.Height += dis;
            }
            else if (Cursor == Cursors.Hand)
            {
                //拖拽
                //获取方框起点坐标
                //计算拖动距离
                double disY = mousePoint.Y - dragPoint.Y;
                double disX = mousePoint.X - dragPoint.X;

                ////查看有无越界
                //var startPoint = this.CutRectangle.TranslatePoint(new Point(0, 0), this.Img);
                //if ((startPoint.X + disX + this.CutRectangle.Width) >= this.Img.ActualWidth)
                //    return;
                //if ((startPoint.Y + disY + this.CutRectangle.Height) >= this.Img.ActualHeight)
                //    return;
                //if (startPoint.Y + disY <= 0)
                //    return;
                //if (startPoint.X + disX <= 0)
                //    return;

                cutTrans.X += disX;
                cutTrans.Y += disY;
                dragPoint = mousePoint;
            }
        }

        /// <summary>
        /// 重置截图框位置
        /// </summary>
        private void ResizeCutPanel()
        {
            if (CutPanel.Visibility == Visibility.Collapsed || ImageSource == null)
                return;

            double width = CutRectangle.Width;
            double height = CutRectangle.Height;
            //查看宽度是否足够
            if (width > Img.ActualWidth)
            {
                //不够,则减成足够的宽度先
                width = Img.ActualWidth;
            }

            //查看高度是否足够
            if (height > Img.ActualHeight)
            {
                //不够,则减成足够的高度先
                height = Img.ActualHeight;
            }

            Size minSize = CutPanelMinSize;
            Size maxSize = CutPanelMaxSize;

            //能够接受的在图像上的宽高值
            double minWidth = minSize.Width / w;
            double minHeight = minSize.Height / h;
            double maxWidth = maxSize.Width / w;
            double maxHeight = maxSize.Height / h;

            //转换完后的宽高,当前的宽高只是控件本身的宽高,转为对应图片的像素宽高
            double wTmp = width * w;
            double hTmp = height * h;

            if (wTmp < minSize.Width)
            {
                //当前转换后的像素宽小于定义宽
                width = minWidth;
            }

            if (wTmp > maxSize.Width)
            {
                width = maxWidth;
            }

            if (hTmp < minSize.Height)
            {
                height = minHeight;
            }

            if (hTmp > maxSize.Height)
                height = maxHeight;

            if (double.IsInfinity(width) || double.IsNaN(width) || double.IsNaN(height) || double.IsInfinity(height)
                || double.IsPositiveInfinity(width) || double.IsPositiveInfinity(height) || double.IsNegativeInfinity(width) || double.IsNegativeInfinity(height))
                return;
            CutRectangle.Width = width;
            CutRectangle.Height = height;

            //查看当前框在图片中的左上角坐标
            Point topLeft = CutRectangle.TranslatePoint(new Point(), Img);

            //查看当前框在图片中的右下角坐标
            Point bottomRight = CutRectangle.TranslatePoint(new Point(CutRectangle.Width, CutRectangle.Height), Img);

            //X超限
            if (topLeft.X < 0)
            {
                //超出了左边区域,则让X贴Img左边
                cutTrans.X = 0;
                topLeft.X = 0;
                topLeft = Img.TranslatePoint(new Point(), ImageCanvas);

                //将左上角点转为Canvas中的坐标
                Canvas.SetLeft(CutPanel, topLeft.X);
            }

            if (bottomRight.X > Img.ActualWidth)
            {
                //超出了右边区域
                cutTrans.X = 0;
                topLeft.X -= bottomRight.X - Img.ActualWidth;
                topLeft = Img.TranslatePoint(topLeft, ImageCanvas);

                //将左上角点转为Canvas中的坐标
                Canvas.SetLeft(CutPanel, topLeft.X);
            }

            //Y超限
            if (topLeft.Y < 0)
            {
                //超出了上边区域
                cutTrans.Y = 0;
                topLeft.Y = 0;
                topLeft = Img.TranslatePoint(topLeft, ImageCanvas);

                //将左上角点转为Canvas中的坐标
                Canvas.SetTop(CutPanel, topLeft.Y);
            }

            if (bottomRight.Y > Img.ActualHeight)
            {
                //超出了下边区域
                cutTrans.Y = 0;
                topLeft.Y -= bottomRight.Y - Img.ActualHeight;
                topLeft = Img.TranslatePoint(topLeft, ImageCanvas);
                Canvas.SetTop(CutPanel, topLeft.Y);
            }
        }

        /// <summary>
        /// 鼠标在已截取的方框中移动时显示缩放鼠标图形
        /// </summary>
        private void ShowSizeInCutRectangleCursors()
        {
            if (CutRectangle.Width == 0)
            {
                return;
            }

            //确定当前鼠标点的位置

            Point mousePoint = Mouse.GetPosition(ImageCanvas);

            //获取方框在canvas中的右底终点
            Point recEndPoint = CutRectangle.TranslatePoint(new Point(CutRectangle.Width, CutRectangle.Height), ImageCanvas);
            //获取方框在canvas中的左上起点,也是左上顶点
            Point recStartPoint = CutRectangle.TranslatePoint(new Point(0, 0), ImageCanvas);

            //获取右上角顶点
            Point recRightTopPoint = CutRectangle.TranslatePoint(new Point(CutRectangle.Width, 0), ImageCanvas);

            //获取左下角顶点
            Point recLeftBottomPoint = CutRectangle.TranslatePoint(new Point(0, CutRectangle.Height), ImageCanvas);

            //判断
            //右距离,方框右边的x坐标与鼠标x坐标的距离
            double xDis = recEndPoint.X - mousePoint.X;
            //左距离,方框左边的x坐标与鼠标x坐标的距离
            double leftXDis = recStartPoint.X - mousePoint.X;
            //下距离,方框下边的y坐标与鼠标y坐标的距离
            double yDis = recEndPoint.Y - mousePoint.Y;
            //上距离,方框上边的y坐标与鼠标y坐标的距离
            double topYDis = recStartPoint.Y - mousePoint.Y;

            //判断鼠标是否在右边框
            bool mouseInRightLine = xDis > -1 / Slider.Value && xDis < 3 / Slider.Value && mousePoint.Y > (recRightTopPoint.Y + 1 / Slider.Value) && mousePoint.Y < recEndPoint.Y - 1 / Slider.Value;

            //判断鼠标是否在左边框
            bool mouseInLeftLine = leftXDis > -1 / Slider.Value && leftXDis < 3 / Slider.Value && mousePoint.Y > (recRightTopPoint.Y + 1 / Slider.Value) && mousePoint.Y < recEndPoint.Y - 1 / Slider.Value;

            //判断鼠标是否在下边框
            bool mouseInBottomLine = yDis > -1 / Slider.Value && yDis < 3 / Slider.Value && mousePoint.X > recLeftBottomPoint.X + 1 / Slider.Value && mousePoint.X < recEndPoint.X - 1 / Slider.Value;

            //判断鼠标是否在上边框
            bool mouseInTopLine = topYDis > -1 / Slider.Value && topYDis < 3 / Slider.Value && mousePoint.X > recStartPoint.X + 1 / Slider.Value && mousePoint.X < recEndPoint.X - 1 / Slider.Value;

            //右边的显示应该处于右顶点到右底点之间
            if (mouseInRightLine)
            {
                //处于右边
                Cursor = Cursors.SizeWE;
                drawType = DrawType.Right;
            }
            else if (mouseInLeftLine)
            {
                //处于左边
                Cursor = Cursors.SizeWE;
                drawType = DrawType.Left;
            }
            else if (mouseInBottomLine)
            {
                //处于下方
                Cursor = Cursors.SizeNS;
                drawType = DrawType.Bottom;
            }
            else if (mouseInTopLine)
            {
                //处于上方
                Cursor = Cursors.SizeNS;
                drawType = DrawType.Top;
            }
            else
            {
                Cursor = null;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                WheelScale(currentWheelPoint, e.NewValue > e.OldValue ? 120d : -120d, true);
            }
        }

        /// <summary>
        /// 将当前的像素点转为画布点
        /// </summary>
        /// <param name="pixelPoint"></param>
        /// <returns></returns>
        public Point TranslateToCanvasPoint(Point pixelPoint)
        {
            //转换为目标点
            PixelTrans wh = GetPixelTrans;
            double x = pixelPoint.X / wh.W;
            double y = pixelPoint.Y / wh.H;
            Point pos = Img.TranslatePoint(new Point(x, y), ImageCanvas);
            return pos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlPoint"></param>
        /// <returns></returns>
        public Point TranslateToPixelPoint(Point controlPoint)
        {
            //将相对于Canvas的点转为像素点
            Point pos = ImageCanvas.TranslatePoint(controlPoint, Img);
            PixelTrans wh = GetPixelTrans;
            double x = pos.X * wh.W;
            double y = pos.Y * wh.H;
            return new Point(x, y);
        }

        /// <summary>
        /// 开始滚动缩放
        /// </summary>
        /// <param name="point">
        /// 滚动缩放的中心点坐标
        /// </param>
        /// <param name="delta">
        /// 缩放大小
        /// </param>
        /// <param name="sliderChanged">
        /// 当前是否处于滚动条调节缩放
        /// </param>
        private void WheelScale(Point point, double delta, bool sliderChanged = false)
        {
            if (CutPanel.Visibility == Visibility.Visible)
            {
                return;
            }
            currentWheelPoint = point;// e.GetPosition(viewBox); // 实际点击的点
            Point actualPoint = group.Inverse.Transform(currentWheelPoint); // 想要缩放的点
            if (!sliderChanged)
            {
                Slider.ValueChanged -= Slider_ValueChanged;
                Slider.Value += delta / 500;
                Slider.ValueChanged += Slider_ValueChanged;
            }
            tt.X = -(actualPoint.X * (Slider.Value - 1)) + currentWheelPoint.X - actualPoint.X;
            tt.Y = -(actualPoint.Y * (Slider.Value - 1)) + currentWheelPoint.Y - actualPoint.Y;
            MouseWheelScaleEvent?.Invoke(Img, new ImageScaleEventArgs(Slider.Value));

            ImageScale = Slider.Value;
            //更新大小
            foreach (BaseShape item in CustomeShapes)
            {
                if (item.AutoScale)
                    item.UpdateScaleSize(ImageScale);
            }
        }

        #endregion Private 方法

        #region Private 结构

        private struct PixelTrans
        {
            #region Public 构造函数

            public PixelTrans(double w, double h)
            {
                W = w;
                H = h;
            }

            #endregion Public 构造函数

            #region Public 属性

            public double H { get; set; }

            public double W { get; set; }

            #endregion Public 属性

            #region Public 方法

            public override string ToString()
            {
                return $"{W} {H}";
            }

            #endregion Public 方法
        }

        #endregion Private 结构

    }

    /// <summary>
    /// 拖动绘制方向
    /// </summary>
    public enum DrawType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 左
        /// </summary>
        Left,
        /// <summary>
        /// 右
        /// </summary>
        Right,
        /// <summary>
        /// 上
        /// </summary>
        Top,
        /// <summary>
        /// 下
        /// </summary>
        Bottom
    }
    /// <summary>
    /// 工具箱按钮集合
    /// </summary>
    public class ToolButtonCollection : FreezableCollection<ButtonBase>
    {
        #region Protected 方法

        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore() => new ToolButtonCollection();

        /// <inheritdoc/>
        protected override bool FreezeCore(bool isChecking) => !isChecking;

        #endregion Protected 方法
    }
}