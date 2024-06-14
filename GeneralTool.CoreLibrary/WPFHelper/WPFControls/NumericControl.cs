using System.Text.RegularExpressions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls
{
    /// <summary>
    /// 数值类型控件
    /// </summary>
    [StyleTypedProperty(Property = nameof(IconStyle), StyleTargetType = typeof(TextBlock))]
    [StyleTypedProperty(Property = nameof(ValueBoxStyle), StyleTargetType = typeof(TextBox))]
    public class NumericControl : Control
    {
        private bool isUpdating;
        private TextBox ValueTxt;
        private RepeatButton UpPath;
        private RepeatButton DownPath;
        static NumericControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericControl), new FrameworkPropertyMetadata(typeof(NumericControl)));
        }



        public override void OnApplyTemplate()
        {
            this.ValueTxt = GetTemplateChild(nameof(this.ValueTxt)) as TextBox;
            this.ValueTxt.Text = this.Value + "";
            this.ValueTxt.HorizontalContentAlignment = HorizontalAlignment.Right;
            this.ValueTxt.VerticalContentAlignment = VerticalAlignment.Center;
            this.ValueTxt.BorderThickness = new Thickness(0);

            this.ValueTxt.VerticalAlignment = VerticalAlignment.Center;
            this.UpPath = GetTemplateChild(nameof(this.UpPath)) as RepeatButton;
            this.DownPath = GetTemplateChild(nameof(this.DownPath)) as RepeatButton;

            this.ValueTxt.PreviewKeyUp += ValueTxt_PreKeyUp;
            this.ValueTxt.LostFocus += ValueTxt_LostFocus;
            this.ValueTxt.PreviewMouseWheel += ValueTxt_MouseWheel;
            this.UpPath.Click += this.Up;
            this.DownPath.Click += this.Down;

            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1);


        }


        private void ValueTxt_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) this.Up(sender, e);
            else if (e.Delta < 0) this.Down(sender, e);
        }

        private void Up(object sender, RoutedEventArgs e)
        {
            this.Value += this.Interval;
        }

        private void Down(object sender, RoutedEventArgs e)
        {
            this.Value -= this.Interval;
        }


        private void ValueTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            this.ValueValidate();
        }

        private void ValueTxt_PreKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ValueValidate();
            }
            else if (e.Key == Key.Down)
            {
                this.Down(null, null);
            }
            else if (e.Key == Key.Up)
            {
                this.Up(null, null);
            }
        }

        private void ValueValidate()
        {
            double val = 0;

            var txt = this.ValueTxt.Text.Trim();
            if (string.IsNullOrWhiteSpace(txt))
            {
                val = 0;
            }
            else
            {
                if (this.IsDouble)
                {
                    // 提取带有小数点的数字，该方式会将所有带有小数的数字拼接在一起，如："ABC#123.56@AS8.9测试"提取出来就是123.568.9
                    var result2 = Regex.Replace(txt, @"[^-\d.\d]", "");
                    //如果是数字，则转换为decimal类型
                    if (Regex.IsMatch(result2, @"^[+-]?\d*[.]?\d*$"))
                    {
                        val = double.Parse(result2);
                    }
                    else
                    {
                        val = 0;
                    }
                }
                else
                {
                    var result1 = Regex.Replace(txt, @"[^-\d.\d]+", "");
                    if (string.IsNullOrWhiteSpace(result1))
                    {
                        val = 0;
                    }
                    else
                    {
                        val = Convert.ToInt64(Convert.ToDouble(result1));
                    }
                }
            }
            this.isUpdating = true;


            this.Value = val;

            this.ValueTxt.Text = val + "";
            this.isUpdating = false;
            this.ValueChangedEvent?.Invoke(this, val);
        }

        /// <summary>
        /// 值
        /// </summary>
        public double Value
        {
            get => (double)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        public readonly static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumericControl), new PropertyMetadata(0d, ValueChangedMethod));

        /// <summary>
        /// 是否为double类型
        /// </summary>
        public bool IsDouble
        {
            get => (bool)this.GetValue(IsDoubleProperty);
            set => this.SetValue(IsDoubleProperty, value);
        }

        public readonly static DependencyProperty IsDoubleProperty = DependencyProperty.Register(nameof(IsDouble), typeof(bool), typeof(NumericControl), new PropertyMetadata(true,IsDoubleChangedMethod));

        private static void IsDoubleChangedMethod(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           if(d is NumericControl n)
            {
                n.SetTxt(n.Value);
            }
        }

        /// <summary>
        /// 每次变动的值
        /// </summary>
        public double Interval
        {
            get => (double)this.GetValue(IntervalProperty);
            set => this.SetValue(IntervalProperty, value);
        }

        public readonly static DependencyProperty IntervalProperty = DependencyProperty.Register(nameof(Interval), typeof(double), typeof(NumericControl), new PropertyMetadata(1d));


        private static void ValueChangedMethod(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericControl n)
            {
                n.SetTxt((double)e.NewValue);
            }
        }

        private void SetTxt(double val)
        {
            if (this.ValueTxt == null) return;
            this.ValueTxt.Text = val + "";
            if (!this.isUpdating)
                this.ValueValidate();
        }


        #region 对外属性



        public event EventHandler<double> ValueChangedEvent;

        public Style IconStyle
        {
            get => this.GetValue(IconStyleProperty) as Style;
            set => this.SetValue(IconStyleProperty, value);
        }

        public static readonly DependencyProperty IconStyleProperty = DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(NumericControl));

        public Style ValueBoxStyle
        {
            get => this.GetValue(ValueBoxStyleProperty) as Style;
            set => this.SetValue(ValueBoxStyleProperty, value);
        }

        public static readonly DependencyProperty ValueBoxStyleProperty = DependencyProperty.Register(nameof(ValueBoxStyle), typeof(Style), typeof(NumericControl));

        public Brush UpButtonForeceColor
        {
            get => this.GetValue(UpButtonForeceColorProperty) as Brush;
            set => this.SetValue(UpButtonForeceColorProperty, value);
        }

        public static readonly DependencyProperty UpButtonForeceColorProperty = DependencyProperty.Register(nameof(UpButtonForeceColor), typeof(Brush), typeof(NumericControl), new PropertyMetadata(Brushes.Black));

        public Brush DownButtonForeceColor
        {
            get => this.GetValue(DownButtonForeceColorProperty) as Brush;
            set => this.SetValue(DownButtonForeceColorProperty, value);
        }

        public static readonly DependencyProperty DownButtonForeceColorProperty = DependencyProperty.Register(nameof(DownButtonForeceColor), typeof(Brush), typeof(NumericControl), new PropertyMetadata(Brushes.Black));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)this.GetValue(CornerRadiusProperty);
            set => this.SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(NumericControl), new PropertyMetadata(new CornerRadius(0)));
    }
    #endregion
}
