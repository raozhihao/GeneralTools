using System;
using System.Windows;
using System.Windows.Markup;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 语言绑定
    /// </summary>
    public class LangBindExtension : MarkupExtension
    {
        /// <summary>
        /// 对应的语言Key
        /// </summary>
        [ConstructorArgument(nameof(LangKey))]
        public string LangKey { get; set; }

        /// <summary>
        /// 默认文本
        /// </summary>
        public string DefaultText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="langKey"></param>
        public LangBindExtension(string langKey)
        {
            this.LangKey = langKey;
        }

        /// <summary>
        /// 
        /// </summary>
        public LangBindExtension()
        {

        }

        /// <summary>
        /// 当前文本
        /// </summary>
        private string currentLabel;
        /// <summary>
        /// 绑定依赖属性
        /// </summary>
        private DependencyProperty dependencyProperty;
        /// <summary>
        /// 绑定对象
        /// </summary>
        private DependencyObject dependencyObject;

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            this.dependencyObject = target.TargetObject as DependencyObject;
            this.dependencyProperty = target.TargetProperty as DependencyProperty;
            if (dependencyProperty == null)
                throw new Exception("语言只能绑定在 DependencyProperty 上");

            if (this.dependencyObject == null)
                return null;

            //设置默认库
            if (!LangProvider.LangProviderInstance.DefaultResource.ContainsKey(this.LangKey))
            {
                LangProvider.LangProviderInstance.DefaultResource.Add(this.LangKey, this.DefaultText + "");
            }

            LangProviderInstance_LangChanged(LangProvider.LangProviderInstance.CurrentResource);
            LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;

            //找到其父窗体
            var window = Window.GetWindow(this.dependencyObject);
            if (window != null)
            {
                //如果有父窗体,则绑定父窗体的显示事件
                if (BindingStaticClass.BindingLangWindow.Contains(window))
                    return this.currentLabel;
                else
                    BindingStaticClass.BindingLangWindow.Add(window);
                window.Loaded += Window_Loaded;
                window.Closing += Window_Closing;
                return this.currentLabel;
            }

            return this.currentLabel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LangProvider.LangProviderInstance.LangChanged -= LangProviderInstance_LangChanged;
            BindingStaticClass.BindingLangWindow.Remove(sender as Window);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LangProvider.LangProviderInstance.LangChanged -= LangProviderInstance_LangChanged;
            LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;
        }

        private void LangProviderInstance_LangChanged(ResourceDictionary resx)
        {
            var value = LangProvider.LangProviderInstance.GetLangValue(this.LangKey);
            if (string.IsNullOrEmpty(value))
            {
                value = this.DefaultText;
            }

            this.dependencyObject.SetValue(this.dependencyProperty, value);
            this.currentLabel = value;
        }
    }
}
