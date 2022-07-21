using GeneralTool.General.TaskLib;
using GeneralTool.General.WPFHelper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 任务项目
    /// </summary>
    [Serializable]
    public class DoTaskParameterItem : BaseNotifyModel
    {
        private string url;
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url
        {
            get => this.url;
            set => this.RegisterProperty(ref this.url, value);
        }

        /// <summary>
        /// 方法对象
        /// </summary>
        public MethodInfo Method
        {
            get;
            set;
        }

        private ObservableCollection<ParameterItem> parameters = new ObservableCollection<ParameterItem>();
        /// <summary>
        /// 参数列表
        /// </summary>
        public ObservableCollection<ParameterItem> Paramters
        {
            get => this.parameters;
            set
            {
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        item.ValueChanged -= this.Item_ValueChanged;
                        item.ValueChanged += this.Item_ValueChanged;
                    }
                }
                this.RegisterProperty(ref this.parameters, value);
            }
        }

        private void Item_ValueChanged()
        {
            this.SocketArgs = GetArgs();
        }

        private string socketArgs = "";

        /// <summary>
        /// 获取Socket调用时的参数示例
        /// </summary>
        public string SocketArgs
        {
            get
            {

                return this.GetArgs();
            }
            set
            {
                this.RegisterProperty(ref this.socketArgs, value);
            }
        }

        private string GetArgs()
        {
            var builder = new StringBuilder();
            builder.Append("{\"Url\":\"" + this.Url + "\",\"Paramters\":");

            var list = this.Paramters;
            if (list.Count == 0)
                builder.Append("null}");
            else
            {
                var listStr = list.Select(p =>
                {
                    return string.Format("\"{0}\":\"{1}\"", p.ParameterName, p.Value);
                });
                builder.Append("{" + string.Join(",", listStr) + "}}");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 任务执行对象
        /// </summary>
        public BaseTaskInvoke TaskObj
        {
            get;
            set;
        }

        private string explanation;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Explanation
        {
            get => this.explanation;
            set => this.RegisterProperty(ref this.explanation, value);
        }

        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ResultType
        {
            get;
            set;
        }

        /// <summary>
        /// 加入参数信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
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
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            return Paramters.Where(p => p.ParameterName.Equals(key)).FirstOrDefault().Value;
        }
    }
}
