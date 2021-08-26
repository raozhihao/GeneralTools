using System;

namespace GeneralTool.General.AuboSixAxisMechanicalArm
{
    /// <summary>
    /// 路径参数
    /// </summary>
    public class WayPointsArgs : EventArgs
    {
        /// <summary>
        /// 描述机械臂的路点信息
        /// </summary>
        public MetaData.WayPoint_S WayPoints { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wayPoints"></param>
        public WayPointsArgs(MetaData.WayPoint_S wayPoints)
        {
            this.WayPoints = wayPoints;
        }
    }
}
