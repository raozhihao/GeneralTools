using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// 任务对象
    /// </summary>
    public class TaskManager : ITaskInoke
    {
        #region Private 字段

        private readonly ILog log;

        /// <summary>
        /// 任务函数核参数列表
        /// </summary>
        private readonly Dictionary<string, DoTaskParameterItem> TaskParameterItems = new Dictionary<string, DoTaskParameterItem>();
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

            JsonCovert = jsonConvert;
            this.log = log;
            Station serverStation = new Station(jsonConvert, log, station);
            //this.serverStationBases.Add(serverStation);
            ServerStation = serverStation;
        }

        /// <summary>
        /// </summary>
        public TaskManager() : this(null, new ConsoleLogInfo())
        {

        }

        /// <summary>
        /// 初始化,但不添加任何站点
        /// </summary>
        /// <param name="jsonConvert"></param>
        /// <param name="log"></param>
        public TaskManager(IJsonConvert jsonConvert, ILog log)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();

            JsonCovert = jsonConvert;
            this.log = log;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 获取所有任务
        /// </summary>
        public Dictionary<object, Dictionary<string, DoTaskParameterItem>> Currents { get; } = new Dictionary<object, Dictionary<string, DoTaskParameterItem>>();

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErroMsg { get; private set; }

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
        public ObservableCollection<StationInfo> ServerStations { get; } = new ObservableCollection<StationInfo>();

        /// <summary>
        /// 当前的单站点(通过构造函数创建的单站点)
        /// </summary>
        public Station ServerStation { get; private set; }

        #endregion Internal 属性

        #region Public 索引器

        /// <inheritdoc/>
        public Dictionary<string, DoTaskParameterItem> this[BaseTaskInvoke obj]
        {
            get
            {
                return TaskParameterItems.Count == 0
                    ? new Dictionary<string, DoTaskParameterItem>()
                    : Currents.ContainsKey(obj) ? Currents[obj] : new Dictionary<string, DoTaskParameterItem>();
            }
        }

        #endregion Public 索引器

        #region Public 方法

        /// <summary>
        /// 添加远程服务站点
        /// </summary>
        /// <param name="station"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="jsonConvert"></param>
        /// <param name="log"></param>
        public virtual void AddServerStation(ServerStationBase station, string ip, int port, IJsonConvert jsonConvert = null, ILog log = null)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();

            Station stationInfo = new Station(jsonConvert, log, station);
            StationInfo info = new StationInfo(ip, port, stationInfo);
            ServerStations.Add(info);
        }

        /// <summary>
        /// 添加远程服务站点
        /// </summary>
        /// <param name="station"></param>
        public virtual void AddServerStation(StationInfo station)
        {
            ServerStations.Add(station);
        }

        /// <summary>
        /// 执行接口
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <param name="parameterItem">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual object DoInterface(string url, DoTaskParameterItem parameterItem)
        {
            MethodInfo method = parameterItem.Method;

            //获取参数
            ParameterInfo[] paramterInfos = method.GetParameters();

            //创建参数数组
            object[] paras = new object[paramterInfos.Length];
            foreach (ParameterInfo info in paramterInfos)
            {
                if (info.IsOut)
                    continue;

                object valueMsg = parameterItem.GetValue(info.Name);
                Type parameterType = info.ParameterType;

                //如果是值类型/String类型
                if (parameterType.IsValueType || parameterType == typeof(string))
                {
                    try
                    {
                        paras[info.Position] = Convert.ChangeType(valueMsg, parameterType);
                    }
                    catch (Exception ex)
                    {
                        string message = $"无法将参数 {info.Name} 转换为值 {valueMsg} 错误:{ex.GetInnerExceptionMessage()}";
                        log.Fail(message);
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
        public virtual Dictionary<string, DoTaskParameterItem> GetInterfaces()
        {
            if (TaskParameterItems.Count() != 0)
            {
                return TaskParameterItems;
            }
            else
            {
                foreach (BaseTaskInvoke obj in taskInokes)
                {
                    AddTaskModel(obj);
                }

                return TaskParameterItems;
            }
        }

        /// <summary>
        /// 添加一个项
        /// </summary>
        /// <param name="obj"></param>
        public void AddTaskModel(BaseTaskInvoke obj)
        {
            TaskModel taskModel = new TaskModel();
            RouteAttribute classRoute = obj.GetAttributeByClass<RouteAttribute>();
            if (classRoute == null)
            {
                return;
            }

            taskModel.LangKey = classRoute.LangKey;
            taskModel.Explanation = classRoute.Explanation;
            string rootPath = classRoute.Url;

            Dictionary<string, DoTaskParameterItem> tmpDics = new Dictionary<string, DoTaskParameterItem>();
            IOrderedEnumerable<MethodInfo> methods = obj.GetType().GetMethods().OrderBy(m => m.GetCustomAttribute<RouteAttribute>()?.SortIndex);

            methods.Foreach(m =>
            {
                DoTaskModel doModel = new DoTaskModel();
                IEnumerable<Attribute> attributes = m.GetCustomAttributes().Where(a => a is RouteAttribute);
                if (attributes.Count() > 0)
                {
                    RouteAttribute route = attributes.First() as RouteAttribute;
                    ObservableCollection<ParameterItem> paramters = new ObservableCollection<ParameterItem>();
                    _ = m.GetParameters().Foreach(p =>
                    {
                        ParameterItem it = new ParameterItem()
                        {
                            ParameterName = p.Name,
                            ParameterType = p.ParameterType,
                            Index = p.Position
                        };
                        if (p.HasDefaultValue)
                        {
                            it.Value = p.DefaultValue.ToString();
                        }

                        //查看是否有水印提示
                        WaterMarkAttribute water = p.GetCustomAttribute<WaterMarkAttribute>();
                        if (water != null)
                        {
                            it.WaterMark = water.WaterMark;
                        }
                        paramters.Add(it);
                    });

                    string key = rootPath + route.Url;

                    DoTaskParameterItem item = new DoTaskParameterItem()
                    {
                        //转换类型
                        Paramters = paramters,
                        Url = rootPath + route.Url,
                        Method = m,
                        TaskObj = obj,
                        LangKey = route.LangKey,
                        Explanation = route.Explanation,
                        ResultType = m.ReturnType,
                        HttpMethod = route.Method,
                        ReturnString = route.ReturnString,
                    };

                    if (string.IsNullOrWhiteSpace(route.ReturnString))
                    {
                        //如果没有的话,则直接用type
                        item.ReturnString = item.ResultType.Name;
                    }
                    item.InitLangKey();

                    if (TaskParameterItems.ContainsKey(key))
                    {
                        log.Error($"已经存在 {key} ,跳过不加入");
                    }
                    else
                    {
                        TaskParameterItems.Add(key, item);
                        tmpDics.Add(key, item);
                        doModel.DoTaskParameterItem = item;
                        doModel.Url = key;
                        taskModel.DoTaskModels.Add(doModel);
                    }
                }
            });
            if (taskModel.DoTaskModels.Count > 0)
            {
                Currents.Add(obj, tmpDics);
                taskModel.InitLangKey();
                TaskModels.Add(taskModel);
            }

        }

        /// <inheritdoc/>
        public virtual bool Open(string ip, int port, params BaseTaskInvoke[] target)
        {
            if (!OpenWithoutServer(target))
                return false;

            try
            {
                if (IsSocketInit)
                {
                    ServerStation.Close();
                }

                _ = ServerStation.Start(ip, port);
                log.Debug($"服务已开启 IP:{ip} PORT:{port}");
                IsSocketInit = true;
                return true;
            }
            catch (Exception ex)
            {
                IsSocketInit = false;
                ErroMsg = "启动中有模块失败" + ex.GetInnerExceptionMessage();
                log.Fail(ErroMsg);
                return false;
            }
        }

        /// <summary>
        /// 打开单个站点
        /// </summary>
        /// <param name="station"></param>
        /// <param name="target"></param>
        public virtual bool OpenServerStation(StationInfo station, params BaseTaskInvoke[] target)
        {
            ServerStations.Add(station);
            return OpenStation(station, target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool OpenStations(params BaseTaskInvoke[] target)
        {
            foreach (StationInfo item in ServerStations)
            {
                try
                {
                    if (!IsInterfacesInit)
                    {
                        taskInokes = target;
                        IsInterfacesInit = true;
                    }

                    _ = taskInokes.Foreach(o =>
                    {
                        _ = item.Station.AddStationObjectClass(o);
                    });
                }
                catch (Exception ex)
                {
                    ErroMsg = "开启接口列表失败:" + ex.GetInnerExceptionMessage();
                    log.Fail(ErroMsg);
                    IsInterfacesInit = false;
                    return false;
                }

                try
                {
                    if (item.IsSocketInit)
                    {
                        item.Station.Close();
                    }

                    _ = item.Station.Start(item.Ip, item.Port);
                    log.Debug($"服务已开启 IP:{item.Ip} PORT:{item.Port}");
                    item.IsSocketInit = true;
                }
                catch (Exception ex)
                {
                    item.IsSocketInit = false;
                    ErroMsg = "启动中有模块失败" + ex.GetInnerExceptionMessage();
                    log.Fail(ErroMsg);
                    return false;
                }
            }
            return true;
        }

        private bool OpenStation(StationInfo info, params BaseTaskInvoke[] target)
        {
            try
            {
                if (!IsInterfacesInit)
                {
                    taskInokes = target;
                    IsInterfacesInit = true;
                }

                _ = taskInokes.Foreach(o =>
                {
                    _ = info.Station.AddStationObjectClass(o);
                });
            }
            catch (Exception ex)
            {
                ErroMsg = "开启接口列表失败:" + ex.GetInnerExceptionMessage();
                log.Fail(ErroMsg);
                IsInterfacesInit = false;
                return false;
            }

            try
            {
                if (info.IsSocketInit)
                {
                    info.Station.Close();
                }

                _ = info.Station.Start(info.Ip, info.Port);
                log.Debug($"服务已开启 IP:{info.Ip} PORT:{info.Port}");
                info.IsSocketInit = true;
            }
            catch (Exception ex)
            {
                info.IsSocketInit = false;
                ErroMsg = "启动中有模块失败" + ex.GetInnerExceptionMessage();
                log.Fail(ErroMsg);
                return false;
            }
            return true;
        }

        /// <inheritdoc/>
        public virtual bool OpenWithoutServer(params BaseTaskInvoke[] taskInokes)
        {
            try
            {
                if (!IsInterfacesInit)
                {
                    this.taskInokes = taskInokes;
                    _ = taskInokes.Foreach(o =>
                    {
                        _ = ServerStation.AddStationObjectClass(o);
                    });
                    IsInterfacesInit = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = "开启接口列表失败:" + ex.GetInnerExceptionMessage();
                log.Fail(ErroMsg);
                IsInterfacesInit = false;
                return false;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
            try
            {
                ServerStation?.Close();
            }
            catch (Exception)
            {

            }
        }
        #endregion Public 方法
    }
}