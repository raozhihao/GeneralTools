﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

using GeneralTool.General.Interfaces;
using GeneralTool.General.WPFHelper.Extensions;
using GeneralTool.General.WPFHelper.WPFControls;

namespace GeneralTool.General.WPFHelper.UIEditorConverts
{
    /// <summary>
    /// 枚举UI编辑器
    /// </summary>
    public class EnumEditorConvert : IUIEditorConvert
    {
        #region Public 方法

        ///<inheritdoc/>
        public void ConvertTo(Grid gridParent, object instance, PropertyInfo propertyInfo, bool? sort, ref int Row, string header = null)
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
            var right = new ComboBox()
            {
                Margin = new System.Windows.Thickness(5),
                BorderThickness = new System.Windows.Thickness(0, 0, 0, 1),
                IsReadOnly = UIEditorHelper.GetReadOnly(propertyInfo),
            };

            left.Visibility = right.Visibility = UIEditorHelper.GetVisibility(propertyInfo);
            right.Text = instance == null ? "" : instance + "";

            BindingMode bindingMode = BindingMode.TwoWay;
            if (propertyInfo.SetMethod == null)//只有get方法
            {
                right.Text = instance + "";
                bindingMode = BindingMode.OneWay;
            }

            if (right.IsReadOnly)
            {
                //不允许修改值
                right.IsEnabled = false;
                bindingMode = BindingMode.OneWay;
            }
            var arr = Enum.GetValues(propertyInfo.PropertyType);
            right.ItemsSource = arr;

            right.SetBinding(ComboBox.TextProperty, new Binding(propertyInfo.Name) { Converter = new CoverterEx().ObjectToStringConverter, Mode = bindingMode });

            Grid.SetRow(right, Row++);
            Grid.SetColumn(right, 1);
            gridParent.Children.Add(left);
            gridParent.Children.Add(right);
        }

        #endregion Public 方法
    }
}