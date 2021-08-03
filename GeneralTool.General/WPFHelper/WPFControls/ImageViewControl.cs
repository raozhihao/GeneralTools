using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GeneralTool.General.WPFHelper.WPFControls
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
        Rectangle
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

    /// <summary> 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 元素中:
    ///
    /// xmlns:MyNamespace="clr-namespace:GeneralTool.General.WPFHelper.WPFControls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 元素中:
    ///
    /// xmlns:MyNamespace="clr-namespace:GeneralTool.General.WPFHelper.WPFControls;assembly=GeneralTool.General.WPFHelper.WPFControls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用， 并重新生成以避免编译错误:
    ///
    /// 在解决方案资源管理器中右击目标项目，然后依次单击 “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2) 继续操作并在 XAML 文件中使用控件。
    ///
    /// <MyNamespace:ImageViewControl/>
    ///
    /// </summary>
    [StyleTypedProperty(Property = nameof(ToolExpanderStyle), StyleTargetType = typeof(Expander))]
    [StyleTypedProperty(Property = nameof(SliderStyle), StyleTargetType = typeof(Slider))]
    [StyleTypedProperty(Property = nameof(ToolCutButtonStyle), StyleTargetType = typeof(ToggleButton))]
    [StyleTypedProperty(Property = nameof(MenuOkStyle), StyleTargetType = typeof(Button))]
    [StyleTypedProperty(Property = nameof(MenuCancelStyle), StyleTargetType = typeof(Button))]
    public class ImageViewControl : Control
    {
        #region Public 字段

        /// <summary>
        /// 向图像控件中增加右键菜单,需要使用 ContextItemCollection
        /// </summary>
        public static readonly DependencyProperty ContextMenus = DependencyProperty.RegisterAttached("ContextMenus", typeof(ContextMenu), typeof(ImageViewControl), new FrameworkPropertyMetadata(MenuItemsChanged));

        /// <summary>
        /// 右侧工具条截图按钮显示状态
        /// </summary>
        public static readonly DependencyProperty CutButtonVisibilityProperty = DependencyProperty.Register(nameof(CutButtonVisibility), typeof(Visibility), typeof(ImageViewControl), new FrameworkPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 最大缩放倍数
        /// </summary>
        public static readonly DependencyProperty ImageMaxScaleValueProperty = DependencyProperty.Register(nameof(ImageMaxScaleValue), typeof(double), typeof(ImageViewControl), new FrameworkPropertyMetadata(25.0));

        /// <summary>
        /// 当前缩放倍数
        /// </summary>
        public static readonly DependencyProperty ImageScaleProperty = DependencyProperty.Register(nameof(ImageScale), typeof(double), typeof(ImageViewControl), new PropertyMetadata(1.0, ValueChanged));

        /// <summary>
        /// 当前图像源
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(ImageViewControl), new PropertyMetadata(ImageSourceChanged));

        /// <summary>
        /// 右侧工具条是否展开
        /// </summary>
        public static readonly DependencyProperty IsToolExpanderExpandedProperty = DependencyProperty.Register(nameof(IsToolExpanderExpanded), typeof(bool), typeof(ImageViewControl), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// 取消截图按钮样式
        /// </summary>
        public static readonly DependencyProperty MenuCancelStyleProperty = DependencyProperty.Register(nameof(MenuCancelStyle), typeof(Style), typeof(ImageViewControl));

        /// <summary>
        /// 确定截图按钮样式
        /// </summary>
        public static readonly DependencyProperty MenuOkStyleProperty = DependencyProperty.Register(nameof(MenuOkStyle), typeof(Style), typeof(ImageViewControl));

        /// <summary>
        /// 左侧滚动条样式
        /// </summary>
        public static readonly DependencyProperty SliderStyleProperty = DependencyProperty.Register(nameof(SliderStyle), typeof(Style), typeof(ImageViewControl));

        /// <summary>
        /// 缩放条的显示状态
        /// </summary>
        public static readonly DependencyProperty SliderVisibilityProperty = DependencyProperty.Register(nameof(SliderVisibility), typeof(Visibility), typeof(ImageViewControl), new FrameworkPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 向工具条按钮中附加一组自定义控件,需要使用 ToolButtonCollection
        /// </summary>
        public static readonly DependencyProperty ToolButtons = DependencyProperty.RegisterAttached("ToolButtons", typeof(ToolButtonCollection), typeof(ImageViewControl), new FrameworkPropertyMetadata(ToolButtonsChanged));

        /// <summary>
        /// 右侧工具条截图按钮样式
        /// </summary>
        public static readonly DependencyProperty ToolCutButtonStyleProperty = DependencyProperty.Register(nameof(ToolCutButtonStyle), typeof(Style), typeof(ImageViewControl));

        /// <summary>
        /// 右侧工具条样式
        /// </summary>
        public static readonly DependencyProperty ToolExpanderStyleProperty = DependencyProperty.Register(nameof(ToolExpanderStyle), typeof(Style), typeof(ImageViewControl), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// 右侧工具条显示状态
        /// </summary>
        public static readonly DependencyProperty ToolExpanderVisibilityProperty = DependencyProperty.Register(nameof(ToolExpanderVisibility), typeof(Visibility), typeof(ImageViewControl), new FrameworkPropertyMetadata(Visibility.Visible));

        #endregion Public 字段

        #region Private 字段

        /// <summary>
        /// 当前鼠标在滚动的时候的坐标
        /// </summary>
        private Point currentWheelPoint;

        /// <summary>
        /// 当前按下的坐标
        /// </summary>
        private Point curretnMouseDownPoint;

        private bool cuting;

        private StackPanel CutPanel;

        private Rectangle CutRectangle;

        private ToggleButton CutRectButton;

        private TranslateTransform cutTrans;

        private Point dragPoint;

        private Grid GridBox;

        /// <summary>
        /// 定义变幻组合对象
        /// </summary>
        private TransformGroup group;

        /// <summary>
        /// 当前图片的缩放h比例
        /// </summary>
        private double h;

        private double height = 0;

        private Grid ImageBox;

        private Canvas ImageCanvas;

        private ScrollViewer ImageScroll;

        private Viewbox ImageViewBox;

        private Image Img;

        /// <summary>
        /// 是否处于拖动状态
        /// </summary>
        private bool isDrag = false;

        private bool IsImageMouseDown = false;

        private Button MenuCancel;

        private Button MenuOk;

        private ScaleTransform scaleTrans;

        private Slider Slider;

        private StackPanel StackMenu;

        private StackPanel stackTools;

        //截图起始坐标
        private Point StartPoint;

        private IEnumerable<UIElement> toolButtons;

        private Expander ToolExpander;

        private ContextMenu toolMenus;

        /// <summary>
        /// 定义移动对象
        /// </summary>
        private TranslateTransform tt;

        /// <summary>
        /// 当前图片的缩放w比例
        /// </summary>
        private double w;

        //截图的长宽
        private double width = 0;

        #endregion Private 字段

        #region Public 构造函数

        static ImageViewControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageViewControl), new FrameworkPropertyMetadata(typeof(ImageViewControl)));

        #endregion Public 构造函数

        #region Public 事件

        /// <summary>
        /// 确定截图成功后触发事件
        /// </summary>
        [Description("确定截图成功后触发事件"), Category("自定义事件")]
        public event EventHandler<ImageEventArgs> CutImageDownEvent;

        /// <summary>
        /// 确定截图成功后触发事件
        /// </summary>
        [Description("确定截图成功后触发事件"), Category("自定义事件")]
        public event EventHandler<ImageCutRectEventArgs> CutImageEvent;

        /// <summary>
        /// 在图像进行缩放时触发事件
        /// </summary>
        [Description("在截图框状态更改时触发事件"), Category("自定义事件")]
        public event EventHandler<ImageCutRectEventArgs> CutPanelVisibleChanged;

        /// <summary>
        /// 鼠标在画面上移动事件
        /// </summary>
        [Description("鼠标在画面上移动事件"), Category("自定义事件")]
        public event Action<ImageMouseEventArgs> ImageMouseMoveEvent;

        /// <summary>
        /// 在图像进行缩放时触发事件
        /// </summary>
        [Description("在图像进行缩放时触发事件"), Category("自定义事件")]
        public event EventHandler<ImageScaleEventArgs> MouseWheelScaleEvent;

        #endregion Public 事件

        #region Public 属性

        /// <summary>
        /// 设置或获取是否能够对控件进行绘制截图操作
        /// </summary>
        public bool CanImageDraw { get; set; }

        /// <summary>
        /// 保存当前点击的像素点坐标
        /// </summary>
        [Description("保存当前点击的像素点坐标"), Category("自定义属性")]
        public Point CurrentMouseDownPixelPoint
        {
            get; private set;
        }

        /// <summary>
        /// 保存当前点击的屏幕坐标
        /// </summary>
        [Description("保存当前点击的屏幕坐标"), Category("自定义属性")]
        public Point CurrentMouseDownPoint { get; private set; }

        /// <summary>
        /// 右侧工具条截图按钮显示状态
        /// </summary>
        [Description("右侧工具条截图按钮显示状态"), Category("自定义属性")]
        public Visibility CutButtonVisibility
        {
            get => (Visibility)this.GetValue(CutButtonVisibilityProperty);
            set => this.SetValue(CutButtonVisibilityProperty, value);
        }

        /// <summary>
        /// 当前图片框的显示高度
        /// </summary>
        public double? ImageActualHeight
        {
            get => this.Img?.ActualHeight;
        }

        /// <summary>
        /// 当前图片框的显示宽度
        /// </summary>
        public double? ImageActualWidth
        {
            get => this.Img?.ActualWidth;
        }

        /// <summary>
        /// 最大缩放倍数
        /// </summary>
        [Description("最大缩放倍数"), Category("自定义属性")]
        public double ImageMaxScaleValue
        {
            get => Convert.ToInt32(this.GetValue(ImageMaxScaleValueProperty));

            set
            {
                if (value < 1.0)
                    value = 1.0;
                this.SetValue(ImageMaxScaleValueProperty, value);
            }
        }

        /// <summary>
        /// 当前缩放倍数
        /// </summary>
        [Description("当前缩放倍数"), Category("自定义属性")]
        public double ImageScale
        {
            get => Convert.ToDouble(this.GetValue(ImageScaleProperty));

            set
            {
                if (value < 1.0)
                {
                    value = 1.0;
                }

                this.SetValue(ImageScaleProperty, value);
            }
        }

        /// <summary>
        /// 当前图像源
        /// </summary>
        [Description("当前图像源"), Category("自定义属性")]
        public ImageSource ImageSource
        {
            get
            {
                if (this.Img == null)
                    return null;
                var source = this.GetValue(ImageSourceProperty);
                if (source == null)
                    return null;
                return (ImageSource)source;
            }

            set
            {
                this.SetValue(ImageSourceProperty, value);
            }
        }

        /// <summary>
        /// 右侧工具条是否展开
        /// </summary>
        [Description("右侧工具条是否展开"), Category("自定义属性")]
        public bool IsToolExpanderExpanded
        {
            get => (bool)this.GetValue(IsToolExpanderExpandedProperty);
            set => this.SetValue(IsToolExpanderExpandedProperty, value);
        }

        /// <summary>
        /// 取消截图按钮样式
        /// </summary>
        [Description("取消截图按钮样式"), Category("自定义属性")]
        public Style MenuCancelStyle
        {
            get => (Style)this.GetValue(MenuCancelStyleProperty);
            set => this.SetValue(MenuCancelStyleProperty, value);
        }

        /// <summary>
        /// 确定截图按钮样式
        /// </summary>
        [Description("确定截图按钮样式"), Category("自定义属性")]
        public Style MenuOkStyle
        {
            get => (Style)this.GetValue(MenuOkStyleProperty);
            set => this.SetValue(MenuOkStyleProperty, value);
        }

        /// <summary>
        /// 鼠标在图像上移动时当前的像素点位置
        /// </summary>
        [Description("鼠标在图像上移动时当前的像素点位置"), Category("自定义属性")]
        public Point MouseOverPoint => this.Img.IsMouseOver ? this.GetCurrentPixelPoint(new MouseEventArgs(Mouse.PrimaryDevice, 1)) : new Point();

        /// <summary>
        /// 左侧滚动条样式
        /// </summary>
        [Description("左侧滚动条样式"), Category("自定义属性")]
        public Style SliderStyle
        {
            get => (Style)this.GetValue(SliderStyleProperty);
            set => this.SetValue(SliderStyleProperty, value);
        }

        /// <summary>
        /// 缩放条的显示状态
        /// </summary>
        [Description("缩放条的显示状态"), Category("自定义属性")]
        public Visibility SliderVisibility
        {
            get => (Visibility)this.GetValue(SliderVisibilityProperty);
            set { this.SetValue(SliderVisibilityProperty, value); }
        }

        /// <summary>
        /// 右侧工具条截图按钮样式
        /// </summary>
        [Description("右侧工具条截图按钮样式"), Category("自定义属性")]
        public Style ToolCutButtonStyle
        {
            get => (Style)this.GetValue(ToolCutButtonStyleProperty);
            set => this.SetValue(ToolCutButtonStyleProperty, value);
        }

        /// <summary>
        /// 右侧工具条样式
        /// </summary>
        [Description("右侧工具条样式"), Category("自定义属性")]
        public Style ToolExpanderStyle
        {
            get => (Style)this.GetValue(ToolExpanderStyleProperty);
            set => this.SetValue(ToolExpanderStyleProperty, value);
        }

        /// <summary>
        /// 右侧工具条显示状态
        /// </summary>
        [Description("右侧工具条显示状态"), Category("自定义属性")]
        public Visibility ToolExpanderVisibility
        {
            get => (Visibility)this.GetValue(ToolExpanderVisibilityProperty);
            set => this.SetValue(ToolExpanderVisibilityProperty, value);
        }

        #endregion Public 属性

        #region Private 属性

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

        #endregion Private 属性

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
        /// 清除所有指定类型的图形
        /// </summary>
        /// <param name="drawType">
        /// </param>
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
        public void DrawRect(Int32Rect drawRect, Brush stroke, Brush fill, double thickness = 1)
        {
            if (!this.CanImageDraw) return;
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

            var rect = new Int32Rect((int)x, (int)y, (int)width, (int)height);
            return rect;
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
            this.Img.MouseMove += Img_MouseMove;
            this.Img.MouseUp += this.Img_MouseUp;

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
        public void SetPoint(Point pixelPoint, Brush brush, double r = 4)
        {
            if (!this.CanImageDraw) return;
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
            var pos = this.TranslatePoint(pixelPoint);
            if (double.IsInfinity(pos.X) || double.IsInfinity(pos.Y))
            {
                return;
            }
            myEllipse.Tag = pixelPoint;
            Canvas.SetLeft(myEllipse, pos.X - r / 2);
            Canvas.SetTop(myEllipse, pos.Y - r / 2);
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

            //if (this.toolMenus == null )
            //    return;
            //if (this.Img.ContextMenu == null)
            //    this.Img.ContextMenu = new ContextMenu();
            //foreach (var item in this.toolMenus)
            //{
            //    this.Img.ContextMenu.Items.Add(item);
            //}
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
                    System.Diagnostics.Trace.WriteLine($"重新设置截取框大小出错:{ex.Message}");
                }
                if (CutPanel.Visibility == Visibility.Visible)
                    this.CutPanelVisibleChanged?.Invoke(sender, new ImageCutRectEventArgs(this.GetChooseRect()));
            }
            else
            {
                this.ShowSizeInCutRectangleCursors();
            }

            if (e.LeftButton == MouseButtonState.Pressed && cuting && this.IsImageMouseDown)
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

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.cuting && this.IsImageMouseDown)
            {
                this.StackMenu.Visibility = Visibility.Visible;
                this.CutRectangle.Fill = new SolidColorBrush(Color.FromArgb(30, 2, 3, 4));

                this.cuting = false;
            }

            //判断当前截图框的位置
            ResizeCutPanel(e);
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

            if (e.ClickCount >= 2)
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
            this.CutPanel.Visibility = Visibility.Collapsed;
            var handler = new ImageCutRectEventArgs(this.GetChooseRect(), true);
            this.CutImageEvent?.Invoke(sender, handler);
            if (handler.HandleToNext)
            {
                var sucess = false;
                var msg = "";
                BitmapSource source = null;
                try
                {
                    source = this.GetChooseRectImageSouce();
                    sucess = true;
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                this.CutImageDownEvent?.Invoke(sender, new ImageEventArgs(source, sucess, msg));
            }

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

            //var p = e.GetPosition(this.ImageScroll);
            //var topPoint = this.ImageScroll.TranslatePoint(new Point(0, 0), this.ImageBox);
            //var bottomPoint = this.ImageScroll.TranslatePoint(new Point(ImageBox.ActualWidth, this.ImageBox.ActualHeight), this.ImageBox);

            //var moveX = p.X - this.curretnMouseDownPoint.X;
            //var moveY = p.Y - this.curretnMouseDownPoint.Y;

            ////向上向下移动条件判断（会有一点点的小偏移，如果想更精确的控制，那么分向上和向下两种情况，并判断边距）
            //if ((moveY < 0 && bottomPoint.Y > ImageBox.ActualHeight) || (moveY > 0 && topPoint.Y < 0))
            //{
            //    this.tt.Y += p.Y - this.curretnMouseDownPoint.Y;
            //    this.curretnMouseDownPoint.Y = p.Y;
            //}

            ////向左向右移动条件判断
            //if ((moveX < 0 && bottomPoint.X > ImageBox.ActualWidth) || (moveX > 0 && topPoint.X < 0))
            //{
            //    this.tt.X += p.X - this.curretnMouseDownPoint.X;
            //    this.curretnMouseDownPoint.X = p.X;
            //}

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
            //鼠标当前的point
            Point Endpoint = e.GetPosition(Img);

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
            StartLeft = this.Img.TranslatePoint(StartLeft, ImageCanvas);
            Canvas.SetLeft(this.CutPanel, StartLeft.X);
            Canvas.SetTop(this.CutPanel, StartLeft.Y);
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

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //记录鼠标按下时的坐标
            this.StartPoint = e.GetPosition(Img);
            this.CurrentMouseDownPixelPoint = GetCurrentPixelPoint(e);
            this.CurrentMouseDownPoint = this.StartPoint;
            this.IsImageMouseDown = true;
        }

        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            var point = this.GetCurrentPixelPoint(e);
            this.ImageMouseMoveEvent?.Invoke(new ImageMouseEventArgs(e.MouseDevice, e.Timestamp, e.StylusDevice, point));

            // this.Canvas_MouseMove(this.Img, e); this.lbPosition.Text = point.X +
            // Environment.NewLine + point.Y;
        }

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void InitStyles()
        {
            //获取样式表
            var dic = new ResourceDictionary { Source = new Uri("pack://application:,,,/GeneralTool.General;component/Themes/BaseStyle.xaml", UriKind.Absolute) };

            Style styleToolCutButton = (Style)dic["CutButtonIconFontStyle"];

            //初始化截图按钮
            if (this.ToolCutButtonStyle == null)
            {
                this.ToolCutButtonStyle = styleToolCutButton;
            }

            //初始化Header
            if (this.ToolExpanderStyle == null)
            {
                this.ToolExpander.Header = "工具";
            }

            Style styleMenuOk = (Style)dic["MenuOkStyle"];

            //查看OK按钮有无内容
            if (this.MenuOkStyle == null)
            {
                this.MenuOkStyle = styleMenuOk;
            }

            Style styleMenuCancel = (Style)dic["MenuCancelStyle"];

            //查看Cancel按钮有无内容
            if (this.MenuCancelStyle == null)
            {
                this.MenuCancelStyle = styleMenuCancel;
            }
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
        /// <param name="e">
        /// </param>
        private void ResizeCutPanel(MouseButtonEventArgs e)
        {
            if (this.CutPanel.Visibility == Visibility.Collapsed)
                return;

            //查看宽度是否足够
            if (this.CutRectangle.Width > this.Img.ActualWidth)
            {
                //不够,则减成足够的宽度先
                this.CutRectangle.Width = this.Img.ActualWidth;
            }

            //查看高度是否足够
            if (this.CutRectangle.Height > this.Img.ActualHeight)
            {
                //不够,则减成足够的高度先
                this.CutRectangle.Height = this.Img.ActualHeight;
            }

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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsLoaded)
            {
                this.WheelScale(this.currentWheelPoint, e.NewValue > e.OldValue ? 120d : -120d, true);
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