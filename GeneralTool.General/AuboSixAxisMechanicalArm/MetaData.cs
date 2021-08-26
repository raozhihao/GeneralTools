using System;
using System.Runtime.InteropServices;

namespace GeneralTool.General.AuboSixAxisMechanicalArm
{
    /// <summary>
    /// 机械臂控制元数据类
    /// </summary>
    public class MetaData
    {
        /// <summary>
        /// 坐标系标定方法索引值
        /// </summary>
        [Serializable]
        public enum CoordMethodIndex
        {
            /// <summary>
            /// 原点、x轴正半轴、y轴正半轴
            /// </summary>
            xOy = 0, // 
            /// <summary>
            /// 原点、y轴正半轴、z轴正半轴
            /// </summary>
            yOz, // 
            /// <summary>
            /// 原点、z轴正半轴、x轴正半轴
            /// </summary>
            zOx,
            /// <summary>
            /// 原点、x轴正半轴、x、y轴平面的第一象限上任意一点
            /// </summary>
            xOxy,
            /// <summary>
            /// 原点、x轴正半轴、x、z轴平面的第一象限上任意一点
            /// </summary>
            xOxz, // 
            /// <summary>
            /// 原点、y轴正半轴、y、z轴平面的第一象限上任意一点
            /// </summary>
            yOyz, // 
            /// <summary>
            /// 原点、y轴正半轴、y、x轴平面的第一象限上任意一点
            /// </summary>
            yOyx, // 
            /// <summary>
            /// 原点、z轴正半轴、z、x轴平面的第一象限上任意一点
            /// </summary>
            zOzx, // 
            /// <summary>
            /// 原点、z轴正半轴、z、y轴平面的第一象限上任意一点
            /// </summary>
            zOzy, // 
            /// <summary>
            /// 
            /// </summary>
            CoordTypeCount
        }

        /// <summary>
        /// 示教接口运动模式
        /// </summary>
        [Serializable]
        public enum TeachMode
        {
            /// <summary>
            /// 
            /// </summary>
            NO_TEACH = 0,
            /// <summary>
            /// 轴动示教
            /// </summary>
            JOINT1, //
            /// <summary>
            /// 
            /// </summary>
            JOINT2,
            /// <summary>
            /// 
            /// </summary>
            JOINT3,
            /// <summary>
            /// 
            /// </summary>
            JOINT4,
            /// <summary>
            /// 
            /// </summary>
            JOINT5,
            /// <summary>
            /// 
            /// </summary>
            JOINT6,
            /// <summary>
            /// 位置示教
            /// </summary>
            MOV_X,
            /// <summary>
            /// 
            /// </summary>
            MOV_Y,
            /// <summary>
            /// 
            /// </summary>
            MOV_Z,
            /// <summary>
            /// 姿态示教
            /// </summary>
            ROT_X,
            /// <summary>
            /// 
            /// </summary>
            ROT_Y,
            /// <summary>
            /// 
            /// </summary>
            ROT_Z,
        };

        /// <summary>
        /// 路点位置信息的表示方法
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct Pos
        {
            /// <summary>
            /// 
            /// </summary>
            public double x;
            /// <summary>
            /// 
            /// </summary>
            public double y;
            /// <summary>
            /// 
            /// </summary>
            public double z;
        }

        /// <summary>
        /// 路点位置信息的表示方法
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct CartesianPos_U
        {
            /// <summary>
            /// 
            /// </summary>
            // 指定数组尺寸
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public double[] positionVector;
        };

        /// <summary>
        /// 关节角信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct JointStatus
        {
            /// <summary>
            /// 关节电流
            /// </summary>
            public int jointCurrentI;
            /// <summary>
            /// 关节速度
            /// </summary>
            public int jointSpeedMoto;
            /// <summary>
            /// 关节角
            /// </summary>
            public float jointPosJ;
            /// <summary>
            /// 关节电压
            /// </summary>
            public float jointCurVol;
            /// <summary>
            /// 当前温度
            /// </summary>
            public float jointCurTemp;
            /// <summary>
            /// 电机目标电流
            /// </summary>
            public int jointTagCurrentI;
            /// <summary>
            /// 电机目标速度
            /// </summary>
            public float jointTagSpeedMoto;
            /// <summary>
            /// 目标关节角
            /// </summary>
            public float jointTagPosJ;
            /// <summary>
            /// 关节错误码
            /// </summary>
            public UInt16 jointErrorNum;
        }

        /// <summary>
        /// 机械臂诊断信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct RobotDiagnosis
        {
            //CAN通信状态:0x01~0x80：关节CAN通信错误（每个关节占用1bit）
            /// <summary>
            /// 0x00：无错误 0xff：CAN总线存在错误
            /// </summary>
            public byte armCanbusStatus;
            /// <summary>
            /// 机械臂48V电源当前电流
            /// </summary>
            public float armPowerCurrent;
            /// <summary>
            /// 机械臂48V电源当前电压
            /// </summary>
            public float armPowerVoltage;
            /// <summary>
            /// 机械臂48V电源状态（开、关）
            /// </summary>
            public bool armPowerStatus;
            /// <summary>
            /// 控制箱温度
            /// </summary>
            public char contorllerTemp;
            /// <summary>
            /// 控制箱湿度
            /// </summary>
            public byte contorllerHumidity;
            /// <summary>
            /// 远程关机信号
            /// </summary>
            public bool remoteHalt;
            /// <summary>
            /// 机械臂软急停
            /// </summary>
            public bool softEmergency;
            /// <summary>
            /// 远程急停信号
            /// </summary>
            public bool remoteEmergency;
            /// <summary>
            /// 碰撞检测位
            /// </summary>
            public bool robotCollision;
            /// <summary>
            /// 机械臂进入力控模式标志位
            /// </summary>
            public bool forceControlMode;
            /// <summary>
            /// 刹车状态
            /// </summary>
            public bool brakeStuats;
            /// <summary>
            /// 末端速度
            /// </summary>
            public float robotEndSpeed;
            /// <summary>
            /// 最大加速度
            /// </summary>
            public int robotMaxAcc;
            /// <summary>
            /// 上位机软件状态位
            /// </summary>
            public bool orpeStatus;
            /// <summary>
            /// 位姿读取使能位
            /// </summary>
            public bool enableReadPose;
            /// <summary>
            /// 安装位置状态
            /// </summary>
            public bool robotMountingPoseChanged;
            /// <summary>
            /// 磁编码器错误状态
            /// </summary>
            public bool encoderErrorStatus;
            /// <summary>
            /// 静止碰撞检测开关
            /// </summary>
            public bool staticCollisionDetect;
            /// <summary>
            /// 关节碰撞检测 每个关节占用1bit 0-无碰撞 1-存在碰撞
            /// </summary>
            public byte jointCollisionDetect;
            /// <summary>
            /// 光电编码器不一致错误 0-无错误 1-有错误
            /// </summary>
            public bool encoderLinesError;
            /// <summary>
            /// joint error status
            /// </summary>
            public bool jointErrorStatus;
            /// <summary>
            /// 机械臂奇异点过速警告
            /// </summary>
            public bool singularityOverSpeedAlarm;
            /// <summary>
            /// 机械臂电流错误警告
            /// </summary>
            public bool robotCurrentAlarm;
            /// <summary>
            /// tool error
            /// </summary>
            public byte toolIoError;
            /// <summary>
            /// 机械臂安装位置错位（只在力控模式下起作用）
            /// </summary>
            public bool robotMountingPoseWarning;
            /// <summary>
            /// mac缓冲器长度
            /// </summary>
            public UInt16 macTargetPosBufferSize;
            /// <summary>
            /// mac缓冲器有效数据长度
            /// </summary>
            public UInt16 macTargetPosDataSize;
            /// <summary>
            /// /mac数据中断
            /// </summary>
            public byte macDataInterruptWarning;
        }

        /// <summary>
        /// 姿态的四元素表示方法
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct Ori
        {
            /// <summary>
            /// 
            /// </summary>
            public double w;
            /// <summary>
            /// 
            /// </summary>
            public double x;
            /// <summary>
            /// 
            /// </summary>
            public double y;
            /// <summary>
            /// 
            /// </summary>
            public double z;
        };

        /// <summary>
        /// 姿态的欧拉角表示方法
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct Rpy
        {
            /// <summary>
            /// 
            /// </summary>
            public double rx;
            /// <summary>
            /// 
            /// </summary>
            public double ry;
            /// <summary>
            /// 
            /// </summary>
            public double rz;
        };

        /// <summary>
        /// 描述机械臂的路点信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct WayPoint_S
        {
            /// <summary>
            /// 机械臂的位置信息　X,Y,Z
            /// </summary>
            public Pos cartPos;
            /// <summary>
            /// 机械臂姿态信息
            /// </summary>
            public Ori orientation;
            /// <summary>
            /// 机械臂关节角信息
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Util.ARM_DOF)]
            public double[] jointpos;
        };

        /// <summary>
        /// 机械臂关节速度加速度信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct JointVelcAccParam
        {
            /// <summary>
            /// 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Util.ARM_DOF)]
            public double[] jointPara;
        };

        /// <summary>
        /// 机械臂关节角度
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct JointRadian
        {
            /// <summary>
            /// 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Util.ARM_DOF)]
            public double[] jointRadian;
        };

        /// <summary>
        /// 机械臂工具端参数
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct ToolInEndDesc
        {
            /// <summary>
            /// 工具相对于末端坐标系的位置
            /// </summary>
            public Pos cartPos;
            /// <summary>
            /// 工具相对于末端坐标系的姿态
            /// </summary>
            public Ori orientation;
        };

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public enum CoordType
        {
            /// <summary>
            /// 
            /// </summary>
            BaseCoordinate = 0,
            /// <summary>
            /// 
            /// </summary>
            EndCoordinate,
            /// <summary>
            /// 
            /// </summary>
            WorldCoordinate
        }

        /// <summary>
        /// 坐标系结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct CoordCalibrate
        {
            /// <summary>
            /// 坐标系类型：当coordType==BaseCoordinate或者coordType==EndCoordinate是，下面3个参数不做处理
            /// </summary>
            public CoordType coordType;
            /// <summary>
            /// 坐标系标定方法
            /// </summary>
            public int methods;
            /// <summary>
            /// 用于标定坐标系的３个点（关节角），对应于机械臂法兰盘中心点基于基座标系
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public JointRadian[] jointPara;
            /// <summary>
            /// 标定的时候使用的工具描述
            /// </summary>
            public ToolInEndDesc toolDesc;
        };

        /// <summary>
        /// 转轴定义
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct MoveRotateAxis
        {
            /// <summary>
            /// 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public double[] rotateAxis;
        };

        /// <summary>
        /// 描述运动属性中的偏移属性
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct MoveRelative
        {
            /// <summary>
            /// 是否使能偏移
            /// </summary>
            public byte enable;
            /// <summary>
            /// 偏移量 x,y,z
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] pos;
            //public Pos pos;
            /// <summary>
            /// 相对姿态偏移量
            /// </summary>
            public Ori orientation;
        };

        /// <summary>
        /// 该结构体描述工具惯量
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct ToolInertia
        {
            /// <summary>
            /// 
            /// </summary>
            public double xx;
            /// <summary>
            /// 
            /// </summary>
            public double xy;
            /// <summary>
            /// 
            /// </summary>
            public double xz;
            /// <summary>
            /// 
            /// </summary>
            public double yy;
            /// <summary>
            /// 
            /// </summary>
            public double yz;
            /// <summary>
            /// 
            /// </summary>
            public double zz;
        };

        /// <summary>
        /// 动力学参数
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct ToolDynamicsParam
        {
            /// <summary>
            /// 工具重心的X坐标
            /// </summary>
            public double positionX; //
            /// <summary>
            /// 工具重心的Y坐标
            /// </summary>
            public double positionY; //
            /// <summary>
            /// 工具重心的Z坐标
            /// </summary>
            public double positionZ; //
            /// <summary>
            /// 工具重量
            /// </summary>
            public double payload; //
            /// <summary>
            /// 工具惯量
            /// </summary>
            public ToolInertia toolInertia; //
        };

        /// <summary>
        /// 事件类型
        /// </summary>
        [Serializable]
        public enum RobotEventType
        {
            /// <summary>
            /// 机械臂CAN总线错误
            /// </summary>
            [RobotEvent("机械臂CAN总线错误", true, "Arm can bus error")]
            RobotEvent_armCanbusError,

            /// <summary>
            /// 远程关机
            /// </summary>
            [RobotEvent("远程关机", true, "Remote halt")]
            RobotEvent_remoteHalt,

            /// <summary>
            /// 机械臂远程急停
            /// </summary>
            [RobotEvent("机械臂远程急停", true, "Remote emergency stop")]
            RobotEvent_remoteEmergencyStop,

            /// <summary>
            /// 关节错误
            /// </summary>
            [RobotEvent("关节错误", true, "joint error")]
            RobotEvent_jointError,

            /// <summary>
            /// 力控制
            /// </summary>
            [RobotEvent("力控制", true, "Force control")]
            RobotEvent_forceControl,

            /// <summary>
            /// 退出力控制
            /// </summary>
            [RobotEvent("退出力控制", false, "Exit force control")]
            RobotEvent_exitForceControl,

            /// <summary>
            /// 软急停
            /// </summary>
            [RobotEvent("软急停", true, "Soft emergency")]
            RobotEvent_softEmergency,

            /// <summary>
            /// 退出软急停
            /// </summary>
            [RobotEvent("退出软急停", false, "Exit soft emergency")]
            RobotEvent_exitSoftEmergency,

            /// <summary>
            /// 机械臂发生碰撞
            /// </summary>
            [RobotEvent("机械臂发生碰撞", true, "Collision")]
            RobotEvent_collision,

            /// <summary>
            /// 碰撞状态改变
            /// </summary>
            [RobotEvent("碰撞状态改变", false, "Collision status changed")]
            RobotEvent_collisionStatusChanged,

            /// <summary>
            /// 工具动力学参数设置成功
            /// </summary>
            [RobotEvent("工具动力学参数设置成功", false, "Tcp parameters succ")]
            RobotEvent_tcpParametersSucc,

            /// <summary>
            /// 机械臂电源开关状态改变
            /// </summary>
            [RobotEvent("机械臂电源开关状态改变", false, "Power changed")]
            RobotEvent_powerChanged,

            /// <summary>
            /// 机械臂电源关闭,不建议使用　　已用RobotEventArmPowerOff(2600) 替代
            /// </summary>
            [RobotEvent("机械臂电源关闭,不建议使用　　已用RobotEventArmPowerOff(2600) 替代", true, "Arm power off")]
            RobotEvent_ArmPowerOff,

            /// <summary>
            /// 安装位置发生改变
            /// </summary>
            [RobotEvent("安装位置发生改变", false, "Mounting pose changed")]
            RobotEvent_mountingPoseChanged,

            /// <summary>
            /// 编码器错误
            /// </summary>
            [RobotEvent("编码器错误", true, "Encoder error")]
            RobotEvent_encoderError,

            /// <summary>
            /// 编码器线数不一致
            /// </summary>
            [RobotEvent("编码器线数不一致", true, "Encoder lines error")]
            RobotEvent_encoderLinesError,

            /// <summary>
            /// 奇异点超速
            /// </summary>
            [RobotEvent("奇异点超速", true, "Singularity over speed")]
            RobotEvent_singularityOverspeed,

            /// <summary>
            /// 机械臂电流异常
            /// </summary>
            [RobotEvent("机械臂电流异常", true, "Current alarm")]
            RobotEvent_currentAlarm,

            /// <summary>
            /// 机械臂工具端错误
            /// </summary>
            [RobotEvent("机械臂工具端错误", true, "Toolio error")]
            RobotEvent_toolioError,

            /// <summary>
            /// 机械臂启动阶段
            /// </summary>
            [RobotEvent("机械臂启动阶段", false, "Robot startup phase")]
            RobotEvent_robotStartupPhase,

            /// <summary>
            /// 机械臂启动完成结果
            /// </summary>
            [RobotEvent("机械臂启动完成结果", false, "Robot startup done result")]
            RobotEvent_robotStartupDoneResult,

            /// <summary>
            /// 机械臂关机结果
            /// </summary>
            [RobotEvent("机械臂关机结果", false, "Robot shutdown done")]
            RobotEvent_robotShutdownDone,

            /// <summary>
            /// 机械臂轨迹运动到位信号通知
            /// </summary>
            [RobotEvent("机械臂轨迹运动到位信号通知", false, "At track target pos")]
            RobotEvent_atTrackTargetPos,

            /// <summary>
            /// 设置电源状态完成
            /// </summary>
            [RobotEvent("设置电源状态完成", false, "Set power on done")]
            RobotSetPowerOnDone,

            /// <summary>
            /// 机械臂刹车释放完成
            /// </summary>
            [RobotEvent("机械臂刹车释放完成", false, "Release brake done")]
            RobotReleaseBrakeDone,

            /// <summary>
            /// 机械臂控制状态改变
            /// </summary>
            [RobotEvent("机械臂控制状态改变", false, "Robot controller state chaned")]
            RobotEvent_robotControllerStateChaned,

            /// <summary>
            /// 机械臂控制错误----一般是算法规划出现问题时返回
            /// </summary>
            [RobotEvent("机械臂控制错误----一般是算法规划出现问题时返回", true, "Robot controller error")]
            RobotEvent_robotControllerError,

            /// <summary>
            /// socket断开连接
            /// </summary>
            [RobotEvent("socket断开连接", true, "Socket disconnected")]
            RobotEvent_socketDisconnected,

            /// <summary>
            /// 机械臂控制异常
            /// </summary>
            [RobotEvent("机械臂控制异常", true, "Robot control exception")]
            RobotEvent_robotControlException,

            /// <summary>
            /// RobotEvent_trackPlayInterrupte
            /// </summary>
            [RobotEvent("RobotEvent_trackPlayInterrupte", false, "Track play interrupte")]
            RobotEvent_trackPlayInterrupte,

            /// <summary>
            /// 静态冲突状态已更改
            /// </summary>
            [RobotEvent("静态冲突状态已更改", false, "Static collision status changed")]
            RobotEvent_staticCollisionStatusChanged,

            /// <summary>
            /// 安装姿势警告
            /// </summary>
            [RobotEvent("安装姿势警告", true, "Mounting pose warning")]
            RobotEvent_MountingPoseWarning,

            /// <summary>
            /// Mac数据中断警告
            /// </summary>
            [RobotEvent("Mac数据中断警告", true, "Mac data interrupt warning")]
            RobotEvent_MacDataInterruptWarning,

            /// <summary>
            /// 工具IO错误
            /// </summary>
            [RobotEvent("工具IO错误", true, "Tool Io error")]
            RobotEvent_ToolIoError,

            /// <summary>
            /// 接口板安全IO事件
            /// </summary>
            [RobotEvent("接口板安全IO事件", false, "Interfac board safe Io event")]
            RobotEvent_InterfacBoardSafeIoEvent,

            /// <summary>
            /// 机械臂握手成功
            /// </summary>
            [RobotEvent("机械臂握手成功", false, "Robot hand shake succ")]
            RobotEvent_RobotHandShakeSucc,

            /// <summary>
            /// 机械臂握手失败
            /// </summary>
            [RobotEvent("机械臂握手失败", false, "Robot hand shake failed")]
            RobotEvent_RobotHandShakeFailed,

            /// <summary>
            /// 机械臂错误信息通知
            /// </summary>
            [RobotEvent("机械臂错误信息通知", false, "Robot error info notify")]
            RobotEvent_RobotErrorInfoNotify,

            /// <summary>
            /// 接口板DI改变
            /// </summary>
            [RobotEvent("接口板DI改变", false, "Interfac board DI changed")]
            RobotEvent_InterfacBoardDIChanged,

            /// <summary>
            /// 接口板DO改变
            /// </summary>
            [RobotEvent("接口板DO改变", false, "Interfac board DO changed")]
            RobotEvent_InterfacBoardDOChanged,

            /// <summary>
            /// 接口板AI改变
            /// </summary>
            [RobotEvent("接口板AI改变", false, "Interfac board AI changed")]
            RobotEvent_InterfacBoardAIChanged,

            /// <summary>
            /// 接口板AO改变
            /// </summary>
            [RobotEvent("接口板AO改变", false, "Interfac board AO changed")]
            RobotEvent_InterfacBoardAOChanged,

            /// <summary>
            /// UpdateJoint6Rot360Flag
            /// </summary>
            [RobotEvent("UpdateJoint6Rot360Flag", false, "Update joint 6 Rot 360 flag")]
            RobotEvent_UpdateJoint6Rot360Flag,

            /// <summary>
            /// 机械臂移动控制完成
            /// </summary>
            [RobotEvent("机械臂移动控制完成", false, "Robot move control done")]
            RobotEvent_RobotMoveControlDone,

            /// <summary>
            /// 机械臂移动控制停止
            /// </summary>
            [RobotEvent("机械臂移动控制停止", false, "Robot move control stop done")]
            RobotEvent_RobotMoveControlStopDone,

            /// <summary>
            /// 机械臂移动暂停完成
            /// </summary>
            [RobotEvent("机械臂移动暂停完成", false, "Robot move control pause done")]
            RobotEvent_RobotMoveControlPauseDone,

            /// <summary>
            /// 机械臂移动控制继续
            /// </summary>
            [RobotEvent("机械臂移动控制继续", false, "Robot move control continue done")]
            RobotEvent_RobotMoveControlContinueDone,

            //主从模式切换
            /// <summary>
            /// 机械臂切换到在线主机
            /// </summary>
            [RobotEvent("机械臂切换到在线主机", false, "Robot switch to online master")]
            RobotEvent_RobotSwitchToOnlineMaster,

            /// <summary>
            /// 机械臂切换到在线从机
            /// </summary>
            [RobotEvent("机械臂切换到在线从机", false, "Robot switch to online slave")]
            RobotEvent_RobotSwitchToOnlineSlave,

            /// <summary>
            /// 输送机轨道机械臂启动
            /// </summary>
            [RobotEvent("输送机轨道机械臂启动", false, "Conveyor track robot startup")]
            RobotEvent_ConveyorTrackRobotStartup,

            /// <summary>
            /// 输送机轨道机械臂捕捉
            /// </summary>
            [RobotEvent("输送机轨道机械臂捕捉", false, "Conveyor track robot catchup")]
            RobotEvent_ConveyorTrackRobotCatchup,

            /// <summary>
            /// exceptEvent
            /// </summary>
            [RobotEvent("exceptEvent", false, "exceptEvent")]
            RobotEvent_exceptEvent = 100,

            /// <summary>
            /// 无效事件
            /// </summary>
            [RobotEvent("无效事件", false, "")]
            RobotEventInvalid = 1000,   // 无效的事件

            /**
             * RobotControllerErrorEvent  控制器异常事件 1001~1499
             *
             * 事件处理建议
             * 建议采取措施:停止当前运动
             *
             * PS: 这些事件会引起机械臂运动的错误返回
             *     使用时尽量用枚举变量　　枚举变量值只是为了查看日志方便
             *
             **/


            /// <summary>
            /// 关节运动属性配置错误
            /// </summary>
            [RobotEvent("关节运动属性配置错误", true, "MoveJ configuration error")]
            RobotEventMoveJConfigError = 1001,

            /// <summary>
            /// 直线运动属性配置错误
            /// </summary>
            [RobotEvent("直线运动属性配置错误", true, "MoveL configuration error")]
            RobotEventMoveLConfigError = 1002,

            /// <summary>
            /// 轨迹运动属性配置错误
            /// </summary>
            [RobotEvent("轨迹运动属性配置错误", true, "MoveP configuration error")]
            RobotEventMovePConfigError = 1003,

            /// <summary>
            /// 无效的运动属性配置
            /// </summary>
            [RobotEvent("无效的运动属性配置", true, "Invail configuration")]
            RobotEventInvailConfigError = 1004,

            /// <summary>
            /// 等待机器人停止
            /// </summary>
            [RobotEvent("请等待机器人停止", true, "Please wait robot stopped")]
            RobotEventWaitRobotStopped = 1005,

            /// <summary>
            /// 超出关节运动范围
            /// </summary>
            [RobotEvent("超出关节运动范围", true, "Joint out of range")]
            RobotEventJointOutRange = 1006,

            /// <summary>
            /// 第一个路点设置错误,请正确设置MODEP第一个路点
            /// </summary>
            [RobotEvent("第一个路点设置错误,请正确设置MODEP第一个路点", true, "Please set first waypoint correctly in modep")]
            RobotEventFirstWaypointSetError = 1007,

            /// <summary>
            /// 传送带跟踪配置错误
            /// </summary>
            [RobotEvent("传送带跟踪配置错误", true, "Configuration error for conveyor tracking")]
            RobotEventConveyorTrackConfigError = 1008,

            /// <summary>
            /// 传送带轨迹类型错误
            /// </summary>
            [RobotEvent("传送带轨迹类型错误", true, "Unsupported conveyor tracking trajectory type")]
            RobotEventConveyorTrackTrajectoryTypeError = 1009,

            /// <summary>
            /// 相对坐标变换逆解失败
            /// </summary>
            [RobotEvent("相对坐标变换逆解失败", true, "Inverse kinematics failure due to invalid relative transform")]
            RobotEventRelativeTransformIKFailed = 1010,

            /// <summary>
            /// 示教模式发生碰撞
            /// </summary>
            [RobotEvent("示教模式发生碰撞", true, "Collision in teach-mode")]
            RobotEventTeachModeCollision = 1011,

            /// <summary>
            /// 运动属性配置错误,外部工具或手持工件配置错误
            /// </summary>
            [RobotEvent("运动属性配置错误,外部工具或手持工件配置错误", true, "Configuration error for external tool and hand workobject")]
            RobotEventextErnalToolConfigError = 1012,  // configuration error for external tool and hand workobject     

            /// <summary>
            /// 轨迹异常
            /// </summary>
            [RobotEvent("轨迹异常", true, "Trajectory is abnormal ")]
            RobotEventTrajectoryAbnormal = 1101,  // Trajectory is abnormal 

            /// <summary>
            /// 轨迹规划错误
            /// </summary>
            [RobotEvent("轨迹规划错误", true, "Trajectory is abnormal,online planning failed")]
            RobotEventOnlineTrajectoryPlanError = 1102,  // Trajectory is abnormal,online planning failed  

            /// <summary>
            /// 二型在线轨迹规划失败
            /// </summary>
            [RobotEvent("二型在线轨迹规划失败", true, "Trajectory is abnormal,type II online planning failed ")]
            RobotEventOnlineTrajectoryTypeIIError = 1103,  // Trajectory is abnormal,type II online planning failed 

            /// <summary>
            /// 逆解失败
            /// </summary>
            [RobotEvent("逆解失败", true, "Trajectory is abnormal,inverse kinematics failed ")]
            RobotEventIKFailed = 1104,  // Trajectory is abnormal,inverse kinematics failed 

            /// <summary>
            /// 动力学限制保护
            /// </summary>
            [RobotEvent("动力学限制保护", true, "Trajectory is abnormal,abnormal limit protection ")]
            RobotEventAbnormalLimitProtect = 1105,  // Trajectory is abnormal,abnormal limit protection 

            /// <summary>
            /// 传送带跟踪失败
            /// </summary>
            [RobotEvent("传送带跟踪失败", true, "Trajectory is abnormal,conveyor tracking failed")]
            RobotEventConveyorTrackingFailed = 1106,  // Trajectory is abnormal,conveyor tracking failed  

            /// <summary>
            /// 超出传送带工作范围
            /// </summary>
            [RobotEvent("超出传送带工作范围", true, "Trajectory is abnormal,exceeding the conveyor working range")]
            RobotEventConveyorOutWorkingRange = 1107,  // Trajectory is abnormal,exceeding the conveyor working range 

            /// <summary>
            /// 关节超出范围
            /// </summary>
            [RobotEvent("关节超出范围", true, "Trajectory is abnormal,joint out of range")]
            RobotEventTrajectoryJointOutOfRange = 1108,  // Trajectory is abnormal,joint out of range 

            /// <summary>
            /// 关节超速
            /// </summary>
            [RobotEvent("关节超速", true, "Trajectory is abnormal,joint overspeed")]
            RobotEventTrajectoryJointOverspeed = 1109,  // Trajectory is abnormal,joint overspeed 

            /// <summary>
            /// 离线轨迹规划失败
            /// </summary>
            [RobotEvent("离线轨迹规划失败", true, "Trajectory is abnormal,Offline track planning failed")]
            RobotEventOfflineTrajectoryPlanFailed = 1110,  // Trajectory is abnormal,Offline track planning failed 

            /// <summary>
            /// 轨迹异常,关节加速度超限
            /// </summary>
            [RobotEvent("轨迹异常,关节加速度超限", true, "Trajectory is abnormal,joint acc out of range ")]
            RobotEventTrajectoryJointAccOutOfRange = 1111,  // 

            /// <summary>
            /// 控制器异常，逆解失败
            /// </summary>
            [RobotEvent("控制器异常，逆解失败", true, "The controller has an exception and the inverse kinematics failed")]
            RobotEventControllerIKFailed = 1200,  //  

            /// <summary>
            /// 控制器异常，状态异常
            /// </summary>
            [RobotEvent("控制器异常，状态异常", true, "The controller has an exception and the status is abnormal")]
            RobotEventControllerStatusException = 1201,  //  

            /// <summary>
            /// 关节跟踪误差过大
            /// </summary>
            [RobotEvent("关节跟踪误差过大", true, "Exception that joint tracking is lost")]
            RobotEventControllerTrackingLost = 1202,  // , .

            /// <summary>
            /// 关节跟踪误差过大
            /// </summary>
            [RobotEvent("关节跟踪误差过大", true, "Exception that joint tracking is lost")]
            RobotEventMonitorErrTrackingLost = 1203,  // , .

            /// <summary>
            /// 
            /// </summary>
            [RobotEvent("预留,不使用,RobotEventMonitorErrNoArrivalInTime")]
            RobotEventMonitorErrNoArrivalInTime = 1204,  // not used 

            /// <summary>
            /// 
            /// </summary>
            [RobotEvent("预留,不使用,RobotEventMonitorErrCurrentOverload")]
            RobotEventMonitorErrCurrentOverload = 1205,  // not used 预留

            /// <summary>
            /// 机械臂关节超出限制范围
            /// </summary>
            [RobotEvent("机械臂关节超出限制范围", true, "Exception that joint out of range")]
            RobotEventMonitorErrJointOutOfRange = 1206,  //   　

            /// <summary>
            /// 运动进入到stop阶段
            /// </summary>
            [RobotEvent("运动进入到stop阶段", false, "Movement enters the stop state")]
            RobotEventMoveEnterStopState = 1300,  //  


            /**
             * RobotHardwareErrorEvent  来自硬件反馈的异常事件 2001~2999
             *
             * 事件处理建议
             * RobotEventJointEncoderPollustion   建议采取措施:警告性通知
             * RobotEventDriveVersionError        建议采取措施:警告性通知
             * RobotEventJointCollision           建议采取措施:暂停当前运动 以便恢复运动
             * 其余的事件　　  建议采取措施:停止当前运动
             **/
            [RobotEvent("机械臂硬件错误", true, "Robot hardware error ")]
            RobotEventHardwareErrorNotify = 2001,  // 

            /// <summary>
            /// 机械臂关节错误
            /// </summary>
            [RobotEvent("机械臂关节错误", true, "Robot joint error ")]
            RobotEventJointError = 2101,  // 

            /// <summary>
            /// 机械臂关节过流
            /// </summary>
            [RobotEvent("机械臂关节过流", true, "Robot joint over current.")]
            RobotEventJointOverCurrent = 2102,  // 

            /// <summary>
            /// 机械臂关节过压
            /// </summary>
            [RobotEvent("机械臂关节过压", true, "Robot joint over voltage")]
            RobotEventJointOverVoltage = 2103,  // .　 

            /// <summary>
            /// 机械臂关节欠压
            /// </summary>
            [RobotEvent("机械臂关节欠压", true, "Robot joint low voltage")]
            RobotEventJointLowVoltage = 2104,  // .　  

            /// <summary>
            /// 机械臂关节过温
            /// </summary>
            [RobotEvent("机械臂关节过温", true, "Robot joint over temperature")]
            RobotEventJointOverTemperature = 2105,  // . 

            /// <summary>
            /// 机械臂关节霍尔错误
            /// </summary>
            [RobotEvent("机械臂关节霍尔错误", true, "Robot joint hall error")]
            RobotEventJointHallError = 2106,  // . 

            /// <summary>
            /// 机械臂关节编码器错误
            /// </summary>
            [RobotEvent("机械臂关节编码器错误", true, "Robot joint encoder error")]
            RobotEventJointEncoderError = 2107,  // . 

            /// <summary>
            /// 机械臂关节绝对编码器错误
            /// </summary>
            [RobotEvent("机械臂关节绝对编码器错误", true, "Robot joint absolute encoder error.")]
            RobotEventJointAbsoluteEncoderError = 2108,  //  

            /// <summary>
            /// 机械臂关节当前位置错误
            /// </summary>
            [RobotEvent("机械臂关节当前位置错误", true, "Robot joint current position error")]
            RobotEventJointCurrentDetectError = 2109,  // . 

            /// <summary>
            /// 机械臂关节编码器污染
            /// </summary>
            [RobotEvent("机械臂关节编码器污染", true, "Robot joint encoder pollustion")]
            RobotEventJointEncoderPollustion = 2110,  // .             建议采取措施:警告性通知

            /// <summary>
            /// 机械臂关节编码器Z信号错误
            /// </summary>
            [RobotEvent("机械臂关节编码器Z信号错误", true, "Robot joint encoder Z signal error")]
            RobotEventJointEncoderZSignalError = 2111,  // . 

            /// <summary>
            /// 机械臂关节编码器校准失效
            /// </summary>
            [RobotEvent("机械臂关节编码器校准失效", true, "Robot joint encoder calibrate　invalid")]
            RobotEventJointEncoderCalibrateInvalid = 2112,  // . 

            /// <summary>
            /// 机械臂关节IMU传感器失效
            /// </summary>
            [RobotEvent("机械臂关节IMU传感器失效", true, "Robot joint IMU sensor invalid")]
            RobotEventJoint_IMU_SensorInvalid = 2113,  // . 

            /// <summary>
            /// 机械臂关节温度传感器出错
            /// </summary>
            [RobotEvent("机械臂关节温度传感器出错", true, "Robot joint temperature sensor error")]
            RobotEventJointTemperatureSensorError = 2114,  // . 

            /// <summary>
            /// 机械臂关节CAN总线出错
            /// </summary>
            [RobotEvent("机械臂关节CAN总线出错", true, "Robot joint CAN BUS error")]
            RobotEventJointCanBusError = 2115,  // . 

            /// <summary>
            /// 机械臂关节当前电流错误
            /// </summary>
            [RobotEvent("机械臂关节当前电流错误", true, "Robot joint current error")]
            RobotEventJointCurrentError = 2116,  // . 

            /// <summary>
            /// 机械臂关节当前位置错误
            /// </summary>
            [RobotEvent("机械臂关节当前位置错误", true, "Robot joint current position error")]
            RobotEventJointCurrentPositionError = 2117,  // . 

            /// <summary>
            /// 机械臂关节超速
            /// </summary>
            [RobotEvent("机械臂关节超速", true, "Robot joint over speed.")]
            RobotEventJointOverSpeed = 2118,  //  

            /// <summary>
            /// 机械臂关节加速度过大错误
            /// </summary>
            [RobotEvent("机械臂关节加速度过大错误", true, "Robot joint over accelerate.")]
            RobotEventJointOverAccelerate = 2119,  //  

            /// <summary>
            /// 机械臂关节跟踪精度错误
            /// </summary>
            [RobotEvent("机械臂关节跟踪精度错误", true, "Robot joint trace accuracy")]
            RobotEventJointTraceAccuracy = 2120,  // . 

            /// <summary>
            /// 机械臂关节目标位置超范围
            /// </summary>
            [RobotEvent("机械臂关节目标位置超范围", true, "Robot joint target position out of range")]
            RobotEventJointTargetPositionOutOfRange = 2121,  // .  

            /// <summary>
            /// 机械臂关节目标速度超范围
            /// </summary>
            [RobotEvent("机械臂关节目标速度超范围", true, "Robot joint target speed out of range")]
            RobotEventJointTargetSpeedOutOfRange = 2122,  // . 

            /// <summary>
            /// 机械臂碰撞
            /// </summary>
            [RobotEvent("机械臂碰撞", true, "Robot joint collision")]
            RobotEventJointCollision = 2123,  // .     　　　建议采取措施:暂停当前运动

            /// <summary>
            /// 机械臂信息异常
            /// </summary>
            [RobotEvent("机械臂信息异常", true, "Robot data abnormal ")]
            RobotEventDataAbnormal = 2200,  // 

            /// <summary>
            /// 机械臂类型错误
            /// </summary>
            [RobotEvent("机械臂类型错误", true, "Robot type error ")]
            RobotEventRobotTypeError = 2201,  // 

            /// <summary>
            /// 机械臂加速度计芯片错误
            /// </summary>
            [RobotEvent("机械臂加速度计芯片错误", true, "Robot acceleration sensor error ")]
            RobotEventAccelerationSensorError = 2202,  // 

            /// <summary>
            /// 
            /// </summary>
            [RobotEvent("机械臂编码器线数错误", true, " Robot encoder line error  ")]
            RobotEventEncoderLineError = 2203,  //

            /// <summary>
            /// 机械臂进入拖动示教模式错误
            /// </summary>
            [RobotEvent("机械臂进入拖动示教模式错误", true, "Robot enter drag and teach mode error ")]
            RobotEventEnterDragAndTeachModeError = 2204,  // 

            /// <summary>
            /// 机械臂退出拖动示教模式错误
            /// </summary>
            [RobotEvent("机械臂退出拖动示教模式错误", true, "Robot exit drag and teach mode error ")]
            RobotEventExitDragAndTeachModeError = 2205,  // 

            /// <summary>
            /// 
            /// </summary>
            [RobotEvent("机械臂MAC数据中断错误", true, "Robot MAC data interruption error ")]
            RobotEventMACDataInterruptionError = 2206,  // 

            /// <summary>
            /// 驱动器版本错误(关节固件版本不一致)
            /// </summary>
            [RobotEvent("驱动器版本错误(关节固件版本不一致)", true, "Drive version error ")]
            RobotEventDriveVersionError = 2207,  // 

            /// <summary>
            /// 机械臂初始化异常
            /// </summary>
            [RobotEvent("机械臂初始化异常", true, "Robot init abnormal  ")]
            RobotEventInitAbnormal = 2300,  // 

            /// <summary>
            /// 机械臂驱动器使能失败
            /// </summary>
            [RobotEvent("机械臂驱动器使能失败", true, "Robot driver enable failed  ")]
            RobotEventDriverEnableFailed = 2301,  // 

            /// <summary>
            /// 机械臂驱动器使能自动回应失败
            /// </summary>
            [RobotEvent("机械臂驱动器使能自动回应失败", true, "Robot driver enable auto back failed")]
            RobotEventDriverEnableAutoBackFailed = 2302,  //   

            /// <summary>
            /// 机械臂驱动器使能电流环失败
            /// </summary>
            [RobotEvent("机械臂驱动器使能电流环失败", true, " Robot driver enable current loop failed")]
            RobotEventDriverEnableCurrentLoopFailed = 2303,  //  

            /// <summary>
            /// 机械臂驱动器设置目标电流失败
            /// </summary>
            [RobotEvent("机械臂驱动器设置目标电流失败", true, "Robot driver set target current failed")]
            RobotEventDriverSetTargetCurrentFailed = 2304,  //   

            /// <summary>
            /// 机械臂释放刹车失败
            /// </summary>
            [RobotEvent("机械臂释放刹车失败", true, "Robot driver release brake failed ")]
            RobotEventDriverReleaseBrakeFailed = 2305,  //  

            /// <summary>
            /// 机械臂使能位置环失败
            /// </summary>
            [RobotEvent("机械臂使能位置环失败", true, "Robot driver enable postion loop failed")]
            RobotEventDriverEnablePostionLoopFailed = 2306,  //   

            /// <summary>
            /// 设置最大加速度失败
            /// </summary>
            [RobotEvent("设置最大加速度失败", true, "Robot set max accelerate failed ")]
            RobotEventSetMaxAccelerateFailed = 2307,  //  

            /// <summary>
            /// 机械臂安全出错
            /// </summary>
            [RobotEvent("机械臂安全出错", true, "Robot Safety error")]
            RobotEventSafetyError = 2400,  //   

            /// <summary>
            /// 机械臂外部紧急停止
            /// </summary>
            [RobotEvent("机械臂外部紧急停止", true, "Robot extern emergency stop")]
            RobotEventExternEmergencyStop = 2401,  //   

            /// <summary>
            /// 机械臂系统紧急停止
            /// </summary>
            [RobotEvent("机械臂系统紧急停止", true, "Robot system emergency stop")]
            RobotEventSystemEmergencyStop = 2402,  //   

            /// <summary>
            /// 机械臂示教器紧急停止
            /// </summary>
            [RobotEvent("机械臂示教器紧急停止", true, "Robot teachpendant emergency stop")]
            RobotEventTeachpendantEmergencyStop = 2403,  //   

            /// <summary>
            /// 机械臂控制柜紧急停止
            /// </summary>
            [RobotEvent("机械臂控制柜紧急停止", true, "Robot control cabinet emergency stop")]
            RobotEventControlCabinetEmergencyStop = 2404,  //   

            /// <summary>
            /// 机械臂保护停止超时
            /// </summary>
            [RobotEvent("机械臂保护停止超时", true, "Robot protection stop timeout")]
            RobotEventProtectionStopTimeout = 2405,  //   

            /// <summary>
            /// 机械臂缩减模式超时
            /// </summary>
            [RobotEvent("机械臂缩减模式超时", true, "Robot reduced mode timeout")]
            RobotEventEeducedModeTimeout = 2406,  //   

            /// <summary>
            /// 机械臂mcu通信异常
            /// </summary>

            [RobotEvent("机械臂mcu通信异常", true, "Robot mcu communication error")]
            RobotEventSystemAbnormal = 2500,  //   

            /// <summary>
            /// 机械臂485通信异常
            /// </summary>
            [RobotEvent("机械臂485通信异常", true, "Robot RS485 communication error")]
            RobotEvent_MCU_CommunicationAbnormal = 2501,  //   

            /// <summary>
            /// 机械臂系统异常
            /// </summary>
            [RobotEvent("机械臂系统异常", true, "Robot systen abnormal  ")]
            RobotEvent485CommunicationAbnormal = 2502,  // 

            /// <summary>
            /// 控制柜接触器断开导致机械臂48V断电
            /// </summary>
            [RobotEvent("控制柜接触器断开导致机械臂48V断电", true, "Disconnecting the contactor causes the arm 48V power off")]
            RobotEventArmPowerOff = 2600,  // 

            /// <summary>
            /// 最大索引
            /// </summary>
            [RobotEvent("最大索引")]
            RobotEventHardwareErrorNotifyMaximumIndex = 2999,  // 

            /// <summary>
            /// 机械臂通知性事件
            /// </summary>
            [RobotEvent("机械臂通知性事件", false, " Robot notification event")]
            RobotEventNotifyEvent = 3000,  // 

            /// <summary>
            /// 机械臂事件通知-碰撞等级被改变
            /// </summary>
            [RobotEvent("机械臂事件通知-碰撞等级被改变", false, "Robot Collision level change")]
            RobotEventNotifyCollisionLevelChange = 3001,  //  机械臂事件通知-碰撞等级被改变

            /// <summary>
            /// 未知
            /// </summary>
            //unknown event
            [RobotEvent("未知")]
            robot_event_unknown,

            /// <summary>
            /// 开始用户
            /// </summary>
            //user event
            [RobotEvent("开始用户(未知)", false, "first user event id")]
            RobotEvent_User = 9000,                            // 

            /// <summary>
            /// 最大用户
            /// </summary>
            [RobotEvent("最大用户(未知)", false, "last user event id")]
            RobotEvent_MaxUser = 9999                             // 
        }
        /// <summary>
        /// IO类型
        /// </summary>
        [Serializable]
        public enum RobotIoType //
        {
            /// <summary>
            /// 接口板控制器DI(数字量输入)　　　只读(一般系统内部使用)
            /// </summary>
            RobotBoardControllerDI,    //
            /// <summary>
            /// 接口板控制器DO(数字量输出)     只读(一般系统内部使用)
            /// </summary>
            RobotBoardControllerDO,    //
            /// <summary>
            /// 接口板控制器AI(模拟量输入)　   只读(一般系统内部使用)
            /// </summary>
            RobotBoardControllerAI,    //
            /// <summary>
            /// 接口板控制器AO(模拟量输出)　　　只读(一般系统内部使用)
            /// </summary>
            RobotBoardControllerAO,    //
            /// <summary>
            /// 接口板用户DI(数字量输入)　　可读可写
            /// </summary>
            RobotBoardUserDI,          //
            /// <summary>
            /// 接口板用户DO(数字量输出)   可读可写
            /// </summary>
            RobotBoardUserDO,          //
            /// <summary>
            /// 接口板用户AI(模拟量输入)   可读可写
            /// </summary>
            RobotBoardUserAI,          //
            /// <summary>
            /// 接口板用户AO(模拟量输出)   可读可写
            /// </summary>
            RobotBoardUserAO,          //
            /// <summary>
            /// 工具端DI
            /// </summary>
            RobotToolDI,               //
            /// <summary>
            /// 工具端DO
            /// </summary>
            RobotToolDO,               //
            /// <summary>
            /// 工具端AI
            /// </summary>
            RobotToolAI,               //
            /// <summary>
            /// 工具端AO
            /// </summary>
            RobotToolAO,               //
        }

        /// <summary>
        /// 机械臂启动完成状态
        /// </summary>
        [Serializable]
        public enum ROBOT_SERVICE_STATE
        {
            /// <summary>
            /// 
            /// </summary>
            ROBOT_SERVICE_READY = 0,
            /// <summary>
            /// 
            /// </summary>
            ROBOT_SERVICE_STARTING,
            /// <summary>
            /// 
            /// </summary>
            ROBOT_SERVICE_WORKING,
            /// <summary>
            /// 
            /// </summary>
            ROBOT_SERVICE_CLOSING,
            /// <summary>
            /// 
            /// </summary>
            ROBOT_SERVICE_CLOSED,
            /// <summary>
            /// 
            /// </summary>
            ROBOT_SETVICE_FAULT_POWER,
            /// <summary>
            /// 
            /// </summary>
            ROBOT_SETVICE_FAULT_BRAKE,
            /// <summary>
            /// 
            /// </summary>
            ROBOT_SETVICE_FAULT_NO_ROBOT
        }

        /// <summary>
        /// ＩＯ状态
        /// </summary>
        [Serializable]
        public enum IO_STATUS //
        {
            /// <summary>
            /// 有效
            /// </summary>
            IO_STATUS_INVALID = 0, //
            /// <summary>
            /// 无效
            /// </summary>
            IO_STATUS_VALID        //
        }

        /// <summary>
        /// 工具的电源类型
        /// </summary>
        [Serializable]
        public enum ToolPowerType
        {
            /// <summary>
            /// 
            /// </summary>
            OUT_0V = 0,
            /// <summary>
            /// 
            /// </summary>
            OUT_12V = 1,
            /// <summary>
            /// 
            /// </summary>
            OUT_24V = 2
        }

        /// <summary>
        /// 地址
        /// </summary>
        [Serializable]
        public enum ToolDigitalIOAddr
        {
            /// <summary>
            /// 
            /// </summary>
            TOOL_DIGITAL_IO_0 = 0,
            /// <summary>
            /// 
            /// </summary>
            TOOL_DIGITAL_IO_1 = 1,
            /// <summary>
            /// 
            /// </summary>
            TOOL_DIGITAL_IO_2 = 2,
            /// <summary>
            /// 
            /// </summary>
            TOOL_DIGITAL_IO_3 = 3
        }

        /// <summary>
        /// 机械臂状态枚举
        /// </summary>
        [Serializable]
        public enum RobotState
        {
            /// <summary>
            /// 
            /// </summary>
            RobotStopped = 0,
            /// <summary>
            /// 
            /// </summary>
            RobotRunning,
            /// <summary>
            /// 
            /// </summary>
            RobotPaused,
            /// <summary>
            /// 
            /// </summary>
            RobotResumed
        }

        /// <summary>
        /// 机械臂服务器工作模式
        /// </summary>
        [Serializable]
        public enum RobotWorkMode
        {
            /// <summary>
            /// 机械臂仿真模式
            /// </summary>
            RobotModeSimulator, //
            /// <summary>
            /// 机械臂真实模式
            /// </summary>
            RobotModeReal       //
        }


        /// <summary>
        /// 机械臂事件
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct RobotEventInfo
        {
            /// <summary>
            /// 事件类型号
            /// </summary>
            public int eventType; //
            /// <summary>
            /// 事件代码
            /// </summary>
            public int eventCode; //
            /// <summary>
            /// 事件内容(std::string)
            /// </summary>
            public IntPtr eventContent; //
        };

        /// <summary>
        /// 枚举描述
        /// </summary>
        public class EnumDescriptionAttribute : Attribute
        {
            /// <summary>
            /// 描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public EnumDescriptionAttribute(string description)
            {
                this.Description = description;
            }
        }

        /// <summary>
        /// 机械臂事件特性
        /// </summary>
        public class RobotEventAttribute : Attribute
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="description"></param>
            /// <param name="isWaring"></param>
            /// <param name="enDescription"></param>
            public RobotEventAttribute(string description, bool isWaring = false, string enDescription = "")
            {
                this.IsWaring = isWaring;
                this.ZhCnDescription = description;
                this.EnDescription = enDescription;
            }

            /// <summary>
            /// 英文释义
            /// </summary>
            public string EnDescription { get; set; }

            /// <summary>
            /// 是否警告
            /// </summary>
            public bool IsWaring { get; set; }

            /// <summary>
            /// 中文释义
            /// </summary>
            public string ZhCnDescription { get; set; }

        }


        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public enum LoginCode
        {
            /// <summary>
            /// 成功
            /// </summary>
            ErrnoSucc = 0,  /** 成功　**/

            /// <summary>
            /// 
            /// </summary>
            ErrCode_Base = 10000,
            /// <summary>
            /// 通用失败
            /// </summary>
            ErrCode_Failed,       /** 通用失败　**/
            /// <summary>
            /// 参数错误
            /// </summary>
            ErrCode_ParamError,   /** 参数错误　**/
            /// <summary>
            /// Socket连接失败
            /// </summary>
            ErrCode_ConnectSocketFailed,        /** Socket连接失败　**/
            /// <summary>
            /// Socket断开连接
            /// </summary>
            ErrCode_SocketDisconnect,           /** Socket断开连接　**/
            /// <summary>
            /// 创建请求失败
            /// </summary>
            ErrCode_CreateRequestFailed,        /** 创建请求失败　**/
            /// <summary>
            /// 请求相关的内部变量出错
            /// </summary>
            ErrCode_RequestRelatedVariableError,/** 请求相关的内部变量出错　**/
            /// <summary>
            /// 请求超时
            /// </summary>
            ErrCode_RequestTimeout,             /** 请求超时　**/
            /// <summary>
            /// 发送请求信息失败
            /// </summary>
            ErrCode_SendRequestFailed,          /** 发送请求信息失败　**/
            /// <summary>
            /// 响应信息为空
            /// </summary>
            ErrCode_ResponseInfoIsNULL,        /** 响应信息为空　**/
            /// <summary>
            /// 解析响应失败
            /// </summary>
            ErrCode_ResolveResponseFailed,     /** 解析响应失败　**/
            /// <summary>
            /// 正解出错
            /// </summary>
            ErrCode_FkFailed,                   /** 正解出错　**/
            /// <summary>
            /// 逆解出错
            /// </summary>
            ErrCode_IkFailed,                   /** 逆解出错　**/
            /// <summary>
            /// 工具标定参数有错
            /// </summary>
            ErrCode_ToolCalibrateError,              /** 工具标定参数有错**/
            /// <summary>
            /// 工具标定参数有错
            /// </summary>
            ErrCode_ToolCalibrateParamError,         /** 工具标定参数有错**/
            /// <summary>
            /// 坐标系标定失败
            /// </summary>
            ErrCode_CoordinateSystemCalibrateError,  /** 坐标系标定失败　**/
            /// <summary>
            /// 基坐标系转用户座标失败
            /// </summary>
            ErrCode_BaseToUserConvertFailed,         /** 基坐标系转用户座标失败　**/
            /// <summary>
            /// 用户坐标系转基座标失败
            /// </summary>
            ErrCode_UserToBaseConvertFailed,
            //move
            /// <summary>
            /// 运动相关的内部变量出错
            /// </summary>
            ErrCode_MotionRelatedVariableError,      /** 运动相关的内部变量出错　**/
            /// <summary>
            /// 运动请求失败
            /// </summary>
            ErrCode_MotionRequestFailed,             /** 运动请求失败**/
            /// <summary>
            /// 生成运动请求失败
            /// </summary>
            ErrCode_CreateMotionRequestFailed,       /** 生成运动请求失败**/
            /// <summary>
            /// 运动被事件中断
            /// </summary>
            ErrCode_MotionInterruptedByEvent,        /** 运动被事件中断　**/
            /// <summary>
            /// 运动相关的路点容器的长度不符合规定
            /// </summary>
            ErrCode_MotionWaypointVetorSizeError,    /** 运动相关的路点容器的长度不符合规定　**/
            /// <summary>
            /// 服务器响应返回错误
            /// </summary>
            ErrCode_ResponseReturnError,             /** 服务器响应返回错误　**/
            /// <summary>
            /// 真实机械臂不存在，因为有些接口只有在真是机械臂存在的情况下才可以被调用
            /// </summary>
            ErrCode_RealRobotNoExist,                /** 真实机械臂不存在，因为有些接口只有在真是机械臂存在的情况下才可以被调用　**/


            /// <summary>
            /// 
            /// </summary>
            ErrCode_Count = ErrCode_RealRobotNoExist - ErrCode_Base + 2,
        }

        /// <summary>
        /// 返回代码
        /// </summary>
        [Serializable]
        public enum RetunCode
        {
            /// <summary>
            /// 调用成功
            /// </summary>
            [EnumDescription("调用成功")]
            InterfaceCallSuccCode = 0,
            /// <summary>
            /// 通用失败
            /// </summary>
            [EnumDescription("通用失败")]
            ErrCode_Failed = 1001,
            /// <summary>
            /// 参数错误
            /// </summary>
            [EnumDescription("参数错误")]
            ErrCode_ParamError = 1002,
            /// <summary>
            /// Socket 连接失败
            /// </summary>
            [EnumDescription("Socket 连接失败")]
            ErrCode_ConnectSocketFailed = 10003,
            /// <summary>
            /// Socket 断开连接
            /// </summary>
            [EnumDescriptionAttribute("Socket 断开连接")]
            ErrCode_SocketDisconnect = 1004,
            /// <summary>
            /// 创建请求失败
            /// </summary>
            [EnumDescription("创建请求失败")]
            ErrCode_CreateRequestFailed = 1005,
            /// <summary>
            /// 请求相关的内部变量出错
            /// </summary>
            [EnumDescription("请求相关的内部变量出错")]
            ErrCode_RequestRelatedVariableError = 1006,
            /// <summary>
            /// 请求超时
            /// </summary>
            [EnumDescription("请求超时")]
            ErrCode_RequestTimeout = 1007,
            /// <summary>
            /// 发送请求信息失败
            /// </summary>
            [EnumDescription("发送请求信息失败")]
            ErrCode_SendRequestFailed = 1008,
            /// <summary>
            /// 响应信息为空
            /// </summary>
            [EnumDescription("响应信息为空")]
            ErrCode_ResponseInfoIsNULL = 1009,
            /// <summary>
            /// 解析响应失败
            /// </summary>
            [EnumDescription("解析响应失败")]
            ErrCode_ResolveResponseFailed = 1010,
            /// <summary>
            /// 正解出错
            /// </summary>
            [EnumDescription("正解出错")]
            ErrCode_FkFailed = 1011,
            /// <summary>
            /// 逆解出错
            /// </summary>
            [EnumDescription("逆解出错")]
            ErrCode_IkFailed = 1012,
            /// <summary>
            /// 工具标定参数有错
            /// </summary>
            [EnumDescription("工具标定参数有错")]
            ErrCode_ToolCalibrateError = 1013,
            /// <summary>
            /// 工具标定参数有错
            /// </summary>
            [EnumDescription("工具标定参数有错")]
            ErrCode_ToolCalibrateParamError = 1014,
            /// <summary>
            /// 坐标系标定失败
            /// </summary>
            [EnumDescription("坐标系标定失败")]
            ErrCode_CoordinateSystemCalibrateError = 1015,
            /// <summary>
            /// 基坐标系转用户座标失败
            /// </summary>
            [EnumDescription("基坐标系转用户座标失败")]
            ErrCode_BaseToUserConvertFailed = 1016,
            /// <summary>
            /// 用户坐标系转基座标失败
            /// </summary>
            [EnumDescription("用户坐标系转基座标失败")]
            ErrCode_UserToBaseConvertFailed = 1017,
            /// <summary>
            /// 运动相关的内部变量出错
            /// </summary>
            [EnumDescription("运动相关的内部变量出错")]
            ErrCode_MotionRelatedVariableError = 1018,
            /// <summary>
            /// 运动请求失败
            /// </summary>
            [EnumDescription("运动请求失败")]
            ErrCode_MotionRequestFailed = 1019,
            /// <summary>
            /// 生成运动请求失败
            /// </summary>
            [EnumDescription("生成运动请求失败")]
            ErrCode_CreateMotionRequestFailed = 1020,
            /// <summary>
            /// 运动被事件中断
            /// </summary>
            [EnumDescription("运动被事件中断")]
            ErrCode_MotionInterruptedByEvent = 1021,
            /// <summary>
            /// 运动相关的路点容器的长度不符合规定
            /// </summary>
            [EnumDescription("运动相关的路点容器的长度不符合规定")]
            ErrCode_MotionWaypointVetorSizeError = 1022,
            /// <summary>
            /// 服务器响应返回错误
            /// </summary>
            [EnumDescription("服务器响应返回错误")]
            ErrCode_ResponseReturnError = 1023,
            /// <summary>
            /// 真实机械臂不存在，因为有些接口只有在真是机械臂存在的情况下才可以被调用
            /// </summary>
            [EnumDescription("真实机械臂不存在，因为有些接口只有在真是机械臂存在的情况下才可以被调用")]
            ErrCode_RealRobotNoExist = 1024,
            /// <summary>
            /// 调用缓停接口失败
            /// </summary>
            [EnumDescription("调用缓停接口失败")]
            ErrCode_moveControlSlowStopFailed = 1025,
            /// <summary>
            /// 调用急停接口失败
            /// </summary>
            [EnumDescription("调用急停接口失败")]
            ErrCode_moveControlFastStopFailed = 1026,
            /// <summary>
            /// 调用暂停接口失败
            /// </summary>
            [EnumDescription("调用暂停接口失败")]
            ErrCode_moveControlPauseFailed = 1027,
            /// <summary>
            /// 调用继续接口失败
            /// </summary>
            [EnumDescription("调用继续接口失败")]
            ErrCode_moveControlContinueFailed = 1028
        }

        /// <summary>
        /// 获取返回信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetRetunDescription<T>(int code, Type enumType = null)
        {
            if (enumType == null)
                enumType = typeof(EnumDescriptionAttribute);
            RetunCode retunCode = (RetunCode)code;
            var re = retunCode.GetType().GetField(retunCode.ToString());
            if (re == null)
                return "未知错误";
            var datas = re.GetCustomAttributes(enumType, false); ;
            if (datas.Length == 0)
                return "未知错误";
            else
                return ((EnumDescriptionAttribute)datas[0]).Description;
        }

        /// <summary>
        /// 获取基坐标系
        /// </summary>
        /// <returns></returns>
        public static CoordCalibrate GetBaseCoordCalibrate()
        {
            return new CoordCalibrate()
            {
                coordType = 0,
                methods = 9,
                jointPara = default,
                toolDesc = new ToolInEndDesc()
                {
                    cartPos = default,
                    orientation = new Ori()
                    {
                        w = 1,
                        x = 0,
                        y = 0,
                        z = 0
                    }
                }
            };
        }

    }
}
