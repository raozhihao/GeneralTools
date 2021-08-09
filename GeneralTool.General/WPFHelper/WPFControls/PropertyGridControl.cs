using GeneralTool.General.Attributes;
using GeneralTool.General.WPFHelper.UIEditorConverts;
using System.Windows;
using System.Windows.Controls;

namespace GeneralTool.General.WPFHelper.WPFControls
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
            get => (string)this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// 要展示的对象
        /// </summary>
        public object SelectedObject
        {
            get => this.GetValue(SelectedObjectProperty);
            set => this.SetValue(SelectedObjectProperty, value);
        }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool? Sort
        {
            get => (bool)this.GetValue(SortProperty);
            set => this.SetValue(SortProperty, value);
        }

        #endregion Public 属性

        #region Public 方法

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            this.GridContent = this.GetTemplateChild(nameof(this.GridContent)) as Grid;
            this.InitSelectedObject();
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
            if (this.SelectedObject == null || this.GridContent == null)
                return;

            this.GridContent.Children.Clear();
            var objType = this.SelectedObject.GetType();
            var attrs = objType.GetCustomAttributes(typeof(UIEditorAttribute), false);
            UIEditorAttribute attribute = null;
            if (attrs.Length == 1)
                attribute = attrs[0] as UIEditorAttribute;
            else
                attribute = new UIEditorAttribute(typeof(ObjectExpandeUIEditor));

            int row = 0;

            attribute.GetConvert().ConvertTo(this.GridContent, this.SelectedObject, null, sort, ref row, this.Header);
        }

        #endregion Private 方法
    }
}