using System;
using System.Runtime.InteropServices;
using static GeneralTool.General.AuboSixAxisMechanicalArm.MetaData;

namespace GeneralTool.General.AuboSixAxisMechanicalArm
{
    /// <summary>
    /// 机械臂控制适配器类
    /// </summary>
    public partial class RobotAdepter
    {
        private string error;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErroMsg
        {
            get
            {
                return error;
            }
        }

        private REALTIME_ROADPOINT_CALLBACK RobotPosCallBack;
        private REALTIME_JOINT_STATUS_CALLBACK JointStatusCallBack;
        private ROBOT_EVENT_CALLBACK RobotEventCallbackPtr;

        /// <summary>
        /// 六轴信息事件
        /// </summary>
        public event EventHandler<WayPointsArgs> WayPointsEvent;

        /// <summary>
        /// 机械臂信息回调
        /// </summary>
        public event EventHandler<RobotEventHandler> RobotEventHandler;

        /// <summary>
        /// 机械臂关节角变化事件
        /// </summary>
        public event EventHandler<JointStatusArgs> JointStatusEventHandler;

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 连接机械臂
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Link(string ip, int port)
        {
            this.IP = ip;
            this.Port = port;
            if (rs_initialize() == Util.RSERR_SUCC)
            {
                int reCode = rs_create_context(ref Rshd);
                if (reCode != Util.RSERR_SUCC)
                {
                    this.error = MetaData.GetRetunDescription<RetunCode>(reCode);
                    return false;
                }

                reCode = rs_login(this.Rshd, ip, port);
                if (reCode != Util.RSERR_SUCC)
                {
                    this.error = MetaData.GetRetunDescription<LoginCode>(reCode);
                    return false;
                }

                //设置是否允许实时路点信息推送
                rs_enable_push_realtime_roadpoint(Rshd, true);

                //函数指针实例化
                RobotPosCallBack = new REALTIME_ROADPOINT_CALLBACK(CurrentPositionCallback);
                rs_setcallback_realtime_roadpoint(Rshd, RobotPosCallBack, IntPtr.Zero);


                //机械臂事件回调

                RobotEventCallbackPtr = new ROBOT_EVENT_CALLBACK(RobotEventCallback);
                rs_setcallback_robot_event(Rshd, RobotEventCallbackPtr, IntPtr.Zero);


                //设置是否允许实时关节角状态信息推送
                rs_enable_push_realtime_joint_status(Rshd, true);

                //函数指针实例化
                JointStatusCallBack = new REALTIME_JOINT_STATUS_CALLBACK(JointStatusCallBackMethod);
                rs_setcallback_realtime_joint_status(Rshd, JointStatusCallBack, IntPtr.Zero);

                return true;
            }
            return false;
        }



        private void JointStatusCallBackMethod(ref MetaData.JointStatus jointStatus, int size, IntPtr arg)
        {
            this.JointStatusEventHandler?.Invoke(this, new JointStatusArgs(jointStatus));
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disctonnected()
        {
            rs_logout(this.Rshd);
            rs_destory_context(this.Rshd);
            rs_uninitialize();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~RobotAdepter()
        {
            rs_uninitialize();
        }


        //回调函数
        void CurrentPositionCallback(ref MetaData.WayPoint_S waypoint, IntPtr arg)
        {
            this.WayPointsEvent?.Invoke(this, new WayPointsArgs(waypoint));
        }

        void RobotEventCallback(ref MetaData.RobotEventInfo rs_event, IntPtr arg)
        {
            var eventHandler = new RobotEventHandler((RobotEventType)rs_event.eventType, rs_event.eventCode, Marshal.PtrToStringAnsi(rs_event.eventContent));
            this.RobotEventHandler?.Invoke(this, eventHandler);
        }


    }
}
