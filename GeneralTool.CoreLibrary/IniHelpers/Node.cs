using System;
using System.Reflection;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;

namespace GeneralTool.CoreLibrary.IniHelpers
{
    /// <summary>
    /// 配置项
    /// </summary>
    public abstract class Category
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="sectionName">
        /// </param>
        public Category(string sectionName)
        {
            SectionName = sectionName;

        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 节点名称
        /// </summary>
        public string SectionName { get; set; }

        #endregion Public 属性

    }



    /// <summary>
    /// 节点
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class Node<T>
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="sectionName">
        /// 节点名称
        /// </param>
        /// <param name="keyName">
        /// 键名
        /// </param>
        /// <param name="defaultValue">
        /// 默认值
        /// </param>
        /// <param name="create">是否在空时自动创建默认值</param>
        /// <param name="iniPath">单独保存到的文件路径,如无,则使用 Configs/default.ini</param>
        /// <param name="isJosn">是否Json</param>
        /// <param name="convert">Json的转换器,如无则使用默认转换器</param>
        public Node(string sectionName, string keyName, T defaultValue, bool create = false, string iniPath = "", bool isJosn = false, IJsonConvert convert = null)
        {
            SectionName = sectionName;
            KeyName = keyName;

            DefaultValue = defaultValue;
            IniPath = iniPath;

            IsJosn = isJosn;
            JsonConvert = convert;

          
            if (create)
            {
                string tmp = this.IniHelper.GetValue<string>(SectionName, KeyName);
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    Value = defaultValue;
                }
            }
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 是否为Json类型
        /// </summary>
        public bool IsJosn { get; set; }

        /// <summary>
        /// Json的转换器
        /// </summary>
        public IJsonConvert JsonConvert { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IniHelper IniHelper { get; set; }

        private string iniPath;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string IniPath
        {
            get => iniPath;
            set
            {
                IniHelper = string.IsNullOrEmpty(iniPath) ? IniHelper.IniHelperInstance : new IniHelper(iniPath);
                iniPath = value;
            }
        }

        /// <summary>
        /// 当前默认值
        /// </summary>
        public T DefaultValue { get; set; }

        /// <summary>
        /// 当前键名称
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 当前节点的所属
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public T Value
        {
            get
            {
                //获取值
                string tmp = this.IniHelper.GetValue<string>(SectionName, KeyName);
                if (string.IsNullOrWhiteSpace(tmp))
                    return DefaultValue;
                Type type = typeof(T);


                if (IsJosn)
                {
                    //转换为Json,查看有无Json转换器
                    T obj = (T)DesrializeToObj(JsonConvert, type);

                    return obj;
                }

                //如果是泛型或数组类型
                if (type.IsGenericType || type.IsArray)
                {
                    //创建类型
                    string[] arr = tmp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    object obj;
                    if (type.IsGenericType)
                    {
                        if (type.GenericTypeArguments[0] == typeof(string))
                        {
                            obj = Activator.CreateInstance(type, new object[] { arr });
                        }
                        else
                        {
                            obj = Activator.CreateInstance(type, new object[] { arr.Length });
                            MethodInfo method = type.GetMethod("Add", new Type[] { type.GenericTypeArguments[0] });
                            for (int i = 0; i < arr.Length; i++)
                            {
                                _ = method.Invoke(obj, new object[] { Convert.ChangeType(arr[i], type.GenericTypeArguments[0]) });
                            }
                        }
                    }
                    else
                    {
                        string assName = type.FullName.Replace("[]", string.Empty);
                        Type t = Type.GetType(assName);
                        obj = Activator.CreateInstance(type, new object[] { arr.Length });
                        MethodInfo method = type.GetMethod("Set", new Type[] { typeof(int), t });
                        for (int i = 0; i < arr.Length; i++)
                        {
                            _ = method.Invoke(obj, new object[] { i, Convert.ChangeType(arr[i], t) });
                        }
                    }

                    return (T)obj;
                }
                else
                {
                    return this.IniHelper.GetValue<T>(SectionName, KeyName, default);
                }
            }
            set
            {
                Type type = value.GetType();

                if (IsJosn)
                {
                    //转换为Json,查看有无Json转换器

                    string jsonStr = SerializeToString(JsonConvert, value);
                    this.IniHelper.WriteValueString(SectionName, KeyName, jsonStr);

                    return;
                }
                if (type.IsGenericType || type.IsArray)
                {
                    //获取长度
                    int len = 0;
                    MethodInfo get = null;
                    if (type.IsGenericType)
                    {
                        len = (int)type.GetMethod("get_Count").Invoke(value, null);
                        get = type.GetMethod("get_Item", new Type[] { typeof(int) });
                    }
                    else if (type.IsArray)
                    {
                        len = (int)type.GetMethod("get_Length").Invoke(value, null);
                        get = type.GetMethod("GetValue", new Type[] { typeof(int) });
                    }

                    // var set = type.GetMethod("SetValue");
                    object[] objs = new object[len];
                    for (int i = 0; i < len; i++)
                    {
                        objs[i] = get.Invoke(value, new object[] { i });
                    }
                    string val = string.Join(",", objs);
                    this.IniHelper.WriteValueString(SectionName, KeyName, val);
                }
                else
                {
                    this.IniHelper.WriteValueT<T>(SectionName, KeyName, value);
                }
            }
        }

        private object DesrializeToObj(IJsonConvert jsonType, Type type)
        {
            string value = this.IniHelper.GetString(SectionName, KeyName, GetDefaultValue(jsonType));
            object obj = jsonType != null ? jsonType.DeserializeObject(value, type) : value.DeserializeJsonToObject(type);
            return obj;
        }

        private string GetDefaultValue(IJsonConvert jsonType)
        {
            return SerializeToString(jsonType, DefaultValue);
        }

        private string SerializeToString(IJsonConvert jsonConvert, T value)
        {
            string jsonStr = jsonConvert != null ? jsonConvert.SerializeObject(value) : value.SerializeToJsonString();
            return jsonStr;
        }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value">
        /// </param>
        public static implicit operator T(Node<T> value)
        {
            return value.Value;
        }


        #endregion Public 方法
    }
}