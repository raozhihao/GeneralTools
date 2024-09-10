using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.Models;

using GeneralTool.CoreLibrary.TaskLib;

namespace GeneralTool.CoreLibrary.TaskExport
{
    public enum PythonExportMode
    {
        Socket,
        Http
    }

    /// <summary>
    /// python导出(Socket)
    /// </summary>
    public class PythonExport
    {
        public ILog Log { get; }
        private TaskManager taskManager;
        private readonly string ip;
        private readonly int port;
        private readonly PythonExportMode exportMode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskManager"></param>
        /// <param name="ip">服务开启ip</param>
        /// <param name="port">服务开启port</param>
        /// <param name="log"></param>
        public PythonExport(TaskManager taskManager, string ip = "127.0.0.1", int port = 8877, ILog log = null, PythonExportMode export = PythonExportMode.Socket)
        {
            if (log == null) log = new ConsoleLogInfo();

            this.Log = log;
            this.taskManager = taskManager;
            this.ip = ip;
            this.port = port;
            this.exportMode = export;
        }



        public void CreatePython(string scriptDir)
        {
            _ = Directory.CreateDirectory(scriptDir);

            var logDir = Path.Combine(scriptDir, "logs");
            Directory.CreateDirectory(logDir);

            if (this.exportMode == PythonExportMode.Socket)
                File.WriteAllText(Path.Combine(scriptDir, "task_client_socket.py"), PythonFile.TaskClientSocketString);
            else
                File.WriteAllText(Path.Combine(scriptDir, "task_client_http.py"), PythonFile.TaskClientHttpString);

            File.WriteAllText(Path.Combine(scriptDir, "log.py"), PythonFile.LogString);
            File.WriteAllText(Path.Combine(scriptDir, "request.py"), PythonFile.RequestString);

            _ = Directory.CreateDirectory(Path.Combine(scriptDir, "logs"));


            System.Collections.ObjectModel.ObservableCollection<TaskModel> models = taskManager.TaskModels;
            if (models.Count == 0)
                throw new Exception("没有可供导出的类型");

            foreach (TaskModel model in models)
            {
                try
                {
                    this.Log.Info($"正在生成[{model.Explanation}]的python脚本");
                    this.CreatePythonScript(model, scriptDir);
                }
                catch (Exception ex)
                {
                    this.Log.Error($"[{model.Explanation}] -> 生成python脚本失败:[{ex}]");
                }
            }
        }

        private void CreatePythonScript(TaskModel model, string scriptDir)
        {
            if (model.DoTaskModels.Count == 0) return;
            var scriptBuilder = new StringBuilder();
            if (this.exportMode == PythonExportMode.Socket)
                _ = scriptBuilder.AppendLine("from task_client_socket import task_client_socket");
            else
                _ = scriptBuilder.AppendLine("from task_client_http import task_client_http");
            _ = scriptBuilder.AppendLine("\r\n");
            var cname = model.DoTaskModels[0].DoTaskParameterItem.TaskObj.GetType().Name;
            _ = scriptBuilder.AppendLine($"class {cname}:");
            _ = scriptBuilder.AppendLine($"\t\"\"\"{model.Explanation}");
            _ = scriptBuilder.AppendLine("\t\"\"\"\r\n");

            var socketHost = this.ip;
            var socketPort = this.port;
            var url = model.DoTaskModels[0].DoTaskParameterItem.TaskObj.GetType().GetCustomAttribute<RouteAttribute>().Url;

            _ = scriptBuilder.AppendLine($"\tdef __init__(self, host: str = '{socketHost}', port: int = {socketPort}) -> None:");
            if (this.exportMode == PythonExportMode.Socket)
                _ = scriptBuilder.AppendLine("\t\tself.client = task_client_socket(ip = host,port = port)");
            else
                _ = scriptBuilder.AppendLine("\t\tself.client = task_client_http(ip = host,port = port)");
            _ = scriptBuilder.AppendLine($"\t\tself.url: str = '{url}'");
            _ = scriptBuilder.AppendLine("");
            foreach (DoTaskModel item in model.DoTaskModels)
            {
                DoTaskParameterItem m = item.DoTaskParameterItem;
                var mRoute = m.Method.GetCustomAttribute<RouteAttribute>();
                if (mRoute == null) continue;
                var mUrl = mRoute.Url;
                ParameterInfo[] ps = m.Method.GetParameters();

                var listMark = new List<string>();
                var listName = new List<string>();
                var listNameAndTypes = new List<string>();
                foreach (ParameterInfo p in ps)
                {
                    var mark = p.GetCustomAttribute<WaterMarkAttribute>()?.WaterMark;
                    var pname = p.Name;
                    listMark.Add(mark);
                    listName.Add(pname);

                    object value = null;
                    if (!(p.DefaultValue is DBNull))
                    {
                        if (p.ParameterType == typeof(string))
                        {
                            value = " = '" + p.DefaultValue + "'";
                        }
                        else if (p.ParameterType.IsEnum)
                        {
                            value = " = " + (int)p.DefaultValue;
                        }
                        else if (p.ParameterType == typeof(bool))
                        {
                            value = " = " + (bool)p.DefaultValue;
                        }
                        else if (p.ParameterType == typeof(int) || p.ParameterType == typeof(byte) || p.ParameterType == typeof(double) || p.ParameterType == typeof(float) || p.ParameterType == typeof(decimal))
                        {
                            value = " = " + p.DefaultValue;
                        }
                    }


                    listNameAndTypes.Add($"{pname}: {this.GetTypeToPython(p.ParameterType)}{value}");
                }

                Type rtype = m.Method.ReturnType;
                var typeStr = this.GetTypeToPython(rtype);

                _ = listName.Count > 0
                    ? scriptBuilder.AppendLine($"\tdef {mUrl}(self, {string.Join(", ", listNameAndTypes)}) -> {typeStr}:")
                    : scriptBuilder.AppendLine($"\tdef {mUrl}(self) -> {typeStr}:");

                var expArr = mRoute.Explanation;//(m.Target + "").Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var expStr = string.Join("\r\n\t\t", expArr);
                _ = scriptBuilder.AppendLine($"\t\t\"\"\"{expStr}\r\n");

                var args = "args = None";
                if (listMark.Count > 0)
                {
                    var argsList = new List<string>();
                    _ = scriptBuilder.AppendLine("\t\tArgs:");
                    for (var i = 0; i < listMark.Count; i++)
                    {
                        _ = scriptBuilder.AppendLine($"\t\t\t{listName[i]}: {listMark[i]}");
                        argsList.Add($"'{listName[i]}':{listName[i]}");
                    }
                    args = "args = {" + string.Join(", ", argsList) + "}";
                }

                if (this.exportMode == PythonExportMode.Http)
                {
                    //获取当前的http类型
                    var isGet = mRoute.Method == HttpMethod.GET ? "True" : "False";
                    args += $",get = {isGet}";
                }
                _ = scriptBuilder.AppendLine("\t\t\"\"\"");

                _ = scriptBuilder.AppendLine($"\t\treturn self.client.send(url = self.url, method = self.{mUrl}.__name__, {args})");
                _ = scriptBuilder.AppendLine("\r\n");
            }

            var pythonPath = Path.Combine(scriptDir, cname + ".py");
            File.WriteAllText(pythonPath, scriptBuilder.ToString());
            this.Log.Info($"脚本生成到:[{pythonPath}]");
        }

        public virtual string GetTypeToPython(Type rtype)
        {
            var numberTypes = new List<Type>() { typeof(int),typeof(uint),typeof(long),typeof(ulong),typeof(byte),
                typeof(UInt16),typeof(UInt32),typeof(UInt64),
            typeof(short),typeof(ushort),typeof(sbyte)};

            var numericTypes = new List<Type>() { typeof(double), typeof(float), typeof(Single), typeof(decimal) };

            return numberTypes.Contains(rtype) || rtype.IsEnum
                ? "int"
                : numericTypes.Contains(rtype)
                ? "float"
                : rtype == typeof(string) ? "str" : rtype == typeof(bool) ? "bool" : rtype == typeof(void) ? "None" : "object";
        }

        private static class PythonFile
        {
            public static string LogString
            {
                get
                {
                    var builder = new StringBuilder();
                    builder.AppendLine("# coding:utf-8");
                    builder.AppendLine();
                    builder.AppendLine("import os");
                    builder.AppendLine("import datetime");
                    builder.AppendLine("logaddr = os.path.split(os.path.realpath(__file__))[0]");
                    builder.AppendLine("class log():");
                    builder.AppendLine("    @staticmethod");
                    builder.AppendLine("    def outputlog(msg, log_file_name: str = 'Total_Log'):");
                    builder.AppendLine("        output = open(");
                    builder.AppendLine("            logaddr + '\\logs\\{}_'.format(log_file_name) + str(datetime.datetime.now().strftime('%Y_%m_%d')) + '.txt','a')");
                    builder.AppendLine("        print(str(datetime.datetime.now()) + ' :' + str(msg) + '')");
                    builder.AppendLine("        output.write(str(datetime.datetime.now()) + ' :' + str(msg) + '')");
                    builder.AppendLine("        output.close()");

                    return builder.ToString();
                }
            }

            public static string TaskClientSocketString
            {
                get
                {
                    var builder = new StringBuilder();

                    builder.AppendLine("import json");
                    builder.AppendLine("import socket");
                    builder.AppendLine("import struct");
                    builder.AppendLine("from log import log");
                    builder.AppendLine("from request import request");
                    builder.AppendLine("class task_client_socket:");
                    builder.AppendLine("    \"\"\"socket 客户端");
                    builder.AppendLine("    \"\"\"");
                    builder.AppendLine("    def __init__(self, ip:str, port: int):");
                    builder.AppendLine("        \"\"\"初始化");
                    builder.AppendLine("");
                    builder.AppendLine("        Args:");
                    builder.AppendLine("            ip (str): 远程ip地址");
                    builder.AppendLine("            port (int): 远程端口");
                    builder.AppendLine("        \"\"\"");
                    builder.AppendLine("        self.ip = ip");
                    builder.AppendLine("        self.port = port");
                    builder.AppendLine("");
                    builder.AppendLine("    def send(self, url, method, args=None):");
                    builder.AppendLine("        \"\"\"发送socket数据,短连接");
                    builder.AppendLine("");
                    builder.AppendLine("        Args:");
                    builder.AppendLine("            url (str): 远程任务类的Url,不用加 / 会自动追加");
                    builder.AppendLine("            method (str): 要调用的方法");
                    builder.AppendLine("            args (json): 默认的参数 json数据. Defaults to None.");
                    builder.AppendLine("");
                    builder.AppendLine("        Raises:");
                    builder.AppendLine("            BaseException: 远程返回异常消息则会原样抛出");
                    builder.AppendLine("");
                    builder.AppendLine("        Returns:");
                    builder.AppendLine("            _type_: 根据远程方法自行转换输出");
                    builder.AppendLine("        \"\"\"");
                    builder.AppendLine("        client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)");
                    builder.AppendLine("        send_request = request(f'{url}{method}', args)");
                    builder.AppendLine("        try:");
                    builder.AppendLine("            server_address = (self.ip, self.port)");
                    builder.AppendLine("            client_socket.connect(server_address)");
                    builder.AppendLine("            send_json = send_request.JsonDump()");
                    builder.AppendLine("            log.outputlog(f'current execute {send_json}')");
                    builder.AppendLine("            send_buffer = send_json.encode()");
                    builder.AppendLine("            send_len = len(send_buffer)");
                    builder.AppendLine("            len_buffer = struct.pack('<i', send_len)");
                    builder.AppendLine("            send_buffer = len_buffer + send_buffer");
                    builder.AppendLine("            client_socket.sendall(send_buffer)");
                    builder.AppendLine("            data = client_socket.recv(4)");
                    builder.AppendLine("            recv_len = int.from_bytes(data, byteorder='big')");
                    builder.AppendLine("            data = client_socket.recv(recv_len)");
                    builder.AppendLine("            recv_str = data.decode()");
                    builder.AppendLine("            recv_json = json.loads(recv_str)");
                    builder.AppendLine("            if recv_json['RequestSuccess'] is not True:");
                    builder.AppendLine("                error = recv_json['ErroMsg']");
                    builder.AppendLine("                log.outputlog(f'current interface:{send_json} ex:{error}')");
                    builder.AppendLine("                raise BaseException(error)");
                    builder.AppendLine("            result = recv_json['Result']");
                    builder.AppendLine("            log.outputlog(f'current interface:{send_request.Url} result:{result}')");
                    builder.AppendLine("            return result");
                    builder.AppendLine("        except BaseException as e:");
                    builder.AppendLine("            log.outputlog(f'current interface:{send_request.Url} ex:{e.args}')");
                    builder.AppendLine("            raise BaseException(e)");
                    builder.AppendLine("        finally:");
                    builder.AppendLine("            if client_socket is not None:");
                    builder.AppendLine("                try:");
                    builder.AppendLine("                    client_socket.close()");
                    builder.AppendLine("                except:");
                    builder.AppendLine("                    pass");

                    return builder.ToString();
                }
            }

            public static string TaskClientHttpString
            {
                get
                {
                    var builder = new StringBuilder();

                    builder.AppendLine("import json");
                    builder.AppendLine("import requests");
                    builder.AppendLine("from request import request");
                    builder.AppendLine("from log import log");
                    builder.AppendLine("");
                    builder.AppendLine("class task_client_http:");
                    builder.AppendLine("    def __init__(self, ip: str, port: int):");
                    builder.AppendLine("        \"\"\"");
                    builder.AppendLine("        初始化");
                    builder.AppendLine("        :param host: 要访问的IP");
                    builder.AppendLine("        :param port: 要访问的端口");
                    builder.AppendLine("        \"\"\"");
                    builder.AppendLine("        self.host = ip");
                    builder.AppendLine("        self.port = port");
                    builder.AppendLine("");
                    builder.AppendLine("    def send(self, url: str, method: str, args=None, get: bool = True):");
                    builder.AppendLine("        \"\"\"");
                    builder.AppendLine("        进行远程Http方法调用");
                    builder.AppendLine("        :param url: 方法的任务路由");
                    builder.AppendLine("        :param method: 方法名称");
                    builder.AppendLine("        :param get: 是否为get方法,False为Post方法");
                    builder.AppendLine("        :param args: 方法的参数字典列表");
                    builder.AppendLine("        :return:");
                    builder.AppendLine("        \"\"\"");
                    builder.AppendLine("        full_url = f'http://{self.host}:{self.port}/{url}{method}'");
                    builder.AppendLine("        send_request = request(f'{url}{method}', args)");
                    builder.AppendLine("        send_json = send_request.JsonDump()");
                    builder.AppendLine("        if get is True:");
                    builder.AppendLine("            http_data = requests.get(full_url, params=args)");
                    builder.AppendLine("        else:");
                    builder.AppendLine("            http_data = requests.post(full_url, str(args))");
                    builder.AppendLine("        data = http_data.content");
                    builder.AppendLine("        return_data = json.loads(data)");
                    builder.AppendLine("        if not return_data['RequestSuccess']:");
                    builder.AppendLine("            error = return_data['ErroMsg']");
                    builder.AppendLine("            log.outputlog(f'当前执行接口:{send_json} 出现异常:{error}')");
                    builder.AppendLine("            raise Exception(return_data['ErroMsg'])");
                    builder.AppendLine("        result = return_data['Result']");
                    builder.AppendLine("        log.outputlog(f'当前执行接口:{send_request.Url} 结果信息:{result}')");
                    builder.AppendLine("        return return_data['Result']");
                    return builder.ToString();
                }
            }

            public static string RequestString
            {
                get
                {
                    var builder = new StringBuilder();

                    builder.AppendLine("import json");
                    builder.AppendLine("");
                    builder.AppendLine("class request:");
                    builder.AppendLine("    \"\"\"");
                    builder.AppendLine("    请求类型对象");
                    builder.AppendLine("    \"\"\"");
                    builder.AppendLine("");
                    builder.AppendLine("    def __init__(self, url, parameters):");
                    builder.AppendLine("        self.Url = url");
                    builder.AppendLine("        if parameters is None:");
                    builder.AppendLine("            parameters = {}");
                    builder.AppendLine("        self.Parameters = parameters");
                    builder.AppendLine("");
                    builder.AppendLine("    def JsonDump(self):");
                    builder.AppendLine("        return json.dumps(self, default=lambda obj: obj.__dict__, ensure_ascii=False)");

                    return builder.ToString();
                }
            }
        }
    }
}
