namespace GeneralTool.CoreLibrary.AuboSixAxisMechanicalArm
{
    /// <summary>
    /// 关节角信息事件参数
    /// </summary>
    public class JointStatusArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jointStatus"></param>
        public JointStatusArgs(MetaData.JointStatus jointStatus)
        {
            JointStatus = jointStatus;
        }

        /// <summary>
        /// 关节角信息
        /// </summary>
        public MetaData.JointStatus JointStatus { get; set; }
    }
}
