//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Diagnostics;
//using GeneralTool.General.Models;
//using GeneralTool.General.ExceptionHelper;

//namespace GeneralTool.General.SocketHelper
//{
//    public class Proxy : ClientProxy, ClassLibrary.IFile
//    {
//        public override event Action<ProxyErroModel> ErroMsg;
//        ClientHelper client = new ClientHelper("127.0.0.1", 55155);
//        string className;
//        public Proxy(string cn)
//        {
//            this.className = cn;
//        }
//        public T GetT<T>()
//        {
//            var methodName = new StackFrame(false).GetMethod().ToString();
//            var cmd = new RequestCommand()
//            {
//                ClassName = this.className,
//                MethodName = methodName,
//                Parameters = new object[] { },
//            };
//            var reCmd = client.SendCommand(cmd);
//            if (!reCmd.Success)
//            {
//                if (this.ErroMsg != null)
//                {
//                    this.ErroMsg.Invoke(new ProxyErroModel(cmd, reCmd));
//                    return reCmd.Default<T>();
//                }
//                else
//                { throw new Exception(reCmd.Messages); }
//            }
//            return reCmd.GetResultObj<T>();
//        }

//        public System.Collections.Generic.IEnumerable<System.String> GetAllFiles()
//        {
//            var methodName = new StackFrame(false).GetMethod().ToString();
//            var cmd = new RequestCommand()
//            {
//                ClassName = this.className,
//                MethodName = methodName,
//                Parameters = new object[] { },
//            };
//            var reCmd = client.SendCommand(cmd);
//            if (!reCmd.Success)
//            {
//                if (this.ErroMsg != null)
//                {
//                    this.ErroMsg.Invoke(new ProxyErroModel(cmd, reCmd));
//                    return reCmd.Default<System.Collections.Generic.IEnumerable<System.String>>();
//                }
//                else
//                { throw new Exception(reCmd.Messages); }
//            }
//            return reCmd.GetResultObj<System.Collections.Generic.IEnumerable<System.String>>();
//        }

//        public void SaveFile(System.String fileName, System.Byte[] buffer)
//        {
//            var methodName = new StackFrame(false).GetMethod().ToString();
//            var cmd = new RequestCommand()
//            {
//                ClassName = this.className,
//                MethodName = methodName,
//                Parameters = new object[] { fileName, buffer },
//            };
//            var reCmd = client.SendCommand(cmd);
//            if (!reCmd.Success)
//            {
//                if (this.ErroMsg != null)
//                {
//                    this.ErroMsg.Invoke(new ProxyErroModel(cmd, reCmd));
//                }
//                else
//                { throw new Exception(reCmd.Messages); }
//            }
//        }

//        public void SaveFile(System.String fileName, System.String content)
//        {
//            var methodName = new StackFrame(false).GetMethod().ToString();
//            var cmd = new RequestCommand()
//            {
//                ClassName = this.className,
//                MethodName = methodName,
//                Parameters = new object[] { fileName, content },
//            };
//            var reCmd = client.SendCommand(cmd);
//            if (!reCmd.Success)
//            {
//                if (this.ErroMsg != null)
//                {
//                    this.ErroMsg.Invoke(new ProxyErroModel(cmd, reCmd));
//                }
//                else
//                { throw new Exception(reCmd.Messages); }
//            }
//        }

//        public System.String ReadFile(System.String fileName)
//        {
//            var methodName = new StackFrame(false).GetMethod().ToString();
//            var cmd = new RequestCommand()
//            {
//                ClassName = this.className,
//                MethodName = methodName,
//                Parameters = new object[] { fileName },
//            };

//            ResponseCommand reCmd = new ResponseCommand();
//            try
//            {
//                reCmd = client.SendCommand(cmd);
//            }
//            catch (Exception ex)
//            {
//                reCmd.Messages = ex.GetInnerExceptionMessage();
//                reCmd.Success = false;
//            }

//            if (!reCmd.Success)
//            {
//                if (this.ErroMsg != null)
//                {
//                    this.ErroMsg.Invoke(new ProxyErroModel(cmd, reCmd));
//                    return reCmd.Default<System.String>();
//                }
//                else
//                { throw new Exception(reCmd.Messages); }
//            }
//            return reCmd.GetResultObj<System.String>();
//        }

//        public void change(System.String txt)
//        {
//            var methodName = new StackFrame(false).GetMethod().ToString();
//            var cmd = new RequestCommand()
//            {
//                ClassName = this.className,
//                MethodName = methodName,
//                Parameters = new object[] { txt },
//            };
//            var reCmd = client.SendCommand(cmd);
//            if (!reCmd.Success)
//            {
//                if (this.ErroMsg != null)
//                {
//                    this.ErroMsg.Invoke(new ProxyErroModel(cmd, reCmd));
//                }
//                else
//                { throw new Exception(reCmd.Messages); }
//            }
//        }

//        protected override void CloseClinetProxy()
//        { client.Close(); }
//    }
//}
