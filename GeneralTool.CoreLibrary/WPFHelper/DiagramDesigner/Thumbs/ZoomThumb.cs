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
            DragStarted += ZoomThumb_DragStarted;
            DragCompleted += ZoomThumb_DragCompleted;
            DragDelta += ZoomThumb_DragDelta;
            Loaded += ZoomThumb_Loaded;
        }

        private void ZoomThumb_Loaded(object sender, RoutedEventArgs e)
        {
            width = DragObject.ActualWidth;
        }

        private void ZoomThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            Thickness margin = DragObject.Margin;
            double right = margin.Right;
            double bottom = margin.Bottom;
            if (parentControl != null)
            {
                //不能超过父控件的left
                double xtmp = margin.Right + DragObject.ActualWidth;
                right = xtmp > parentControl.ActualWidth ? parentControl.ActualWidth - DragObject.ActualWidth : right;

                right = right < 0 ? 0 : right;

                double ytmp = margin.Bottom + DragObject.ActualHeight;
                bottom = ytmp > parentControl.ActualHeight ? parentControl.ActualHeight - DragObject.ActualHeight : bottom;
                //bottom = bottom < 0 ? 0 : bottom;
                bottom = bottom < -144 ? -144 : bottom;

            }

            DragObject.Margin = new Thickness(0, 0, right, bottom);

        }

        private void ZoomThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            parentControl = DragObject.Parent as FrameworkElement;
            parentControl.SizeChanged -= ParentControl_SizeChanged;
            parentControl.SizeChanged += ParentControl_SizeChanged;
        }

        private void ParentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size newSize = e.NewSize;
            Size oldSize = e.PreviousSize;
            double widthScale = newSize.Width / oldSize.Width;
            double heightScale = newSize.Height / oldSize.Height;
            //根据比例进行位置缩放
            Thickness oldMargin = DragObject.Margin;
            double right = oldMargin.Right * widthScale;
            double bottom = oldMargin.Bottom * heightScale;
            DragObject.Margin = new Thickness(0, 0, right, bottom);
        }

        private void ZoomThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thickness margin = DragObject.Margin;
            double x = margin.Right - e.HorizontalChange;
            double y = margin.Bottom - e.VerticalChange;

            if (margin.Right + width >= parentControl.ActualWidth && e.HorizontalChange < 0)
                return;
            DragObject.Margin = new Thickness(0, 0, x, y);

        }

    }
}
