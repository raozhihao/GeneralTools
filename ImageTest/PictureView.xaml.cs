using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageTest
{
    /// <summary>
    /// PictureView.xaml 的交互逻辑
    /// </summary>
    public partial class PictureView : UserControl
    {
        public PictureView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否可以在图像上进行绘制方框依赖属性
        /// </summary>
        public static readonly DependencyProperty CanDrawRectProperty =
            DependencyProperty.Register(nameof(CanDrawRect),
                typeof(bool),
                typeof(PictureView),
                new PropertyMetadata(false));

        /// <summary>
        /// 获取或设置在截取框完成后自动重置截取框状态依赖属性
        /// </summary>
        public static readonly DependencyProperty DrawRectAutoEndInitProperty =
            DependencyProperty.Register(nameof(DrawRectAutoEndInit),
                typeof(bool),
                typeof(PictureView),
                new PropertyMetadata(false));

        /// <summary>
        /// 获取或设置在截取框完成后自动重置截取框状态
        /// </summary>
        public bool DrawRectAutoEndInit
        {
            get => (bool)this.GetValue(DrawRectAutoEndInitProperty);
            set => this.SetValue(DrawRectAutoEndInitProperty, value);
        }
        /// <summary>
        /// 获取或设置是否可以在图像上进行绘制方框
        /// </summary>
        public bool CanDrawRect
        {
            get => (bool)this.GetValue(CanDrawRectProperty);
            set => this.SetValue(CanDrawRectProperty, value);
        }

        /// <summary>
        /// 在画布上按下鼠标记录的上一次位置信息
        /// </summary>
        private Point gridLastMouseDownPosition;

        /// <summary>
        /// 画布拖动缩放依赖属性
        /// </summary>
        public static readonly DependencyProperty IsMoveAndScaleProperty =
           DependencyProperty.Register(nameof(IsMoveAndScale),
               typeof(bool),
               typeof(PictureView),
               new PropertyMetadata(true));

        /// <summary>
        /// 获取或设置画面是否可以画布拖动缩放
        /// </summary>
        public bool IsMoveAndScale
        {
            get => (bool)GetValue(IsMoveAndScaleProperty);
            set => SetValue(IsMoveAndScaleProperty, value);
        }

        public static readonly DependencyProperty StartDrawRectProperty =
            DependencyProperty.Register(nameof(StartDrawRect),
                typeof(bool),
                typeof(PictureView),
                new PropertyMetadata(false));

        public bool StartDrawRect
        {
            get => (bool)this.GetValue(StartDrawRectProperty);
            set => this.SetValue(StartDrawRectProperty, value);
        }



        /// <summary>
        /// 当前鼠标在画布上的状态
        /// </summary>
        private GridMouseType MouseType;

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            // 这里要判断好几个状态

            this.panelMouseDown = e.GetPosition(this.Image);
            this.CurrentPoint = this.panelMouseDown;
            this.CurrentWheelPoint = e.GetPosition(sender as IInputElement);

            var point = panelMouseDown;
            if (this.panelMouseDown.X < 0 || this.panelMouseDown.X > this.Image.Width)
            {
                point.X = double.PositiveInfinity;
            }
            if (this.panelMouseDown.Y < 0 || this.panelMouseDown.Y > this.Image.Height)
            {
                point.Y = double.PositiveInfinity;
            }
            this.ImageDownEvent?.Invoke(sender, point);
            //当前是否已经发出了截取方框的命令
            if (this.StartDrawRect)
            {
                this.sta.Visibility = Visibility.Collapsed;
                //需要开始绘制截取框
                //截取框未出现,则是需要开始绘制
                this.MouseType = GridMouseType.PanelDraw;

                //更改截取框的坐标
                this.PanelTrans.X = this.panelMouseDown.X;
                this.PanelTrans.Y = this.panelMouseDown.Y;
                e.Handled = true;
            }
            else if (sender == this.sta)  //1.当前鼠标在截取框按下
            {
                //1.1判断鼠标是否在边缘,如果在截取框边缘,则为更新截取框大小
                if (this.Cursor != null)
                {
                    this.MouseType = GridMouseType.PanelResize;//在边缘,则是更改截取框大小
                }
                else
                {
                    //否则为拖动截取框
                    this.panelMove = true;
                    this.MouseType = GridMouseType.PanelMove;//不在边缘,则是移动截取框
                }
                //屏蔽掉之后的Grid冒泡,不然会再一次进来
                e.Handled = true;
            }
            else
            {
                //鼠标不在截取框,则是拖动
                this.MouseType = GridMouseType.GridMove;
                this.gridLastMouseDownPosition = this.CurrentWheelPoint;
            }

        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            //当前终点坐标
            Point imagePos = e.GetPosition(this.Image);
            if (this.Image.Source != null)
            {
                if (imagePos.X >= 0 && imagePos.Y >= 0 && imagePos.X <= this.Image.Source.Width && imagePos.Y <= this.Image.Height)
                    this.ImageMouseMoveEvent?.Invoke(this, new GeneralTool.General.Models.ImageMouseEventArgs(e.MouseDevice, e.Timestamp, e.StylusDevice, imagePos));
            }

            //
            ShowReizeSytleCutPanel(e);
            bool re = ResizeCutPanel(e);
            if (re)
            {
                return;
            }

            if (this.CanDrawRect && e.LeftButton == MouseButtonState.Pressed && this.MouseType == GridMouseType.PanelDraw)
            {

                //向量
                var vector = imagePos - panelMouseDown;
                if (vector.X <= 0 || vector.Y <= 0)
                {
                    return;
                }
                //宽高
                this.CutRect.Width = vector.X;
                this.CutRect.Height = vector.Y;
                this.sta.Visibility = Visibility.Visible;
            }
            else if (this.panelMove && this.MouseType == GridMouseType.PanelMove)
            {
                var vector = imagePos - panelMouseDown;
                this.PanelTrans.X += vector.X;
                this.PanelTrans.Y += vector.Y;
                this.panelMouseDown = imagePos;
            }
            else if (IsMoveAndScale && this.MouseType == GridMouseType.GridMove)
            {
                if (this.ScaleSlider.Value != 1)
                {
                    Point position = e.GetPosition((IInputElement)sender);
                    var vector = position - gridLastMouseDownPosition;
                    this.GridTrans.X += vector.X;
                    this.GridTrans.Y += vector.Y;
                    gridLastMouseDownPosition = position;
                }
                else
                {
                    this.GridTrans.X = this.GridTrans.Y = 0;
                }
            }
            e.Handled = true;

        }

        /// <summary>
        /// 截取框更改大小的方向
        /// </summary>
        private CutPanelResizeDirection direction;
        private bool ResizeCutPanel(MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed && this.MouseType != GridMouseType.PanelResize)
            {
                //不是鼠标左键且不处于更改截取框大小的状态时,直接退出
                return false;
            }
            //重置截取框大小
            //获取鼠标当前的点
            var mousePoint = e.GetPosition(this.Image);

            //获取当前方框在canvas中的终点
            var recEndPoint = this.CutRect.TranslatePoint(new Point(CutRect.Width, this.CutRect.Height), this.Image);
            //获取当前方框在canvas中的起点
            var recStartPoint = this.CutRect.TranslatePoint(new Point(0, 0), this.Image);

            //判断当前鼠标开状
            if (this.Cursor == Cursors.SizeWE && this.direction == CutPanelResizeDirection.Right)//在右边拉
            {
                if (mousePoint.X < recEndPoint.X && this.CutRect.Width < 10)
                {
                    return false;
                }

                var dis = mousePoint.X - recEndPoint.X;
                if (dis + this.CutRect.Width < 10)
                    return false;

                Console.WriteLine(dis);
                //左右方拉取,重新赋值宽度
                this.CutRect.Width += dis;
                return true;
            }
            else if (this.Cursor == Cursors.SizeWE && this.direction == CutPanelResizeDirection.Left)//在左边拉
            {
                if (mousePoint.X < recStartPoint.X && this.CutRect.Width < 10)
                {
                    return false;
                }

                //在左边左右拉取,重新赋左上角坐标
                var dis = recStartPoint.X - mousePoint.X;
                if (dis + this.CutRect.Width < 10)
                    return false;
                this.PanelTrans.X += mousePoint.X - recStartPoint.X;
                //
                this.CutRect.Width += dis;
                return true;
            }
            else if (this.Cursor == Cursors.SizeNS && this.direction == CutPanelResizeDirection.Bottom)//在下边拉
            {
                if (mousePoint.Y < recEndPoint.Y && this.CutRect.Height < 10)
                {
                    return false;
                }

                //上下拉取,重新赋值高度
                var dis = mousePoint.Y - recEndPoint.Y;
                if (dis + this.CutRect.Height < 10)
                    return false;
                this.CutRect.Height += dis;
                return true;
            }
            else if (this.Cursor == Cursors.SizeNS && this.direction == CutPanelResizeDirection.Top)//在上边拉
            {
                if (mousePoint.Y < recEndPoint.Y && this.CutRect.Height < 10)
                    return false;

                //在上方上下拉取,重新赋左上角坐标
                var dis = recStartPoint.Y - mousePoint.Y;
                if (dis + this.CutRect.Height < 10)
                    return false;
                this.PanelTrans.Y += mousePoint.Y - recStartPoint.Y; ;
                //
                this.CutRect.Height += dis;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 鼠标在移动过程中时,判断鼠标的位置
        /// </summary>
        /// <param name="e"></param>
        private void ShowReizeSytleCutPanel(MouseEventArgs e)
        {
            //确定鼠标在边缘
            //判断状态
            if (this.MouseType != GridMouseType.None)
            {
                //在做其它操作
                return;
            }

            //确定当前鼠标点的位置

            var mousePoint = e.GetPosition(this.Image);

            //获取方框在canvas中的右底终点
            var recEndPoint = this.CutRect.TranslatePoint(new Point(CutRect.Width, CutRect.Height), this.Image);
            //获取方框在canvas中的左上起点,也是左上顶点
            var recStartPoint = this.CutRect.TranslatePoint(new Point(0, 0), this.Image);

            //获取右上角顶点
            var recRightTopPoint = this.CutRect.TranslatePoint(new Point(CutRect.Width, 0), this.Image);

            //获取左下角顶点
            var recLeftBottomPoint = this.CutRect.TranslatePoint(new Point(0, CutRect.Height), this.Image);


            //Console.WriteLine($"mousePoint:{mousePoint}  --  recStartPoint:{recStartPoint}  --  recRightTopPoint:{recRightTopPoint}  --  recEndPoint:{recEndPoint}  --  recLeftBottomPoint:{recLeftBottomPoint}");
            //判断
            //右距离,方框右边的x坐标与鼠标x坐标的距离
            var xDis = recEndPoint.X - mousePoint.X;
            //左距离,方框左边的x坐标与鼠标x坐标的距离
            var leftXDis = recStartPoint.X - mousePoint.X;
            //下距离,方框下边的y坐标与鼠标y坐标的距离
            var yDis = recEndPoint.Y - mousePoint.Y;
            //上距离,方框上边的y坐标与鼠标y坐标的距离
            var topYDis = recStartPoint.Y - mousePoint.Y;

            double tmp = 10 / this.ScaleSlider.Value;
            //判断鼠标是否在右边框
            var mouseInRightLine = xDis > -tmp && xDis < tmp && mousePoint.Y > (recRightTopPoint.Y + tmp) && mousePoint.Y < recEndPoint.Y - tmp;

            //判断鼠标是否在左边框
            var mouseInLeftLine = leftXDis > -tmp && leftXDis < tmp && mousePoint.Y > (recRightTopPoint.Y + tmp) && mousePoint.Y < recEndPoint.Y - tmp;

            //判断鼠标是否在下边框
            var mouseInBottomLine = yDis > -tmp && yDis < tmp && mousePoint.X > recLeftBottomPoint.X + tmp && mousePoint.X < recEndPoint.X - tmp;

            //判断鼠标是否在上边框
            var mouseInTopLine = topYDis > -tmp && topYDis < tmp && mousePoint.X > recStartPoint.X + tmp && mousePoint.X < recEndPoint.X - tmp;


            var move = this.panelMove;
            //右边的显示应该处于右顶点到右底点之间
            if (mouseInRightLine)
            {
                //处于右边
                this.Cursor = Cursors.SizeWE;
                this.direction = CutPanelResizeDirection.Right;
                this.panelMove = false;
            }
            else if (mouseInLeftLine)
            {
                //处于左边
                this.Cursor = Cursors.SizeWE;
                this.direction = CutPanelResizeDirection.Left;
                this.panelMove = false;
            }
            else if (mouseInBottomLine)
            {
                //处于下方
                this.Cursor = Cursors.SizeNS;
                this.direction = CutPanelResizeDirection.Bottom;
                this.panelMove = false;
            }
            else if (mouseInTopLine)
            {
                //处于上方
                this.Cursor = Cursors.SizeNS;
                this.direction = CutPanelResizeDirection.Top;
                this.panelMove = false;
            }
            else
            {
                this.Cursor = null;
                this.panelMove = move;
            }
        }


        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ImageUpEvent?.Invoke(sender, e.GetPosition(this.Image));
            ClearStatus(e);
            if (this.DrawRectAutoEndInit)
                this.StartDrawRect = false;

            if (this.CutRect.Visibility == Visibility.Collapsed)
                return;
            //不能超越父边界

            //左上角坐标
            var topLeft = this.CutRect.TranslatePoint(new Point(), this.Image);
            if (topLeft.X < 0)
                this.PanelTrans.X -= topLeft.X;
            if (topLeft.Y < 0)
                this.PanelTrans.Y -= topLeft.Y;

            //右下角坐标
            var bottomRight = this.CutRect.TranslatePoint(new Point(this.CutRect.ActualWidth, this.CutRect.ActualHeight), this.Image);
            if (bottomRight.X > this.Image.ActualWidth)
                this.PanelTrans.X -= bottomRight.X - this.Image.ActualWidth;
            if (bottomRight.Y > this.Image.ActualHeight)
                this.PanelTrans.Y -= bottomRight.Y - this.Image.ActualHeight;


            //获取已截取到的图形
            var startPoint = this.CutRect.TranslatePoint(new Point(), this.Image);

            var cutPixelRect = new Rect(startPoint, new Size(this.CutRect.Width, this.CutRect.Height));

            var imageRect = new Rect(0, 0, this.Image.ActualWidth, this.Image.ActualHeight);
            var result = Rect.Intersect(imageRect, cutPixelRect);
            Int32Rect rect = Int32Rect.Empty;
            BitmapSource source = null;
            bool sucess = false;
            string message = "";
            if (!result.IsEmpty)
            {
                rect = new Int32Rect((int)Math.Floor(result.X), (int)Math.Floor(result.Y), (int)Math.Ceiling(result.Width), (int)Math.Ceiling(result.Height));

                try
                {
                    source = GeneralTool.General.WPFHelper.BitmapSouceExtension.GetChooseRectImageSouce(this.Image.Source, rect);
                    sucess = true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }

            }
            this.CutImageDownEvent?.Invoke(this, new GeneralTool.General.Models.ImageEventArgs(source, sucess, "") { Int32Rect = rect });
        }


        private Point CurrentWheelPoint = new Point(double.PositiveInfinity, double.PositiveInfinity);
        private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.CurrentWheelPoint = e.GetPosition(sender as IInputElement);
            this.WheelScale(this.CurrentWheelPoint, e.Delta, false);
        }


        private void WheelScale(Point currentWheelPoint, double delta, bool sliderChanged = false)
        {
            if (IsMoveAndScale)
            {
                var actualPoint = group.Inverse.Transform(this.CurrentWheelPoint); // 想要缩放的点
                if (!sliderChanged)
                {
                    this.ScaleSlider.ValueChanged -= this.ScaleSlider_ValueChanged;
                    ScaleSlider.Value += delta / 500;
                    this.ScaleSlider.ValueChanged += this.ScaleSlider_ValueChanged;
                }

                if (this.ScaleSlider.Value == 1)
                {
                    this.GridTrans.X = this.GridTrans.Y = 0;
                }
                else
                {
                    this.GridTrans.X = -(actualPoint.X * (this.ScaleSlider.Value - 1)) + this.CurrentWheelPoint.X - actualPoint.X;
                    this.GridTrans.Y = -(actualPoint.Y * (this.ScaleSlider.Value - 1)) + this.CurrentWheelPoint.Y - actualPoint.Y;

                }

                Console.WriteLine(this.ScaleSlider.Value);
            }
        }

        private void ClearStatus(MouseButtonEventArgs e)
        {
            //回复状态
            this.MouseType = GridMouseType.None;
            Mouse.Capture(default);
            this.panelMove = false;
            this.panelMouseDown = e.GetPosition(this.Image);
        }

        Point panelMouseDown;
        private bool panelMove;


        private void MoveScaleClick(object sender, RoutedEventArgs e)
        {
            this.IsMoveAndScale = true;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ClearStatus(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left, e.StylusDevice));
            this.UIElement_OnMouseUp(sender, new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left));
        }

        private enum CutPanelResizeDirection
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }

        /// <summary>
        /// 鼠标状态
        /// </summary>
        private enum GridMouseType
        {
            /// <summary>
            /// 无
            /// </summary>
            None,
            /// <summary>
            /// 整个画面移动状态
            /// </summary>
            GridMove,
            /// <summary>
            /// 整个画面缩放状态
            /// </summary>
            GridSacle,
            /// <summary>
            /// 截取框重置状态
            /// </summary>
            PanelResize,
            /// <summary>
            /// 截取框移动状态
            /// </summary>
            PanelMove,
            /// <summary>
            /// 鼠标在绘制截取框
            /// </summary>
            PanelDraw,
        }

        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (this.IsLoaded)
            {
                this.WheelScale(this.CurrentWheelPoint, e.NewValue > e.OldValue ? 120d : -120d, true);

            }

        }


    }


}
