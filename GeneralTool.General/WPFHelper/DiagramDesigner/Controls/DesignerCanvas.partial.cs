using System;
using System.Windows;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Controls
{
    /// <summary>
    /// 
    /// </summary>
    partial class DesignerCanvas
    {
        /// <summary>
        /// 
        /// </summary>

        public static readonly DependencyProperty DragEndArraryProperty = DependencyProperty.Register(nameof(DragEndArrary),
            typeof(bool),
            typeof(DesignerCanvas),
            new PropertyMetadata(true));

        /// <summary>
        /// 在将动作块拖动到画布及更新了线段后是否进行自动布局
        /// </summary>
        public bool DragEndArrary
        {
            get => (bool)GetValue(DragEndArraryProperty);
            set => this.SetValue(DragEndArraryProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedBlockItemProperty = DependencyProperty.Register(nameof(SelectedBlockItem),
            typeof(BlockItem),
            typeof(DesignerCanvas));

        /// <summary>
        /// 获取当前选择的块
        /// </summary>
        public BlockItem SelectedBlockItem
        {
            get => this.GetValue(SelectedBlockItemProperty) as BlockItem;
            set => this.SetValue(SelectedBlockItemProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedBreakBlockItemProperty = DependencyProperty.Register(nameof(SelectedBreakBlock),
           typeof(BlockItem),
           typeof(DesignerCanvas));

        /// <summary>
        /// 获取当前选择的块
        /// </summary>
        public BlockItem SelectedBreakBlock
        {
            get => this.GetValue(SelectedBreakBlockItemProperty) as BlockItem;
            set => this.SetValue(SelectedBreakBlockItemProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CanOperationProperty = DependencyProperty.Register(nameof(CanOperation),
           typeof(bool),
           typeof(DesignerCanvas),
           new PropertyMetadata(true));

        /// <summary>
        /// 是否允许对动作块进行删除,线段进行修改和删除
        /// </summary>
        public bool CanOperation
        {
            get => (bool)this.GetValue(CanOperationProperty);
            set => this.SetValue(CanOperationProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ZoomPanelVisibilityProperty = DependencyProperty.Register(nameof(ZoomPanelVisibility),
            typeof(bool),
            typeof(DesignerCanvas),
            new PropertyMetadata(true, ZoomPanelVisibilityChanged));

        /// <summary>
        /// 
        /// </summary>
        public event Action<bool> ZoomPanelVisibilityChangedEvent;
        private static void ZoomPanelVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DesignerCanvas c)
            {
                if (e.NewValue == null)
                {
                    return;
                }
                var v = (bool)e.NewValue;
                c.ZoomPanelVisibilityChangedEvent?.Invoke(v);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ZoomPanelVisibility
        {
            get => (bool)this.GetValue(ZoomPanelVisibilityProperty);
            set => this.SetValue(ZoomPanelVisibilityProperty, value);
        }
    }
}
