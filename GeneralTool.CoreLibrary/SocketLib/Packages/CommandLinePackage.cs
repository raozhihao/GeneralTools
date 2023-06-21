namespace GeneralTool.CoreLibrary.SocketLib.Packages
{
    /// <summary>
    /// 以\r\n做结尾的解包程序
    /// </summary>
    // Token: 0x02000094 RID: 148
    public class CommandLinePackage : CustomCommandPackage
    {
        /// <summary>
        ///
        /// </summary>
        // Token: 0x06000608 RID: 1544 RVA: 0x0001E184 File Offset: 0x0001C384
        public CommandLinePackage() : base("\\r\\n")
        {
        }
    }
}
