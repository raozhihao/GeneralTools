using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ZoomBox : Control
    {
        private Thumb zoomThumb;
        private Thumb dragThumb;
        private Canvas zoomCanvas;
        private Slider zoomSlider;
        private Label resetLabel;
        private ScaleTransform scaleTransform;
        private DesignerCanvas designerCanvas;

        /// <summary>
        /// 
        /// </summary>
        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ScrollViewerProperty =
            DependencyProperty.Register(nameof(ScrollViewer), typeof(ScrollViewer), typeof(ZoomBox), new PropertyMetadata(null, OnScrollViewerChanged));

        private static void OnScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomBox ctl = d as ZoomBox;
            ctl.FindParts();
        }

        static ZoomBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoomBox), new FrameworkPropertyMetadata(typeof(ZoomBox)));
        }

        private bool templateApplied = false;
        private bool eventsAdded = false;
        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            templateApplied = true;

            FindParts();
        }


        private void FindParts()
        {
            if (eventsAdded)
            {
                designerCanvas.LayoutUpdated -= DesignerCanvas_LayoutUpdated;
                zoomThumb.DragDelta -= Thumb_DragDelta;
                zoomSlider.ValueChanged -= ZoomSlider_ValueChanged;
                resetLabel.MouseDoubleClick -= Reset_MouseDoubleClick;
                eventsAdded = false;
            }
            if (ScrollViewer == null || !templateApplied)
                return;

            designerCanvas = ScrollViewer.Content as DesignerCanvas;
            if (designerCanvas == null)
                return;

            zoomThumb = Template.FindName("PART_ZoomThumb", this) as Thumb;
            if (zoomThumb == null)
                throw new Exception("PART_ZoomThumb template is missing!");

            dragThumb = Template.FindName("PART_DragThumb", this) as Thumb;
            if (dragThumb == null)
                throw new Exception("PART_DragThumb template is missing!");

            zoomCanvas = Template.FindName("PART_ZoomCanvas", this) as Canvas;
            if (zoomCanvas == null)
                throw new Exception("PART_ZoomCanvas template is missing!");

            zoomSlider = Template.FindName("PART_ZoomSlider", this) as Slider;
            if (zoomSlider == null)
                throw new Exception("PART_ZoomSlider template is missing!");

            resetLabel = Template.FindName("PART_Reset", this) as Label;
            if (resetLabel != null)
                resetLabel.MouseDown += Reset_MouseDoubleClick;

            designerCanvas.LayoutUpdated += DesignerCanvas_LayoutUpdated;
            zoomThumb.DragDelta += Thumb_DragDelta;

            zoomSlider.ValueChanged += ZoomSlider_ValueChanged;
            eventsAdded = true;

            scaleTransform = new ScaleTransform();
            TransformGroup group = new TransformGroup();
            group.Children.Add(scaleTransform);
            group.Children.Add(new TranslateTransform());

            designerCanvas.LayoutTransform = group;
            designerCanvas.ScaleChanged += DesignerCanvas_ScaleChanged;
            designerCanvas.ZoomPanelVisibilityChangedEvent += DesignerCanvas_ZoomPanelVisibilityChangedEvent;
            designerCanvas.AutoZoomChangedEvent += DesignerCanvas_AutoZoomChangedEvent;
            this.designerCanvas.SizeChanged += DesignerCanvas_SizeChanged1;
            this.ScrollViewer.SizeChanged += ScrollViewer_SizeChanged;
            this.designerCanvas.Loaded += DesignerCanvas_Loaded;
            this.zoomCanvas.SizeChanged += ZoomCanvas_SizeChanged;
            this.Loaded += ZoomBox_Loaded;

            VisualBrush visual = new VisualBrush
            {
                Visual = ScrollViewer.Content as Visual
            };
            zoomCanvas.Background = visual;
        }

        private void DesignerCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            this.DesignerCanvas_AutoZoomChangedEvent(this.designerCanvas.AutoZoom);
        }

        private void DesignerCanvas_SizeChanged1(object sender, SizeChangedEventArgs e)
        {
            if (this.designerCanvas.AutoZoom)
            {
                this.DesignerCanvas_AutoZoomChangedEvent(true);
            }
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.designerCanvas.AutoZoom)
            {
                this.DesignerCanvas_AutoZoomChangedEvent(true);
            }
        }


        private  void DesignerCanvas_AutoZoomChangedEvent(bool zoom)
        {
            if (!this.IsLoaded) return;
            InvalidateScale(out double scale, out double xOffset, out double yOffset);
            Trace.WriteLine(this.ScrollViewer.ComputedHorizontalScrollBarVisibility + "  ||  " + this.ScrollViewer.ComputedVerticalScrollBarVisibility);
            if (zoom)
            {
                ////反向推衍画布的Scale x y

                this.zoomSlider.Value = 100;
                this.ScrollViewer.UpdateLayout();
                var visible = this.ScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible || this.ScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible;
                var sliderValue = this.zoomSlider.Value;
                if (visible)
                {
                    sliderValue -= 1;
                    this.zoomSlider.Value = sliderValue;
                    this.ScrollViewer.UpdateLayout();
                    visible = this.ScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible || this.ScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible;
                    while (visible)
                    {
                        this.zoomSlider.Value -= 1;
                        this.ScrollViewer.UpdateLayout();
                        visible = this.ScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible || this.ScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible;
                        if (this.zoomSlider.Value <= this.zoomSlider.Minimum)
                        {
                            break;
                        }
                    }
                }



                //double scale = e.NewValue / e.OldValue;

                //double halfViewportHeight = ScrollViewer.ViewportHeight / 2;
                //double newVerticalOffset = ((ScrollViewer.VerticalOffset + halfViewportHeight) * scale -
                //                            halfViewportHeight);

                //double halfViewportWidth = ScrollViewer.ViewportWidth / 2;
                //double newHorizontalOffset = ((ScrollViewer.HorizontalOffset + halfViewportWidth) * scale -
                //                              halfViewportWidth);

                //scaleTransform.ScaleX *= scale;
                //scaleTransform.ScaleY *= scale;

                //ScrollViewer.ScrollToHorizontalOffset(newHorizontalOffset);
                //ScrollViewer.ScrollToVerticalOffset(newVerticalOffset);

            }
            else
            {
                this.zoomSlider.Value = 100;
            }
        }

        private void ZoomBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ZoomCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void DesignerCanvas_ZoomPanelVisibilityChangedEvent(bool obj)
        {
            Visibility = obj ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Reset_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (zoomSlider != null)
            {
                zoomSlider.Value = 100;
            }
            e.Handled = true;
            try
            {
                SaveBitmap();
            }
            catch (Exception)
            {

            }
        }

        private void SaveBitmap()
        {
            RenderTargetBitmap targetBitmap = new RenderTargetBitmap(
            (int)designerCanvas.DesiredSize.Width,
            (int)designerCanvas.DesiredSize.Height,
            96,
            96,
            PixelFormats.Default
            );

            targetBitmap.Render(designerCanvas);
            BmpBitmapEncoder encoer = new BmpBitmapEncoder();
            encoer.Frames.Add(BitmapFrame.Create(targetBitmap));
            using (System.IO.FileStream fs = System.IO.File.Open("back.bmp", System.IO.FileMode.OpenOrCreate))
            {
                encoer.Save(fs);
            }
        }

        private void DesignerCanvas_ScaleChanged(double scale)
        {
            zoomSlider.ValueChanged -= ZoomSlider_ValueChanged;
            zoomSlider.Value = scale;
            zoomSlider.ValueChanged += ZoomSlider_ValueChanged;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ScrollViewer != null)
            {
                double scale = e.NewValue / e.OldValue;

                double halfViewportHeight = ScrollViewer.ViewportHeight / 2;
                double newVerticalOffset = ((ScrollViewer.VerticalOffset + halfViewportHeight) * scale -
                                            halfViewportHeight);

                double halfViewportWidth = ScrollViewer.ViewportWidth / 2;
                double newHorizontalOffset = ((ScrollViewer.HorizontalOffset + halfViewportWidth) * scale -
                                              halfViewportWidth);

                scaleTransform.ScaleX *= scale;
                scaleTransform.ScaleY *= scale;

                ScrollViewer.ScrollToHorizontalOffset(newHorizontalOffset);
                ScrollViewer.ScrollToVerticalOffset(newVerticalOffset);
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ScrollViewer != null)
            {
                InvalidateScale(out double scale, out _, out _);

                ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + e.HorizontalChange / scale);
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + e.VerticalChange / scale);
            }
        }

        private void DesignerCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            if (ScrollViewer != null)
            {
                InvalidateScale(out double scale, out double xOffset, out double yOffset);

                var Width = ScrollViewer.ViewportWidth * scale;
                var Height = ScrollViewer.ViewportHeight * scale;
                if (Width + xOffset > this.zoomCanvas.ActualWidth)
                    Width = this.zoomCanvas.ActualWidth - xOffset;
                if (Height + yOffset > this.zoomCanvas.ActualHeight)
                    Height = this.zoomCanvas.ActualHeight - yOffset;

                this.zoomThumb.Width = Width;
                this.zoomThumb.Height = Height;

                Canvas.SetLeft(zoomThumb, xOffset + ScrollViewer.HorizontalOffset * scale);
                Canvas.SetTop(zoomThumb, yOffset + ScrollViewer.VerticalOffset * scale);

                if (Parent is UIElement u)
                {
                    double bottom = Margin.Bottom;
                    double right = Margin.Right;
                    if (Margin.Bottom + ActualHeight > u.DesiredSize.Height)
                    {
                        bottom = u.DesiredSize.Height - ActualHeight;
                    }
                    if (Margin.Right + ActualWidth > u.DesiredSize.Width)
                    {
                        right = u.DesiredSize.Width - ActualWidth;
                    }
                    Margin = new Thickness(0, 0, right, bottom);
                }


            }
        }

        private void InvalidateScale(out double scale, out double xOffset, out double yOffset)
        {
            // designer canvas size
            double w = designerCanvas.ActualWidth * scaleTransform.ScaleX;
            double h = designerCanvas.ActualHeight * scaleTransform.ScaleY;

            // zoom canvas size
            double x = zoomCanvas.ActualWidth;
            double y = zoomCanvas.ActualHeight;

            double scaleX = x / w;
            double scaleY = y / h;

            scale = (scaleX < scaleY) ? scaleX : scaleY;

            xOffset = (x - scale * w) / 2;
            yOffset = (y - scale * h) / 2;
        }
    }
}
