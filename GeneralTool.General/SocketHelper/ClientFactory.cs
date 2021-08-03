using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 客户端接口创建类
    /// </summary>
    /// <typeparam name="TInterface">
    /// 需要创建的远程对象的接口类,接口中不应包含委托,属性,只应包含纯粹的方法
    /// </typeparam>
    public static class ClientFactory<TInterface>
    {
        #region Public 方法

        /// <summary>
        /// 根据指定的Url和端口创建远程对象
        /// </summary>
        /// <param name="host">
        /// 路径
        /// </param>
        /// <param name="port">
        /// 端口
        /// </param>
        /// <returns>
        /// </returns>
        public static TInterface GetClientObj(string host = "127.0.0.1", int port = 55155)
        {
            Type type = typeof(TInterface);
            if (!type.IsInterface)
            {
                throw new Exception("参数 TInterface 不是一个接口");
            }

            //获取模板
            string templete = ProxyTemple.Templete;
            MethodInfo[] methods = type.GetMethods();
            //获取并填充所有方法模板
            IEnumerable<string> list = GetMethodStrs(methods);

            //替换填充模板

            templete = templete.Replace("@ClientBuilder@", $"ClientHelper client = new ClientHelper(\"{host}\",{port});");

            string tempClass = templete.Replace("@Interface@", type.FullName).Replace("@Content@", string.Join(Environment.NewLine, list));

            //创建对象
            object obj = CreateObject(tempClass);

            return (TInterface)obj;
        }

        #endregion Public 方法

        #region Private 方法

        /// <summary>
        /// 根据模板创建对象
        /// </summary>
        /// <param name="tempClass">
        /// 模板类
        /// </param>
        /// <returns>
        /// </returns>
        private static object CreateObject(string tempClass)
        {
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();
            string fileName = "ProxyTmp.cs";
            File.WriteAllText(fileName, tempClass);
            // 2.ICodeComplier
            string lan = "CSharp";
            CompilerInfo info = CodeDomProvider.GetCompilerInfo(lan);
            CompilerParameters options = info.CreateDefaultCompilerParameters();
            //引用当前类型的dll
            string location = typeof(TInterface).Assembly.Location;

            string startPath = AppDomain.CurrentDomain.BaseDirectory;
            options.ReferencedAssemblies.Add(location);
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            options.ReferencedAssemblies.Add("System.Data.dll");
            options.ReferencedAssemblies.Add("System.Core.dll");
            options.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
            options.ReferencedAssemblies.Add("System.Net.Http.dll");
            options.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            options.ReferencedAssemblies.Add(Path.Combine(startPath, "GeneralTool.General.dll"));

            CompilerResults cr = objCSharpCodePrivoder.CompileAssemblyFromFile(options, fileName);

            //如果当前编译有错误
            if (cr.Errors.HasErrors)
            {
                StringBuilder builder = new StringBuilder();
                foreach (CompilerError error in cr.Errors)
                {
                    builder.AppendLine($"Line :{error.Line} , Message :{error.ErrorText}");
                }
                throw new Exception(builder.ToString());
            }
            else
            {
                Assembly ass = cr.CompiledAssembly;
                Type type = ass.GetTypes()[0];
                object activeObj = Activator.CreateInstance(type, typeof(TInterface).Name);
                return activeObj;
            }
        }

        private static IEnumerable<string> GetMethodStrs(MethodInfo[] methods)
        {
            foreach (MethodInfo method in methods)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("public ");
                string methodName = method.Name;
                Type reType = method.ReturnType;
                string returnTypeStr = reType.FullName;

                if (reType.Equals(typeof(void)))
                {
                    returnTypeStr = "void";
                }
                else if (reType.IsGenericType)
                {
                    //Ienumable'1
                    Type[] generType = reType.GetGenericArguments();
                    string genName = reType.Namespace + "." + reType.Name.Substring(0, reType.Name.IndexOf("`"));
                    returnTypeStr = genName + "<" + string.Join(",", generType.Select(g => g.FullName)) + ">";
                }
                else if (method.IsGenericMethod)//如果是泛型方法
                {
                    //返回类型直接使用
                    returnTypeStr = method.ReturnType.Name;
                }
                else if (reType.IsValueType || reType.IsClass)
                {
                }
                else
                {
                    throw new Exception($"方法 {method.Name} 的返回类型 {reType.FullName} 不受支持");
                }

                if (method.IsGenericMethod)
                {
                    //泛型方法的参数不同
                    var genTypes = method.GetGenericArguments();//泛型参数
                    List<string> genListStr = new List<string>();
                    foreach (var item in genTypes)
                    {
                        genListStr.Add(item.Name);
                    }

                    if (genListStr.Count > 0)
                    {
                        string genStr = string.Join(",", genListStr);
                        builder.Append($" {returnTypeStr} {methodName}<{genStr}>(");
                    }
                }
                else
                {
                    builder.Append($" {returnTypeStr} {methodName}(");
                }

                ParameterInfo[] parameters = method.GetParameters();
                List<string> parList = new List<string>();
                List<string> parNames = new List<string>();
                foreach (ParameterInfo par in parameters)
                {
                    string parName = par.Name;
                    string parType = par.ParameterType.FullName;
                    parList.Add($"{parType} {parName}");
                    parNames.Add(parName);
                }
                builder.Append(string.Join(",", parList) + ")");

                builder.AppendLine("{");

                builder.AppendLine("var methodName = new StackFrame(false).GetMethod().ToString();");
                builder.AppendLine(" var cmd = new RequestCommand(){ ClassName = this.className,MethodName = methodName,");
                string pn = string.Join(",", parNames);
                builder.AppendLine($"  Parameters=new object[] {{{pn}}},");
                builder.AppendLine("};");

                builder.AppendLine("ResponseCommand reCmd = new ResponseCommand();");
                builder.AppendLine("try");
                builder.AppendLine("{");
                builder.AppendLine("reCmd = client.SendCommand(cmd);");
                builder.AppendLine("}");
                builder.AppendLine("catch (Exception ex)");
                builder.AppendLine("{");
                builder.AppendLine("reCmd.Messages = ex.GetInnerExceptionMessage();");
                builder.AppendLine("reCmd.Success = false;");
                builder.AppendLine("}");

                builder.AppendLine("if (!reCmd.Success)");
                builder.AppendLine("{");
                builder.AppendLine("if (this.ErroMsg != null) ");
                builder.AppendLine("{");
                builder.AppendLine("this.ErroMsg.Invoke(new ProxyErroModel(cmd, reCmd));");
                if (returnTypeStr != "void")
                {
                    builder.AppendLine($"return reCmd.Default<{returnTypeStr}>();");
                    builder.AppendLine("}");
                }
                else
                {
                    builder.AppendLine("}");
                }

                builder.AppendLine("else ");
                builder.AppendLine("   {  throw new Exception(reCmd.Messages);}");
                builder.AppendLine("}");
                if (returnTypeStr != "void")
                {
                    builder.AppendLine($"return reCmd.GetResultObj<{returnTypeStr}>();");
                }

                builder.AppendLine("}");
                yield return builder.ToString();
            }
        }

        #endregion Private 方法
    }
}