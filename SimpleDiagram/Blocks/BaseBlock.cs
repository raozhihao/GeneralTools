using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

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
                return ParentCanvas != null ? ParentCanvas.LayoutId : layoutId;
            }
            set
            {
                layoutId = value;
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
            OnCreateInCanvas();
            if (BlockViewModel != null)
            {
                BlockViewModel.IsSelected += BlockViewModel_IsSelected;
                BlockViewModel.SetBreakBlockEvent += BlockViewModel_SetBreakBlockEvent;
                BlockViewModel.IsBreakPointEvent += BlockViewModel_IsBreakPointEvent;
                DataContext = BlockViewModel;
            }
            //this.SelectByDb();
        }

        private bool BlockViewModel_IsBreakPointEvent()
        {
            return Dispatcher.Invoke(() =>
            {
                return IsBreakPoint;
            });

        }

        private async void BlockViewModel_SetBreakBlockEvent(bool obj)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                IsBreakBlock = obj;
                if (ParentCanvas != null && obj)
                {
                    ParentCanvas.SelectedBreakBlock = this;
                }
            });
        }

        private async void BlockViewModel_IsSelected(bool obj)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                IsSelected = obj;
            });
        }

        public override void OnDragToCanvas(Connection connection)
        {

            //在块被拖入画布时,如果有子窗口,则会显示,没有则不返回None
            WindowResult re;
            try
            {
                re = OpenWindow();
            }
            catch
            {
                ParentCanvas.RemoveItem(this);
                return;
            }

            if (re == WindowResult.None)
            {
                ParentCanvas.DragArrary();
            }
            else if (re == WindowResult.Null || re == WindowResult.False)
            {
                //用户在拖拽到画布上后选择了取消,则清除此项,释放资源
                ParentCanvas.RemoveItem(this);
            }
            else
            {
                SetProperty();
                BlockViewModel.ScriptId = LayoutId;
                BlockViewModel.BlockId = ID.ToString();
                ParentCanvas.DragArrary();
            }
        }

        protected WindowResult GetResult(bool? result)
        {
            return result.HasValue ? result.Value ? WindowResult.True : WindowResult.False : WindowResult.Null;
        }

        protected override void OnDoubleClickInCanvas(MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            WindowResult result = OpenWindow();
            if (result == WindowResult.True)
            {
                BlockViewModel.ScriptId = LayoutId;
                BlockViewModel.BlockId = ID.ToString();
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
            BlockViewModel.IsSelected -= BlockViewModel_IsSelected;
            OnDispose();

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
                Type contextType = DataContext.GetType();

                string contextStr = "";
                string windowContextType = "";
                if (contextType != null)
                {
                    contextStr = contextType.AssemblyQualifiedName;
                    System.Reflection.PropertyInfo property = contextType.GetProperties().FirstOrDefault(p => p.PropertyType.IsSubclassOf(typeof(BaseBlockWindowViewModel)));

                    if (property != null)
                        windowContextType = property.PropertyType.AssemblyQualifiedName;
                }

                string content = string.IsNullOrWhiteSpace(Content + "") ? "" : System.Windows.Markup.XamlWriter.Save(Content);
                Type type = GetType();
                return new BlockItemDo()
                {
                    // BackGround = (Color)ColorConverter.ConvertFromString(this.Background.ToString()),
                    BlockAssmeblyName = type.Assembly.FullName,
                    BlockTypeName = type.FullName,
                    CanvasLocation = new System.Windows.Point(Canvas.GetLeft(this), Canvas.GetTop(this)),
                    Header = Header,
                    Conent = content,
                    // Foreground = (Color)ColorConverter.ConvertFromString(this.Foreground.ToString()),
                    Id = ID,
                    IsEnd = IsEnd,
                    IsStart = IsStart,
                    MinSize = new System.Windows.Size(MinWidth, MinHeight),
                    Size = DesiredSize,
                    Padding = Padding,
                    CanRepeatToCanvas = CanRepeatToCanvas,
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
                if (ConnectorThumbs != null)
                {
                    Connection connections = ParentCanvas.Children.OfType<Connection>().FirstOrDefault(f => f.SourceBlock == this);

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
