using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

using GeneralTool.General.ExceptionHelper;

using Microsoft.CSharp;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 绑定扩展
    /// </summary>
    public class BindingExtension : MarkupExtension
    {
        /// <summary>
        /// 绑定路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public BindingExtension(string path) => this.Path = path;

        #region Binding Properties


        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public IValueConverter Converter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public object ConverterParameter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnValidationError { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool ValidatesOnDataErrors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool ValidatesOnExceptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        public bool ValidatesOnNotifyDataErrors { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public RelativeSource RelativeSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public string ElementName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        public string StringFormat { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public object FallbackValue { get; set; } = DependencyProperty.UnsetValue;

        /// <summary>
        /// 添加分析符列表
        /// </summary>
        [TypeConverter(typeof(StringToCharArrConveter))]
        public char[] SymbolParameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BindingExtension()
        {

        }


        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var obj = target.TargetObject as DependencyObject;

            //解析出各项属性
            var parameters = this.ParseStrings(this.Path);

            BindingBase bindingBase = null;

            //如果属性值解析出来小于2,那就是0 和 1
            if (parameters.Count < 2)
            {
                //单独绑定
                var binding = new Binding()
                {
                    //为0的时候,就是没有绑定Path,则为单项绑定
                    Mode = parameters.Count == 0 ? BindingMode.OneWay : BindingMode.TwoWay,
                    NotifyOnSourceUpdated = NotifyOnSourceUpdated,
                    NotifyOnTargetUpdated = NotifyOnTargetUpdated,
                    NotifyOnValidationError = NotifyOnValidationError,
                    UpdateSourceExceptionFilter = UpdateSourceExceptionFilter,
                    UpdateSourceTrigger = UpdateSourceTrigger,
                    ValidatesOnDataErrors = ValidatesOnDataErrors,
                    ValidatesOnExceptions = ValidatesOnExceptions,
                    FallbackValue = FallbackValue,
                    ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors,
                };

                //查看有没有Path
                binding.Path = new PropertyPath(parameters.Count == 0 ? null : parameters[0]);
                if (Source != null)
                    binding.Source = Source;

                if (ElementName != null)
                    binding.ElementName = ElementName;

                if (RelativeSource != null)
                    binding.RelativeSource = RelativeSource;

                if (StringFormat != null)
                    binding.StringFormat = StringFormat;

                binding.Converter = this.Converter;
                bindingBase = binding;
            }
            else
            {
                //多值绑定
                var multi = new MultiBinding()
                {
                    ConverterCulture = ConverterCulture,
                    Mode = BindingMode.OneWay,
                    NotifyOnSourceUpdated = NotifyOnSourceUpdated,
                    NotifyOnTargetUpdated = NotifyOnTargetUpdated,
                    NotifyOnValidationError = NotifyOnValidationError,
                    UpdateSourceExceptionFilter = UpdateSourceExceptionFilter,
                    UpdateSourceTrigger = UpdateSourceTrigger,
                    ValidatesOnDataErrors = ValidatesOnDataErrors,
                    ValidatesOnExceptions = ValidatesOnExceptions,
                    FallbackValue = FallbackValue,
                    ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors,
                };

                var window = Window.GetWindow(obj);
                List<string> list = new List<string>();
                foreach (var item in parameters)
                {
                    var start = item[0] + "";
                    //绑定的属性名称是否符合以字母开头(因为解析出来的属性有可能是例如3ab这样的)
                    var re = Regex.Match(start, "[a-z,A-Z]").Success;
                    if (!re)
                    {
                        continue;
                    }
                    var binding = new Binding();
                    if (Source != null)
                        binding.Source = Source;

                    if (this.ElementName != null)
                    {
                        if (window != null)
                        {
                            //查看当前绑定的对象是否存在
                            var value = window.FindName(this.ElementName);
                            if (value != null)
                            {
                                //查看对象上是否有对应的属性,没有的话,就不要绑定ElementName了,不然会去xaml中查找
                                //而这个属性可能是在ViewModel中
                                var pro = value.GetType().GetProperty(item);
                                if (pro != null)
                                    binding.ElementName = ElementName;
                            }
                        }

                    }


                    if (RelativeSource != null)
                        binding.RelativeSource = RelativeSource;

                    binding.Path = new PropertyPath(item);
                    multi.Bindings.Add(binding);
                    list.Add(item);
                }

                //解析工作交给了Converter
                if (window != null)//没有window的话,就不要进行转换
                    multi.Converter = new MultiValueConverter(list, this.Path, obj);
                bindingBase = multi;
            }

            return bindingBase.ProvideValue(serviceProvider);
        }

        private List<string> ParseStrings(string path)
        {
            //分解属性
            if (string.IsNullOrWhiteSpace(path))
            {
                return new List<string>();
            }
            var splitChar = new char[] { ',', ' ', '+', '-', '*', '/', '%', '(', ')', '[', ']', '?', ':', '\'', '\"', '^', '~', '!', '=' }.ToList();
            if (this.SymbolParameters != null)
                splitChar.AddRange(this.SymbolParameters); //将用户自定义的符号数组加入
            var arr = path.Split(splitChar.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var narr = arr.Distinct();
            return narr.ToList();
        }
    }

    /// <summary>
    /// 多值解析器
    /// </summary>
    public class MultiValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// 已经解析的属性值
        /// </summary>
        protected readonly List<string> propertyList;
        /// <summary>
        /// Path绑定路径
        /// </summary>
        protected readonly string path;
        /// <summary>
        /// 绑定的对象
        /// </summary>
        protected readonly DependencyObject BindingObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pros"></param>
        /// <param name="path"></param>
        /// <param name="binding"></param>
        public MultiValueConverter(List<string> pros, string path, DependencyObject binding)
        {
            this.propertyList = pros;
            this.path = path;
            this.BindingObject = binding;
        }

        /// <summary>
        /// 保存每次已经处理的传入数据
        /// </summary>
        private readonly List<object> valueList = new List<object>();
        /// <inheritdoc />
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            #region 编译模式

            if (objMI != null)
            {
                this.valueList.Clear();
                //更新数据
                for (int i = 0; i < values.Length; i++)
                {
                    var value = values[i];
                    //未解析正确的值
                    if (value == DependencyProperty.UnsetValue)
                        continue;
                    this.valueList.Add(value);
                }
                var re = objMI.Invoke(objDynamicCodeEval, this.valueList.ToArray());
                re = System.Convert.ChangeType(re, targetType);
                return re;
            }

            string npath = this.path;
            var strs = new List<string>();
            for (int i = 0; i < propertyList.Count; i++)
            {
                var value = values[i];
                //未解析正确的值
                if (value == DependencyProperty.UnsetValue)
                    continue;
                string pn = propertyList[i];
                var rrrr = GetValueType(pn);
                if (rrrr == null)
                {
                    continue;
                }

                if (pn.Contains("."))
                {
                    pn = pn.Substring(pn.LastIndexOf('.') + 1);
                    npath = npath.Replace(propertyList[i], pn);
                }

                this.valueList.Add(value);
                strs.Add($"{rrrr.FullName} {pn}");
            }

            var str = string.Join(",", strs);
            var result = this.CsharpCalculate(npath, str, this.valueList.ToArray());
            //将object类型转换成对应需要的类型
            result = System.Convert.ChangeType(result, targetType);
            return result;

            #endregion

            #region JS引擎模式

            //string npath = this.path;
            //for (int i = 0; i < propertyList.Count; i++)
            //{
            //    var proName = this.propertyList[i];
            //    //将属性名称替换成真实的值,因为是按照先后顺序进入的,所以这里只用按照顺序来替换就行了
            //    //最后会变成例如这样
            //    //在xaml上为  <TextBlock Text="{e:Binding ElementName=window,Path='Width%2==0?Width/2*1.0:20'}" />
            //    //例如 Width=400
            //    //其中 path 为 Width%2==0?Width/2*1.0:20 ,最后替换就成了  400%2==0?400/2*1.0:20 成了一个纯粹的数学表达式
            //    npath = npath.Replace(proName, values[i] + "");
            //}
            ////动态解析
            //var result = JSEvalExpress(npath);
            ////最后的值必须为基元类型,否则会出错,让其自己出错就行了
            //result = System.Convert.ChangeType(result, targetType);
            //return result; 

            #endregion
        }

        private Type GetValueType(string proName)
        {
            //递归查找属性
            //分解出属性链条 例如 p.age
            PropertyInfo pro = null;
            var splitArr = proName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitArr.Length > 0)
            {
                //找出第一个
                pro = GetInstance(splitArr[0]);
                if (pro == null)
                    return null;
                for (int i = 1; i < splitArr.Length; i++)
                {
                    pro = GetInstance(splitArr[i], pro);
                    if (pro == null)
                        return null;

                }
            }

            if (pro != null)
                return pro.PropertyType;
            return null;
        }

        /// <summary>
        /// 从属性信息中获取包含的属性信息
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected PropertyInfo GetInstance(string proName, PropertyInfo property)
        {
            return property.PropertyType.GetProperty(proName);
        }

        /// <summary>
        /// 通过属性名称获取对应的属性信息
        /// </summary>
        /// <param name="proName"></param>
        /// <returns></returns>
        protected PropertyInfo GetInstance(string proName)
        {
            var window = Window.GetWindow(this.BindingObject);
            if (window != null)
            {
                var pro = window.GetType().GetProperty(proName);
                if (pro != null)
                    return pro;
            }

            //再从datacontext中查找
            var context = this.BindingObject.GetValue(FrameworkElement.DataContextProperty) ?? this.BindingObject.GetValue(FrameworkContentElement.DataContextProperty);
            if (context != null)
            {
                if (context is ObjectDataProvider c)
                    context = c.ObjectInstance;
                var pro = context.GetType().GetProperty(proName);
                if (pro != null)
                    return pro;
            }
            return null;
        }

        /// <summary>
        /// 使用JS引擎进行处理表达式
        /// </summary>
        /// <param name="sExpression"></param>
        /// <returns></returns>
        [Obsolete("该方法已过时,请考虑使用其它实现")]
        protected object JSEvalExpress(string sExpression)
        {
            //利用Js引擎去计算
            //使用过 CSharpCodeProvider 去动态编译 ,但是性能特别拉胯,放弃了
            try
            {
                var ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                return Microsoft.JScript.Eval.JScriptEvaluate(sExpression, ve);
            }
            catch (Exception e)
            {
                //这里是因为在设计时 sExpression 会没有转换成功,而导致出现异常,直接抛 Exception 会在设计时不会显示
                //但运行时如果错误还是会抛的
                throw new Exception(e.GetInnerExceptionMessage());
            }
        }

        private MethodInfo objMI;
        private object objDynamicCodeEval;
        private object CsharpCalculate(string expression, string parameterInfos, object[] values)
        {
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

            // 2.ICodeComplier
            string lan = "CSharp";
            CompilerInfo info = CodeDomProvider.GetCompilerInfo(lan);
            CompilerParameters options = info.CreateDefaultCompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            options.TreatWarningsAsErrors = false;


            options.ReferencedAssemblies.AddRange(TypeDescriptor.AssembliesLocation);

            //引用当前类型的dll
            // 这里为生成的动态代码
            StringBuilder sb = new StringBuilder();

            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{ ");
            sb.Append(Environment.NewLine);
            sb.Append("public class DynamicCodeEval");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("public object Eval(" + parameterInfos + ")");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("return " + expression + ";");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            sb.Append(Environment.NewLine);
            sb.Append("}");

            string code = sb.ToString();

            CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromSource(options, code);

            //如果当前编译有错误
            if (cr.Errors.HasErrors)
            {
                StringBuilder builder = new StringBuilder();
                foreach (CompilerError error in cr.Errors)
                {
                    builder.AppendLine($"Line position:{error.Line} , Message :{error.ErrorText}");
                }
                throw new Exception(builder.ToString());
            }
            else
            {
                Assembly objAssembly = cr.CompiledAssembly;
                objDynamicCodeEval = objAssembly.CreateInstance("DynamicCodeGenerate.DynamicCodeEval");
                objMI = objDynamicCodeEval.GetType().GetMethod("Eval");
                var result = objMI.Invoke(objDynamicCodeEval, values);
                return result;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        struct ParameterInfo
        {
            public ParameterInfo(Type parameterType, string parameterName)
            {
                ParameterType = parameterType;
                ParameterName = parameterName;
            }

            public Type ParameterType { get; set; }
            public string ParameterName { get; set; }

        }
    }

    static class TypeDescriptor
    {
        public static string[] AssembliesLocation { get; }
        static TypeDescriptor()
        {
            //获取当前应用程序域所加载的所有CLR程序集
            AssembliesLocation = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.Location).ToArray();
        }
    }

    /// <summary>
    /// 字符串转字符数组
    /// </summary>
    public class StringToCharArrConveter : TypeConverter
    {
        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return (value + "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(char[]);
        }

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var arr = (value + "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToChar(s)).ToArray();
            return arr;
        }
    }



}
