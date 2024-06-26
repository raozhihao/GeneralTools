﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using GeneralTool.CoreLibrary.TaskLib;

namespace GeneralTool.CoreLibrary.Attributes
{
   
    public abstract class RouteVisibleEditor
    {

        /// <summary>
        /// 带入的 Route
        /// </summary>
        public RouteAttribute Route { get; set; }

        public abstract bool Visible();
    }

    /// <summary>
    /// 路由特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
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
            Method = method;
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
            get => returnString;
            set => RegisterProperty(ref returnString, value);
        }

        private string explantion;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Explanation
        {
            get => explantion;
            set => RegisterProperty(ref explantion, value);
        }

        private object target;
        /// <summary>
        /// 其它额外信息
        /// </summary>
        public object Target
        {
            get => this.target;
            set => this.RegisterProperty(ref this.target, value);
        }

        private object target2;
        /// <summary>
        /// 其它额外信息
        /// </summary>
        public object Target2
        {
            get => this.target2;
            set => this.RegisterProperty(ref this.target2, value);
        }

        private Type routeVisibleEditor;
        /// <summary>
        /// 需要显示的类型编辑器,其类型必须继承自 <see cref="RouteVisibleEditor"/>
        /// </summary>
        public Type RouteVisibleEditor
        {
            get => this.routeVisibleEditor;
            set => this.RegisterProperty(ref this.routeVisibleEditor, value);
        }

        private string url;
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url
        {
            get => url;
            set => RegisterProperty(ref url, value);
        }

        private HttpMethod method;
        /// <summary>
        /// 请示的Http方法
        /// </summary>
        public HttpMethod Method
        {
            get => method;
            set => RegisterProperty(ref method, value);
        }

        private string langKey;
        /// <summary>
        /// 语言Key
        /// </summary>
        public string LangKey
        {
            get => langKey;
            set => RegisterProperty(ref langKey, value);
        }

        private int sortIndex;
        /// <summary>
        /// 排序
        /// </summary>
        public int SortIndex
        {
            get => sortIndex;
            set => RegisterProperty(ref sortIndex, value);
        }

        private bool reReponse;
        /// <summary>
        /// 是否原样返回
        /// </summary>
        public bool ReReponse
        {
            get => reReponse;
            set => RegisterProperty(ref reReponse, value);
        }

        private string reReponseFomartErroString;
        /// <summary>
        /// 当原样返回出现未捕获的异常时,应当返回的错误信息格式
        /// </summary>
        public string ReReponseErroFomartString
        {
            get => reReponseFomartErroString;
            set => RegisterProperty(ref reReponseFomartErroString, value);
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