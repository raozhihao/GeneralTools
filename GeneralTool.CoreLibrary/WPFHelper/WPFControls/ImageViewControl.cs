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
            var c = new ContextItemCollection();
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
            var c = new ToolButtonCollection();
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
        public void EndCutRect() => this.CutPanel.Visibility = Visibility.Collapsed;

        /// <summary>
        /// 清除所有指定类型的图形
        /// </summary>
        /// <param name="drawType">
        /// </param>
        public void ClearAll(ImageDrawType drawType = ImageDrawType.Ellipse | ImageDrawType.Rectangle | ImageDrawType.Line)
        {
            List<UIElement> list = new List<UIElement>();
            foreach (UIElement item in this.ImageCanvas.Children)
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

            foreach (var item in list)
            {
                this.ImageCanvas.Children.Remove(item);
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
            foreach (UIElement item in this.ImageCanvas.Children)
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
                this.ImageCanvas.Children.Remove(el);
            }
        }

        /// <summary>
        /// 清除指定的矩形框
        /// </summary>
        /// <param name="rect">
        /// </param>
        public void ClearRect(Int32Rect rect)
        {
            var list = new List<UIElement>();
            foreach (UIElement item in this.ImageCanvas.Children)
            {
                if (item is Rectangle r)
                {
                    if ((Int32Rect)r.Tag == rect)
                        list.Add(r);
                }
            }

            foreach (var item in list)
            {
                this.ImageCanvas.Children.Remove(item);
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
            if (!this.CanImageDraw) return null;
            var leftPoint = new Point(drawRect.X, drawRect.Y);
            var pos = this.TranslateToCanvasPoint(leftPoint);
            var pixelRightTop = new Point(drawRect.Width + leftPoint.X, leftPoint.Y);
            var rightTop = this.TranslateToCanvasPoint(pixelRightTop);

            var pixelLeftBottom = new Point(leftPoint.X, drawRect.Height + leftPoint.Y);
            var leftBoom = this.TranslateToCanvasPoint(pixelLeftBottom);

            var width = rightTop.X - pos.X;
            var height = leftBoom.Y - pos.Y;

            var rect = new Rectangle
            {
                Stroke = stroke,
                Fill = fill,
                StrokeThickness = thickness / this.Slider.Value,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = width,
                Height = height,
                Visibility = Visibility.Visible
            };
            ImageCanvas.Children.Add(rect);

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
            if (!this.CanImageDraw) return null;

            p1 = this.TranslateToCanvasPoint(p1);
            p2 = this.TranslateToCanvasPoint(p2);
            var line = new Line()
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
            ImageCanvas.Children.Add(line);
            return line;
        }

        private readonly List<Path> addUielements = new List<Path>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public void RemoveElement(Path element)
        {
            this.ImageCanvas.Children.Remove(element);
            this.addUielements.Remove(element);
        }

        /// <summary>
        /// 删除自定义图形
        /// </summary>
        /// <param name="shape"></param>
        public void RemoveCustomeShape(BaseShape shape)
        {
            this.RemoveElement(shape.Path);
            this.CustomeShapes.Remove(shape);
            shape.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public void AddElement(Path element)
        {
            this.ImageCanvas.Children.Add(element);
            this.addUielements.Add(element);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearElements()
        {
            foreach (var item in this.addUielements)
            {
                this.ImageCanvas.Children.Remove(item);
            }
            this.addUielements.Clear();
        }

        /// <summary>
        /// 获取到当前的截取范围
        /// </summary>
        /// <returns>
        /// </returns>
        public Int32Rect GetChooseRect()
        {
            var recStartPoint = this.CutRectangle.TranslatePoint(new Point(), Img);

            var x = recStartPoint.X * this.w;
            var y = recStartPoint.Y * this.h;

            //将其取整
            var width = Math.Ceiling(this.CutRectangle.Width * this.w);
            var height = Math.Ceiling(this.CutRectangle.Height * this.h);
            //var width = Math.Ceiling(this.CutRectangle.Width);
            //var height = Math.Ceiling(this.CutRectangle.Height);

            var rect = new Int32Rect((int)x, (int)y, (int)width, (int)height);
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
            this.ImageSource.SaveBitmapSouce(rect, path, encoder);
        }

        /// <summary>
        /// 获取当前截取到的范围内的图片
        /// </summary>
        /// <returns>
        /// </returns>
        public BitmapSource GetChooseRectImageSouce()
        {
            var rect = this.GetChooseRect();
            var source = this.Img.Source as BitmapSource;

            var stride = source.Format.BitsPerPixel * rect.Width / 8;
            var data = new byte[rect.Height * stride];
            source.CopyPixels(rect, data, stride, 0);
            var newSource = BitmapSource.Create(rect.Width, rect.Height, source.DpiX, source.DpiY, source.Format, source.Palette, data, stride);
            return newSource;
        }


        /// <summary>
        /// 相当于Load
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.stackTools = this.GetTemplateChild(nameof(stackTools)) as StackPanel;
            this.CutRectButton = this.GetTemplateChild(nameof(CutRectButton)) as ToggleButton;
            this.CutRectButton.Click += this.CutArrClick;

            this.ImageBox = this.GetTemplateChild(nameof(ImageBox)) as Grid;
            this.GridBox = this.GetTemplateChild(nameof(GridBox)) as Grid;
            this.ImageScroll = this.GetTemplateChild(nameof(ImageScroll)) as ScrollViewer;

            this.ImageCanvas = this.GetTemplateChild(nameof(ImageCanvas)) as Canvas;
            this.ImageCanvas.SizeChanged += ImageCanvas_SizeChanged;
            this.ImageCanvas.MouseWheel += this.Control_MouseWheel;
            this.ImageCanvas.MouseDown += this.Canvas_MouseDown;
            this.ImageCanvas.MouseUp += this.Canvas_MouseUp;
            this.ImageCanvas.MouseLeave += this.Canvas_MouseLeave;
            this.ImageCanvas.MouseMove += this.Canvas_MouseMove;

            this.CutPanel = this.GetTemplateChild(nameof(CutPanel)) as StackPanel;
            this.cutTrans = this.GetTemplateChild(nameof(cutTrans)) as TranslateTransform;
            this.CutRectangle = this.GetTemplateChild(nameof(CutRectangle)) as Rectangle;
            this.CutRectangle.MouseDown += this.CutRectangle_MouseDown;

            this.StackMenu = this.GetTemplateChild(nameof(StackMenu)) as StackPanel;
            this.MenuOk = this.GetTemplateChild(nameof(MenuOk)) as Button;
            this.MenuOk.Click += CutRectButton_Click;

            this.MenuCancel = this.GetTemplateChild(nameof(MenuCancel)) as Button;
            this.MenuCancel.Click += this.CutTempCancelClick;

            this.Img = this.GetTemplateChild(nameof(Img)) as Image;
            this.Img.MouseDown += this.Img_MouseDown;
            this.Img.PreviewMouseMove += Img_MouseMove;
            this.Img.MouseUp += this.Img_MouseUp;
            this.Img.SizeChanged += Img_SizeChanged;

            this.ImageSource = this.writeable;
            this.ImageViewBox = this.GetTemplateChild(nameof(ImageViewBox)) as Viewbox;

            var contentGrid = this.GetTemplateChild("ContentGrid") as Grid;
            contentGrid.MouseUp += this.ContentGrid_MouseUp;

            this.Slider = this.GetTemplateChild(nameof(Slider)) as Slider;

            this.ToolExpander = this.GetTemplateChild(nameof(ToolExpander)) as Expander;

            this.group = (TransformGroup)this.ImageCanvas.RenderTransform;
            scaleTrans = this.group.Children[0] as ScaleTransform;

            //创建绑定,在XAML中绑定会提示错误,虽然不会报错
            var aBinding = new Binding
            {
                ElementName = nameof(this.Slider), // name of the slider
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

            this.tt = this.group.Children[3] as TranslateTransform;

            this.SetButtons();

            this.InitStyles();
            if (this.ImageSource != null && this.CutPanelMaxSize.IsEmpty)
            {
                this.CutPanelMaxSize = new Size(this.ImageSource.Width, this.ImageSource.Height);
            }
            this.CanMoveImage = true;
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
            if (this.ImageSource == null) return;


            this.ImageSizeChangedEvent?.Invoke(sender, e);

            //更改了尺寸后,为了不变截取的框的位置,要重新调整
            if (this.CutPanel.Visibility == Visibility.Collapsed) return;
        }

        /// <summary>
        /// 重置ImageSource的各项值
        /// </summary>
        public void ResertImageSource()
        {
            this.cutTrans.X = this.cutTrans.Y = 0;
            this.Slider.Value = 1;
            this.tt.X = this.tt.Y = 0;

            //重置了Img后重新计算比例
            var pos = GetPixelTrans;
            System.Diagnostics.Trace.WriteLine(pos);
        }

        /// <summary>
        /// 通知开始执行绘制文本框截取操作
        /// </summary>
        public void SendMouseCutRectStart()
        {
            if (!this.CanImageDraw) return;
            this.CutRectButton.IsChecked = true;
            this.CutArrClick(this.CutRectButton, null);
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
            if (!this.CanImageDraw) return null;
            var myEllipse = new Ellipse
            {
                Stroke = brush,
                Fill = brush,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = r,
                Height = r
            };
            _ = this.ImageCanvas.Children.Add(myEllipse);
            var pos = this.TranslateToCanvasPoint(pixelPoint);
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
            shape.CreateShape();
            this.AddElement(shape.Path);
            this.CustomeShapes.Add(shape);

            shape.UpdateScaleSize(this.ImageScale);
        }

        private void ImageCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.CanvasSizeChanged?.Invoke(sender, e);
        }



        #endregion Public 方法

        #region Internal 方法

        /// <summary>
        /// 设置按钮
        /// </summary>
        internal void SetButtons()
        {
            if (this.toolButtons != null)
            {
                foreach (var item in this.toolButtons)
                {
                    this.stackTools.Children.Add(item);
                }
            }

            this.Img.ContextMenu = this.toolMenus;
        }

        internal void SetToolButtons(IEnumerable<UIElement> elements) => this.toolButtons = elements;

        internal void SetToolMenus(ContextMenu elements) => this.toolMenus = elements;

        #endregion Internal 方法

        #region Private 方法

        private static void ImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void MenuItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var list = e.NewValue as ContextMenu;
                var c = d as ImageViewControl;
                SetContextMenus(c, list);
            }
        }

        private static void ToolButtonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var list = e.NewValue as ToolButtonCollection;
                var c = d as ImageViewControl;
                SetToolButtons(c, list);
            }
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this.Img);
            var re = point.X < 0 || point.Y < 0;

            if (e.LeftButton == MouseButtonState.Pressed && !re)
            {
                ////记录鼠标按下时的坐标
                //this.StartPoint = e.GetPosition(ImageCanvas);
                //清除截取框的移动轨迹,否则下一次截图将会受其影响
                if (this.CutPanel.Visibility == Visibility.Collapsed)
                {
                    this.cutTrans.X = this.cutTrans.Y = 0;
                }
            }
            else
            {
                //重置StartPoint
            }

            this.isDrag = true;
            this.curretnMouseDownPoint = e.GetPosition(this.ImageViewBox);
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.isDrag = false;
            this.Canvas_MouseUp(sender, new MouseButtonEventArgs(e.MouseDevice, 0, MouseButton.Left));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //获取当前鼠标坐标
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    this.ResizeAndDragCutRectangle(e);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine($"重新设置截取框大小出错:{ex.GetInnerExceptionMessage()}");
                }
                if (CutPanel.Visibility == Visibility.Visible)
                    this.CutPanelVisibleChanged?.Invoke(sender, new ImageCutRectEventArgs(this.GetChooseRect()));
            }
            else
            {
                this.ShowSizeInCutRectangleCursors();
            }

            if (e.LeftButton == MouseButtonState.Pressed && cuting && this.IsImageMouseDown && this.CanImageDraw)
            {
                this.DrawRectangle(e);
            }
            else
            {
                if (this.isDrag && this.CanMoveImage)
                {
                    this.DragCanvas(e);
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.cuting && this.IsImageMouseDown)
            {
                var rect = this.GetChooseRect();
                if (rect.Width > 0 && rect.Height > 0)
                {
                    this.StackMenu.Visibility = Visibility.Visible;
                    this.CutRectangle.Fill = new SolidColorBrush(Color.FromArgb(30, 2, 3, 4));
                    this.cuting = false;
                }
                else
                {
                    //this.CutTempCancelClick(null, null);
                }

            }

            //判断当前截图框的位置
            ResizeCutPanel();
            if (this.CutRectangle.Visibility == Visibility.Visible)
                this.CutPanelVisibleChanged?.Invoke(sender, new ImageCutRectEventArgs(this.GetChooseRect()));
            this.isDrag = false;
            this.Cursor = null;
            this.dragPoint = new Point();
            this.IsImageMouseDown = false;
        }

        private void ClearRectangle()
        {
            this.CutPanel.Visibility = Visibility.Collapsed;
            this.cuting = false;

            //清空所有画出来的框
            this.CutRectangle.Width = 0;
            this.CutRectangle.Height = 0;

            this.cutTrans.X = 0;
            this.cutTrans.Y = 0;
        }

        private void ContentGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Control_MouseWheel(object sender, MouseWheelEventArgs e) => this.WheelScale(e.GetPosition(ImageScroll), e.Delta * 1.0);

        private void CutArrClick(object sender, RoutedEventArgs e)
        {
            //截取
            if (sender is ToggleButton toggle)
            {
                if (!this.CanImageDraw)
                {
                    toggle.IsChecked = false;
                    return;
                }
                this.cuting = toggle.IsChecked.Value;
                this.CutPanel.Visibility = this.cuting ? Visibility.Visible : Visibility.Collapsed;
                this.CutRectangle.Visibility = this.cuting ? Visibility.Visible : Visibility.Collapsed;
                this.CutRectangle.Width = this.CutRectangle.Height = 0;
                this.StackMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void CutRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                this.CutTempCancelClick(null, null);
                return;
            }

            if (e.ClickCount >= 2 && this.DoubleClickRaiseImage)
            {
                CutRectButton_Click(this.MenuOk, null);
                return;
            }
            if (this.Cursor != null)
            {
                return;
            }
            this.Cursor = Cursors.Hand;
            this.dragPoint = e.GetPosition(this.Img);
        }

        private void CutRectButton_Click(object sender, RoutedEventArgs e)
        {
            var rect = this.GetChooseRect();
            this.CutPanel.Visibility = Visibility.Collapsed;
            var handler = new ImageCutRectEventArgs(rect, true);
            this.CutImageEvent?.Invoke(sender, handler);
            BitmapSource source = null;
            var sucess = false;
            var msg = "";
            if (handler.HandleToNext)
            {
                try
                {
                    source = this.GetChooseRectImageSouce();
                    sucess = true;
                }
                catch (Exception ex)
                {
                    msg = ex.GetInnerExceptionMessage();
                }

            }

            this.CutRectButton.IsChecked = false;
            this.isDrag = false;
            this.ClearRectangle();
            if (handler.HandleToNext)
            {
                this.CutImageDownEvent?.Invoke(sender, new ImageEventArgs(source, sucess, msg) { Int32Rect = rect });
            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        public event EventHandler CutMenuCancelEvent;
        private void CutTempCancelClick(object sender, RoutedEventArgs e)
        {
            this.RaiseCancelCutImage();
            this.CutMenuCancelEvent?.Invoke(sender, e);
        }

        /// <summary>
        /// 取消截图操作
        /// </summary>
        public void RaiseCancelCutImage()
        {
            foreach (UIElement item in this.stackTools.Children)
            {
                if (item is ToggleButton button)
                {
                    button.IsChecked = false;
                }
            }

            this.isDrag = false;
            this.ClearRectangle();
        }

        /// <summary>
        /// 拖拽画布
        /// </summary>
        /// <param name="e">
        /// </param>
        private void DragCanvas(MouseEventArgs e)
        {
            if (this.CutPanel.Visibility == Visibility.Visible)
            {
                //处于截图状态则不让其可以平移(如果不限制,则用户在截取图片后再移动画布处理起来比较麻烦)
                return;
            }


            Point p = e.GetPosition(ImageViewBox);
            Point topPoint = this.ImageCanvas.TranslatePoint(new Point(0, 0), ImageViewBox);
            Point bottomPoint = this.ImageCanvas.TranslatePoint(new Point(this.ImageCanvas.ActualWidth, this.ImageCanvas.ActualHeight), this.ImageViewBox);

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
            if (this.ImageSource == null)
                return;
            //鼠标当前的point
            Point Endpoint = e.GetPosition(Img);

            //清空所有画出来的框
            this.CutRectangle.Width = this.CutRectangle.Height = 0;

            //转换成基于Image控件的坐标
            this.width = Endpoint.X - this.StartPoint.X - 1;
            this.height = Endpoint.Y - this.StartPoint.Y;

            //用来记录左上角坐标
            var StartLeft = new Point();

            //如果从左往右画
            if (Endpoint.X > this.StartPoint.X)
            {
                width = Endpoint.X - this.StartPoint.X;
                StartLeft.X = this.StartPoint.X;

                //如果从上往下画
                if (Endpoint.Y > this.StartPoint.Y)
                {
                    this.height = Endpoint.Y - this.StartPoint.Y;
                    StartLeft.Y = this.StartPoint.Y;
                }
                else
                {
                    this.height = this.StartPoint.Y - Endpoint.Y;
                    StartLeft.Y = this.StartPoint.Y - this.height;
                }
            }
            else
            {
                this.width = this.StartPoint.X - Endpoint.X;
                StartLeft.X = Endpoint.X;
                if (Endpoint.Y > this.StartPoint.Y)
                {
                    this.height = Endpoint.Y - this.StartPoint.Y;
                    StartLeft.Y = Endpoint.Y - this.height;
                }
                else
                {
                    this.height = this.StartPoint.Y - Endpoint.Y;
                    StartLeft.Y = Endpoint.Y;
                }
            }

            var scale = 1 / this.Slider.Value;
            this.CutRectangle.StrokeThickness = scale;
            this.StackMenu.RenderTransform = new ScaleTransform(scale * 2, scale * 2);


            var minSize = this.CutPanelMinSize;
            var maxSize = this.CutPanelMaxSize;
            //转换完后的宽高
            width *= this.w;
            height *= this.h;

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

            width /= this.w;
            height /= this.h;
            this.CutRectangle.Width = width;
            this.CutRectangle.Height = height;
            StartLeft = this.Img.TranslatePoint(StartLeft, ImageCanvas);
            Canvas.SetLeft(this.CutPanel, StartLeft.X);
            Canvas.SetTop(this.CutPanel, StartLeft.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Point GetCurrentPixelPoint(MouseEventArgs e)
        {
            var ims = (BitmapSource)this.Img.Source;

            var mousePoint = e.GetPosition(this.Img);
            this.CurrentMouseDownPoint = mousePoint;

            this.w = ims.PixelWidth / this.Img.ActualWidth;
            var pixelMousePositionX = mousePoint.X * this.w;

            this.h = ims.PixelHeight / this.Img.ActualHeight;

            var pixelMousePositionY = mousePoint.Y * this.h;

            this.CurrentMouseDownPixelPoint = new Point(pixelMousePositionX, pixelMousePositionY);
            return this.CurrentMouseDownPixelPoint;
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //记录鼠标按下时的坐标
            this.StartPoint = e.GetPosition(Img);
            this.CurrentMouseDownPixelPoint = GetCurrentPixelPoint(e);
            this.CurrentMouseDownPoint = this.StartPoint;
            this.IsImageMouseDown = true;
            this.ImageMouseDownEvent?.Invoke(this, e);
        }

        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            var point = this.GetCurrentPixelPoint(e);
            var canvasPoint = e.GetPosition(this.ImageCanvas);
            this.ImageMouseMoveEvent?.Invoke(new ImageMouseEventArgs(e.MouseDevice, e.Timestamp, e.StylusDevice, point, canvasPoint));

            // this.Canvas_MouseMove(this.Img, e); this.lbPosition.Text = point.X +
            // Environment.NewLine + point.Y;
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void InitStyles()
        {
            //获取样式表
            var dic = new ResourceDictionary { Source = new Uri("pack://application:,,,/GeneralTool.CoreLibrary;component/Themes/BaseStyle.xaml", UriKind.Absolute) };

            Style styleToolCutButton = (Style)dic["CutButtonIconFontStyle"];

            //初始化截图按钮
            if (this.ToolCutButtonStyle == null)
                this.ToolCutButtonStyle = styleToolCutButton;

            //初始化Header
            if (this.ToolExpanderStyle == null)
            {
                this.ToolExpander.Header = "工具";
            }

            Style styleMenuOk = (Style)dic["MenuOkStyle"];

            //查看OK按钮有无内容
            if (this.MenuOkStyle == null)
                this.MenuOkStyle = styleMenuOk;

            Style styleMenuCancel = (Style)dic["MenuCancelStyle"];

            //查看Cancel按钮有无内容
            if (this.MenuCancelStyle == null)
                this.MenuCancelStyle = styleMenuCancel;
        }


        /// <summary>
        /// 拖拽截取图形
        /// </summary>
        /// <param name="e">
        /// </param>
        private void ResizeAndDragCutRectangle(MouseEventArgs e)
        {
            //获取鼠标当前的点
            var mousePoint = e.GetPosition(this.Img);

            //获取当前方框在canvas中的终点
            var recEndPoint = this.CutRectangle.TranslatePoint(new Point(CutRectangle.Width, this.CutRectangle.Height), this.Img);
            //获取当前方框在canvas中的起点
            var recStartPoint = this.CutRectangle.TranslatePoint(new Point(0, 0), this.Img);

            //判断当前鼠标开状
            if (this.Cursor == Cursors.SizeWE && this.drawType == DrawType.Right)//在右边拉
            {
                if (mousePoint.X < recEndPoint.X && this.CutRectangle.Width < 10 / this.Slider.Value)
                {
                    return;
                }

                var dis = mousePoint.X - recEndPoint.X;
                if (dis + this.CutRectangle.Width < 10 / Slider.Value)
                    return;

                //左右方拉取,重新赋值宽度
                this.CutRectangle.Width += dis;
            }
            else if (this.Cursor == Cursors.SizeWE && this.drawType == DrawType.Left)//在左边拉
            {
                if (mousePoint.X < recStartPoint.X && this.CutRectangle.Width < 10 / this.Slider.Value)
                {
                    return;
                }

                //在左边左右拉取,重新赋左上角坐标
                var dis = recStartPoint.X - mousePoint.X;
                if (dis + this.CutRectangle.Width < 10 / Slider.Value)
                    return;
                this.cutTrans.X += mousePoint.X - recStartPoint.X;
                //
                this.CutRectangle.Width += dis;
            }
            else if (this.Cursor == Cursors.SizeNS && this.drawType == DrawType.Bottom)//在下边拉
            {
                if (mousePoint.Y < recEndPoint.Y && this.CutRectangle.Height < 10 / this.Slider.Value)
                {
                    return;
                }

                //上下拉取,重新赋值高度
                var dis = mousePoint.Y - recEndPoint.Y;
                if (dis + this.CutRectangle.Height < 10 / Slider.Value)
                    return;
                this.CutRectangle.Height += dis;
            }
            else if (this.Cursor == Cursors.SizeNS && this.drawType == DrawType.Top)//在上边拉
            {
                if (mousePoint.Y < recEndPoint.Y && this.CutRectangle.Height < 10 / this.Slider.Value)
                    return;

                //在上方上下拉取,重新赋左上角坐标
                var dis = recStartPoint.Y - mousePoint.Y;
                if (dis + this.CutRectangle.Height < 10 / Slider.Value)
                    return;
                this.cutTrans.Y += mousePoint.Y - recStartPoint.Y; ;
                //
                this.CutRectangle.Height += dis;
            }
            else if (this.Cursor == Cursors.Hand)
            {
                //拖拽
                //获取方框起点坐标
                //计算拖动距离
                var disY = mousePoint.Y - this.dragPoint.Y;
                var disX = mousePoint.X - this.dragPoint.X;

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

                this.cutTrans.X += disX;
                this.cutTrans.Y += disY;
                this.dragPoint = mousePoint;
            }
        }

        /// <summary>
        /// 重置截图框位置
        /// </summary>
        private void ResizeCutPanel()
        {
            if (this.CutPanel.Visibility == Visibility.Collapsed || this.ImageSource == null)
                return;

            var width = this.CutRectangle.Width;
            var height = this.CutRectangle.Height;
            //查看宽度是否足够
            if (width > this.Img.ActualWidth)
            {
                //不够,则减成足够的宽度先
                width = this.Img.ActualWidth;
            }

            //查看高度是否足够
            if (height > this.Img.ActualHeight)
            {
                //不够,则减成足够的高度先
                height = this.Img.ActualHeight;
            }

            var minSize = this.CutPanelMinSize;
            var maxSize = this.CutPanelMaxSize;

            //能够接受的在图像上的宽高值
            var minWidth = minSize.Width / this.w;
            var minHeight = minSize.Height / this.h;
            var maxWidth = maxSize.Width / this.w;
            var maxHeight = maxSize.Height / this.h;

            //转换完后的宽高,当前的宽高只是控件本身的宽高,转为对应图片的像素宽高
            var wTmp = width * this.w;
            var hTmp = height * this.h;

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
            this.CutRectangle.Width = width;
            this.CutRectangle.Height = height;

            //查看当前框在图片中的左上角坐标
            var topLeft = this.CutRectangle.TranslatePoint(new Point(), this.Img);

            //查看当前框在图片中的右下角坐标
            var bottomRight = this.CutRectangle.TranslatePoint(new Point(this.CutRectangle.Width, this.CutRectangle.Height), this.Img);

            var x = topLeft.X;
            var y = topLeft.Y;

            //X超限
            if (topLeft.X < 0)
            {
                //超出了左边区域,则让X贴Img左边
                cutTrans.X = 0;
                topLeft.X = 0;
                topLeft = this.Img.TranslatePoint(new Point(), this.ImageCanvas);

                //将左上角点转为Canvas中的坐标
                Canvas.SetLeft(this.CutPanel, topLeft.X);
            }

            if (bottomRight.X > this.Img.ActualWidth)
            {
                //超出了右边区域
                cutTrans.X = 0;
                topLeft.X -= bottomRight.X - this.Img.ActualWidth;
                topLeft = this.Img.TranslatePoint(topLeft, this.ImageCanvas);

                //将左上角点转为Canvas中的坐标
                Canvas.SetLeft(this.CutPanel, topLeft.X);
            }

            //Y超限
            if (topLeft.Y < 0)
            {
                //超出了上边区域
                cutTrans.Y = 0;
                topLeft.Y = 0;
                topLeft = this.Img.TranslatePoint(topLeft, this.ImageCanvas);

                //将左上角点转为Canvas中的坐标
                Canvas.SetTop(this.CutPanel, topLeft.Y);
            }

            if (bottomRight.Y > this.Img.ActualHeight)
            {
                //超出了下边区域
                cutTrans.Y = 0;
                topLeft.Y -= bottomRight.Y - this.Img.ActualHeight;
                topLeft = this.Img.TranslatePoint(topLeft, this.ImageCanvas);
                Canvas.SetTop(this.CutPanel, topLeft.Y);
            }
        }

        /// <summary>
        /// 鼠标在已截取的方框中移动时显示缩放鼠标图形
        /// </summary>
        private void ShowSizeInCutRectangleCursors()
        {
            if (this.CutRectangle.Width == 0)
            {
                return;
            }

            //确定当前鼠标点的位置

            var mousePoint = Mouse.GetPosition(this.ImageCanvas);

            //获取方框在canvas中的右底终点
            var recEndPoint = this.CutRectangle.TranslatePoint(new Point(CutRectangle.Width, CutRectangle.Height), this.ImageCanvas);
            //获取方框在canvas中的左上起点,也是左上顶点
            var recStartPoint = this.CutRectangle.TranslatePoint(new Point(0, 0), this.ImageCanvas);

            //获取右上角顶点
            var recRightTopPoint = this.CutRectangle.TranslatePoint(new Point(CutRectangle.Width, 0), this.ImageCanvas);

            //获取左下角顶点
            var recLeftBottomPoint = this.CutRectangle.TranslatePoint(new Point(0, CutRectangle.Height), this.ImageCanvas);


            //判断
            //右距离,方框右边的x坐标与鼠标x坐标的距离
            var xDis = recEndPoint.X - mousePoint.X;
            //左距离,方框左边的x坐标与鼠标x坐标的距离
            var leftXDis = recStartPoint.X - mousePoint.X;
            //下距离,方框下边的y坐标与鼠标y坐标的距离
            var yDis = recEndPoint.Y - mousePoint.Y;
            //上距离,方框上边的y坐标与鼠标y坐标的距离
            var topYDis = recStartPoint.Y - mousePoint.Y;

            //判断鼠标是否在右边框
            var mouseInRightLine = xDis > -1 / this.Slider.Value && xDis < 3 / this.Slider.Value && mousePoint.Y > (recRightTopPoint.Y + 1 / this.Slider.Value) && mousePoint.Y < recEndPoint.Y - 1 / Slider.Value;

            //判断鼠标是否在左边框
            var mouseInLeftLine = leftXDis > -1 / this.Slider.Value && leftXDis < 3 / this.Slider.Value && mousePoint.Y > (recRightTopPoint.Y + 1 / this.Slider.Value) && mousePoint.Y < recEndPoint.Y - 1 / Slider.Value;

            //判断鼠标是否在下边框
            var mouseInBottomLine = yDis > -1 / this.Slider.Value && yDis < 3 / this.Slider.Value && mousePoint.X > recLeftBottomPoint.X + 1 / this.Slider.Value && mousePoint.X < recEndPoint.X - 1 / Slider.Value;

            //判断鼠标是否在上边框
            var mouseInTopLine = topYDis > -1 / this.Slider.Value && topYDis < 3 / this.Slider.Value && mousePoint.X > recStartPoint.X + 1 / this.Slider.Value && mousePoint.X < recEndPoint.X - 1 / Slider.Value;


            //右边的显示应该处于右顶点到右底点之间
            if (mouseInRightLine)
            {
                //处于右边
                this.Cursor = Cursors.SizeWE;
                this.drawType = DrawType.Right;
            }
            else if (mouseInLeftLine)
            {
                //处于左边
                this.Cursor = Cursors.SizeWE;
                this.drawType = DrawType.Left;
            }
            else if (mouseInBottomLine)
            {
                //处于下方
                this.Cursor = Cursors.SizeNS;
                this.drawType = DrawType.Bottom;
            }
            else if (mouseInTopLine)
            {
                //处于上方
                this.Cursor = Cursors.SizeNS;
                this.drawType = DrawType.Top;
            }
            else
            {
                this.Cursor = null;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsLoaded)
            {
                this.WheelScale(this.currentWheelPoint, e.NewValue > e.OldValue ? 120d : -120d, true);
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
            var wh = this.GetPixelTrans;
            var x = pixelPoint.X / wh.W;
            var y = pixelPoint.Y / wh.H;
            var pos = this.Img.TranslatePoint(new Point(x, y), ImageCanvas);
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
            var pos = this.ImageCanvas.TranslatePoint(controlPoint, this.Img);
            var wh = this.GetPixelTrans;
            var x = pos.X * wh.W;
            var y = pos.Y * wh.H;
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
            if (this.CutPanel.Visibility == Visibility.Visible)
            {
                return;
            }
            this.currentWheelPoint = point;// e.GetPosition(viewBox); // 实际点击的点
            var actualPoint = group.Inverse.Transform(this.currentWheelPoint); // 想要缩放的点
            if (!sliderChanged)
            {
                this.Slider.ValueChanged -= Slider_ValueChanged;
                Slider.Value += delta / 500;
                this.Slider.ValueChanged += Slider_ValueChanged;
            }
            this.tt.X = -(actualPoint.X * (this.Slider.Value - 1)) + this.currentWheelPoint.X - actualPoint.X;
            this.tt.Y = -(actualPoint.Y * (this.Slider.Value - 1)) + this.currentWheelPoint.Y - actualPoint.Y;
            this.MouseWheelScaleEvent?.Invoke(this.Img, new ImageScaleEventArgs(this.Slider.Value));

            this.ImageScale = Slider.Value;
            //更新大小
            foreach (var item in this.CustomeShapes)
            {
                if (item.AutoScale)
                    item.UpdateScaleSize(this.ImageScale);
            }
        }



        #endregion Private 方法

        #region Private 结构

        private struct PixelTrans
        {
            #region Public 构造函数

            public PixelTrans(double w, double h)
            {
                this.W = w;
                this.H = h;
            }

            #endregion Public 构造函数



            #region Public 属性

            public double H { get; set; }

            public double W { get; set; }

            #endregion Public 属性



            #region Public 方法

            public override string ToString()
            {
                return $"{this.W} {this.H}";
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