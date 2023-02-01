using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using GeneralTool.General.ValueTypeExtensions;

namespace GeneralTool.General
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public class SerializeHelpers
    {
        #region Public 方法

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="data">
        /// </param>
        /// <returns>
        /// </returns>
        public T Desrialize<T>(byte[] data)
        {
            return (T)Desrialize(data);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data">
        /// </param>
        /// <returns>
        /// </returns>
        public object Desrialize(byte[] data)
        {
            Type type = null;

            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryReader binary = new BinaryReader(ms);
                int len = binary.ReadInt32();
                byte[] typeData = binary.ReadBytes(len);
                string typeStr = typeData.ToStrings();//获取当前类型信息
                type = Type.GetType(typeStr);//得到当前类型
                data = binary.ReadBytes(Convert.ToInt32(ms.Length - len));
            }
            object obj;
            if (type.IsGenericType)
            {
                obj = DesrializeGeneric(data, type);
            }
            else if (type.IsArray)
            {
                obj = DesrializeArrary(data, type);
            }
            else if (type.GetCustomAttribute<SerializableAttribute>() != null)
            {
                obj = DeserializeToObj(data);
            }
            else if (type.IsValueType && !type.IsPrimitive)
            {
                //反序列化自定义结构
                obj = DesrializeClass(data, type);
            }
            else if (type == typeof(string))
            {
                obj = Encoding.UTF8.GetString(data);
            }
            else if (type == typeof(byte))
            {
                obj = (data);
            }
            else if (type == typeof(bool))
            {
                obj = (BitConverter.ToBoolean(data, 0));
            }
            else if (type == typeof(short))
            {
                obj = (BitConverter.ToInt16(data, 0));
            }
            else if (type == typeof(int))
            {
                obj = (BitConverter.ToInt32(data, 0));
            }
            else if (type == typeof(long))
            {
                obj = (BitConverter.ToInt64(data, 0));
            }
            else if (type == typeof(float))
            {
                obj = (BitConverter.ToSingle(data, 0));
            }
            else if (type == typeof(double))
            {
                obj = (BitConverter.ToDouble(data, 0));
            }
            else if (type == typeof(decimal))
            {
                obj = (BitConverter.ToDouble(data, 0));
            }
            else if (type == typeof(DateTime))
            {
                string dstr = Encoding.UTF8.GetString(data);
                obj = new DateTime(long.Parse(dstr));
            }
            else if (type.BaseType == typeof(Enum))
            {
                Type numType = Enum.GetUnderlyingType(type);

                if (numType == typeof(byte))
                {
                    obj = Enum.ToObject(type, data[0]);
                }
                else if (numType == typeof(short))
                {
                    obj = Enum.ToObject(type, BitConverter.ToInt16(data, 0));
                }
                else if (numType == typeof(int))
                {
                    obj = Enum.ToObject(type, BitConverter.ToInt32(data, 0));
                }
                else
                {
                    obj = Enum.ToObject(type, BitConverter.ToInt64(data, 0));
                }
            }
            else if (type == typeof(DataTable) || type == typeof(DataSet) || type.IsSubclassOf(typeof(Delegate)))
            {
                obj = DeserializeToObj(data);
            }
            else if (type == typeof(byte[]))
            {
                obj = data;
            }
            else if (type.IsClass)
            {
                obj = DesrializeClass(data, type);
            }
            else
            {
                obj = null;
            }
            return obj;
        }

        /// <summary>
        /// 序列化(注意,对于打了 SerializableAttribute 的类,则会使用系统自带的序列化方式.
        /// 请自行处理内部需要序列化的属性(如果该属性类未不支持SerializableAttribute标记则会报错). 如果类中有循环引用,请务必使用 SerializableAttribute
        /// </summary>
        /// <param name="param">
        /// </param>
        /// <returns>
        /// </returns>
        public byte[] Serialize(object param)
        {
            Type type = param.GetType();
            return Serialize(param, type);
        }

        #endregion Public 方法

        #region Private 方法

        private object DeserializeToObj(byte[] data)
        {
            object obj = null;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter binary = new BinaryFormatter();
                obj = binary.Deserialize(stream);
            }

            return obj;
        }

        private object DesrializeArrary(byte[] data, Type type)
        {
            object obj = null;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader formart = new BinaryReader(stream);
                int len = formart.ReadInt32();//获取数据的长度
                                              //生成数组
                obj = Activator.CreateInstance(type, len) as Array;
                Type elType = type.GetElementType();
                MethodInfo method = type.GetMethod("SetValue", new Type[] { typeof(object), typeof(int) });//添加方法SetValue
                                                                                                           //获取每一项
                for (int i = 0; i < len; i++)
                {
                    //读取长度
                    int clen = formart.ReadInt32();
                    byte[] cdata = formart.ReadBytes(clen);//当前项的长度
                    object elObj = Desrialize(cdata);
                    //将数据加入进去
                    method.Invoke(obj, new object[] { elObj, i });                                  //反序列化出来

                }
            }
            return obj;
        }

        private object DesrializeClass(byte[] data, Type type)
        {
            object obj = Activator.CreateInstance(type);
            //创建类型

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader binary = new BinaryReader(stream);
                //获取所有属性
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    //先获取当前属性类型

                    //反序列化,获取当前属性的值
                    int plen = binary.ReadInt32();//当前属性的长度
                    if (plen == 0)
                    {
                        continue;//当前属性没有值
                    }
                    byte[] cdata = binary.ReadBytes(plen);
                    object proObj = Desrialize(cdata);
                    //将当前属性加入进去
                    MethodInfo method = type.GetMethod("set_" + property.Name);
                    if (method == null)
                    {
                        //没有set方法,直接跳过
                        continue;
                    }
                    //有,则加入
                    method.Invoke(obj, new object[] { proObj });

                }
            }
            return obj;
        }

        private object DesrializeGeneric(byte[] data, Type type)
        {
            object obj = null;

            Type inter = type.GetInterface(typeof(IEnumerable).FullName, false);
            if (inter != null)
            {
                //生成泛型
                obj = Activator.CreateInstance(type);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    BinaryReader formart = new BinaryReader(stream);

                    //判断该泛型是数组类型还是自定义类型

                    //数据类型
                    //填充数据
                    //获取数据长度
                    int len = formart.ReadInt32();

                    Type[] argTypes = type.GenericTypeArguments;
                    MethodInfo addMethod = type.GetMethod("Add");
                    for (int i = 0; i < len; i++)
                    {
                        if (argTypes.Length == 1)
                        {
                            int clen = formart.ReadInt32();
                            byte[] cdata = formart.ReadBytes(clen);
                            object proObj = Desrialize(cdata);
                            addMethod.Invoke(obj, new object[] { proObj });
                        }
                        else
                        {
                            object[] paraObj = new object[argTypes.Length];

                            for (int j = 0; j < argTypes.Length; j++)
                            {
                                //循环每一项,获取其字节数据
                                //获取数据
                                int clen = formart.ReadInt32();//数据长度
                                byte[] cdata = formart.ReadBytes(clen);//当前项的字节数据
                                object proObj = Desrialize(cdata);
                                paraObj[j] = proObj;
                            }
                            addMethod.Invoke(obj, paraObj);
                        }
                    }
                }
            }
            else
            {
                using (MemoryStream stream = new MemoryStream(data))
                {
                    BinaryReader binary = new BinaryReader(stream);
                    int len = binary.ReadInt32();//类的长度
                    byte[] cdata = binary.ReadBytes(len);//类的信息
                                                         //自定义类型
                    obj = DesrializeClass(cdata, type);
                }
            }

            return obj;
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="param">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private byte[] Serialize(object param, Type type)
        {
            List<byte> datas = new List<byte>();
            byte[] typeData = type.AssemblyQualifiedName.ToBytes();//写入当前类型的长度
            datas.AddRange(BitConverter.GetBytes(typeData.Length));//写入当前类型信息的数据
            datas.AddRange(typeData);//写入类型信息

            byte[] data = null;
            SerializableAttribute attr = type.GetCustomAttribute<SerializableAttribute>();
            if (type.IsGenericType)
            {
                data = SerializeGeneric(param, type);//读取泛型数据
            }
            else if (type.IsArray)
            {
                data = SerializeArrary(param, type);//读取数组数据
            }
            else if (attr != null)
            {
                data = SerializeToBytes(param);
            }
            else if (param is string @string)
            {
                data = Encoding.UTF8.GetBytes(@string);
            }
            else if (param is byte @byte)
            {
                data = new byte[] { @byte };
            }
            else if (param is bool boolean)
            {
                data = BitConverter.GetBytes(boolean);
            }
            else if (param is short @int)
            {
                data = BitConverter.GetBytes(@int);
            }
            else if (param is int int1)
            {
                data = BitConverter.GetBytes(int1);
            }
            else if (param is long int2)
            {
                data = BitConverter.GetBytes(int2);
            }
            else if (param is float single)
            {
                data = BitConverter.GetBytes(single);
            }
            else if (param is double @double)
            {
                data = BitConverter.GetBytes(@double);
            }
            else if (param is DateTime time)
            {
                string str = time.Ticks.ToString();
                data = Encoding.UTF8.GetBytes(str);
            }
            else if (param is Enum)
            {
                Type enumValType = Enum.GetUnderlyingType(param.GetType());

                if (enumValType == typeof(byte))
                {
                    data = new byte[] { (byte)param };
                }
                else if (enumValType == typeof(short))
                {
                    data = BitConverter.GetBytes((short)param);
                }
                else if (enumValType == typeof(int))
                {
                    data = BitConverter.GetBytes((int)param);
                }
                else
                {
                    data = BitConverter.GetBytes((long)param);
                }
            }
            else if (param is DataTable || param is DataSet || param is Delegate)
            {
                data = SerializeToBytes(param);
            }
            else if (param is byte[] v)
            {
                data = v;
            }
            else if (param is ValueType && !type.IsPrimitive)
            {
                //查看是否有序列化标记
                attr = type.GetCustomAttribute<SerializableAttribute>();
                if (attr != null)
                {
                    data = SerializeToBytes(param);
                }
                else
                {
                    //自定义结构
                    data = SerializeClass(param, type);
                }
            }
            else if (type.IsClass || param is IntPtr)
            {
                data = SerializeClass(param, type);//读取类型数据
            }
            //else
            //{
            //    if (type.IsGenericType)
            //    {
            //        data = SerializeGeneric(param, type);//读取泛型数据
            //    }
            //    else if (type.IsArray)
            //    {
            //        data = SerializeArrary(param, type);//读取数组数据
            //    }
            //    else if (type.IsClass || param is IntPtr)
            //    {
            //        data = SerializeClass(param, type);//读取类型数据
            //    }

            //}

            if (data != null)
            {
                datas.AddRange(data);
            }

            return datas.ToArray();
        }

        private byte[] SerializeArrary(object param, Type type)
        {
            //查看是否一维数组
            if (type.GetArrayRank() != 1)
            {
                throw new Exception("数组只支持一维格式");
            }

            List<byte> datas = new List<byte>();

            //查看数组类型
            Array arr = param as Array;
            int len = arr.Length;//数组长度
            datas.AddRange(BitConverter.GetBytes(len));//写入数组长度
            foreach (object item in arr)
            {
                //循环数组中的每一项

                byte[] cd = Serialize(item);

                datas.AddRange(BitConverter.GetBytes(cd.Length));
                datas.AddRange(cd);
            }
            return datas.ToArray();
        }

        private byte[] SerializeClass(object param, Type type)
        {
            List<byte> data = new List<byte>();

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo item in properties)//循环类的每一个属性
            {
                //获取属性值
                object value = item.GetValue(param, null);
                byte[] cdata;
                if (value == null)
                {
                    cdata = new byte[0];
                }
                else
                {
                    cdata = Serialize(value);//读取当前属性的值数据(其中包括该属性的类型元数据信息与值信息 4+X+Y)
                }

                //写入当前属性的长度
                data.AddRange(BitConverter.GetBytes(cdata.Length));
                data.AddRange(cdata);//写入当前属性的数据信息(其中包括该属性的类型元数据信息与值信息 4+X+Y)
            }
            return data.ToArray();
        }

        private byte[] SerializeGeneric(object param, Type type)
        {
            List<byte> datas = new List<byte>();
            //如果是数组类泛型
            if (param is IEnumerable)
            {
                int len = (int)type.GetMethod("get_Count").Invoke(param, null);//泛型长度
                datas.AddRange(BitConverter.GetBytes(len));

                Type[] argsType = type.GenericTypeArguments;

                IEnumerable parInumerator = param as IEnumerable;
                IEnumerator inumerator = parInumerator.GetEnumerator();
                while (inumerator.MoveNext())
                {
                    object obj = inumerator.Current;//泛型集合中每一项
                    if (argsType.Length == 1)
                    {
                        byte[] cdata = Serialize(obj);//序列化

                        datas.AddRange(BitConverter.GetBytes(cdata.Length));//获取长度
                        datas.AddRange(cdata);//获取序列化后的内容
                    }
                    else
                    {
                        PropertyInfo[] properies = obj.GetType().GetProperties();
                        for (int i = 0; i < argsType.Length; i++)//循环每一个占位符
                        {
                            PropertyInfo property = properies[i];

                            MethodInfo method = obj.GetType().GetMethod("get_" + property.Name);

                            if (method != null)
                            {
                                object tmpObj = method.Invoke(obj, null);
                                var argData = Serialize(tmpObj);
                                datas.AddRange(BitConverter.GetBytes(argData.Length));//获取长度
                                datas.AddRange(argData);//获取序列化后的内容
                            }
                        }
                    }
                }
            }
            else
            {
                //自定义类泛型,获取数据类型, 获取所有属性
                byte[] cdata = SerializeClass(param, type);//读取泛型类的所有数据
                datas.AddRange(BitConverter.GetBytes(cdata.Length));//获取长度
                datas.AddRange(cdata);//获取序列化后的内容
            }

            return datas.ToArray();
        }

        private byte[] SerializeToBytes(object param)
        {
            byte[] byts = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binary = new BinaryFormatter();
                binary.Serialize(stream, param);
                byts = stream.ToArray();
            }
            return byts;
        }

        #endregion Private 方法
    }
}