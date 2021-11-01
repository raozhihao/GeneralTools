using GeneralTool.General.NetHelper;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// 路由特性
    /// </summary>
    public class RouteAttribute : Attribute, INotifyPropertyChanged
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <param name="explanation">
        /// </param>
        /// <param name="method"></param>
        public RouteAttribute(string url, string explanation = "", HttpMethod method = HttpMethod.GET)
        {
            Url = url;
            Explanation = explanation;
            this.Method = method;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;


        #endregion Public 构造函数

        #region Public 属性

        private string returnString;
        /// <summary>
        /// 自定义返回字符串
        /// </summary>
        public string ReturnString
        {
            get => this.returnString;
            set => this.RegisterProperty(ref this.returnString, value);
        }

        private string explantion;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Explanation
        {
            get => this.explantion;
            set => this.RegisterProperty(ref this.explantion, value);
        }

        private string url;
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url
        {
            get => this.url;
            set => this.RegisterProperty(ref this.url, value);
        }

        private HttpMethod method;
        /// <summary>
        /// 请示的Http方法
        /// </summary>
        public HttpMethod Method
        {
            get => this.method;
            set => this.RegisterProperty(ref this.method, value);
        }

        private string langKey;
        /// <summary>
        /// 语言Key
        /// </summary>
        public string LangKey
        {
            get => this.langKey;
            set => this.RegisterProperty(ref this.langKey, value);
        }

        #endregion Public 属性


        /// <summary>
        /// 向组件注册属性更改 <example>
        /// <code><para>private int _id;</para><para>public int Id</para><para>{</para><para>    get =&gt; this.id;</para><para>    set =&gt; RegisterProperty(ref this.id, value);</para><para>}</para></code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">
        /// 注册的属性类型
        /// </typeparam>
        /// <param name="fieldValue">
        /// 注册的属性的字段
        /// </param>
        /// <param name="value">
        /// 注册的属性的实际值
        /// </param>
        /// <param name="propertyName">
        /// 注册的属性的名称(可忽略不指定,默认生成)
        /// </param>
        public void RegisterProperty<T>(ref T fieldValue, T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                fieldValue = value;
                return;
            }

            if (fieldValue != null && fieldValue.Equals(value))
            {
                return;
            }

            fieldValue = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}