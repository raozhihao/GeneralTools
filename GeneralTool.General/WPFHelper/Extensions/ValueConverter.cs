using System;
using System.Globalization;
using System.Windows.Data;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ValueConverter
    {
        /// <summary>
        /// 创建转换器
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="convertFunc"></param>
        /// <param name="convertBackFunc"></param>
        /// <returns></returns>
        public static IValueConverter Create<TInput, TOutput>(Func<ValueConverterArgs<TInput>, TOutput> convertFunc, Func<ValueConverterArgs<TOutput>, TInput> convertBackFunc = null)
        {
            return new InnerValueConverter<TInput, TOutput>(convertFunc, convertBackFunc);
        }


        private class InnerValueConverter<TInput, TOutput> : IValueConverter
        {
            private Func<ValueConverterArgs<TInput>, TOutput> _convertFunc = null;
            private Func<ValueConverterArgs<TOutput>, TInput> _convertBackFunc = null;

            public InnerValueConverter(Func<ValueConverterArgs<TInput>, TOutput> convertFunc, Func<ValueConverterArgs<TOutput>, TInput> convertBackFunc)
            {
                this._convertFunc = convertFunc;
                this._convertBackFunc = convertBackFunc;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return this._convertFunc(new ValueConverterArgs<TInput>((TInput)value, parameter, targetType, culture) { InputType=typeof(TInput), OutType=typeof(TOutput) });
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (this._convertBackFunc == null)
                {
                    throw new NotImplementedException();
                }
                return this._convertBackFunc(new ValueConverterArgs<TOutput>((TOutput)value, parameter, targetType, culture){ InputType=typeof(TInput), OutType=typeof(TOutput) });
            }
        }


        public static IValueConverter CreateObjectConverter(Type inputType,Type outType,Func<ValueConverterObjectArgs,object> convertFunc, Func<ValueConverterObjectArgs,object> convertBackFunc = null)
        {
            return new InnerValueConverter(inputType,outType,convertFunc, convertBackFunc);
        }

        private class InnerValueConverter: IValueConverter
        {
            private Func<ValueConverterObjectArgs,object> _convertFunc = null;
            private Func<ValueConverterObjectArgs,object> _convertBackFunc = null;
            private Type inputType, outType;

            public InnerValueConverter(Type inputType, Type outType, Func<ValueConverterObjectArgs,object> convertFunc, Func<ValueConverterObjectArgs,object> convertBackFunc)
            {
                this._convertFunc = convertFunc;
                this._convertBackFunc = convertBackFunc;
                this.inputType = inputType;
                this.outType = outType;
            }

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
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ValueConverterObjectArgs
    {
        /// <summary>
        /// 传入类型
        /// </summary>
        public Type InputType { get; set; }

        /// <summary>
        /// 输出类型
        /// </summary>
        public Type OutType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public object Parameter { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TargetType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        /// <param name="targetType"></param>
        /// <param name="culture"></param>
        public ValueConverterObjectArgs(object value, object parameter, Type targetType, CultureInfo culture)
        {
            this.Value = value;
            this.Parameter = parameter;
            this.Culture = culture;
            this.TargetType = targetType;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueConverterArgs<T>
    {
        /// <summary>
        /// 传入类型
        /// </summary>
        public Type InputType { get; set; }

        /// <summary>
        /// 输出类型
        /// </summary>
        public Type OutType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T Value { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public object Parameter { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// 目标类型
        /// </summary>
        public Type TargetType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        /// <param name="targetType"></param>
        /// <param name="culture"></param>
        public ValueConverterArgs(T value, object parameter, Type targetType, CultureInfo culture)
        {
            this.Value = value;
            this.Parameter = parameter;
            this.Culture = culture;
            this.TargetType = targetType;
        }
    }
}
