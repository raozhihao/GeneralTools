using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls
{
    public class EnumCombox : ComboBox
    {
        public static readonly DependencyProperty EnumTypeProperty = DependencyProperty.Register(nameof(EnumType), typeof(Type), typeof(EnumCombox), new PropertyMetadata(null, TypeChanged));

        /// <summary>
        /// 枚举的类型,默认枚举上特性的类型为 DescriptionAttribute
        /// </summary>
        public Type EnumType
        {
            get => (Type)GetValue(EnumTypeProperty);
            set => SetValue(EnumTypeProperty, value);
        }

        private static void TypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EnumCombox b)
            {
                b.ChangedDataSource(e.NewValue as Type);
            }
        }

        public static readonly DependencyProperty AttrTypeProperty = DependencyProperty.Register(nameof(AttrType), typeof(Type), typeof(EnumCombox));

        /// <summary>
        /// 枚举上特性的类型
        /// </summary>
        public Type AttrType
        {
            get => (Type)GetValue(AttrTypeProperty);
            set => SetValue(AttrTypeProperty, value);
        }

        public static readonly DependencyProperty AttrTypeDescProperty = DependencyProperty.Register(nameof(AttrTypeDesc), typeof(string), typeof(EnumCombox));

        /// <summary>
        /// 枚举特性上要显示的属性
        /// </summary>
        public string AttrTypeDesc
        {
            get => GetValue(AttrTypeDescProperty) + "";
            set => SetValue(AttrTypeDescProperty, value);
        }

        private void ChangedDataSource(Type type)
        {
            if (type == null) return;

            if (!type.IsEnum) return;

            //获取所有的项
            var enumDic = new Dictionary<string, object>();
            var values = Enum.GetValues(type);
            foreach (var value in values)
            {
                var @enum = (Enum)value;
                if (string.IsNullOrWhiteSpace(this.AttrTypeDesc))
                {
                    var descAttr = @enum.GetEnumCustomAttribute<DescriptionAttribute>();
                    if (descAttr != null)
                        enumDic.Add(descAttr.Description, @enum);
                }
                else
                {
                    if (this.AttrType == null) continue;
                    if (this.AttrType.IsDefined(typeof(Attribute), true))
                    {
                        var desc = @enum.GetDescription(this.AttrType, this.AttrTypeDesc);
                        enumDic.Add(desc, @enum);
                    }
                }

            }
            this.ItemsSource = enumDic;
            this.DisplayMemberPath = "Key";
            this.SelectedValuePath = "Value";
        }
    }
}
