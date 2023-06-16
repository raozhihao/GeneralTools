using GeneralTool.CoreLibrary.Extensions;

using static GeneralTool.CoreLibrary.AuboSixAxisMechanicalArm.MetaData;

namespace GeneralTool.CoreLibrary.AuboSixAxisMechanicalArm
{
    /// <summary>
    /// 机械臂事件参数信息 
    /// </summary>
    public class RobotEventHandler
    {
        /// <summary>
        /// 机械臂诊断信息
        /// </summary>
        public RobotEventType RobotEventType { get; internal set; }

        /// <summary>
        /// 获取 RobotEventType 枚举的描述信息
        /// </summary>
        public RobotEventAttribute EventDescription
        {
            get
            {
                return RobotEventType.GetEnumCustomAttribute<RobotEventAttribute>();
            }
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public int EventCode { get; internal set; }

        /// <summary>
        /// 事件详情
        /// </summary>
        public string EventContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="code"></param>
        /// <param name="content"></param>
        public RobotEventHandler(RobotEventType eventType, int code, string content)
        {
            RobotEventType = eventType;
            EventCode = code;
            EventContent = content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"EventCode: {EventCode}\tRobotEventType: {RobotEventType}\tEventContent: {EventContent}\tEventDescription: {EventDescription.ZhCnDescription}";
        }
    }
}