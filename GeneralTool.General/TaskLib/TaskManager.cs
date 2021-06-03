using GeneralTool.General.Attributes;
using GeneralTool.General.Interfaces;
using GeneralTool.General.LinqExtensions;
using GeneralTool.General.Logs;
using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 任务对象
    /// </summary>
    public class TaskManager
    {
        private string erroMsg;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErroMsg
        {
            get
            {
                return erroMsg;
            }
        }

        /// <summary>
        /// 服务器站点
        /// </summary>
        internal Station ServerStation { get; set; }
        /// <summary>
        /// 是否被初始化
        /// </summary>
        private bool IsInit { get; set; } = false;
        /// <summary>
        /// 任务函数核参数列表
        /// </summary>
        private readonly Dictionary<string, DoTaskParameterItem> TaskParameterItems = new Dictionary<string, DoTaskParameterItem>();

        private readonly ILog log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonConvert">Json转换器</param>
        /// <param name="log">日志组件</param>
        public TaskManager(IJsonConvert jsonConvert, ILog log)
        {
            if (log == null)
                log = new ConsoleLogInfo();

            this.log = log;
            this.ServerStation = new Station(jsonConvert, log);
        }


        /// <summary>
        /// 
        /// </summary>
        public TaskManager() : this(null, new ConsoleLogInfo())
        {

        }

        private BaseTaskInvoke[] taskInokes;

        /// <inheritdoc/>
        public bool Open(string ip, int port, params BaseTaskInvoke[] target)
        {
            try
            {
                if (this.IsInit)
                {
                    this.ServerStation.Close();
                }
                if (!this.IsInit)
                {
                    this.taskInokes = target;
                    target.Foreach(o =>
                    {
                        this.ServerStation.AddStationObjectClass(o);
                    });
                    this.IsInit = true;
                }
                this.ServerStation.Start(ip, port);
                this.log.Debug($"服务已开启 IP:{ip} PORT:{port}");
                return true;
            }
            catch (Exception ex)
            {
                this.erroMsg = "启动中有模块失败" + ex.Message;
                this.log.Fail(this.erroMsg);
                return false;
            }
        }

        /// <inheritdoc/>
        public bool OpenWithoutServer(params BaseTaskInvoke[] taskInokes)
        {
            try
            {
                this.taskInokes = taskInokes;
                taskInokes.Foreach(o =>
                {
                    this.ServerStation.AddStationObjectClass(o);
                });

                this.log.Debug("已开启");
                return true;
            }
            catch (Exception ex)
            {
                this.erroMsg = "启动中有模块失败" + ex.Message;
                this.log.Fail(this.erroMsg);
                return false;
            }
        }


        /// <summary>
        /// 执行接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameterItem"></param>
        /// <returns></returns>
        public object DoInterface(string url, DoTaskParameterItem parameterItem)
        {
            var method = parameterItem.Method;
            //获取参数
            var paramterInfos = method.GetParameters();
            //创建参数数组
            var paras = new object[paramterInfos.Length];
            foreach (var info in paramterInfos)
            {
                var valueMsg = parameterItem.GetValue(info.Name);
                var parameterType = info.ParameterType;
                //如果是值类型/String类型
                if (parameterType.IsValueType || parameterType == typeof(string))
                {
                    try
                    {
                        paras[info.Position] = Convert.ChangeType(valueMsg, parameterType);
                    }
                    catch (Exception ex)
                    {
                        this.log.Fail($"无法将参数 {info.Name} 转换为值 {valueMsg} 错误:{ex.Message}");
                        return null;
                    }
                }
                //如果不是值类型
                else
                {

                    paras[info.Position] = valueMsg;// JsonConvert.DeserializeObject(valueMsg, parameterType);
                }
            }

            log.Debug($"执行方法 : {url}");
            return method.Invoke(parameterItem.TaskObj, paras);
        }

        /// <inheritdoc/>
        public Dictionary<string, DoTaskParameterItem> this[ITaskInoke obj]
        {
            get
            {
                if (this.TaskParameterItems.Count == 0)
                {
                    return new Dictionary<string, DoTaskParameterItem>();
                }

                if (this.currents.ContainsKey(obj))
                {
                    return this.currents[obj];
                }
                else
                {
                    return new Dictionary<string, DoTaskParameterItem>();
                }
            }
        }

        private readonly Dictionary<object, Dictionary<string, DoTaskParameterItem>> currents = new Dictionary<object, Dictionary<string, DoTaskParameterItem>>();

        /// <summary>
        /// 获取所有任务
        /// </summary>
        public Dictionary<object, Dictionary<string, DoTaskParameterItem>> Currents
        {
            get
            {
                return currents;
            }
        }

        /// <summary>
        /// 获取接口数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DoTaskParameterItem> GetInterfaces()
        {
            if (this.TaskParameterItems.Count() != 0)
            {
                return this.TaskParameterItems;
            }

            else
            {
                foreach (var obj in this.taskInokes)
                {
                    var classRoute = obj.GetAttributeByClass<RouteAttribute>();
                    if (classRoute == null)
                    {
                        continue;
                    }
                    var rootPath = classRoute.Url;
                    var tmpDics = new Dictionary<string, DoTaskParameterItem>();
                    var methods = obj.GetType().GetMethods().OrderBy(m => m.Name);
                    methods.Foreach(m =>
                    {
                        var attributes = m.GetCustomAttributes().Where(a => a is RouteAttribute);
                        if (attributes.Count() > 0)
                        {
                            var route = attributes.First() as RouteAttribute;
                            var paramters = new List<ParameterItem>();
                            m.GetParameters().Foreach(p =>
                            {
                                var it = new ParameterItem()
                                {
                                    ParameterName = p.Name,
                                    ParameterType = p.ParameterType
                                };
                                if (p.HasDefaultValue)
                                {
                                    it.Value = p.DefaultValue.ToString();
                                }

                                //查看是否有水印提示
                                var water = p.GetCustomAttribute<WaterMarkAttribute>();
                                if (water != null)
                                {
                                    it.WaterMark = water.WaterMark;
                                }
                                paramters.Add(it);
                            });

                            string key = rootPath + route.Url;

                            var item = new DoTaskParameterItem()
                            {
                                //转换类型
                                Paramters = paramters.ToList(),
                                Url = rootPath + route.Url,
                                Method = m,
                                TaskObj = obj,
                                Explanation = route.Explanation,
                                ResultType = m.ReturnType
                            };

                            if (this.TaskParameterItems.ContainsKey(key))
                            {
                                this.log.Erro($"已经存在 {key} ,跳过不加入");
                            }
                            else
                            {
                                this.TaskParameterItems.Add(key, item);
                                tmpDics.Add(key, item);
                            }
                        }
                    });
                    this.currents.Add(obj, tmpDics);
                }

                return this.TaskParameterItems;
            }
        }
    }
}
