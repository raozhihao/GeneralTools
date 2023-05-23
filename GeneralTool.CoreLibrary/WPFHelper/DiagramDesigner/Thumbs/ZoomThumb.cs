using System.Windows;
using System.Windows.Controls.Primitives;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs
{
    /// <summary>
    /// 
    /// </summary>
    public class ZoomThumb : Thumb
    {
        /// <summary>
        /// 
        /// </summary>
        public ZoomBox DragObject
        {
            get { return GetValue(DragObjectProperty) as ZoomBox; }
            set { SetValue(DragObjectProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DragObjectProperty =
            DependencyProperty.Register(nameof(DragObject), typeof(ZoomBox), typeof(ZoomThumb), new PropertyMetadata(null));

        private FrameworkElement parentControl;
        private double width;
        /// <summary>
        /// 
        /// </summary>
        public ZoomThumb()
        {
            this.DragStarted += ZoomThumb_DragStarted;
            this.DragCompleted += ZoomThumb_DragCompleted;
            this.DragDelta += ZoomThumb_DragDelta;
            this.Loaded += ZoomThumb_Loaded;
        }

        private void ZoomThumb_Loaded(object sender, RoutedEventArgs e)
        {
            this.width = this.DragObject.ActualWidth;
        }

        private void ZoomThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var margin = this.DragObject.Margin;
            var right = margin.Right;
            var bottom = margin.Bottom;
            if (this.parentControl != null)
            {
                //不能超过父控件的left
                var xtmp = margin.Right + this.DragObject.ActualWidth;
                right = xtmp > this.parentControl.ActualWidth ? this.parentControl.ActualWidth - this.DragObject.ActualWidth : right;

                right = right < 0 ? 0 : right;

                var ytmp = margin.Bottom + this.DragObject.ActualHeight;
                bottom = ytmp > this.parentControl.ActualHeight ? this.parentControl.ActualHeight - this.DragObject.ActualHeight : bottom;
                //bottom = bottom < 0 ? 0 : bottom;
                bottom = bottom < -144 ? -144 : bottom;

            }

            this.DragObject.Margin = new Thickness(0, 0, right, bottom);

        }

        private void ZoomThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.parentControl = this.DragObject.Parent as FrameworkElement;
            this.parentControl.SizeChanged -= ParentControl_SizeChanged;
            this.parentControl.SizeChanged += ParentControl_SizeChanged;
        }

        private void ParentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var newSize = e.NewSize;
            var oldSize = e.PreviousSize;
            var widthScale = newSize.Width / oldSize.Width;
            var heightScale = newSize.Height / oldSize.Height;
            //根据比例进行位置缩放
            var oldMargin = this.DragObject.Margin;
            var right = oldMargin.Right * widthScale;
            var bottom = oldMargin.Bottom * heightScale;
            this.DragObject.Margin = new Thickness(0, 0, right, bottom);
        }



        private void ZoomThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var margin = this.DragObject.Margin;
            var x = margin.Right - e.HorizontalChange;
            var y = margin.Bottom - e.VerticalChange;

            if (margin.Right + this.width >= this.parentControl.ActualWidth && e.HorizontalChange < 0)
                return;
            this.DragObject.Margin = new Thickness(0, 0, x, y);

        }


    }
}
