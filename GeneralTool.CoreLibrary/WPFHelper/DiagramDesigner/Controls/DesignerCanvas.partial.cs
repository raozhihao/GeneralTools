using System;
using System.Windows;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DesignerCanvas
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
            set => SetValue(DragEndArraryProperty, value);
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
            get => GetValue(SelectedBlockItemProperty) as BlockItem;
            set => SetValue(SelectedBlockItemProperty, value);
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
            get => GetValue(SelectedBreakBlockItemProperty) as BlockItem;
            set => SetValue(SelectedBreakBlockItemProperty, value);
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
            get => (bool)GetValue(CanOperationProperty);
            set => SetValue(CanOperationProperty, value);
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
                bool v = (bool)e.NewValue;
                c.ZoomPanelVisibilityChangedEvent?.Invoke(v);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ZoomPanelVisibility
        {
            get => (bool)GetValue(ZoomPanelVisibilityProperty);
            set => SetValue(ZoomPanelVisibilityProperty, value);
        }
    }
}
