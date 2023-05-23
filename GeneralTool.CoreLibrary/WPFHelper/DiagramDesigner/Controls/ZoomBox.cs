using System;
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
            var ctl = d as ZoomBox;
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
                this.designerCanvas.LayoutUpdated -= this.DesignerCanvas_LayoutUpdated;
                this.zoomThumb.DragDelta -= this.Thumb_DragDelta;
                this.zoomSlider.ValueChanged -= this.ZoomSlider_ValueChanged;
                this.resetLabel.MouseDoubleClick -= Reset_MouseDoubleClick;
                eventsAdded = false;
            }
            if (this.ScrollViewer == null || !templateApplied)
                return;

            this.designerCanvas = this.ScrollViewer.Content as DesignerCanvas;
            if (this.designerCanvas == null)
                return;

            this.zoomThumb = Template.FindName("PART_ZoomThumb", this) as Thumb;
            if (this.zoomThumb == null)
                throw new Exception("PART_ZoomThumb template is missing!");


            this.dragThumb = Template.FindName("PART_DragThumb", this) as Thumb;
            if (this.dragThumb == null)
                throw new Exception("PART_DragThumb template is missing!");

            this.zoomCanvas = Template.FindName("PART_ZoomCanvas", this) as Canvas;
            if (this.zoomCanvas == null)
                throw new Exception("PART_ZoomCanvas template is missing!");

            this.zoomSlider = Template.FindName("PART_ZoomSlider", this) as Slider;
            if (this.zoomSlider == null)
                throw new Exception("PART_ZoomSlider template is missing!");

            this.resetLabel = this.Template.FindName("PART_Reset", this) as Label;
            if (this.resetLabel != null)
                this.resetLabel.MouseDown += Reset_MouseDoubleClick;

            this.designerCanvas.LayoutUpdated += this.DesignerCanvas_LayoutUpdated;
            this.zoomThumb.DragDelta += this.Thumb_DragDelta;
            this.zoomSlider.ValueChanged += this.ZoomSlider_ValueChanged;
            eventsAdded = true;

            this.scaleTransform = new ScaleTransform();
            var group = new TransformGroup();
            group.Children.Add(this.scaleTransform);
            group.Children.Add(new TranslateTransform());

            this.designerCanvas.LayoutTransform = group;
            this.designerCanvas.ScaleChanged += DesignerCanvas_ScaleChanged;
            this.designerCanvas.ZoomPanelVisibilityChangedEvent += DesignerCanvas_ZoomPanelVisibilityChangedEvent;

            var visual = new VisualBrush
            {
                Visual = this.ScrollViewer.Content as Visual
            };
            this.zoomCanvas.Background = visual;
        }

        private void DesignerCanvas_ZoomPanelVisibilityChangedEvent(bool obj)
        {
            this.Visibility = obj ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Reset_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.zoomSlider != null)
            {
                this.zoomSlider.Value = 100;
            }
            e.Handled = true;
            try
            {
                this.SaveBitmap();
            }
            catch (Exception)
            {

            }
        }

        private void SaveBitmap()
        {
            var targetBitmap = new RenderTargetBitmap(
            (int)this.designerCanvas.DesiredSize.Width,
            (int)this.designerCanvas.DesiredSize.Height,
            96,
            96,
            PixelFormats.Default
            );

            targetBitmap.Render(this.designerCanvas);
            var encoer = new BmpBitmapEncoder();
            encoer.Frames.Add(BitmapFrame.Create(targetBitmap));
            using (var fs = System.IO.File.Open("back.bmp", System.IO.FileMode.OpenOrCreate))
            {
                encoer.Save(fs);
            }
        }

        void DesignerCanvas_ScaleChanged(double scale)
        {
            this.zoomSlider.ValueChanged -= this.ZoomSlider_ValueChanged;
            zoomSlider.Value = scale;
            this.zoomSlider.ValueChanged += this.ZoomSlider_ValueChanged;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ScrollViewer != null)
            {
                double scale = e.NewValue / e.OldValue;

                double halfViewportHeight = this.ScrollViewer.ViewportHeight / 2;
                double newVerticalOffset = ((this.ScrollViewer.VerticalOffset + halfViewportHeight) * scale -
                                            halfViewportHeight);

                double halfViewportWidth = this.ScrollViewer.ViewportWidth / 2;
                double newHorizontalOffset = ((this.ScrollViewer.HorizontalOffset + halfViewportWidth) * scale -
                                              halfViewportWidth);

                this.scaleTransform.ScaleX *= scale;
                this.scaleTransform.ScaleY *= scale;

                this.ScrollViewer.ScrollToHorizontalOffset(newHorizontalOffset);
                this.ScrollViewer.ScrollToVerticalOffset(newVerticalOffset);
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.ScrollViewer != null)
            {
                this.InvalidateScale(out double scale, out _, out _);

                this.ScrollViewer.ScrollToHorizontalOffset(this.ScrollViewer.HorizontalOffset + e.HorizontalChange / scale);
                this.ScrollViewer.ScrollToVerticalOffset(this.ScrollViewer.VerticalOffset + e.VerticalChange / scale);
            }
        }

        private void DesignerCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            if (this.ScrollViewer != null)
            {
                this.InvalidateScale(out double scale, out double xOffset, out double yOffset);

                this.zoomThumb.Width = this.ScrollViewer.ViewportWidth * scale;
                this.zoomThumb.Height = this.ScrollViewer.ViewportHeight * scale;

                Canvas.SetLeft(this.zoomThumb, xOffset + this.ScrollViewer.HorizontalOffset * scale);
                Canvas.SetTop(this.zoomThumb, yOffset + this.ScrollViewer.VerticalOffset * scale);

                if (this.Parent is UIElement u)
                {
                    double bottom = Margin.Bottom;
                    double right = Margin.Right;
                    if (this.Margin.Bottom + this.ActualHeight > u.DesiredSize.Height)
                    {
                        bottom = u.DesiredSize.Height - this.ActualHeight;
                    }
                    if (this.Margin.Right + this.ActualWidth > u.DesiredSize.Width)
                    {
                        right = u.DesiredSize.Width - this.ActualWidth;
                    }
                    this.Margin = new Thickness(0, 0, right, bottom);
                }

            }
        }

        private void InvalidateScale(out double scale, out double xOffset, out double yOffset)
        {
            // designer canvas size
            double w = this.designerCanvas.ActualWidth * this.scaleTransform.ScaleX;
            double h = this.designerCanvas.ActualHeight * this.scaleTransform.ScaleY;

            // zoom canvas size
            double x = this.zoomCanvas.ActualWidth;
            double y = this.zoomCanvas.ActualHeight;

            double scaleX = x / w;
            double scaleY = y / h;

            scale = (scaleX < scaleY) ? scaleX : scaleY;

            xOffset = (x - scale * w) / 2;
            yOffset = (y - scale * h) / 2;
        }
    }
}
