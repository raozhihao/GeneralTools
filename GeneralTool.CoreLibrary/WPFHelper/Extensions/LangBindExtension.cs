using System;
using System.Windows;
using System.Windows.Markup;

namespace GeneralTool.CoreLibrary.WPFHelper.Extensions
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
            LangKey = langKey;
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
            IProvideValueTarget target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            dependencyObject = target.TargetObject as DependencyObject;
            dependencyProperty = target.TargetProperty as DependencyProperty;
            if (dependencyProperty == null)
                throw new Exception("语言只能绑定在 DependencyProperty 上");

            if (dependencyObject == null)
                return null;

            //设置默认库
            if (!LangProvider.LangProviderInstance.DefaultResource.ContainsKey(LangKey))
            {
                LangProvider.LangProviderInstance.DefaultResource.Add(LangKey, DefaultText + "");
            }

            LangProviderInstance_LangChanged(LangProvider.LangProviderInstance.CurrentResource);
            LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;

            //找到其父窗体
            Window window = Window.GetWindow(dependencyObject);
            if (window != null)
            {
                //如果有父窗体,则绑定父窗体的显示事件
                if (BindingStaticClass.BindingLangWindow.Contains(window))
                    return currentLabel;
                else
                    BindingStaticClass.BindingLangWindow.Add(window);
                window.Loaded += Window_Loaded;
                window.Closing += Window_Closing;
                return currentLabel;
            }

            return currentLabel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LangProvider.LangProviderInstance.LangChanged -= LangProviderInstance_LangChanged;
            _ = BindingStaticClass.BindingLangWindow.Remove(sender as Window);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LangProvider.LangProviderInstance.LangChanged -= LangProviderInstance_LangChanged;
            LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;
        }

        private void LangProviderInstance_LangChanged(ResourceDictionary resx)
        {
            string value = LangProvider.LangProviderInstance.GetLangValue(LangKey);
            if (string.IsNullOrEmpty(value))
            {
                value = DefaultText;
            }

            dependencyObject.SetValue(dependencyProperty, value);
            currentLabel = value;
        }
    }
}
