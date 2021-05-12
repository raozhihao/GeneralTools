using System;
using System.Reflection;

namespace GeneralTool.General.IniHelpers
{
    /// <summary>
    /// 节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Node<T>
    {
        /// <summary>
        /// 当前节点的所属
        /// </summary>
        public string SectionName { get; set; }
        /// <summary>
        /// 当前键名称
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// 当前默认值
        /// </summary>
        public T DefaultValue { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public T Value
        {
            get
            {
                //获取值
                var tmp = IniHelper.IniHelperInstance.GetValue<string>(SectionName, KeyName);
                if (string.IsNullOrWhiteSpace(tmp))
                    return DefaultValue;
                var type = typeof(T);
                //如果是泛型或数组类型
                if (type.IsGenericType || type.IsArray)
                {
                    //创建类型
                    var arr = tmp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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
                            var method = type.GetMethod("Add", new Type[] { type.GenericTypeArguments[0] });
                            for (int i = 0; i < arr.Length; i++)
                            {
                                method.Invoke(obj, new object[] { Convert.ChangeType(arr[i], type.GenericTypeArguments[0]) });
                            }
                        }


                    }

                    else
                    {
                        string assName = type.FullName.Replace("[]", string.Empty);
                        var t = Type.GetType(assName);
                        obj = Activator.CreateInstance(type, new object[] { arr.Length });
                        var method = type.GetMethod("Set", new Type[] { typeof(int), t });
                        for (int i = 0; i < arr.Length; i++)
                        {
                            method.Invoke(obj, new object[] { i, Convert.ChangeType(arr[i], t) });
                        }
                    }

                    return (T)obj;
                }
                else
                {
                    return IniHelper.IniHelperInstance.GetValue<T>(SectionName, KeyName, default);
                }

            }
            set
            {
                var type = value.GetType();
                if (type.IsGenericType || type.IsArray)
                {
                    //获取长度
                    int len = 0;
                    MethodInfo get = null;
                    if (type.IsGenericType)
                    {
                        len = (int)(type.GetMethod("get_Count").Invoke(value, null));
                        get = type.GetMethod("get_Item", new Type[] { typeof(int) });
                    }

                    else if (type.IsArray)
                    {
                        len = (int)(type.GetMethod("get_Length").Invoke(value, null));
                        get = type.GetMethod("GetValue", new Type[] { typeof(int) });
                    }



                    // var set = type.GetMethod("SetValue");
                    object[] objs = new object[len];
                    for (int i = 0; i < len; i++)
                    {
                        objs[i] = get.Invoke(value, new object[] { i });
                    }
                    string val = string.Join(",", objs);
                    IniHelper.IniHelperInstance.WriteValue(SectionName, KeyName, val);
                }
                else
                {
                    IniHelper.IniHelperInstance.WriteValue<T>(SectionName, KeyName, value);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <param name="keyName">键名</param>
        /// <param name="defaultValue">默认值</param>
        public Node(string sectionName, string keyName, T defaultValue)
        {
            this.SectionName = sectionName;
            this.KeyName = keyName;
            this.DefaultValue = defaultValue;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(Node<T> value)
        {
            return value.Value;
        }

    }
}
