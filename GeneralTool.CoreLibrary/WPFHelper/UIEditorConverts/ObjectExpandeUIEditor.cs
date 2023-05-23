using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.WPFHelper.WPFControls;

namespace GeneralTool.CoreLibrary.WPFHelper.UIEditorConverts
{
    /// <summary>
    /// 展开UI编辑器
    /// </summary>
    public class ExpandeUIEditor : IUIEditorConvert
    {
        #region Public 方法

        /// <inheritdoc/>
        public void ConvertTo(Grid gridParent, object instance, PropertyInfo propertyInfo, bool? sortAsc, ref int Row, string header = null)
        {
            //在对应的grid中写入

            var visible = UIEditorHelper.GetCusomAttr<System.ComponentModel.BrowsableAttribute>(propertyInfo);

            if (visible != null && !visible.Browsable)
                return;

            //添加行
            gridParent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });

            var editorAttribute = UIEditorHelper.GetCusomAttr<UIEditorAttribute>(propertyInfo);
            if (editorAttribute == null)
            {
                //根据类型不同,调用不同的转换器,只处理基础类型,值类型与string
                var proType = propertyInfo.PropertyType;

                if (proType == typeof(string))
                    editorAttribute = new UIEditorAttribute(typeof(StringEditorConvert));
                else if (proType.IsValueType)
                {
                    if (proType.IsEnum)
                        editorAttribute = new UIEditorAttribute(typeof(EnumEditorConvert));
                    else if (proType == typeof(Boolean))
                        editorAttribute = new UIEditorAttribute(typeof(BooleanEditorConvert));
                    else
                        editorAttribute = new UIEditorAttribute(typeof(StringEditorConvert));
                }
                else
                    editorAttribute = new UIEditorAttribute(typeof(StringObjectEditorConvert));
            }
            if (editorAttribute == null || editorAttribute.Convert == null)
                return;

            var c = editorAttribute.GetConvert();

            c.ConvertTo(gridParent, instance, propertyInfo, sortAsc, ref Row);

            #region MyRegion

            //var instanceType = instance.GetType();
            //var gridContent = new Grid() { Margin = new Thickness(5, 0, 0, 0) };
            //gridContent.Name = "Grid_" + Row;
            //Grid.SetRow(gridContent, Row++);
            //Grid.SetColumnSpan(gridContent, 2);

            //gridParent.Children.Add(gridContent);

            //gridContent.DataContext = instance;
            //gridContent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });

            //var expande = new Expander() { Header = headerName ?? instanceType.Name, IsExpanded = true };

            //Grid.SetColumnSpan(expande, 2);
            //Grid.SetRow(expande, 0);

            //gridContent.Children.Add(expande);

            //var newGrid = new Grid();
            //newGrid.Name = "ngrid_" + Row;
            //expande.Content = newGrid;
            //newGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
            //newGrid.ColumnDefinitions.Add(new ColumnDefinition());

            //int row = 0;
            //IEnumerable<PropertyInfo> pros = instanceType.GetProperties();//获取所有属性
            //if (sortAsc != null)
            //{
            //    pros = sortAsc.Value ? pros.OrderBy(p => p.Name) : pros.OrderByDescending(p => p.Name);
            //}

            //foreach (var pro in pros)
            //{
            //    var visible = UIEditorHelper.GetCusomAttr<System.ComponentModel.BrowsableAttribute>(pro);

            // if (visible != null && !visible.Browsable) continue;

            // //添加行 newGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0,
            // GridUnitType.Auto) }); var editorAttribute =
            // UIEditorHelper.GetCusomAttr<UIEditorAttribute>(pro); if (editorAttribute == null) {
            // //根据类型不同,调用不同的转换器,只处理基础类型,值类型与string var proType = pro.PropertyType;

            // if (proType == typeof(string)) editorAttribute = new
            // UIEditorAttribute(typeof(StringEditorConvert)); else if (proType.IsValueType) { if
            // (proType.IsEnum) editorAttribute = new UIEditorAttribute(typeof(EnumEditorConvert));
            // else if (proType == typeof(Boolean)) editorAttribute = new
            // UIEditorAttribute(typeof(BooleanEditorConvert)); else editorAttribute = new
            // UIEditorAttribute(typeof(StringEditorConvert)); } else editorAttribute = new UIEditorAttribute(typeof(StringObjectEditorConvert));

            // } if (editorAttribute == null || editorAttribute.Convert == null) continue;

            //    var c = editorAttribute.GetConvert();
            //    var cObj = pro.GetValue(instance);
            //    c.ConvertTo(newGrid, cObj, pro, sortAsc, ref row);
            //}

            #endregion MyRegion
        }

        #endregion Public 方法
    }

    /// <summary>
    /// 默认的UI扩展编辑器
    /// </summary>
    public class ObjectExpandeUIEditor : IUIEditorConvert
    {
        #region Public 方法

        /// <inheritdoc/>
        public void ConvertTo(Grid gridParent, object instance, PropertyInfo propertyInfo, bool? sort, ref int Row, string header = null)
        {
            if (instance == null) return;
            var instanceType = instance.GetType();
            var gridContent = new Grid
            {
                Margin = new Thickness(5, 0, 0, 0),
                Name = "Grid_" + Row
            };
            Grid.SetRow(gridContent, Row++);
            Grid.SetColumnSpan(gridContent, 2);

            gridParent.Children.Add(gridContent);

            gridContent.DataContext = instance;
            gridContent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });

            if (PropertyGridControl.AttributesDic.TryGetValue(instance.GetType().FullName, out var attrValue))
            {
                if (attrValue is DescriptionAttribute a)
                {
                    header = a.Description;
                }
            }
            var expande = new Expander() { Header = header ?? instanceType.Name, IsExpanded = true };

            Grid.SetColumnSpan(expande, 2);
            Grid.SetRow(expande, 0);

            gridContent.Children.Add(expande);

            var newGrid = new Grid
            {
                Name = "ngrid_" + Row
            };
            expande.Content = newGrid;
            newGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
            newGrid.ColumnDefinitions.Add(new ColumnDefinition());

            int row = 0;
            IEnumerable<PropertyInfo> pros = instanceType.GetProperties();//获取所有属性

            var groups = pros.GroupBy(p =>
             {
                 var categoryAttr = p.GetCustomAttribute<System.ComponentModel.CategoryAttribute>();
                 if (categoryAttr == null)
                 {
                     if (PropertyGridControl.AttributesDic.ContainsKey(p.PropertyType.FullName))
                     {
                         if (PropertyGridControl.AttributesDic[p.PropertyType.FullName] is System.ComponentModel.CategoryAttribute c)
                             return c;
                     }
                     return new System.ComponentModel.CategoryAttribute("其它");
                 }

                 return categoryAttr;
             });
            if (sort != null)
            {
                groups = sort.Value ? groups.OrderBy(p => p.Key.Category) : groups.OrderByDescending(p => p.Key.Category);
            }
            foreach (var item in groups)
            {
                string headerName = item.Key.Category;

                //在其下再增加一个Expande

                newGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
                var newExpander = new Expander() { Header = headerName };
                Grid.SetColumnSpan(newExpander, 2);
                Grid.SetRow(newExpander, row++);
                newGrid.Children.Add(newExpander);
                //增加一个Grid,用以保存对应的属性
                var gridExpander = new Grid();
                gridExpander.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
                gridExpander.ColumnDefinitions.Add(new ColumnDefinition());
                newExpander.Content = gridExpander;
                int newRow = 0;

                var newItem = item.AsEnumerable();
                if (sort != null)
                {
                    newItem = sort.Value ? item.OrderBy(p => p.Name) : item.OrderByDescending(p => p.Name);
                }
                foreach (var pro in newItem)
                {
                    var editor = new ExpandeUIEditor();

                    editor.ConvertTo(gridExpander, pro.GetValue(instance), pro, sort, ref newRow);
                }
            }
        }

        #endregion Public 方法
    }
}