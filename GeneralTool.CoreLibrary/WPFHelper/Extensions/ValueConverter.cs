using System;
using System.Globalization;
using System.Windows.Data;

namespace GeneralTool.CoreLibrary.WPFHelper.Extensions
{
    /// <summary>
    /// </summary>
    public static class ValueConverter
    {
        #region Public 方法

        /// <summary>
        /// 创建转换器
        /// </summary>
        /// <typeparam name="TInput">
        /// </typeparam>
        /// <typeparam name="TOutput">
        /// </typeparam>
        /// <param name="convertFunc">
        /// </param>
        /// <param name="convertBackFunc">
        /// </param>
        /// <returns>
        /// </returns>
        public static IValueConverter Create<TInput, TOutput>(Func<ValueConverterArgs<TInput>, TOutput> convertFunc, Func<ValueConverterArgs<TOutput>, TInput> convertBackFunc = null)
        {
            return new InnerValueConverter<TInput, TOutput>(convertFunc, convertBackFunc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="outType"></param>
        /// <param name="convertFunc"></param>
        /// <param name="convertBackFunc"></param>
        /// <returns></returns>
        public static IValueConverter CreateObjectConverter(Type inputType, Type outType, Func<ValueConverterObjectArgs, object> convertFunc, Func<ValueConverterObjectArgs, object> convertBackFunc = null)
        {
            return new InnerValueConverter(inputType, outType, convertFunc, convertBackFunc);
        }

        #endregion Public 方法

        #region Private 类

        private class InnerValueConverter<TInput, TOutput> : IValueConverter
        {
            #region Private 字段

            private readonly Func<ValueConverterArgs<TOutput>, TInput> _convertBackFunc = null;
            private readonly Func<ValueConverterArgs<TInput>, TOutput> _convertFunc = null;

            #endregion Private 字段

            #region Public 构造函数

            public InnerValueConverter(Func<ValueConverterArgs<TInput>, TOutput> convertFunc, Func<ValueConverterArgs<TOutput>, TInput> convertBackFunc)
            {
                this._convertFunc = convertFunc;
                this._convertBackFunc = convertBackFunc;
            }

            #endregion Public 构造函数

            #region Public 方法

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return this._convertFunc(new ValueConverterArgs<TInput>((TInput)value, parameter, targetType, culture) { InputType = typeof(TInput), OutType = typeof(TOutput) });
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (this._convertBackFunc == null)
                {
                    throw new NotImplementedException();
                }
                return this._convertBackFunc(new ValueConverterArgs<TOutput>((TOutput)value, parameter, targetType, culture) { InputType = typeof(TInput), OutType = typeof(TOutput) });
            }

            #endregion Public 方法
        }

        private class InnerValueConverter : IValueConverter
        {
            #region Private 字段

            private readonly Func<ValueConverterObjectArgs, object> _convertBackFunc = null;
            private readonly Func<ValueConverterObjectArgs, object> _convertFunc = null;

            public InnerValueConverter(Func<ValueConverterObjectArgs, object> convertFunc)
            {
                _convertFunc = convertFunc;
            }

            private readonly Type inputType;
            private readonly Type outType;

            #endregion Private 字段

            #region Public 构造函数

            public InnerValueConverter(Type inputType, Type outType, Func<ValueConverterObjectArgs, object> convertFunc, Func<ValueConverterObjectArgs, object> convertBackFunc)
            {
                this._convertFunc = convertFunc;
                this._convertBackFunc = convertBackFunc;
                this.inputType = inputType;
                this.outType = outType;
            }

            #endregion Public 构造函数

            #region Public 方法

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var args = new ValueConverterObjectArgs(value, parameter, targetType, culture) { InputType = this.inputType, OutType = this.outType };
                return this._convertFunc(args);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (this._convertBackFunc == null)
                {
                    throw new NotImplementedException();
                }
                return this._convertBackFunc(new ValueConverterObjectArgs(value, parameter, targetType, culture) { InputType = this.inputType, OutType = this.outType });
            }

            #endregion Public 方法
        }

        #endregion Private 类
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class ValueConverterArgs<T>
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="parameter">
        /// </param>
        /// <param name="targetType">
        /// </param>
        /// <param name="culture">
        /// </param>
        public ValueConverterArgs(T value, object parameter, Type targetType, CultureInfo culture)
        {
            this.Value = value;
            this.Parameter = parameter;
            this.Culture = culture;
            this.TargetType = targetType;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// 传入类型
        /// </summary>
        public Type InputType { get; set; }

        /// <summary>
        /// 输出类型
        /// </summary>
        public Type OutType { get; set; }

        /// <summary>
        /// </summary>
        public object Parameter { get; private set; }

        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// </summary>
        public T Value { get; private set; }

        #endregion Public 属性
    }

    /// <summary>
    /// </summary>
    public class ValueConverterObjectArgs
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="parameter">
        /// </param>
        /// <param name="targetType">
        /// </param>
        /// <param name="culture">
        /// </param>
        public ValueConverterObjectArgs(object value, object parameter, Type targetType, CultureInfo culture)
        {
            this.Value = value;
            this.Parameter = parameter;
            this.Culture = culture;
            this.TargetType = targetType;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// 传入类型
        /// </summary>
        public Type InputType { get; set; }

        /// <summary>
        /// 输出类型
        /// </summary>
        public Type OutType { get; set; }

        /// <summary>
        /// </summary>
        public object Parameter { get; private set; }

        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// </summary>
        public object Value { get; private set; }

        #endregion Public 属性
    }
}