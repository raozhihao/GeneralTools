using System.Windows;
using System.Windows.Controls;

using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper.WPFControls;

namespace TaskServerUI
{
    public class PropertyDependecy : DependencyObject
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ItemsAutoCreateUiProperty = DependencyProperty.RegisterAttached("ItemsAutoCreate", typeof(bool), typeof(ItemsDoTaskDependecy), new PropertyMetadata(ItemsChanged));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        public static void SetItemsAutoCreateUi(DependencyObject control, bool value) => control.SetValue(ItemsAutoCreateUiProperty, value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static bool GetItemsAutoCreateUi(DependencyObject control) => (bool)control.GetValue(ItemsAutoCreateUiProperty);

        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
            var control = d as Border;
            if (e.NewValue != null && (bool)e.NewValue)
                control.DataContextChanged += Control_DataContextChanged;
            else
                control.DataContextChanged -= Control_DataContextChanged;
        }

        private static void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //自动生成控件
            var border = sender as Border;
            var model = e.NewValue as DoTaskParameterItem;

            var propertyGrid = new PropertyGridControl()
            {
                SelectedObject = model
            };
            border.Child = propertyGrid;
        }
    }
}
