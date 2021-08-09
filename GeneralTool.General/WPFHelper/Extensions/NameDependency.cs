using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 控件名依赖属性,需要搭配Name扩展来使用
    /// </summary>
    public class NameDependency : DependencyObject
    {
        /// <summary>
        /// 控件名依赖属性
        /// </summary>
        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached("Name", typeof(string), typeof(NameDependency));

        /// <summary>
        /// 设置绑定的控件名
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetName(DependencyObject obj, string value) => obj.SetValue(NameProperty, value);
        /// <summary>
        /// 获取绑定的控件名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetName(DependencyObject obj) => obj.GetValue(NameProperty)+"";
    }

    /// <summary>
    /// 控件名称扩展,可以将控件绑定到ViewModel中
    /// </summary>
    public class NameExtension : MarkupExtension
    {
        /// <summary>
        /// 控件在ViewModel中的属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public NameExtension(string name)
        {
            this.Name = name;
        }

        private DependencyObject obj;

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
             obj = target.TargetObject as DependencyObject;
            SetValue();
            return default;
        }

        private void SetValue()
        {
            var context = obj.GetValue(FrameworkElement.DataContextProperty);
            if (context != null)
            {
                if (context is ObjectDataProvider provider)
                    context = provider.ObjectInstance;

                var contextType = context.GetType();
               
                var property = contextType.GetProperty(this.Name + "");
                if (property != null)
                {
                    if (property.SetMethod != null)
                    {
                        property.SetValue(context, obj);
                    }
                }
            }
            else
            {
                SubscribeToDataContextChanged(obj);
            }
        }

        private void SubscribeToDataContextChanged(DependencyObject targetObj)
        {
            DependencyPropertyDescriptor
                .FromProperty(FrameworkElement.DataContextProperty, targetObj.GetType())
                .AddValueChanged(targetObj, TargetObject_DataContextChanged);
        }

        private void UnsubscribeFromDataContextChanged(DependencyObject targetObj)
        {
            DependencyPropertyDescriptor
                .FromProperty(FrameworkElement.DataContextProperty, targetObj.GetType())
                .RemoveValueChanged(targetObj, TargetObject_DataContextChanged);
        }

        private void TargetObject_DataContextChanged(object sender, EventArgs e)
        {
            SetValue();
            UnsubscribeFromDataContextChanged(this.obj);
        }
    }

    
}
