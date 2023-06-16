using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

using GeneralTool.CoreLibrary.TaskLib;
using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 任务项目
    /// </summary>
    [Serializable]
    public class DoTaskParameterItem : BaseNotifyModel
    {
        /// <summary>
        /// 
        /// </summary>
        public DoTaskParameterItem()
        {

        }

        /// <summary>
        /// 初始化语言库
        /// </summary>
        public void InitLangKey()
        {
            if (string.IsNullOrWhiteSpace(LangKey))
                return;
            //设置默认库
            if (!LangProvider.LangProviderInstance.DefaultResource.ContainsKey(LangKey))
            {
                LangProvider.LangProviderInstance.DefaultResource.Add(LangKey, defaultText);
            }

            LangProviderInstance_LangChanged(LangProvider.LangProviderInstance.CurrentResource);
            LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;
        }

        private void LangProviderInstance_LangChanged(System.Windows.ResourceDictionary obj)
        {
            if (string.IsNullOrWhiteSpace(LangKey))
                return;

            string value = LangProvider.LangProviderInstance.GetLangValue(LangKey);
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            Explanation = value;
        }
        #region Private 字段

        private string explanation;
        private ObservableCollection<ParameterItem> parameters = new ObservableCollection<ParameterItem>();
        private string socketArgs = "";
        private string url;
        private string returnString = "";
        private string defaultText;

        #endregion Private 字段

        #region Public 属性

        /// <summary>
        /// 返回字符串
        /// </summary>
        public string ReturnString { get => returnString; set => RegisterProperty(ref returnString, value); }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Explanation
        {
            get => explanation;
            set
            {
                if (string.IsNullOrWhiteSpace(defaultText))
                {
                    defaultText = value;
                }
                RegisterProperty(ref explanation, value);
            }
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
        /// <summary>
        /// 方法对象
        /// </summary>
        public MethodInfo Method
        {
            get;
            set;
        }

        /// <summary>
        /// 参数列表
        /// </summary>
        public ObservableCollection<ParameterItem> Paramters
        {
            get => parameters;
            set
            {
                if (value != null)
                {
                    foreach (ParameterItem item in value)
                    {
                        item.ValueChanged -= Item_ValueChanged;
                        item.ValueChanged += Item_ValueChanged;
                    }
                }
                RegisterProperty(ref parameters, value);
            }
        }

        /// <summary>
        /// 请求方法
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ResultType
        {
            get;
            set;
        }

        /// <summary>
        /// 获取Socket调用时的参数示例
        /// </summary>
        public string SocketArgs
        {
            get
            {
                return string.IsNullOrWhiteSpace(socketArgs) ? GetArgs() : socketArgs;
            }
            set
            {
                RegisterProperty(ref socketArgs, value);
            }
        }

        /// <summary>
        /// 任务执行对象
        /// </summary>
        public BaseTaskInvoke TaskObj
        {
            get;
            set;
        }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url
        {
            get => url;
            set => RegisterProperty(ref url, value);
        }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 加入参数信息
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <param name="value">
        /// </param>
        public void AddParamter(string key, object value)
        {
            Paramters.Add(new ParameterItem
            {
                ParameterName = key,
                Value = value
            });
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        public object GetValue(string key)
        {
            return Paramters.Where(p => p.ParameterName.Equals(key)).FirstOrDefault().Value;
        }

        /// <summary>
        /// 重新加载参数
        /// </summary>
        public void ReloadParameters()
        {
            ParameterInfo[] parameters = Method.GetParameters();
            foreach (ParameterInfo item in parameters)
            {
                Paramters[item.Position].Value = item.DefaultValue;
            }
        }
        #endregion Public 方法

        #region Private 方法

        private string GetArgs()
        {
            StringBuilder builder = new StringBuilder();
            _ = builder.Append("{\"Url\":\"" + Url + "\",\"" + nameof(ServerRequest.Parameters) + "\":");

            ObservableCollection<ParameterItem> list = Paramters;
            if (list.Count == 0)
                _ = builder.Append("null}");
            else
            {
                System.Collections.Generic.IEnumerable<string> listStr = list.Select(p =>
                {
                    return string.Format("\"{0}\":\"{1}\"", p.ParameterName, p.Value);
                });
                _ = builder.Append("{" + string.Join(",", listStr) + "}}");
            }
            return builder.ToString();
        }

        private void Item_ValueChanged()
        {
            SocketArgs = GetArgs();
        }

        #endregion Private 方法
    }
}