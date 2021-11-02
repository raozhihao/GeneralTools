using GeneralTool.General.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ImageTest
{
    partial class PictureView
    {
        /// <summary>
        /// 图像Source依赖属性
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(PictureView), new PropertyMetadata(null, new PropertyChangedCallback(ImageSouceChanged)));

        private static void ImageSouceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PictureView p)
                p.Image.Source = e.NewValue as ImageSource;
        }

        public ImageSource ImageSource
        {
            get => this.GetValue(ImageSourceProperty) as ImageSource;
            set => this.SetValue(ImageSourceProperty, value);
        }

        /// <summary>
        /// 双击截取框时是否通知图片
        /// </summary>
        public static readonly DependencyProperty DoubleClickRaiseImageProperty = DependencyProperty.Register(nameof(DoubleClickRaiseImage), typeof(bool), typeof(PictureView), new PropertyMetadata(true));

        /// <summary>
        /// 双击截取框时是否通知图片
        /// </summary>
        public bool DoubleClickRaiseImage
        {
            get => (bool)this.GetValue(DoubleClickRaiseImageProperty);
            set => this.SetValue(DoubleClickRaiseImageProperty, value);
        }


        public static readonly DependencyProperty ScaleMaxValueProperty =
          DependencyProperty.Register(nameof(ScaleMaxValue),
              typeof(int),
              typeof(PictureView),
              new PropertyMetadata(25, new PropertyChangedCallback(ScaleMaxValueChanged)));

        private static void ScaleMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PictureView p)
            {
                if (int.TryParse(e.NewValue + "", out var max))
                    p.ScaleSlider.Maximum = max;
            }
        }

        public int ScaleMaxValue
        {
            get => Convert.ToInt32(this.GetValue(ScaleMaxValueProperty));
            set => this.SetValue(ScaleMaxValueProperty, value);
        }

        /// <summary>
        /// 当前缩放倍数
        /// </summary>
        public double ScaleValue
        {
            get => this.ScaleSlider.Value;
        }

        public static readonly DependencyProperty RectContextMenuProperty = DependencyProperty.Register("RectContextMenu", typeof(ContextMenu), typeof(PictureView), new PropertyMetadata(new PropertyChangedCallback(RectContextMenuChanged)));

        private static void RectContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PictureView p)
            {
                var menu = e.NewValue as ContextMenu;
                p.CutRect.SetValue(ContextMenuProperty, menu);
            }
        }

        public static void SetRectContextMenu(DependencyObject d, ContextMenu menu) => d.SetValue(RectContextMenuProperty, menu);
        public static ContextMenu GetRectContextMenu(DependencyObject d) => d.GetValue(RectContextMenuProperty) as ContextMenu;



        public static readonly DependencyProperty ImageContextMenuProperty = DependencyProperty.Register("ImageContextMenu", typeof(ContextMenu), typeof(PictureView), new PropertyMetadata(new PropertyChangedCallback(ImageContextMenuChanged)));

        private static void ImageContextMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PictureView p)
            {
                var menu = e.NewValue as ContextMenu;
                p.Image.SetValue(ContextMenuProperty, menu);
            }
        }

        public static void SetImageContextMenu(DependencyObject d, ContextMenu menu) => d.SetValue(RectContextMenuProperty, menu);
        public static ContextMenu GetImageContextMenu(DependencyObject d) => d.GetValue(RectContextMenuProperty) as ContextMenu;

        public static readonly DependencyProperty CutRectOpacityProperty = DependencyProperty.Register("CutRectOpacity", typeof(double), typeof(PictureView), new PropertyMetadata(0.4, new PropertyChangedCallback(CutRectOpacityChanged)));
        private static void CutRectOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PictureView p) p.CutRect.Opacity = (double)e.NewValue;
        }

        public static void SetCutRectOpacity(DependencyObject d, double value) => d.SetValue(CutRectOpacityProperty, value);
        public static double GetCutRectOpacity(DependencyObject d) => (double)d.GetValue(CutRectOpacityProperty);


        /// <summary>
        /// 确定截图成功后触发事件
        /// </summary>
        [Description("确定截图成功后触发事件"), Category("自定义事件")]
        public event EventHandler<ImageEventArgs> CutImageDownEvent;



        /// <summary>
        /// 在图片上点击时
        /// </summary>
        [Description("确定截图成功后触发事件"), Category("自定义事件")]
        public event EventHandler<Point> ImageDownEvent;




        /// <summary>
        /// 在图片上点击弹起时
        /// </summary>
        [Description("确定截图成功后触发事件"), Category("自定义事件")]
        public event EventHandler<Point> ImageUpEvent;

        /// <summary>
        /// 鼠标在画面上移动事件
        /// </summary>
        [Description("鼠标在画面上移动事件"), Category("自定义事件")]
        public event EventHandler<ImageMouseEventArgs> ImageMouseMoveEvent;

        public void AddShape(Shape shape, double x, double y)
        {
            shape.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
            shape.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);

            shape.RenderTransform = new TranslateTransform() { X = x - shape.Width / 2, Y = y - shape.Height / 2 };
            this.GridImageContent.Children.Add(shape);
        }

        public void ClearShape(Shape shape) => this.GridImageContent.Children.Remove(shape);


        public static readonly DependencyProperty CurrentPointProperty = DependencyProperty.Register(nameof(CurrentPoint), typeof(Point), typeof(PictureView));
        public Point CurrentPoint
        {
            get => (Point)this.GetValue(CurrentPointProperty);
            private set => this.SetValue(CurrentPointProperty, value);
        }
    }
}
