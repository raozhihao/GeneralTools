using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace GeneralTool.General.Ioc
{
    /// <summary>
    /// 简单Ioc服务
    /// </summary>
    public class SimpleIocSerivce
    {
        private SimpleIocSerivce() { }

        private static readonly Lazy<SimpleIocSerivce> serivce;
        /// <summary>
        /// 服务实例
        /// </summary>
        public static SimpleIocSerivce SimpleIocSerivceInstance { get; }

        static SimpleIocSerivce()
        {
            serivce = new Lazy<SimpleIocSerivce>(() => new SimpleIocSerivce());
            SimpleIocSerivceInstance = serivce.Value;
        }

        private readonly Dictionary<Type, DefinedClass> typeDics = new Dictionary<Type, DefinedClass>();

        private readonly Dictionary<Type, DefinedClass> interfaceDics = new Dictionary<Type, DefinedClass>();

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            //赋值
            foreach (var item in typeDics)
            {
                //获取所有属性
                var pros = item.Key.GetProperties();
                foreach (var pro in pros)
                {
                    //查看当前属性的类型是否已注册
                    if (this.typeDics.ContainsKey(pro.PropertyType))
                    {
                        //存在,则进行赋值
                        if (pro.SetMethod != null)
                        {
                            var obj = this.typeDics[pro.PropertyType].Instance;
                            pro.SetValue(item.Value.Instance, obj);
                        }
                    }
                    else if (pro.PropertyType.IsInterface)
                    {
                        //如果是接口,则找到与其一致的
                        if (this.interfaceDics.ContainsKey(pro.PropertyType))
                        {
                            //存在,则进行赋值
                            if (pro.SetMethod != null)
                            {
                                var obj = this.interfaceDics[pro.PropertyType].Instance;
                                pro.SetValue(item.Value.Instance, obj);
                            }
                        }
                    }
                }
            }

            //调用构造函数
            foreach (var item in typeDics)
            {
                //进行构造函数调用
                if (item.Value.IsUninitializedObject)
                {
                    var type = item.Value.Instance.GetType();
                    //没有构造函数的,则查找第一个构造函数
                    var constructors = type.GetConstructors();
                    if (constructors.Length == 0)
                    {
                        throw new Exception($"类型 : {type} 没有公共的构造函数");
                    }

                    //直接获取第一个
                    var constructor = constructors[0];
                    var parameters = constructor.GetParameters();

                    var datas = new object[parameters.Length];
                    int index = 0;
                    foreach (var pro in parameters)
                    {
                        //查看当前属性的类型是否已注册
                        if (this.typeDics.ContainsKey(pro.ParameterType))
                        {
                            datas[index] = this.typeDics[pro.ParameterType].Instance;
                        }
                        else if (pro.ParameterType.IsInterface)
                        {
                            //如果是接口,则找到与其一致的
                            if (this.interfaceDics.ContainsKey(pro.ParameterType))
                            {
                                //存在,则进行赋值
                                datas[index] = this.interfaceDics[pro.ParameterType].Instance;
                            }
                            else
                            {
                                datas[index] = default;
                            }
                        }
                        else
                        {
                            datas[index] = default;
                        }
                        index++;
                    }
                    constructor.Invoke(item.Value.Instance, datas);
                }
            }

            //调用函数
            foreach (var item in typeDics)
            {
                if (item.Value.Instance is IInitInterface t)
                    t.Init();
                if (item.Value.InitMethod != null)
                    item.Value.InitMethod.Invoke(item.Value.Instance, null);
            }
        }


        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        public void Inject<T>() where T : class
        {
            var type = typeof(T);
            this.Inject(type);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <param name="methodName"></param>
        public void Inject(Type type, string methodName = null)
        {
            var obj = FormatterServices.GetUninitializedObject(type);
            this.Inject(obj, true, methodName);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="isUninitializedObject"></param>
        /// <param name="methodName">需要调用的方法名称</param>
        public void Inject(object obj, bool isUninitializedObject = false, string methodName = null)
        {
            this.Inject(null, obj, isUninitializedObject, methodName);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="instance">实例</param>
        /// <param name="methodName">需要调用的方法名称</param>
        public void Inject<TInterface>(object instance, string methodName = null) where TInterface : class
        {
            var intefaceType = typeof(TInterface);
            this.Inject(intefaceType, instance, false, methodName);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="intefaceType">接口类型</param>
        /// <param name="instance">实例</param>
        /// <param name="isUninitializedObject"></param>
        /// <param name="methodName">需要调用的方法名称</param>
        private void Inject(Type intefaceType, object instance, bool isUninitializedObject, string methodName = null)
        {
            var type = instance.GetType();
            if (intefaceType != null)
            {
                //判断是否为接口
                if (!intefaceType.IsInterface)
                {
                    throw new Exception($"类型 {intefaceType} 不是一个接口");
                }
                if (type.IsAssignableFrom(intefaceType))
                {
                    throw new Exception($"类型 {type} 不实现接口 {intefaceType}");
                }
            }

            MethodInfo method = null;
            if (!string.IsNullOrWhiteSpace(methodName))
                method = type.GetMethod(methodName);
            var defined = new DefinedClass() { Instance = instance, InterfaceType = intefaceType, InitMethod = method, TType = type, IsUninitializedObject = isUninitializedObject };
            if (!this.typeDics.ContainsKey(type))
                typeDics.Add(type, defined);
            else
                typeDics[type] = defined;

            if (intefaceType != null)
            {
                if (!this.interfaceDics.ContainsKey(intefaceType))
                    this.interfaceDics.Add(intefaceType, defined);
                else
                    this.interfaceDics[intefaceType] = defined;
            }

        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="intefaceType">接口类型</param>
        /// <param name="type">接口对应实例类型(需要无参构造函数)</param>
        /// <param name="methodName">调用方法</param>
        public void Inject(Type intefaceType, Type type, string methodName = null)
        {
            var instance = FormatterServices.GetUninitializedObject(type);
            this.Inject(intefaceType, instance, true, methodName);

        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TType">实例类型</typeparam>
        /// <param name="methodName">实例化成功后调用的方法名称</param>
        public void Inject<TInterface, TType>(string methodName = null) where TType : class
        {
            this.Inject(typeof(TInterface), typeof(TType), methodName);
        }

        /// <summary>
        /// 注册类型并确定实例化后调用的方法名称
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="methodName">实例化成功后调用的方法名称</param>
        public void Inject<T>(string methodName = null) where T : class
        {
            var type = typeof(T);
            this.Inject(type, methodName);
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return this.Resolve(typeof(T)) as T;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">要获取的类型</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            if (typeDics.ContainsKey(type))
                return typeDics[type].Instance;
            return null;
        }
    }


}
