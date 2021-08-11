using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 语言扩展标记
    /// </summary>
    public class LangExtension : MarkupExtension
    {
        /// <summary>
        /// 需要绑定语言的属性名称
        /// </summary>
        public string BindingProperty { get; set; }

        /// <summary>
        /// 绑定的语言Key
        /// </summary>
        public string LangKey { get; set; }

        readonly PropertyLangStruct property = new PropertyLangStruct();

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            property.LangKey = LangKey;
            property.Dependency = target.TargetObject as DependencyObject;
            if (property.Dependency == null)
                throw new Exception("无法绑定此项,只能在DependencyObject上进行绑定");

            property.PropertyInfo = target.TargetObject.GetType().GetProperty(this.BindingProperty);
            if (property.PropertyInfo == null)
                // throw new Exception("无法绑定此项,因为 BindingProperty 设置不正确,应该设置为需要被绑定的属性名称");
                return null;
            property.DefaultLabel = property.GetValue();

            //设置默认库
            if (!LangProvider.LangProviderInstance.DefaultResource.ContainsKey(property.LangKey))
            {
                LangProvider.LangProviderInstance.DefaultResource.Add(property.LangKey, property.DefaultLabel + "");
            }

            LangProviderInstance_LangChanged(LangProvider.LangProviderInstance.CurrentResource);
            LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;

            //找到其父窗体
            var window = Window.GetWindow(property.Dependency);
            if (window != null)
            {
                //如果有父窗体,则绑定父窗体的显示事件
                if (BindingStaticClass.BindingLangWindow.Contains(window))
                    return default;
                else
                    BindingStaticClass.BindingLangWindow.Add(window);
                // window.IsVisibleChanged += Control_IsVisibleChanged;
                window.Loaded += Window_Loaded;
                window.Closing += Window_Closing;
                return default;
            }
            if (property.Dependency is MenuItem)
            {
                //因为右键菜单会自动关闭,所以常驻
                return default;
            }


            return default;
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

        private void Control_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                LangProvider.LangProviderInstance.LangChanged -= LangProviderInstance_LangChanged;
                LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;
            }
            else
            {
                LangProvider.LangProviderInstance.LangChanged -= LangProviderInstance_LangChanged;
                BindingStaticClass.BindingLangWindow.Remove(sender as Window);
            }

        }

        private void LangProviderInstance_LangChanged(ResourceDictionary resx)
        {
            //object value = null;
            //if (resx == null)
            //    value = property.DefaultLabel; //回复默认的
            //else
            //    value = resx[property.LangKey]; //获取对应的值
            //                                    //赋值


            var value = LangProvider.LangProviderInstance.GetLangValue(this.LangKey);
            if (string.IsNullOrEmpty(value))
                value = property.DefaultLabel + "";
            property.SetValue(value ?? property.DefaultLabel);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class BindingStaticClass
    {
        /// <summary>
        /// 
        /// </summary>
        internal static List<Window> BindingLangWindow { get; } = new List<Window>();
    }
}
