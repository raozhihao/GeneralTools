using GeneralTool.General.Attributes;
using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Interfaces;
using GeneralTool.General.LinqExtensions;
using GeneralTool.General.Logs;
using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 任务对象
    /// </summary>
    public class TaskManager : ITaskInoke
    {
        #region Private 字段

        private readonly Dictionary<object, Dictionary<string, DoTaskParameterItem>> currents = new Dictionary<object, Dictionary<string, DoTaskParameterItem>>();

        private readonly ILog log;

        /// <summary>
        /// 任务函数核参数列表
        /// </summary>
        private readonly Dictionary<string, DoTaskParameterItem> TaskParameterItems = new Dictionary<string, DoTaskParameterItem>();

        private string erroMsg;

        private BaseTaskInvoke[] taskInokes;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="jsonConvert">
        /// Json转换器
        /// </param>
        /// <param name="log">
        /// 日志组件
        /// </param>
        /// <param name="station">
        /// 站点
        /// </param>
        public TaskManager(IJsonConvert jsonConvert, ILog log, ServerStationBase station = null)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();

            this.JsonCovert = jsonConvert;
            this.log = log;
            this.ServerStation = new Station(jsonConvert, log, station);
        }

        /// <summary>
        /// </summary>
        public TaskManager() : this(null, new ConsoleLogInfo())
        {

        }

        #endregion Public 构造函数

        #region Public 属性

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
        /// 是否被初始化
        /// </summary>
        public bool IsInterfacesInit { get; set; } = false;

        /// <summary>
        /// socket是否已初始化
        /// </summary>
        public bool IsSocketInit { get; set; }

        /// <summary>
        /// Json转换器
        /// </summary>
        public IJsonConvert JsonCovert { get; set; }

        /// <summary>
        /// 任务集合
        /// </summary>
        public ObservableCollection<TaskModel> TaskModels { get; set; } = new ObservableCollection<TaskModel>();

        #endregion Public 属性

        #region Internal 属性

        /// <summary>
        /// 服务器站点
        /// </summary>
        internal Station ServerStation { get; set; }

        #endregion Internal 属性

        #region Public 索引器

        /// <inheritdoc/>
        public Dictionary<string, DoTaskParameterItem> this[BaseTaskInvoke obj]
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

        #endregion Public 索引器

        #region Public 方法

        /// <summary>
        /// 执行接口
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <param name="parameterItem">
        /// </param>
        /// <returns>
        /// </returns>
        public object DoInterface(string url, DoTaskParameterItem parameterItem)
        {
            var method = parameterItem.Method;

            //获取参数
            var paramterInfos = method.GetParameters();

            //创建参数数组
            var paras = new object[paramterInfos.Length];
            foreach (var info in paramterInfos)
            {
                if (info.IsOut)
                    continue;

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
                        var message = $"无法将参数 {info.Name} 转换为值 {valueMsg} 错误:{ex.Message}";
                        this.log.Fail(message);
                        throw new Exception(message);
                    }
                }

                //如果不是值类型
                else
                {
                    paras[info.Position] = valueMsg;//this.JsonCovert.DeserializeObject(valueMsg, parameterType);
                }
            }

            log.Debug($"执行方法 : {url}");
            return method.Invoke(parameterItem.TaskObj, paras);
        }

        /// <summary>
        /// 获取接口数据
        /// </summary>
        /// <returns>
        /// </returns>
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
                    TaskModel taskModel = new TaskModel();
                    var classRoute = obj.GetAttributeByClass<RouteAttribute>();
                    if (classRoute == null)
                    {
                        continue;
                    }

                    taskModel.Explanation = classRoute.Explanation;
                    var rootPath = classRoute.Url;

                    var tmpDics = new Dictionary<string, DoTaskParameterItem>();
                    var methods = obj.GetType().GetMethods().OrderBy(m => m.Name);
                    methods.Foreach(m =>
                    {
                        var doModel = new DoTaskModel();
                        var attributes = m.GetCustomAttributes().Where(a => a is RouteAttribute);
                        if (attributes.Count() > 0)
                        {
                            var route = attributes.First() as RouteAttribute;
                            var paramters = new ObservableCollection<ParameterItem>();
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
                                Paramters = paramters,
                                Url = rootPath + route.Url,
                                Method = m,
                                TaskObj = obj,
                                Explanation = route.Explanation,
                                ResultType = m.ReturnType,
                                HttpMethod = route.Method,
                                ReturnString = route.ReturnString
                            };

                            if (string.IsNullOrWhiteSpace(route.ReturnString))
                            {
                                //如果没有的话,则直接用type
                                item.ReturnString = item.ResultType.Name;
                            }

                            if (this.TaskParameterItems.ContainsKey(key))
                            {
                                this.log.Error($"已经存在 {key} ,跳过不加入");
                            }
                            else
                            {
                                this.TaskParameterItems.Add(key, item);
                                tmpDics.Add(key, item);
                                doModel.DoTaskParameterItem = item;
                                doModel.Url = key;
                                taskModel.DoTaskModels.Add(doModel);
                            }
                        }
                    });
                    this.currents.Add(obj, tmpDics);
                    this.TaskModels.Add(taskModel);
                }

                return this.TaskParameterItems;
            }
        }

        /// <inheritdoc/>
        public bool Open(string ip, int port, params BaseTaskInvoke[] target)
        {
            if (!this.OpenWithoutServer(target))
                return false;

            try
            {
                if (this.IsSocketInit)
                {
                    this.ServerStation.Close();
                }

                this.ServerStation.Start(ip, port);
                this.log.Debug($"服务已开启 IP:{ip} PORT:{port}");
                this.IsSocketInit = true;
                return true;
            }
            catch (Exception ex)
            {
                this.IsSocketInit = false;
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
                if (!this.IsInterfacesInit)
                {
                    this.taskInokes = taskInokes;
                    taskInokes.Foreach(o =>
                    {
                        this.ServerStation.AddStationObjectClass(o);
                    });
                    this.IsInterfacesInit = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.erroMsg = "开启接口列表失败:" + ex.GetInnerExceptionMessage();
                this.log.Fail(this.erroMsg);
                this.IsInterfacesInit = false;
                return false;
            }
        }

        #endregion Public 方法
    }
}