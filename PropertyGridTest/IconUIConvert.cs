using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

using GeneralTool.General.Interfaces;
using GeneralTool.General.WPFHelper.Extensions;
using GeneralTool.General.WPFHelper.UIEditorConverts;
using GeneralTool.General.WPFHelper.WPFControls;

namespace PropertyGridTest
{
    public class IconUIConvert : IUIEditorConvert
    {
        public void ConvertTo(Grid gridParent, object instance, PropertyInfo propertyInfo, bool? sortAsc, ref int Row, string header = null)
        {
            var left = new TextBlock()
            {
                Text = propertyInfo.Name,
                Margin = new System.Windows.Thickness(5)
            };
            if (PropertyGridControl.AttributesDic.TryGetValue(propertyInfo.PropertyType.FullName, out var attrValue))
            {
                if (attrValue is DescriptionAttribute d)
                {
                    left.Text = d.Description;
                }
            }

            Grid.SetRow(left, Row);
            Grid.SetColumn(left, 0);

            var right = new ImageSourceUIControl(propertyInfo, instance)
            {
                Margin = new System.Windows.Thickness(5)
            };

            left.Visibility = right.Visibility = UIEditorHelper.GetVisibility(propertyInfo);

          

            right.SetBinding(ImageSourceUIControl.IconImageProperty, new
                Binding(propertyInfo.Name)
            { Mode = BindingMode.TwoWay });

            Grid.SetRow(right, Row++);
            Grid.SetColumn(right, 1);
            gridParent.Children.Add(left);
            gridParent.Children.Add(right);
        }
    }
}
