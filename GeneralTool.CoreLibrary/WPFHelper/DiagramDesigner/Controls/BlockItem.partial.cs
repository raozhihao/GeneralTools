using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Common;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls
{
    public partial class BlockItem
    {
        #region 私有方法

        /// <summary>
        /// 
        /// </summary>
        public DragObject GetDragObject()
        {
            return new DragObject()
            {
                BackGround = Background,
                ForceGround = Foreground,
                Content = Content,
                FontSize = FontSize,
                Padding = Padding,
                CanRepeatToCanvas = CanRepeatToCanvas,
                DragType = GetType(),
                Header = Header,
                IsStart = IsStart
            };
        }

        #endregion

        #region 等待子类重写

        /// <summary>
        /// 当添加连接线时
        /// </summary>
        /// <param name="connection"></param>
        public virtual void OnAddConnection(Connection connection)
        {

        }

        /// <summary>
        /// 当移除连接线时
        /// </summary>
        /// <param name="connection"></param>
        public virtual void OnRemoveConnection(Connection connection)
        {

        }

        /// <summary>
        /// 当自身从画布上删除时
        /// </summary>
        protected virtual void OnDelete()
        {

        }

        /// <summary>
        /// 创建时
        /// </summary>
        protected virtual void OnCreate()
        {

        }

        /// <summary>
        /// 在双击时
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDoubleClickInCanvas(MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// 鼠标右键按下
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRightDownClick(MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 在将控件拖拽到画布上时发生
        /// </summary>
        public virtual void OnDragToCanvas(Connection connection)
        {

        }

        /// <summary>
        /// 设置属性
        /// </summary>
        public virtual void SetProperty()
        {

        }

        /// <summary>
        /// 在被加入时
        /// </summary>
        public virtual void OnAddItem()
        {

        }

        /// <summary>
        /// 控件变更完成了位置时
        /// </summary>
        public virtual void RaiseUpdatePosition(Point currPoint)
        {

        }

        /// <summary>
        /// 控件大小变更完成
        /// </summary>
        public virtual void RaiseResizeChanged(Point currPoint, Size currentSize)
        {
            PosChangedEvent?.Invoke(this, new Rect(currPoint, currentSize));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            //this.SourceItems.Clear();
            SinkItems.Clear();
        }

        #endregion

        #region 重写方法

        private RotateTransform _rotateTransform;
        private Grid _partGridContent;
        /// <summary>
        /// 
        /// </summary>
        public void Apply()
        {
            Control control = GetTemplateChild("PART_Connectors") as Control;
            Grid grid = control.Template.FindName("PART_ControlGrid", control) as Grid;
            IEnumerable<ConnectorThumb> thumbs = grid.Children.OfType<ConnectorThumb>();
            ConnectorThumbs = new ConnectorThumbCollection(thumbs);
            ParentCanvas = Parent as DesignerCanvas;

            _rotateTransform = Template.FindName("PART_RotateTransform", this) as RotateTransform;
            _partGridContent = Template.FindName("PART_GridContent", this) as Grid;

            if (ParentCanvas != null)
            {
                OnCreate();//在画布上显示了
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return Header + "";
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);


            if (ParentCanvas == null)
            {
                //prevPoint = e.GetPosition(this);
                _ = CaptureMouse();

                return;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                OnRightDownClick(e);
                return;
            }

            ParentCanvas.SelectedBlockItem = this;
            SetTop();
            if (!IsBreakBlock)
            {
                //不属于断点块时,走正常的逻辑
                if (!IsDebugMode)
                {
                    //不属于调试模式时
                    if (Keyboard.Modifiers != ModifierKeys.Control)
                    {
                        ParentCanvas.ClearSection();
                    }
                }

                IsSelected = true;
                return;
            }
            else
            {
                //属于调试模式时, 不预处理
            }

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (ParentCanvas == null && IsMouseCaptured)
            {

                DragObject dragObj = GetDragObject();

                _ = DragDrop.DoDragDrop(this, dragObj, DragDropEffects.Copy);

                System.Diagnostics.Trace.WriteLine("Send drop");

                e.Handled = true;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDoubleClick(e);
            if (ParentCanvas == null)
            {
                //将其发送给画布
                DragObject obj = GetDragObject();
                obj.IsDoubleClickAdd = true;
                MiddleController.Controller.DragBlockItemToCanvas(obj);
            }
            else
            {
                OnDoubleClickInCanvas(e);
                ParentCanvas.ClearSection();
            }
            // this.ReleaseMouseCapture();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (ParentCanvas == null) return;
            ConnectorVisibility = Visibility.Collapsed;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 添加连接线
        /// </summary>
        /// <param name="connection"></param>
        public void AddConnection(Connection connection)
        {
            OnAddConnection(connection);
        }

        /// <summary>
        /// 移除连接线
        /// </summary>
        /// <param name="connection"></param>
        public void RemoveConnection(Connection connection)
        {
            OnRemoveConnection(connection);
        }

        /// <summary>
        /// 将自身从画而在上删除
        /// </summary>
        public virtual void Delete()
        {
            Dispose();
            OnDelete();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            if (IsStart)
                return;
            //1.调用数据库相关删除逻辑
            OnDelete();
            //清除自身连接线
            RemoveAllConnection();
        }

        private void RemoveAllConnection()
        {
            List<Connection> removeList = new List<Connection>();
            //移除其上的线条,作为源的,作为目标的
            foreach (object item in ParentCanvas.Children)
            {
                if (item is Connection c)
                {
                    if (c.SourceBlock == this)
                    {
                        removeList.Add(c);
                    }
                    else if (c.SinkBlock == this)
                    {
                        removeList.Add(c);
                    }
                }

            }

            foreach (Connection item in removeList)
            {
                item.Dispose();
                RemoveConnection(item);
            }
        }

        /// <summary>
        /// 通知将自身已拖拽至画布
        /// </summary>
        /// <param name="connection">连接线</param>
        public void RaiseDragToCanvas(Connection connection)
        {
            OnDragToCanvas(connection);
        }

        /// <summary>
        /// 设置当前控件为最上方显示
        /// </summary>
        public void SetTop()
        {
            if (Parent is DesignerCanvas canvas)
            {
                canvas.SetTop(this);
            }

        }

        /// <summary>
        /// 设置显示的内容
        /// </summary>
        /// <param name="parameters">内容集合</param>
        public void SetShowText(params object[] parameters)
        {
            Content = string.Join(Environment.NewLine, parameters);
        }

        /// <summary>
        /// 是否包含对应的目标块
        /// </summary>
        /// <param name="sinkBlock"></param>
        /// <returns></returns>
        internal bool ContainsSink(BlockItem sinkBlock)
        {
            return SinkItems.Contains(sinkBlock);
        }

        ///// <summary>
        ///// 加入源块
        ///// </summary>
        ///// <param name="sourceItem"></param>
        //internal void AddSourceItem(BlockItem sourceItem)
        //{
        //    if (this.SourceItems.Contains(sourceItem))
        //        this.SourceItems.Remove(sourceItem);
        //    this.SourceItems.Add(sourceItem);
        //}

        /// <summary>
        /// 加入目标块
        /// </summary>
        /// <param name="sinkItem"></param>
        internal void AddSinkItem(BlockItem sinkItem)
        {
            if (SinkItems.Contains(sinkItem))
                _ = SinkItems.Remove(sinkItem);
            SinkItems.Add(sinkItem);
        }

        /// <summary>
        /// 设置块的连接点能够连接的目标方向
        /// </summary>
        /// <param name="keyValuePairs"></param>
        public void SetCanConnectSinkDirections(Dictionary<Direction, Direction[]> keyValuePairs)
        {
            foreach (KeyValuePair<Direction, Direction[]> item in keyValuePairs)
            {
                ConnectorThumbs[item.Key].SetCanSinkDirections(item.Value);
            }

        }

        /// <summary>
        /// 获取4顶点坐标值
        /// </summary>
        /// <returns></returns>
        public List<Point> GetVertexCoordinates()
        {
            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this);
            Rect rect = new Rect(left, top, Width, Height);
            List<Point> list = new List<Point>();
            if (RotateAngle == 0)
            {
                list.Add(rect.TopLeft);
                list.Add(rect.TopRight);
                list.Add(rect.BottomRight);
                list.Add(rect.BottomLeft);
            }
            else
            {
                double width = _partGridContent.Width;
                if (width == 0 || double.IsNaN(width)) width = Width;
                double height = _partGridContent.Height;
                if (height == 0 || double.IsNaN(height)) height = Height;
                rect = new Rect(0, 0, width, height);
                Point topLeft = _rotateTransform.Transform(rect.TopLeft);
                Point topRight = _rotateTransform.Transform(rect.TopRight);
                Point bottomRight = _rotateTransform.Transform(rect.BottomRight);
                Point bottomLeft = _rotateTransform.Transform(rect.BottomLeft);

                int roundNum = 5;
                topLeft = new Point(Math.Round(topLeft.X + left, roundNum), Math.Round(topLeft.Y + top, roundNum));
                topRight = new Point(Math.Round(topRight.X + left, roundNum), Math.Round(topRight.Y + top, roundNum));
                bottomRight = new Point(Math.Round(bottomRight.X + left, roundNum), Math.Round(bottomRight.Y + top, roundNum));
                bottomLeft = new Point(Math.Round(bottomLeft.X + left, roundNum), Math.Round(bottomLeft.Y + top, roundNum));
                list.Add(topLeft);
                list.Add(topRight);
                list.Add(bottomRight);
                list.Add(bottomLeft);
            }
            return list;
        }

        /// <summary>
        /// 获取外包矩形
        /// </summary>
        /// <returns></returns>
        public Rect GetBoundRect()
        {
            return GetGeometry().Bounds;
        }

        /// <summary>
        /// 获取真实的Geometry
        /// </summary>
        /// <returns></returns>
        public Geometry GetGeometry()
        {
            List<Point> corrdinates = GetVertexCoordinates();
            if (corrdinates.Count < 1)
                return null;

            StringBuilder builder = new StringBuilder();
            Point first = corrdinates[0];
            _ = builder.Append($"M {first} ");
            for (int i = 1; i < corrdinates.Count; i++)
            {
                _ = builder.Append($"L {corrdinates[i]} ");
            }
            return Geometry.Parse(builder.ToString());
        }
        #endregion
    }
}
