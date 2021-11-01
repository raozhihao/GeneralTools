using System.Windows;
using System.Windows.Media;

namespace ImageTest
{
    public class DragMoveHelper
    {
        TranslateTransform translate;
        FrameworkElement parentElement;
        public void DragInit(FrameworkElement element, FrameworkElement parent = null, TranslateTransform translate = default)
        {
            this.parentElement = parent;
            if (translate == default)
                translate = new TranslateTransform();
            this.translate = translate;
            element.RenderTransform = this.translate;

            element.MouseDown += Element_MouseDown;
            element.MouseMove += Element_MouseMove;
            element.MouseUp += Element_MouseUp;
            element.MouseLeave += Element_MouseLeave;
        }

        private void Element_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Element_MouseUp(sender, new System.Windows.Input.MouseButtonEventArgs(e.MouseDevice, e.Timestamp, System.Windows.Input.MouseButton.Left));
        }

        private bool isRealse;
        public void RealseDrag(bool origin = true)
        {
            this.isRealse = true;
            if (origin)
                this.translate.X = this.translate.Y = 0;

        }

        public void DragAgain()
        {
            this.isRealse = false;
        }

        private void Element_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.parentElement != null)
            {
                //不能超越父边界
                var ui = sender as FrameworkElement;
                //左上角坐标
                var topLeft = ui.TranslatePoint(new Point(), this.parentElement);
                if (topLeft.X < 0)
                    this.translate.X -= topLeft.X;
                if (topLeft.Y < 0)
                    this.translate.Y -= topLeft.Y;

                //右下角坐标
                var bottomRight = ui.TranslatePoint(new Point(ui.ActualWidth, ui.ActualHeight), this.parentElement);
                if (bottomRight.X > this.parentElement.ActualWidth)
                    this.translate.X -= bottomRight.X - this.parentElement.ActualWidth;
                if (bottomRight.Y > this.parentElement.ActualHeight)
                    this.translate.Y -= bottomRight.Y - this.parentElement.ActualHeight;
            }
            this.isDrag = false;

        }

        private void Element_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.isDrag && !this.isRealse)
            {
                var curPos = e.GetPosition(sender as IInputElement);
                var vector = curPos - this.mouseDownPoint;
                this.translate.X += vector.X;
                this.translate.Y += vector.Y;
                e.Handled = true;
            }
        }

        private Point mouseDownPoint;
        private bool isDrag;
        private void Element_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.mouseDownPoint = e.GetPosition(sender as IInputElement);
            this.isDrag = true;
            e.Handled = true;
        }
    }
}
