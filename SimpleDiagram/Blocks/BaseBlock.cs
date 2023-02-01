using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using GeneralTool.General.Interfaces;
using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.General.WPFHelper.DiagramDesigner.Models;

using SimpleDiagram.BlockVIewModels;

namespace SimpleDiagram.Blocks
{
    public abstract class BaseBlock : BlockItem
    {
        private string layoutId;

        /// <summary>
        /// 布局ID
        /// </summary>
        public string LayoutId
        {
            get
            {
                if (this.ParentCanvas != null)
                {
                    return this.ParentCanvas.LayoutId;
                }
                return layoutId;
            }
            set
            {
                this.layoutId = value;
            }
        }

        /// <summary>
        /// 日志组件
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 应用程序窗体
        /// </summary>
        protected Window MainWindow => Application.Current.MainWindow;

        public BaseBlock()
        {
        }

        private void SetBreakItem_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnCreate()
        {
            this.OnCreateInCanvas();
            if (this.BlockViewModel != null)
            {
                this.BlockViewModel.IsSelected += BlockViewModel_IsSelected;
                this.BlockViewModel.SetBreakBlockEvent += BlockViewModel_SetBreakBlockEvent;
                this.BlockViewModel.IsBreakPointEvent += BlockViewModel_IsBreakPointEvent;
                this.DataContext = this.BlockViewModel;
            }
            //this.SelectByDb();
        }

        private bool BlockViewModel_IsBreakPointEvent()
        {
            return this.Dispatcher.Invoke(() =>
            {
                return this.IsBreakPoint;
            });

        }

        private async void BlockViewModel_SetBreakBlockEvent(bool obj)
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.IsBreakBlock = obj;
                if (this.ParentCanvas != null && obj)
                {
                    this.ParentCanvas.SelectedBreakBlock = this;
                }
            });
        }

        private async void BlockViewModel_IsSelected(bool obj)
        {
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.IsSelected = obj;
            });
        }

        public override void OnDragToCanvas(Connection connection)
        {
            //if (connection == null)
            //    return;

            //在块被拖入画布时,如果有子窗口,则会显示,没有则不返回None
            WindowResult re;
            try
            {
                re = this.OpenWindow();
            }
            catch (Exception ex)
            {
                this.ParentCanvas.RemoveItem(this);
                return;
            }

            if (re == WindowResult.None)
            {
                this.ParentCanvas.DragArrary();
            }
            else if (re == WindowResult.Null || re == WindowResult.False)
            {
                //用户在拖拽到画布上后选择了取消,则清除此项,释放资源
                this.ParentCanvas.RemoveItem(this);
            }
            else
            {
                this.SetProperty();
                this.BlockViewModel.ScriptId = this.LayoutId;
                this.BlockViewModel.BlockId = this.ID.ToString();
                this.ParentCanvas.DragArrary();
            }
        }

        protected WindowResult GetResult(bool? result)
        {
            if (result.HasValue)
            {
                return result.Value ? WindowResult.True : WindowResult.False;
            }
            else
            {
                return WindowResult.Null;
            }
        }

        protected override void OnDoubleClickInCanvas(MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            var result = OpenWindow();
            if (result == WindowResult.True)
            {
                this.BlockViewModel.ScriptId = this.LayoutId;
                this.BlockViewModel.BlockId = this.ID.ToString();
            }
        }

        protected override void OnRightDownClick(MouseButtonEventArgs e)
        {
            base.OnRightDownClick(e);
        }

        protected override void OnDelete()
        {
            base.OnDelete();
        }

        public override void Dispose()
        {
            this.BlockViewModel.IsSelected -= BlockViewModel_IsSelected;
            this.OnDispose();

        }


        public enum WindowResult
        {
            None,
            True,
            False,
            Null,
        }

        protected BlockItemDo BlockItemDo
        {
            get
            {
                var contextType = this.DataContext.GetType();

                string contextStr = "";
                string windowContextType = "";
                if (contextType != null)
                {
                    contextStr = contextType.AssemblyQualifiedName;
                    var property = contextType.GetProperties().FirstOrDefault(p => p.PropertyType.IsSubclassOf(typeof(BaseBlockWindowViewModel)));

                    if (property != null)
                        windowContextType = property.PropertyType.AssemblyQualifiedName;
                }

                var content = string.IsNullOrWhiteSpace(this.Content + "") ? "" : System.Windows.Markup.XamlWriter.Save(this.Content);
                var type = this.GetType();
                return new BlockItemDo()
                {
                    // BackGround = (Color)ColorConverter.ConvertFromString(this.Background.ToString()),
                    BlockAssmeblyName = type.Assembly.FullName,
                    BlockTypeName = type.FullName,
                    CanvasLocation = new System.Windows.Point(Canvas.GetLeft(this), Canvas.GetTop(this)),
                    Header = this.Header,
                    Conent = content,
                    // Foreground = (Color)ColorConverter.ConvertFromString(this.Foreground.ToString()),
                    Id = this.ID,
                    IsEnd = this.IsEnd,
                    IsStart = this.IsStart,
                    MinSize = new System.Windows.Size(this.MinWidth, this.MinHeight),
                    Size = this.DesiredSize,
                    Padding = this.Padding,
                    CanRepeatToCanvas = this.CanRepeatToCanvas,
                    DataContextType = contextStr,
                    WindowDataContextType = windowContextType
                };
            }
        }

        #region 待子类实现

        /// <summary>
        /// 释放资源
        /// </summary>
        protected abstract void OnDispose();


        /// <summary>
        /// 打开自定义属性编辑窗口(如无,则返回Null)
        /// </summary>
        /// <returns></returns>
        public abstract WindowResult OpenWindow();

        public virtual void OnCreateInCanvas() { }

        public virtual BaseBlockViewModel NextModel
        {
            get
            {
                if (this.ConnectorThumbs != null)
                {
                    var connections = this.ParentCanvas.Children.OfType<Connection>().FirstOrDefault(f => f.SourceBlock == this);

                    if (connections != null)
                        return connections.SinkBlock.DataContext as BaseBlockViewModel;
                }

                return null;
            }
        }

        /// <summary>
        /// 设置动作块在界面上的显示内容
        /// </summary>
        public abstract void SetShow();
        public abstract BaseBlockViewModel BlockViewModel { get; set; }
        #endregion
    }
}
