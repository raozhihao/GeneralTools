namespace GeneralTool.CoreLibrary.IniHelpers
{
    /// <summary>
    /// 简单的Ini管理
    /// </summary>
    public class SimpleIniManager
    {
        /// <summary>
        /// Ini路径
        /// </summary>
        public string IniPath { get; private set; }

        /// <summary>
        /// Ini对象
        /// </summary>
        public IniHelper IniHelper { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iniPath">ini文件保存路径,如果为null,则默认生成ini文件到输出目录下Configs/default.ini文件</param>
        public SimpleIniManager(string iniPath = null)
        {
            this.IniPath = iniPath;
            this.IniHelper = IniHelper.GetInstace(iniPath);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="sectionName">标题名称</param>
        /// <param name="key">键名</param>
        /// <returns>返回值</returns>
        public virtual string GetValue(string sectionName, string key)
        {
            return this.IniHelper.GetString(sectionName, key, "");
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string sectionName, string key, string value)
        {
            this.IniHelper.WriteValueString(sectionName, key, value);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void SetValue(string sectionName, string key, object value)
        {
            this.IniHelper.WriteValueString(sectionName, key, value + "");
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T GetValue<T>(string sectionName, string key)
        {
            return this.IniHelper.GetValue<T>(sectionName, key);
        }

    }
}
