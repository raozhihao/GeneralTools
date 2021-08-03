using System.Text;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 模板类
    /// </summary>
    internal static class ProxyTemple
    {
        #region Internal 属性

        /// <summary>
        /// 获取预编译模板
        /// </summary>
        internal static string Templete
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("using System;");
                builder.AppendLine("using System.Collections.Generic;");
                builder.AppendLine("using System.Linq;");
                builder.AppendLine("using System.Text;");
                builder.AppendLine("using System.Threading.Tasks;");
                builder.AppendLine("using System.Diagnostics;");
                builder.AppendLine("using GeneralTool.General.Models;");
                builder.AppendLine("using GeneralTool.General.ExceptionHelper;");
                builder.AppendLine("");

                builder.AppendLine("namespace GeneralTool.General.SocketHelper");
                builder.AppendLine("{");
                builder.AppendLine("public class Proxy:ClientProxy,@Interface@");
                builder.AppendLine("{");
                builder.AppendLine("public override event Action<ProxyErroModel> ErroMsg;");
                builder.AppendLine("@ClientBuilder@");
                builder.AppendLine("string className;");
                builder.AppendLine("public Proxy(string cn)");
                builder.AppendLine("{");
                //builder.AppendLine("@LongConnect@");
                builder.AppendLine("this.className = cn;");
                builder.AppendLine("}");
                // builder.AppendLine("void Connect(){ client.Start();}");

                builder.AppendLine("@Content@");

                builder.AppendLine("protected override void CloseClinetProxy()");
                builder.AppendLine("{client.Close();}");

                builder.AppendLine("}");
                builder.AppendLine("}");
                return builder.ToString();
            }
        }

        #endregion Internal 属性
    }
}