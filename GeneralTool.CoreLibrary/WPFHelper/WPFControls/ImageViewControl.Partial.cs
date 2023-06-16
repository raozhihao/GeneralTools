using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls
{
    public partial class ImageViewControl
    {
        #region Public 字段

        private DrawType drawType = DrawType.None;

        /// <summary>
        /// 截图框Tooltip
        /// </summary>
        public static readonly DependencyProperty CutPanelToolTipProperty = DependencyProperty.Register(nameof(CutPanelToolTip), typeof(object), typeof(ImageViewControl), new PropertyMetadata(null));

        /// <summary>
        /// 截取框的最大尺寸
        /// </summary>
        public static readonly DependencyProperty CutPanelMaxSizeProperty = DependencyProperty.Register(nameof(CutPanelMaxSize), typeof(Size), typeof(ImageViewControl), new PropertyMetadata(Size.Empty));

        /// <summary>
        /// 截取框的最小尺寸
        /// </summary>
        public static readonly DependencyProperty CutPanelMinSizeProperty = DependencyProperty.Register(nameof(CutPanelMinSize), typeof(Size), typeof(ImageViewControl), new PropertyMetadata(new Size(0, 0)));

        /// <summary>
        /// 双击截取框时是否通知图片
        /// </summary>
        public static readonly DependencyProperty DoubleClickRaiseImageProperty = DependencyProperty.Register(nameof(DoubleClickRaiseImage), typeof(bool), typeof(ImageViewControl), new PropertyMetadata(true));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CanMoveImageProperty = DependencyProperty.Register(nameof(CanMoveImage),
            typeof(bool), typeof(ImageViewControl));

        /// <summary>
        /// 设置或获取是否允许拖动画布
        /// </summary>
        public bool CanMoveImage
        {
            get => (bool)GetValue(CanMoveImageProperty);
            set => SetValue(CanMoveImageProperty, value);
        }

        /// <summary>
        /// 截取框的最小尺寸
        /// </summary>
        public Size CutPanelMinSize
        {
            get => (Size)GetValue(CutPanelMinSizeProperty);
            set => SetValue(CutPanelMinSizeProperty, value);
        }

        /// <summary>
        /// 截取框的最大尺寸
        /// </summary>
        public Size CutPanelMaxSize
        {
            get => (Size)GetValue(CutPanelMaxSizeProperty);
            set => SetValue(CutPanelMaxSizeProperty, value);
        }

        /// <summary>
        /// 双击截取框时是否通知图片
        /// </summary>
        public bool DoubleClickRaiseImage
        {
            get => (bool)GetValue(DoubleClickRaiseImageProperty);
            set => SetValue(DoubleClickRaiseImageProperty, value);
        }

        /// <summary>
        /// 自定义图形集合
        /// </summary>
        public ObservableCollection<BaseShape> CustomeShapes
        {
            get => GetValue(CustomeShapesProperty) as ObservableCollection<BaseShape>;
            set => SetValue(CustomeShapesProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CustomeShapesProperty = DependencyProperty.Register(nameof(CustomeShapes), typeof(ObservableCollection<BaseShape>), typeof(ImageViewControl), new PropertyMetadata(new ObservableCollection<BaseShape>(), ShapesChanged));

        private static void ShapesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageViewControl v)
            {
                if (e.NewValue is ObservableCollection<BaseShape> s)
                {
                    foreach (BaseShape item in s)
                    {
                        item.CreateShape();
                    }
                }
                else
                {
                    v.ClearCustomShapes();
                }
            }
        }

        /// <summary>
        /// 清除所有自定义图形
        /// </summary>
        public void ClearCustomShapes()
        {
            if (CustomeShapes != null)
            {
                foreach (BaseShape item in CustomeShapes)
                {
                    RemoveCustomeShape(item);
                }
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CanImageDrawProperty = DependencyProperty.Register(nameof(CanImageDraw), typeof(bool), typeof(ImageViewControl));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BitmapProperty = DependencyProperty.Register(nameof(Bitmap), typeof(System.Drawing.Bitmap), typeof(ImageViewControl), new PropertyMetadata(BitmapChanged));

        private static void BitmapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageViewControl v)
            {
                (e.OldValue as System.Drawing.Bitmap)?.Dispose();
                System.Drawing.Bitmap value = e.NewValue as System.Drawing.Bitmap;
                if (e.NewValue == null)
                {
                    //清除原有
                    v.ResertImageSource();
                }
                else
                {

                    if (v.writeable == null || v.writeable.Width != value.Width || v.writeable.Height != value.Height)
                    {
                        v.writeable = null;

                        value.WriteBitmap(ref v.writeable, true);
                        v.ImageSource = v.writeable;
                    }
                    else
                    {
                        value.WriteBitmap(ref v.writeable, false);
                        v.Img.Source = v.writeable;
                    }
                }
                //初始化最大尺寸
                if (value != null && v.CutPanelMaxSize.IsEmpty)
                {
                    v.CutPanelMaxSize = new Size(value.Width, value.Height);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register(nameof(SourcePath), typeof(string), typeof(ImageViewControl), new PropertyMetadata(SourcePathChanged));

        private static void SourcePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageViewControl v)
            {
                if (string.IsNullOrWhiteSpace(e.NewValue + ""))
                {
                    v.Bitmap?.Dispose();
                    v.Bitmap = null;
                    return;
                }
                v.Bitmap = new System.Drawing.Bitmap(e.NewValue + "");
            }
        }

        #endregion Public 字段

        #region Private 字段

        private WriteableBitmap writeable;

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

        /// <summary>
        /// 在图像进行进行鼠标单击时触发事件
        /// </summary>
        [Description("在图像进行缩放时触发事件"), Category("自定义事件")]
        public event EventHandler<MouseButtonEventArgs> ImageMouseDownEvent;

        #endregion Public 事件

        #region Public 属性

        /// <summary>
        /// 设置或获取是否能够对控件进行绘制截图操作
        /// </summary>
        public bool CanImageDraw
        {
            get => (bool)GetValue(CanImageDrawProperty);
            set => SetValue(CanImageDrawProperty, value);
        }

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
            get => (Visibility)GetValue(CutButtonVisibilityProperty);
            set => SetValue(CutButtonVisibilityProperty, value);
        }

        /// <summary>
        /// 当前图片框的显示高度
        /// </summary>
        public double? ImageActualHeight
        {
            get => Img?.ActualHeight;
        }

        /// <summary>
        /// 当前图片框的显示宽度
        /// </summary>
        public double? ImageActualWidth
        {
            get => Img?.ActualWidth;
        }

        /// <summary>
        /// 截图框Tooltip
        /// </summary>
        [Description("截图框Tooltip"), Category("自定义属性")]
        public object CutPanelToolTip
        {
            get => GetValue(CutPanelToolTipProperty);
            set => SetValue(CutPanelToolTipProperty, value);
        }

        /// <summary>
        /// 最大缩放倍数
        /// </summary>
        [Description("最大缩放倍数"), Category("自定义属性")]
        public double ImageMaxScaleValue
        {
            get => Convert.ToInt32(GetValue(ImageMaxScaleValueProperty));

            set
            {
                if (value < 1.0)
                    value = 1.0;
                SetValue(ImageMaxScaleValueProperty, value);
            }
        }

        /// <summary>
        /// 当前缩放倍数
        /// </summary>
        [Description("当前缩放倍数"), Category("自定义属性")]
        public double ImageScale
        {
            get => Convert.ToDouble(GetValue(ImageScaleProperty));

            set
            {
                if (value < 1.0)
                {
                    value = 1.0;
                }

                SetValue(ImageScaleProperty, value);
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
                if (Img == null)
                    return null;
                object source = GetValue(ImageSourceProperty);
                return source == null ? null : (ImageSource)source;
            }
            set
            {
                if (value == null) return;
                if (Img != null)
                    BindingOperations.ClearBinding(Img, Image.SourceProperty);
                //初始化最大尺寸
                if (value != null && CutPanelMaxSize.IsEmpty)
                {
                    CutPanelMaxSize = new Size(value.Width, value.Height);
                    //this.ImageSource = null;
                }

                Img?.ClearValue(Image.SourceProperty);
                SetValue(ImageSourceProperty, null);

                Img?.SetValue(Image.SourceProperty, null);
                SetValue(ImageSourceProperty, value);

                if (value == writeable)
                {
                }
                else if (value != null)
                {
                    if (value is BitmapSource b)
                    {
                        using (System.IO.MemoryStream m = new System.IO.MemoryStream())
                        {
                            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(b));
                            encoder.Save(m);

                            using (System.Drawing.Bitmap map = new System.Drawing.Bitmap(m))
                            {
                                bool reload = false;
                                if (writeable == null || writeable.PixelWidth != map.Width || writeable.PixelHeight != map.Height)
                                    reload = true;
                                map.WriteBitmap(ref writeable, reload);
                            }

                        }
                    }
                }

                Img?.SetValue(Image.SourceProperty, writeable);
            }
        }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string SourcePath
        {
            get => GetValue(SourcePathProperty) + "";
            set
            {
                SetValue(SourcePathProperty, value);

            }
        }

        /// <summary>
        /// Bitmap图片
        /// </summary>
        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                if (Img == null)
                    return null;
                object source = GetValue(BitmapProperty);
                return source == null ? null : (System.Drawing.Bitmap)source;
            }
            set
            {

                SetValue(BitmapProperty, value);
            }
        }

        /// <summary>
        /// 右侧工具条是否展开
        /// </summary>
        [Description("右侧工具条是否展开"), Category("自定义属性")]
        public bool IsToolExpanderExpanded
        {
            get => (bool)GetValue(IsToolExpanderExpandedProperty);
            set => SetValue(IsToolExpanderExpandedProperty, value);
        }

        /// <summary>
        /// 取消截图按钮样式
        /// </summary>
        [Description("取消截图按钮样式"), Category("自定义属性")]
        public Style MenuCancelStyle
        {
            get => (Style)GetValue(MenuCancelStyleProperty);
            set => SetValue(MenuCancelStyleProperty, value);
        }

        /// <summary>
        /// 确定截图按钮样式
        /// </summary>
        [Description("确定截图按钮样式"), Category("自定义属性")]
        public Style MenuOkStyle
        {
            get => (Style)GetValue(MenuOkStyleProperty);
            set => SetValue(MenuOkStyleProperty, value);
        }

        /// <summary>
        /// 鼠标在图像上移动时当前的像素点位置
        /// </summary>
        [Description("鼠标在图像上移动时当前的像素点位置"), Category("自定义属性")]
        public Point MouseOverPoint => Img.IsMouseOver ? GetCurrentPixelPoint(new MouseEventArgs(Mouse.PrimaryDevice, 1)) : new Point();

        /// <summary>
        /// 左侧滚动条样式
        /// </summary>
        [Description("左侧滚动条样式"), Category("自定义属性")]
        public Style SliderStyle
        {
            get => (Style)GetValue(SliderStyleProperty);
            set => SetValue(SliderStyleProperty, value);
        }

        /// <summary>
        /// 缩放条的显示状态
        /// </summary>
        [Description("缩放条的显示状态"), Category("自定义属性")]
        public Visibility SliderVisibility
        {
            get => (Visibility)GetValue(SliderVisibilityProperty);
            set { SetValue(SliderVisibilityProperty, value); }
        }

        /// <summary>
        /// 右侧工具条截图按钮样式
        /// </summary>
        [Description("右侧工具条截图按钮样式"), Category("自定义属性")]
        public Style ToolCutButtonStyle
        {
            get => (Style)GetValue(ToolCutButtonStyleProperty);
            set => SetValue(ToolCutButtonStyleProperty, value);
        }

        /// <summary>
        /// 右侧工具条样式
        /// </summary>
        [Description("右侧工具条样式"), Category("自定义属性")]
        public Style ToolExpanderStyle
        {
            get => (Style)GetValue(ToolExpanderStyleProperty);
            set => SetValue(ToolExpanderStyleProperty, value);
        }

        /// <summary>
        /// 右侧工具条显示状态
        /// </summary>
        [Description("右侧工具条显示状态"), Category("自定义属性")]
        public Visibility ToolExpanderVisibility
        {
            get => (Visibility)GetValue(ToolExpanderVisibilityProperty);
            set => SetValue(ToolExpanderVisibilityProperty, value);
        }

        #endregion Public 属性

        #region Private 属性

        private PixelTrans GetPixelTrans
        {
            get
            {
                BitmapSource ims = (BitmapSource)Img.Source;
                if (ims == null)
                {
                    return new PixelTrans();
                }
                w = ims.PixelWidth / Img.ActualWidth;
                h = ims.PixelHeight / Img.ActualHeight;
                return new PixelTrans(w, h);
            }
        }

        #endregion Private 属性
    }
}
