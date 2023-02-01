using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using GeneralTool.General.WPFHelper.DiagramDesigner.Common;
using GeneralTool.General.WPFHelper.DiagramDesigner.Models;
using GeneralTool.General.WPFHelper.DiagramDesigner.Thumbs;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Controls
{
    partial class BlockItem
    {
        #region 私有方法

        /// <summary>
        /// 
        /// </summary>
        public DragObject GetDragObject()
        {
            return new DragObject()
            {
                BackGround = this.Background,
                ForceGround = this.Foreground,
                Content = this.Content,
                FontSize = this.FontSize,
                Padding = this.Padding,
                CanRepeatToCanvas = this.CanRepeatToCanvas,
                DragType = this.GetType(),
                Header = this.Header,
                IsStart = this.IsStart
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

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            //this.SourceItems.Clear();
            this.SinkItems.Clear();
        }

        #endregion

        #region 重写方法


        /// <summary>
        /// 
        /// </summary>
        public void Apply()
        {
            var control = this.GetTemplateChild("PART_Connectors") as Control;
            var grid = control.Template.FindName("PART_ControlGrid", control) as Grid;
            var thumbs = grid.Children.OfType<ConnectorThumb>();
            this.ConnectorThumbs = new ConnectorThumbCollection(thumbs);
            this.ParentCanvas = this.Parent as DesignerCanvas;


            if (this.ParentCanvas != null)
            {
                this.OnCreate();//在画布上显示了
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return this.Header + "";
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (this.ParentCanvas == null)
            {
                //prevPoint = e.GetPosition(this);
                this.CaptureMouse();

                return;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                this.OnRightDownClick(e);
                return;
            }

            this.ParentCanvas.SelectedBlockItem = this;
            SetTop();
            if (!IsBreakBlock)
            {
                //不属于断点块时,走正常的逻辑
                if (!IsDebugMode)
                {
                    //不属于调试模式时
                    if (Keyboard.Modifiers != ModifierKeys.Control)
                    {
                        this.ParentCanvas.ClearSection();
                    }
                }

                this.IsSelected = true;
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
            if (this.ParentCanvas == null && this.IsMouseCaptured)
            {

                var dragObj = GetDragObject();

                DragDrop.DoDragDrop(this, dragObj, DragDropEffects.Copy);

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
            if (this.ParentCanvas == null)
            {
                //将其发送给画布
                var obj = this.GetDragObject();
                obj.IsDoubleClickAdd = true;
                MiddleController.Controller.DragBlockItemToCanvas(obj);
            }
            else
            {
                this.OnDoubleClickInCanvas(e);
                this.ParentCanvas.ClearSection();
            }
            // this.ReleaseMouseCapture();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (this.ParentCanvas == null) return;
            this.ConnectorVisibility = Visibility.Collapsed;
        }




        #endregion

        #region 公共方法

        /// <summary>
        /// 添加连接线
        /// </summary>
        /// <param name="connection"></param>
        public void AddConnection(Connection connection)
        {
            this.OnAddConnection(connection);
        }

        /// <summary>
        /// 移除连接线
        /// </summary>
        /// <param name="connection"></param>
        public void RemoveConnection(Connection connection)
        {
            this.OnRemoveConnection(connection);
        }

        /// <summary>
        /// 将自身从画而在上删除
        /// </summary>
        public virtual void Delete()
        {
            this.Dispose();
            this.OnDelete();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            if (this.IsStart)
                return;
            //1.调用数据库相关删除逻辑
            this.OnDelete();
            //清除自身连接线
            this.RemoveAllConnection();
        }

        private void RemoveAllConnection()
        {
            var removeList = new List<Connection>();
            //移除其上的线条,作为源的,作为目标的
            foreach (var item in this.ParentCanvas.Children)
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

            foreach (var item in removeList)
            {
                item.Dispose();
                this.RemoveConnection(item);
            }
        }


        /// <summary>
        /// 通知将自身已拖拽至画布
        /// </summary>
        /// <param name="connection">连接线</param>
        public void RaiseDragToCanvas(Connection connection)
        {
            this.OnDragToCanvas(connection);
        }

        /// <summary>
        /// 设置当前控件为最上方显示
        /// </summary>
        public void SetTop()
        {
            if (this.Parent is DesignerCanvas canvas)
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
            this.Content = string.Join(Environment.NewLine, parameters);
        }

        /// <summary>
        /// 是否包含对应的目标块
        /// </summary>
        /// <param name="sinkBlock"></param>
        /// <returns></returns>
        internal bool ContainsSink(BlockItem sinkBlock)
        {
            return this.SinkItems.Contains(sinkBlock);
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
            if (this.SinkItems.Contains(sinkItem))
                this.SinkItems.Remove(sinkItem);
            this.SinkItems.Add(sinkItem);
        }

        /// <summary>
        /// 设置块的连接点能够连接的目标方向
        /// </summary>
        /// <param name="keyValuePairs"></param>
        public void SetCanConnectSinkDirections(Dictionary<Direction, Direction[]> keyValuePairs)
        {
            foreach (var item in keyValuePairs)
            {
                this.ConnectorThumbs[item.Key].SetCanSinkDirections(item.Value);
            }

        }
        #endregion
    }
}
