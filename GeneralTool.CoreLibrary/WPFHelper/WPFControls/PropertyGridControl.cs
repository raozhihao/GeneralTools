using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.WPFHelper.UIEditorConverts;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyGridControl : Control
    {
        #region Public 字段

        /// <summary>
        /// 表头
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(PropertyGridControl));

        /// <summary>
        /// 选择的对象依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(nameof(SelectedObject), typeof(object), typeof(PropertyGridControl), new PropertyMetadata(null, PropertyChangedMethod));

        /// <summary>
        /// 排序依赖属性
        /// </summary>
        public static readonly DependencyProperty SortProperty = DependencyProperty.Register(nameof(Sort), typeof(bool?), typeof(PropertyGridControl), new PropertyMetadata(null, SortChanged));

        #endregion Public 字段

        #region Private 字段

        private Grid GridContent;

        #endregion Private 字段

        #region Public 构造函数

        static PropertyGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridControl), new FrameworkPropertyMetadata(typeof(PropertyGridControl)));
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 表头
        /// </summary>
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// 要展示的对象
        /// </summary>
        public object SelectedObject
        {
            get => GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool? Sort
        {
            get => (bool)GetValue(SortProperty);
            set => SetValue(SortProperty, value);
        }

        #endregion Public 属性

        #region Public 方法

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            GridContent = GetTemplateChild(nameof(GridContent)) as Grid;
            InitSelectedObject();
        }

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, Attribute> AttributesDic
        {
            get;
            private set;
        } = new Dictionary<string, Attribute>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributes"></param>
        public virtual void SetAttributes(Dictionary<string, Attribute> attributes)
        {
            AttributesDic = attributes;
        }
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, IUIEditorConvert> Converts { get; private set; } = new Dictionary<string, IUIEditorConvert>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="converts"></param>
        public void SetConverts(Dictionary<string, IUIEditorConvert> converts)
        {
            Converts = converts;
        }
        #endregion Public 方法

        #region Private 方法

        private static void PropertyChangedMethod(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            if (d is PropertyGridControl property)
                property.InitSelectedObject();
        }

        private static void SortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            if (d is PropertyGridControl property)
            {
                property.InitSelectedObject((bool?)e.NewValue);
            }
        }

        private void InitSelectedObject(bool? sort = null)
        {
            if (SelectedObject == null)
            {
                GridContent?.Children.Clear();
                return;
            }
            else if (GridContent == null)
            {
                return;
            }

            GridContent.Children.Clear();
            Type objType = SelectedObject.GetType();
            object[] attrs = objType.GetCustomAttributes(typeof(UIEditorAttribute), false);
            UIEditorAttribute attribute = attrs.Length == 1 ? attrs[0] as UIEditorAttribute : new UIEditorAttribute(typeof(ObjectExpandeUIEditor));
            int row = 0;

            Type type = SelectedObject.GetType();
            _ = type.GetProperties();

            attribute.GetConvert().ConvertTo(GridContent, SelectedObject, null, sort, ref row, Header);
        }

        #endregion Private 方法
    }
}