using GeneralTool.General.Enums;
using static GeneralTool.General.AuboSixAxisMechanicalArm.MetaData;

namespace GeneralTool.General.AuboSixAxisMechanicalArm
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
                return this.RobotEventType.GetEnumCustomAttribute<RobotEventAttribute>();
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
            this.RobotEventType = eventType;
            this.EventCode = code;
            this.EventContent = content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"EventCode: {this.EventCode}\tRobotEventType: {this.RobotEventType}\tEventContent: {this.EventContent}\tEventDescription: {this.EventDescription.ZhCnDescription}";
        }
    }
}