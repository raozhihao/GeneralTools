using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace GeneralTool.General.WPFHelper.WPFControls
{
    /// <summary>
    /// ImageViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageViewControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public ImageViewControl()
        {
            this.InitializeComponent();
            this.Loaded += this.MyControl_Loaded;
        }

        private void MyControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.group = (TransformGroup)this.ImageCanvas.RenderTransform;
            this.tt = this.group.Children[3] as TranslateTransform;
        }

        #region 附加属性

        /// <summary>
        /// 向工具条按钮中附加一组自定义控件,需要使用 ToolButtonCollection
        /// </summary>
        public static readonly DependencyProperty ToolButtons = DependencyProperty.RegisterAttached("ToolButtons", typeof(ToolButtonCollection), typeof(ImageViewControl), new FrameworkPropertyMetadata(ToolButtonsChanged));

        private static void ToolButtonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var list = e.NewValue as ToolButtonCollection;
                var c = d as ImageViewControl;
                SetToolButtons(c, list);
            }
        }

        /// <summary>
        /// 设置工具按钮
        /// </summary>
        /// <param name="control"></param>
        /// <param name="buttons"></param>
        public static void SetToolButtons(ImageViewControl control, ToolButtonCollection buttons)
        {
            foreach (var item in buttons)
            {
                _ = control.stackTools.Children.Add(item);
            }
        }

        /// <summary>
        /// 获取工具按钮
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static ToolButtonCollection GetToolButtons(ImageViewControl control)
        {
            var c = new ToolButtonCollection();
            foreach (ButtonBase item in control.stackTools.Children)
            {
                c.Add(item);
            }
            return c;
        }

        #endregion

        #region 对外属性

        /// <summary>
        /// 最大缩放倍数
        /// </summary>
        [Description("最大缩放倍数"), Category("自定义属性")]
        public int MaxScaleValue
        {
            get => Convert.ToInt32(this.Slider.GetValue(RangeBase.MaximumProperty));
            set
            {
                if (value < 1)
                    value = 1;
                this.Slider.SetValue(RangeBase.MaximumProperty, value * 1.0);
            }
        }

        /// <summary>
        /// 缩放条的显示状态
        /// </summary>
        [Description("缩放条的显示状态"), Category("自定义属性")]
        public Visibility SliderVisibility
        {
            get => (Visibility)this.Slider.GetValue(VisibilityProperty);
            set => this.Slider.SetValue(VisibilityProperty, value);
        }

        /// <summary>
        /// 右侧工具条显示状态
        /// </summary>
        [Description("右侧工具条显示状态"), Category("自定义属性")]
        public Visibility ToolExpanderVisibility
        {
            get => (Visibility)this.ToolExpander.GetValue(VisibilityProperty);
            set => this.ToolExpander.SetValue(VisibilityProperty, value);
        }

        /// <summary>
        /// 右侧工具条是否展开
        /// </summary>
        [Description("右侧工具条是否展开"), Category("自定义属性")]
        public bool IsToolExpanderExpanded
        {
            get => (bool)this.ToolExpander.GetValue(Expander.IsExpandedProperty);
            set => this.ToolExpander.SetValue(Expander.IsExpandedProperty, value);
        }

        /// <summary>
        /// 右侧工具条截图按钮样式
        /// </summary>
        [Description("右侧工具条截图按钮样式"), Category("自定义属性")]
        public Style ToolCutButtonStyle
        {
            get => (Style)this.CutRectButton.GetValue(StyleProperty);
            set => this.CutRectButton.SetValue(StyleProperty, value);
        }

        /// <summary>
        /// 右侧工具条截图按钮内容
        /// </summary>
        [Description("右侧工具条截图按钮内容"), Category("自定义属性")]
        public object ToolCutButtonContent
        {
            get => this.CutRectButton.GetValue(ContentProperty);
            set => this.CutRectButton.SetValue(ContentProperty, value);
        }

        /// <summary>
        /// 确定截图按钮样式
        /// </summary>
        [Description("确定截图按钮样式"), Category("自定义属性")]
        public Style MenuOkStyle
        {
            get => (Style)this.MenuOk.GetValue(StyleProperty);
            set => this.MenuOk.SetValue(StyleProperty, value);
        }

        /// <summary>
        /// 取消截图按钮样式
        /// </summary>
        [Description("取消截图按钮样式"), Category("自定义属性")]
        public Style MenuCancelStyle
        {
            get => (Style)this.MenuCancel.GetValue(StyleProperty);
            set => this.MenuCancel.SetValue(StyleProperty, value);
        }

        /// <summary>
        /// 左侧滚动条样式
        /// </summary>
        [Description("左侧滚动条样式"), Category("自定义属性")]
        public Style SliderStyle
        {
            get => (Style)this.Slider.GetValue(StyleProperty);
            set => this.Slider.SetValue(StyleProperty, value);
        }

        /// <summary>
        /// 鼠标在图像上移动时当前的像素点位置
        /// </summary>
        [Description("鼠标在图像上移动时当前的像素点位置"), Category("自定义属性")]
        public Point MouseOverPoint => this.Img.IsMouseOver ? this.GetCurrentPixelPoint(new MouseEventArgs(Mouse.PrimaryDevice, 1)) : new Point();

        /// <summary>
        /// 当前图像源
        /// </summary>
        [Description("当前图像源"), Category("自定义属性")]
        public ImageSource ImageSource
        {
            get => (ImageSource)this.Img.GetValue(Image.SourceProperty);
            set => this.Img.Source = value;
        }

        #endregion

        #region 对外事件

        /// <summary>
        /// 鼠标在画面上移动事件
        /// </summary>
        [Description("鼠标在画面上移动事件"), Category("自定义事件")]
        public event Action<Point> ImageMouseMoveEvent;

        /// <summary>
        /// 确定截图成功后触发事件
        /// </summary>
        [Description("确定截图成功后触发事件"), Category("自定义事件")]
        public event EventHandler<Int32Rect> CutImageEvent;

        /// <summary>
        /// 确定截图成功后触发事件
        /// </summary>
        [Description("确定截图成功后触发事件"), Category("自定义事件")]
        public event EventHandler<BitmapSource> CutImageDownEvent;

        /// <summary>
        /// 在图像进行缩放时触发事件
        /// </summary>
        [Description("在图像进行缩放时触发事件"), Category("自定义事件")]
        public event EventHandler<double> MouseWheelScaleEvent;

        #endregion

        #region 对外方法

        /// <summary>
        /// 获取到当前的截取范围
        /// </summary>
        /// <returns></returns>
        public Int32Rect GetChooseRect()
        {
            var recStartPoint = this.CutRectangle.TranslatePoint(new Point(), Img);
            var recEndPoint = this.CutRectangle.TranslatePoint(new Point(this.CutRectangle.Width, this.CutRectangle.Height), Img);

            var x = recStartPoint.X * this.w;
            var y = recStartPoint.Y * this.h;

            var endX = recEndPoint.X * this.w;
            var endY = recEndPoint.Y * this.h;

            var width = endX - x;
            var height = endY - y;

            var rect = new Int32Rect((int)x, (int)y, (int)width, (int)height);
            return rect;
        }

        /// <summary>
        /// 通知开始执行绘制文本框截取操作
        /// </summary>
        public void SendMouseCutRectStart()
        {
            this.CutRectButton.IsChecked = true;
            this.CutArrClick(this.CutRectButton, null);
        }

        /// <summary>
        /// 向画布设置点
        /// </summary>
        /// <param name="pixelPoint"></param>
        /// <param name="brush"></param>
        /// <param name="r"></param>
        public void SetPoint(Point pixelPoint, Brush brush, double r = 4)
        {

            Ellipse myEllipse = new Ellipse
            {
                Stroke = brush,
                Fill = brush,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Width = r,
                Height = r
            };
            ImageCanvas.Children.Add(myEllipse);
            var pos = this.TranslatePoint(pixelPoint);
            if (double.IsInfinity(pos.X) || double.IsInfinity(pos.Y))
            {
                return;
            }
            myEllipse.Tag = pixelPoint;
            Canvas.SetLeft(myEllipse, pos.X - r / 2);
            Canvas.SetTop(myEllipse, pos.Y - r / 2);
        }

        /// <summary>
        /// 清除点
        /// </summary>
        /// <param name="pixelPoint"></param>
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
        /// <param name="rect"></param>
        public void ClearRect(Int32Rect rect)
        {
            List<UIElement> list = new List<UIElement>();
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
        /// 清除所有指定类型的图形
        /// </summary>
        /// <param name="drawType"></param>
        public void ClearAll(ImageDrawType drawType = ImageDrawType.Ellipse | ImageDrawType.Rectangle)
        {
            List<UIElement> list = new List<UIElement>();
            foreach (UIElement item in this.ImageCanvas.Children)
            {

                if (item is Ellipse ellipse)
                {
                    if (drawType.HasFlag(ImageDrawType.Ellipse))
                        list.Add(item);
                }

                if (item is Rectangle rect)
                {
                    if (drawType.HasFlag(ImageDrawType.Rectangle))
                        list.Add(rect);
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
        /// <param name="drawRect">绘制区域</param>
        /// <param name="stroke">线条画刷</param>
        /// <param name="fill">填充画刷</param>
        /// <param name="thickness">线条宽度</param>
        public void DrawRect(Int32Rect drawRect, Brush stroke, Brush fill, double thickness = 1)
        {
            var leftPoint = new Point(drawRect.X, drawRect.Y);
            var pos = this.TranslatePoint(leftPoint);
            var pixelRightTop = new Point(drawRect.Width + leftPoint.X, leftPoint.Y);
            var rightTop = this.TranslatePoint(pixelRightTop);

            var pixelLeftBottom = new Point(leftPoint.X, drawRect.Height + leftPoint.Y);
            var leftBoom = this.TranslatePoint(pixelLeftBottom);

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
                return;
            }
            rect.Tag = drawRect;
            Canvas.SetLeft(rect, pos.X);
            Canvas.SetTop(rect, pos.Y);
        }


        /// <summary>
        /// 获取当前截取到的范围内的图片
        /// </summary>
        /// <returns></returns>
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


        #endregion

        #region 窗体事件属性字段 

        private void CutArrClick(object sender, RoutedEventArgs e)
        {
            //截取车头模板
            if (sender is ToggleButton toggle)
            {
                foreach (UIElement item in this.stackTools.Children)
                {
                    if (item is ToggleButton t)
                    {
                        if (t != toggle)
                        {
                            t.IsChecked = false;
                        }
                    }
                }
                this.cuting = toggle.IsChecked.Value;
                this.CutPanel.Visibility = this.cuting ? Visibility.Visible : Visibility.Collapsed;
                this.CutRectangle.Visibility = this.cuting ? Visibility.Visible : Visibility.Collapsed;
                this.CutRectangle.Width = this.CutRectangle.Height = 0;
                this.StackMenu.Visibility = Visibility.Collapsed;
            }
        }

        private Point TranslatePoint(Point pixelPoint)
        {
            //转换为目标点
            var wh = this.GetPixelTrans;
            var x = pixelPoint.X / wh.W;
            var y = pixelPoint.Y / wh.H;
            var pos = this.Img.TranslatePoint(new Point(x, y), ImageCanvas);
            return pos;
        }



        //截图起始坐标
        private Point StartPoint;

        //截图的长宽
        private double width = 0;
        private double height = 0;
        private bool cuting;

        /// <summary>
        /// 当前图片的缩放w比例
        /// </summary>
        private double w;

        /// <summary>
        /// 当前图片的缩放h比例
        /// </summary>
        private double h;

        /// <summary>
        /// 定义移动对象
        /// </summary>
        private TranslateTransform tt;

        /// <summary>
        /// 定义变幻组合对象
        /// </summary>
        private TransformGroup group;

        /// <summary>
        /// 是否处于拖动状态
        /// </summary>
        private bool isDrag = false;

        /// <summary>
        /// 当前按下的坐标
        /// </summary>
        private Point curretnMouseDownPoint;

        /// <summary>
        /// 当前鼠标在滚动的时候的坐标
        /// </summary>
        private Point currentWheelPoint;

        private Point dragPoint;

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsLoaded)
            {
                this.WheelScale(this.currentWheelPoint, e.NewValue > e.OldValue ? 120d : -120d, true);
            }
        }

        private void Control_MouseWheel(object sender, MouseWheelEventArgs e) => this.WheelScale(e.GetPosition(ImageScroll), e.Delta * 1.0);

        /// <summary>
        /// 开始滚动缩放
        /// </summary>
        /// <param name="point">滚动缩放的中心点坐标</param>
        /// <param name="delta">缩放大小</param>
        /// <param name="sliderChanged">当前是否处于滚动条调节缩放</param>
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
            this.MouseWheelScaleEvent?.Invoke(this.Img, this.Slider.Value);
        }


        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //记录鼠标按下时的坐标
                this.StartPoint = e.GetPosition(ImageCanvas);
                //清除截取框的移动轨迹,否则下一次截图将会受其影响
                if (this.CutPanel.Visibility == Visibility.Collapsed)
                {

                    this.cutTrans.X = this.cutTrans.Y = 0;
                }
            }

            this.isDrag = true;
            this.curretnMouseDownPoint = e.GetPosition(this.ImageBox);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.cuting)
            {
                this.CutRectangle.Fill = new SolidColorBrush(Color.FromArgb(30, 2, 3, 4));
                this.StackMenu.Visibility = Visibility.Visible;
                this.cuting = false;
            }
            this.isDrag = false;
            this.Cursor = null;
            this.dragPoint = new Point();
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e) => this.isDrag = false;

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //获取当前鼠标坐标
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.ResizeAndDragCutRectangle(e);
            }
            else
            {
                this.ShowSizeInCutRectangleCursors();
            }

            if (e.LeftButton == MouseButtonState.Pressed && cuting)
            {
                this.DrawRectangle(e);
            }
            else
            {
                if (this.isDrag)
                {
                    this.DragCanvas(e);

                }
            }
        }

        /// <summary>
        /// 绘制截取框
        /// </summary>
        /// <param name="e"></param>
        private void DrawRectangle(MouseEventArgs e)
        {
            //鼠标当前的point
            Point Endpoint = e.GetPosition(ImageCanvas);

            //清空所有画出来的框
            this.CutRectangle.Width = this.CutRectangle.Height = 0;
            //转换成基于Image控件的坐标
            this.width = Endpoint.X - this.StartPoint.X - 1;
            this.height = Endpoint.Y - this.StartPoint.Y;
            //用来记录左上角坐标
            Point StartLeft = new Point();
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
            this.CutRectangle.Width = width;
            this.CutRectangle.Height = height;
            Canvas.SetLeft(this.CutPanel, this.StartPoint.X);
            Canvas.SetTop(this.CutPanel, this.StartPoint.Y);
        }

        /// <summary>
        /// 拖拽画布
        /// </summary>
        /// <param name="e"></param>
        private void DragCanvas(MouseEventArgs e)
        {
            if (this.CutPanel.Visibility == Visibility.Visible)
            {
                //处于截图状态则不让其可以平移(如果不限制,则用户在截取图片后再移动画布处理起来比较麻烦)
                return;
            }
            var p = e.GetPosition(this.ImageBox);
            var topPoint = this.ImageCanvas.TranslatePoint(new Point(0, 0), this.ImageBox);
            var bottomPoint = this.ImageCanvas.TranslatePoint(new Point(ImageCanvas.ActualWidth, this.ImageCanvas.ActualHeight), this.ImageBox);

            var moveX = p.X - this.curretnMouseDownPoint.X;
            var moveY = p.Y - this.curretnMouseDownPoint.Y;

            //向上向下移动条件判断（会有一点点的小偏移，如果想更精确的控制，那么分向上和向下两种情况，并判断边距）
            if ((moveY < 0 && bottomPoint.Y > ImageBox.ActualHeight) || (moveY > 0 && topPoint.Y < 0))
            {
                this.tt.Y += p.Y - this.curretnMouseDownPoint.Y;
                this.curretnMouseDownPoint.Y = p.Y;
            }

            //向左向右移动条件判断
            if ((moveX < 0 && bottomPoint.X > ImageBox.ActualWidth) || (moveX > 0 && topPoint.X < 0))
            {
                this.tt.X += p.X - this.curretnMouseDownPoint.X;
                this.curretnMouseDownPoint.X = p.X;
            }
        }

        /// <summary>
        /// 拖拽截取图形
        /// </summary>
        /// <param name="e"></param>
        private void ResizeAndDragCutRectangle(MouseEventArgs e)
        {
            //获取鼠标当前的点
            var mousePoint = e.GetPosition(this.ImageCanvas);
            //获取当前方框在canvas中的终点
            var recEndPoint = this.CutRectangle.TranslatePoint(new Point(CutRectangle.Width, this.CutRectangle.Height), this.ImageCanvas);

            //判断当前鼠标开状
            if (this.Cursor == Cursors.SizeWE)
            {
                if (mousePoint.X < recEndPoint.X && this.CutRectangle.Width < 10 / this.Slider.Value)
                {
                    return;
                }
                //左右方拉取,重新赋值宽度
                this.CutRectangle.Width += mousePoint.X - recEndPoint.X;
            }
            else if (this.Cursor == Cursors.SizeNS)
            {
                if (mousePoint.Y < recEndPoint.Y && this.CutRectangle.Height < 10 / this.Slider.Value)
                {
                    return;
                }
                //上下拉取,重新赋值高度
                this.CutRectangle.Height += mousePoint.Y - recEndPoint.Y;
            }
            else if (this.Cursor == Cursors.Hand)
            {
                //拖拽
                //获取方框起点坐标
                //计算拖动距离
                var disY = mousePoint.Y - this.dragPoint.Y;
                var disX = mousePoint.X - this.dragPoint.X;
                this.cutTrans.X += disX;
                this.cutTrans.Y += disY;
                this.dragPoint = mousePoint;
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

            //获取右上角顶点
            var recRightTopPoint = this.CutRectangle.TranslatePoint(new Point(CutRectangle.Width, 0), this.ImageCanvas);
            //获取左下角顶点
            var recLeftBottomPoint = this.CutRectangle.TranslatePoint(new Point(0, CutRectangle.Height), this.ImageCanvas);
            //判断
            //左距离
            var xDis = recEndPoint.X - mousePoint.X;
            var yDis = recEndPoint.Y - mousePoint.Y;

            //判断鼠标是否在右边框
            var mouseInRightLine = xDis > -1 / this.Slider.Value && xDis < 5 / this.Slider.Value && mousePoint.Y > (recRightTopPoint.Y + 1 / this.Slider.Value) && mousePoint.Y < recEndPoint.Y - 1 / Slider.Value;

            //判断鼠标是否在下边框
            var mouseInBottomLine = yDis > -1 / this.Slider.Value && yDis < 5 / this.Slider.Value && mousePoint.X > recLeftBottomPoint.X + 1 / this.Slider.Value && mousePoint.X < recEndPoint.X - 1 / Slider.Value;
            //右边的显示应该处于右顶点到右底点之间
            if (mouseInRightLine)
            {
                //处于右边
                this.Cursor = Cursors.SizeWE;
            }
            else if (mouseInBottomLine)
            {
                //处于下方
                this.Cursor = Cursors.SizeNS;
            }
            else
            {
                this.Cursor = null;
            }
        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            var point = this.GetCurrentPixelPoint(e);
            this.ImageMouseMoveEvent?.Invoke(point);
            // this.lbPosition.Text = point.X + Environment.NewLine + point.Y;
        }

        private Point GetCurrentPixelPoint(MouseEventArgs e)
        {
            var ims = (BitmapSource)this.Img.Source;

            var mousePoint = e.GetPosition(this.Img);

            this.w = ims.PixelWidth / this.Img.ActualWidth;
            var pixelMousePositionX = mousePoint.X * this.w;

            this.h = ims.PixelHeight / this.Img.ActualHeight;

            var pixelMousePositionY = mousePoint.Y * this.h;
            return new Point(pixelMousePositionX, pixelMousePositionY);

        }

        private void CutRectButton_Click(object sender, RoutedEventArgs e)
        {
            this.CutPanel.Visibility = Visibility.Collapsed;
            this.CutImageEvent?.Invoke(sender, this.GetChooseRect());
            this.CutImageDownEvent?.Invoke(sender, this.GetChooseRectImageSouce());
            this.Slider.Value = 1;
            this.CutRectButton.IsChecked = false;
            this.isDrag = false;
            this.ClearRectangle();
        }

        private void CutTempCancelClick(object sender, RoutedEventArgs e)
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

        private void ClearRectangle()
        {
            this.CutPanel.Visibility = Visibility.Collapsed;
            this.cuting = false;
            //清空所有画出来的框
            this.CutRectangle.Width = 0;
            this.CutRectangle.Height = 0;
        }

        private void CutRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Cursor != null)
            {
                return;
            }
            this.Cursor = Cursors.Hand;
            this.dragPoint = e.GetPosition(this.ImageCanvas);
        }

        private PixelTrans GetPixelTrans
        {
            get
            {
                var ims = (BitmapSource)this.Img.Source;
                if (ims == null)
                {
                    return new PixelTrans();
                }
                this.w = ims.PixelWidth / this.Img.ActualWidth;
                this.h = ims.PixelHeight / this.Img.ActualHeight;
                return new PixelTrans(this.w, this.h);
            }
        }

        private struct PixelTrans
        {
            public double W { get; set; }
            public double H { get; set; }
            public PixelTrans(double w, double h)
            {
                this.W = w;
                this.H = h;
            }
        }
        #endregion
    }


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
        Rectangle
    }

    /// <summary>
    /// 工具箱按钮集合
    /// </summary>
    public class ToolButtonCollection : FreezableCollection<ButtonBase>
    {

        /// <inheritdoc/>
        protected override bool FreezeCore(bool isChecking) => !isChecking;

        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore() => new ToolButtonCollection();
    }
}
