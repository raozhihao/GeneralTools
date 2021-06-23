using GeneralTool.General.TaskLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 任务项目
    /// </summary>
    [Serializable]
    public class DoTaskParameterItem
    {
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url
        {
            get;
            set;
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
        public List<ParameterItem> Paramters
        {
            get;
            set;
        } = new List<ParameterItem>();

        /// <summary>
        /// 任务执行对象
        /// </summary>
        public BaseTaskInvoke TaskObj
        {
            get;
            set;
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Explanation
        {
            get;
            set;
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
