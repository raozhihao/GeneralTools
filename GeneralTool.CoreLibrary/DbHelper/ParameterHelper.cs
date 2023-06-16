namespace GeneralTool.CoreLibrary.DbHelper
{
    /// <summary>
    /// 参数信息帮助类
    /// (该类如果在DbContext未实例化之前使用,请在DbContext实例化后
    /// 调用DbContext.CreateParameters方法)
    /// </summary>
    public class ParameterHelper
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public ParameterHelper(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public ParameterHelper() { }
    }
}
