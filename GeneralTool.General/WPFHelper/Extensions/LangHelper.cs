using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 语言附加属性帮助类
    /// </summary>
    public class LangHelper : DependencyObject
    {
        /// <summary>
        /// 语言绑定Key的附加属性
        /// </summary>
        public static readonly DependencyProperty LangkeyProperty = DependencyProperty.RegisterAttached("LangKey", typeof(string), typeof(LangHelper), new PropertyMetadata(LangKeyChanged));

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="dependency"></param>
        /// <param name="value"></param>
        public static void SetLangKey(DependencyObject dependency, string value) => dependency.SetValue(LangkeyProperty, value);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        public static string GetLangKey(DependencyObject dependency) => dependency.GetValue(LangkeyProperty) + "";

        private static readonly List<PropertyLangStruct> propertyStructs = new List<PropertyLangStruct>();
        private static void LangKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var langKey = e.NewValue + "";
            var propertyName = GetBindingProperty(d);
            AddLangListener(d, e,langKey,propertyName);
        }

        private static void AddLangListener(DependencyObject d, DependencyPropertyChangedEventArgs e, string langKey, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(langKey) || string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(e.NewValue + ""))
            {
                var pro = new PropertyLangStruct
                {
                    Dependency = d,
                    LangKey =langKey
                };


                if (propertyStructs.Contains(pro, new LangStructEqualityComparer()))
                {
                    //已存在,则不添加
                    return;
                }
                //获取其默认值
                pro.PropertyInfo = d.GetType().GetProperty(propertyName);
                if (pro.PropertyInfo == null)
                {
                    return;
                }
                pro.DefaultLabel = pro.GetValue();
                propertyStructs.Add(pro);
                //设置默认库
                if (!LangProvider.LangProviderInstance.DefaultResource.ContainsKey(pro.LangKey))
                {
                    LangProvider.LangProviderInstance.DefaultResource.Add(pro.LangKey, pro.DefaultLabel + "");
                }
                LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;
                LangProvider.LangProviderInstance.LoadLang();
                var window = Window.GetWindow(d);

                //如果有父窗体,则绑定父窗体的显示事件
                if (!BindingStaticClass.BindingLangWindow.Contains(window))
                {
                    BindingStaticClass.BindingLangWindow.Add(window);
                    window.Loaded += Window_Loaded;
                    window.Closing += Window_Closing;
                } 
            }
        }

        private static void Window_Closing(object sender, CancelEventArgs e)
        {
            LangProvider.LangProviderInstance.LangChanged -= LangProviderInstance_LangChanged;
            BindingStaticClass.BindingLangWindow.Remove(sender as Window);
        }

        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LangProvider.LangProviderInstance.LangChanged -= LangProviderInstance_LangChanged;
            LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;
        }

        private static void LangProviderInstance_LangChanged(ResourceDictionary resx)
        {
            foreach (var item in propertyStructs)
            {
                //object value = null;
                //if (resx == null)
                //    value = item.DefaultLabel; //回复默认的
                //else
                //    value = resx[item.LangKey]; //获取对应的值
                //                                //赋值

                var value = LangProvider.LangProviderInstance.GetLangValue(item.LangKey);
                if (string.IsNullOrEmpty(value))
                    value = item.DefaultLabel+"";
                item.SetValue(value ?? item.DefaultLabel);
            }
        }

        /// <summary>
        /// 绑定属性
        /// </summary>
        public static readonly DependencyProperty BindingPropertyProperty = DependencyProperty.RegisterAttached("BindingProperty", typeof(string), typeof(LangHelper), new PropertyMetadata(LangBindingChanged));

        private static void LangBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var langKey = GetLangKey(d);
            var propertyName = e.NewValue + "";
            AddLangListener(d, e, langKey, propertyName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependency"></param>
        /// <param name="value"></param>
        public static void SetBindingProperty(DependencyObject dependency, object value) => dependency.SetValue(BindingPropertyProperty, value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        public static string GetBindingProperty(DependencyObject dependency) => dependency.GetValue(BindingPropertyProperty) + "";

        /// <summary>
        /// 绑定 LangExtension 扩展标记附加属性
        /// </summary>
        public static readonly DependencyProperty LangMarkupProperty = DependencyProperty.Register("LangMarkup", typeof(LangExtension), typeof(LangHelper), new PropertyMetadata(LangMarkupChanged));

        private static void LangMarkupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependency"></param>
        /// <param name="langExtension"></param>
        public static void SetLangMarkup(DependencyObject dependency, LangExtension langExtension) => dependency.SetValue(LangMarkupProperty, langExtension);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        public static LangExtension GetLangMarkup(DependencyObject dependency) => dependency.GetValue(LangMarkupProperty) as LangExtension;
    }
}
