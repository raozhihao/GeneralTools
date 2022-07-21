using GeneralTool.General.Attributes;
using GeneralTool.General.WPFHelper.UIEditorConverts;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GeneralTool.General.WPFHelper.WPFControls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GeneralTool.General.WPFHelper.WPFControls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GeneralTool.General.WPFHelper.WPFControls;assembly=GeneralTool.General.WPFHelper.WPFControls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:PropertyGridControl/>
    ///
    /// </summary>
    public class PropertyGridControl : Control
    {
        static PropertyGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGridControl), new FrameworkPropertyMetadata(typeof(PropertyGridControl)));
        }

        /// <summary>
        /// 选择的对象依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(nameof(SelectedObject), typeof(object), typeof(PropertyGridControl), new PropertyMetadata(null, PropertyChangedMethod));



        private static void PropertyChangedMethod(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            if (d is PropertyGridControl property)
                property.InitSelectedObject();
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
        /// 排序依赖属性
        /// </summary>
        public static readonly DependencyProperty SortProperty = DependencyProperty.Register(nameof(Sort), typeof(bool?), typeof(PropertyGridControl), new PropertyMetadata(null, SortChanged));

        private static void SortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            if (d is PropertyGridControl property)
            {
                property.InitSelectedObject((bool?)e.NewValue);
            }
        }

        /// <summary>
        /// 表头
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(PropertyGridControl));

        /// <summary>
        /// 表头
        /// </summary>
        public string Header
        {
            get => (string)this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool? Sort
        {
            get => (bool)this.GetValue(SortProperty);
            set => this.SetValue(SortProperty, value);
        }

        private Grid GridContent;
        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            this.GridContent = this.GetTemplateChild(nameof(this.GridContent)) as Grid;
            this.InitSelectedObject();
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

            attribute.GetConvert().ConvertTo(this.GridContent,this.SelectedObject, null, sort, ref row,this.Header);
        }
    }

}
