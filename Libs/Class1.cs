using System;
using System.Runtime.InteropServices;

namespace Libs
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

            CoordTypeCount
        }

        /// <summary>
        /// 示教接口运动模式
        /// </summary>
        [Serializable]
        public enum TeachMode
        {
            NO_TEACH = 0,
            JOINT1, //轴动示教
            JOINT2,
            JOINT3,
            JOINT4,
            JOINT5,
            JOINT6,
            MOV_X, //位置示教
            MOV_Y,
            MOV_Z,
            ROT_X, //姿态示教
            ROT_Y,
            ROT_Z,
        };

        /// <summary>
        /// 路点位置信息的表示方法
        /// </summary>
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct Pos
        {
            public double x;
            public double y;
            public double z;
        }

        /// <summary>
        /// 路点位置信息的表示方法
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct cartesianPos_U
        {
            // 指定数组尺寸
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public double[] positionVector;
        };


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
            public double w;
            public double x;
            public double y;
            public double z;
        };

        /// <summary>
        /// 姿态的欧拉角表示方法
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct Rpy
        {
            public double rx;
            public double ry;
            public double rz;
        };

        /// <summary>
        /// 描述机械臂的路点信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [Serializable]
        public struct wayPoint_S
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

        [Serializable]
        public enum CoordType
        {
            BaseCoordinate = 0,
            EndCoordinate,
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
            public double xx;
            public double xy;
            public double xz;
            public double yy;
            public double yz;
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
            [EnumDescription("机械臂CAN总线错误")]
            RobotEvent_armCanbusError,           //

            /// <summary>
            /// 远程关机
            /// </summary>
            [EnumDescription("远程关机")]
            RobotEvent_remoteHalt,               //　　　　TODO

            /// <summary>
            /// 机械臂远程急停
            /// </summary>
            [EnumDescription("机械臂远程急停")]
            RobotEvent_remoteEmergencyStop,      //

            /// <summary>
            /// 关节错误
            /// </summary>
            [EnumDescription("关节错误")]
            RobotEvent_jointError,               //

            /// <summary>
            /// 力控制
            /// </summary>
            [EnumDescription("力控制")]
            RobotEvent_forceControl,             //

            /// <summary>
            /// 退出力控制
            /// </summary>
            [EnumDescription("退出力控制")]
            RobotEvent_exitForceControl,         //

            /// <summary>
            /// 软急停
            /// </summary>
            [EnumDescription("软急停")]
            RobotEvent_softEmergency,            //

            /// <summary>
            /// 退出软急停
            /// </summary>
            [EnumDescription("退出软急停")]
            RobotEvent_exitSoftEmergency,        //

            /// <summary>
            /// 碰撞
            /// </summary>
            [EnumDescription("碰撞")]
            RobotEvent_collision,                //

            /// <summary>
            /// 碰撞状态改变
            /// </summary>
            [EnumDescription("碰撞状态改变")]
            RobotEvent_collisionStatusChanged,   //

            /// <summary>
            /// 工具动力学参数设置成功
            /// </summary>
            [EnumDescription("工具动力学参数设置成功")]
            RobotEvent_tcpParametersSucc,        //

            /// <summary>
            /// 机械臂电源开关状态改变
            /// </summary>
            [EnumDescription("机械臂电源开关状态改变")]
            RobotEvent_powerChanged,             //

            /// <summary>
            /// 机械臂电源关闭
            /// </summary>
            [EnumDescription("机械臂电源关闭")]
            RobotEvent_ArmPowerOff,              //

            /// <summary>
            /// 安装位置发生改变
            /// </summary>
            [EnumDescription("安装位置发生改变")]
            RobotEvent_mountingPoseChanged,      //

            /// <summary>
            /// 编码器错误
            /// </summary>
            [EnumDescription("编码器错误")]
            RobotEvent_encoderError,             //

            /// <summary>
            /// 编码器线数不一致
            /// </summary>
            [EnumDescription("编码器线数不一致")]
            RobotEvent_encoderLinesError,        //

            /// <summary>
            /// 奇异点超速
            /// </summary>
            [EnumDescription("奇异点超速")]
            RobotEvent_singularityOverspeed,     //

            /// <summary>
            /// 机械臂电流异常
            /// </summary>
            [EnumDescription("机械臂电流异常")]
            RobotEvent_currentAlarm,             //

            /// <summary>
            /// 机械臂工具端错误
            /// </summary>
            [EnumDescription("机械臂工具端错误")]
            RobotEvent_toolioError,             //

            /// <summary>
            /// 机械臂启动阶段
            /// </summary>
            [EnumDescription("机械臂启动阶段")]
            RobotEvent_robotStartupPhase,       //

            /// <summary>
            /// 机械臂启动完成结果
            /// </summary>
            [EnumDescription("机械臂启动完成结果")]
            RobotEvent_robotStartupDoneResult,  //

            /// <summary>
            /// 机械臂关机结果
            /// </summary>
            [EnumDescription("机械臂关机结果")]
            RobotEvent_robotShutdownDone,       //

            /// <summary>
            /// 机械臂轨迹运动到位信号通知
            /// </summary>
            [EnumDescription("机械臂轨迹运动到位信号通知")]
            RobotEvent_atTrackTargetPos,        //

            /// <summary>
            /// 设置电源状态完成
            /// </summary>
            [EnumDescription("设置电源状态完成")]
            RobotSetPowerOnDone,                //

            /// <summary>
            /// 机械臂刹车释放完成
            /// </summary>
            [EnumDescription("机械臂刹车释放完成")]
            RobotReleaseBrakeDone,              //

            /// <summary>
            /// 机械臂控制状态改变
            /// </summary>
            [EnumDescription("机械臂控制状态改变")]
            RobotEvent_robotControllerStateChaned,  //

            /// <summary>
            /// 机械臂控制错误----一般是算法规划出现问题时返回
            /// </summary>
            [EnumDescription("机械臂控制错误----一般是算法规划出现问题时返回")]
            RobotEvent_robotControllerError,        //

            /// <summary>
            /// socket断开连接
            /// </summary>
            [EnumDescription("socket断开连接")]
            RobotEvent_socketDisconnected,          //

            /// <summary>
            /// RobotEvent_robotControlException
            /// </summary>
            [EnumDescription("RobotEvent_robotControlException")]
            RobotEvent_robotControlException,

            /// <summary>
            /// RobotEvent_trackPlayInterrupte
            /// </summary>
            [EnumDescription("RobotEvent_trackPlayInterrupte")]
            RobotEvent_trackPlayInterrupte,

            /// <summary>
            /// RobotEvent_staticCollisionStatusChanged
            /// </summary>
            [EnumDescription("RobotEvent_staticCollisionStatusChanged")]
            RobotEvent_staticCollisionStatusChanged,

            /// <summary>
            /// RobotEvent_MountingPoseWarning
            /// </summary>
            [EnumDescription("RobotEvent_MountingPoseWarning")]
            RobotEvent_MountingPoseWarning,

            /// <summary>
            /// RobotEvent_MacDataInterruptWarning
            /// </summary>
            [EnumDescription("RobotEvent_MacDataInterruptWarning")]
            RobotEvent_MacDataInterruptWarning,

            /// <summary>
            /// RobotEvent_ToolIoError
            /// </summary>
            [EnumDescription("RobotEvent_ToolIoError")]
            RobotEvent_ToolIoError,

            /// <summary>
            /// RobotEvent_InterfacBoardSafeIoEvent
            /// </summary>
            [EnumDescription("RobotEvent_InterfacBoardSafeIoEvent")]
            RobotEvent_InterfacBoardSafeIoEvent,

            /// <summary>
            /// RobotEvent_RobotHandShakeSucc
            /// </summary>
            [EnumDescription("RobotEvent_RobotHandShakeSucc")]
            RobotEvent_RobotHandShakeSucc,

            /// <summary>
            /// RobotEvent_RobotHandShakeFailed
            /// </summary>
            [EnumDescription("RobotEvent_RobotHandShakeFailed")]
            RobotEvent_RobotHandShakeFailed,

            /// <summary>
            /// RobotEvent_RobotErrorInfoNotify
            /// </summary>
            [EnumDescription("RobotEvent_RobotErrorInfoNotify")]
            RobotEvent_RobotErrorInfoNotify,

            /// <summary>
            /// RobotEvent_exceptEvent
            /// </summary>
            [EnumDescription("RobotEvent_exceptEvent")]
            RobotEvent_exceptEvent = 100,

            //unknown event
            /// <summary>
            /// robot_event_unknown
            /// </summary>
            [EnumDescription("robot_event_unknown")]
            robot_event_unknown,

            //user event
            /// <summary>
            /// RobotEvent_User
            /// </summary>
            [EnumDescription("RobotEvent_User")]
            RobotEvent_User = 1000,                            // first user event id

            /// <summary>
            /// RobotEvent_MaxUser
            /// </summary>
            [EnumDescription("RobotEvent_MaxUser")]
            RobotEvent_MaxUser = 65535                         // last user event id
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
            ROBOT_SERVICE_READY = 0,
            ROBOT_SERVICE_STARTING,
            ROBOT_SERVICE_WORKING,
            ROBOT_SERVICE_CLOSING,
            ROBOT_SERVICE_CLOSED,
            ROBOT_SETVICE_FAULT_POWER,
            ROBOT_SETVICE_FAULT_BRAKE,
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
            OUT_0V = 0,
            OUT_12V = 1,
            OUT_24V = 2
        }

        /// <summary>
        /// 地址
        /// </summary>
        [Serializable]
        public enum ToolDigitalIOAddr
        {
            TOOL_DIGITAL_IO_0 = 0,
            TOOL_DIGITAL_IO_1 = 1,
            TOOL_DIGITAL_IO_2 = 2,
            TOOL_DIGITAL_IO_3 = 3
        }

        /// <summary>
        /// 机械臂状态枚举
        /// </summary>
        [Serializable]
        public enum RobotState
        {
            RobotStopped = 0,
            RobotRunning,
            RobotPaused,
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

        public class EnumDescriptionAttribute : Attribute
        {
            public string Description { get; set; }
            public EnumDescriptionAttribute(string description)
            {
                this.Description = description;
            }
        }

        [Serializable]
        public enum LoginCode
        {
            ErrnoSucc = 0,  /** 成功　**/

            ErrCode_Base = 10000,
            ErrCode_Failed,       /** 通用失败　**/
            ErrCode_ParamError,   /** 参数错误　**/
            ErrCode_ConnectSocketFailed,        /** Socket连接失败　**/
            ErrCode_SocketDisconnect,           /** Socket断开连接　**/
            ErrCode_CreateRequestFailed,        /** 创建请求失败　**/
            ErrCode_RequestRelatedVariableError,/** 请求相关的内部变量出错　**/
            ErrCode_RequestTimeout,             /** 请求超时　**/
            ErrCode_SendRequestFailed,          /** 发送请求信息失败　**/
            ErrCode_ResponseInfoIsNULL,        /** 响应信息为空　**/
            ErrCode_ResolveResponseFailed,     /** 解析响应失败　**/
            ErrCode_FkFailed,                   /** 正解出错　**/
            ErrCode_IkFailed,                   /** 逆解出错　**/
            ErrCode_ToolCalibrateError,              /** 工具标定参数有错**/
            ErrCode_ToolCalibrateParamError,         /** 工具标定参数有错**/
            ErrCode_CoordinateSystemCalibrateError,  /** 坐标系标定失败　**/
            ErrCode_BaseToUserConvertFailed,         /** 基坐标系转用户座标失败　**/
            ErrCode_UserToBaseConvertFailed,         /** 用户坐标系转基座标失败　**/

            //move
            ErrCode_MotionRelatedVariableError,      /** 运动相关的内部变量出错　**/
            ErrCode_MotionRequestFailed,             /** 运动请求失败**/
            ErrCode_CreateMotionRequestFailed,       /** 生成运动请求失败**/
            ErrCode_MotionInterruptedByEvent,        /** 运动被事件中断　**/
            ErrCode_MotionWaypointVetorSizeError,    /** 运动相关的路点容器的长度不符合规定　**/
            ErrCode_ResponseReturnError,             /** 服务器响应返回错误　**/
            ErrCode_RealRobotNoExist,                /** 真实机械臂不存在，因为有些接口只有在真是机械臂存在的情况下才可以被调用　**/


            ErrCode_Count = ErrCode_RealRobotNoExist - ErrCode_Base + 2,
        }

        [Serializable]
        public enum RetunCode
        {
            [EnumDescription("调用成功")]
            InterfaceCallSuccCode = 0,

            [EnumDescription("通用失败")]
            ErrCode_Failed = 1001,
            [EnumDescription("参数错误")]
            ErrCode_ParamError = 1002,
            [EnumDescription("Socket 连接失败")]
            ErrCode_ConnectSocketFailed = 10003,
            [EnumDescriptionAttribute("Socket 断开连接")]
            ErrCode_SocketDisconnect = 1004,
            [EnumDescription("创建请求失败")]
            ErrCode_CreateRequestFailed = 1005,
            [EnumDescription("请求相关的内部变量出错")]
            ErrCode_RequestRelatedVariableError = 1006,
            [EnumDescription("请求超时")]
            ErrCode_RequestTimeout = 1007,
            [EnumDescription("发送请求信息失败")]
            ErrCode_SendRequestFailed = 1008,
            [EnumDescription("响应信息为空")]
            ErrCode_ResponseInfoIsNULL = 1009,
            [EnumDescription("解析响应失败")]
            ErrCode_ResolveResponseFailed = 1010,
            [EnumDescription("正解出错")]
            ErrCode_FkFailed = 1011,
            [EnumDescription("逆解出错")]
            ErrCode_IkFailed = 1012,
            [EnumDescription("工具标定参数有错")]
            ErrCode_ToolCalibrateError = 1013,
            [EnumDescription("工具标定参数有错")]
            ErrCode_ToolCalibrateParamError = 1014,
            [EnumDescription("坐标系标定失败")]
            ErrCode_CoordinateSystemCalibrateError = 1015,
            [EnumDescription("基坐标系转用户座标失败")]
            ErrCode_BaseToUserConvertFailed = 1016,
            [EnumDescription("用户坐标系转基座标失败")]
            ErrCode_UserToBaseConvertFailed = 1017,
            [EnumDescription("运动相关的内部变量出错")]
            ErrCode_MotionRelatedVariableError = 1018,
            [EnumDescription("运动请求失败")]
            ErrCode_MotionRequestFailed = 1019,
            [EnumDescription("生成运动请求失败")]
            ErrCode_CreateMotionRequestFailed = 1020,
            [EnumDescription("运动被事件中断")]
            ErrCode_MotionInterruptedByEvent = 1021,
            [EnumDescription("运动相关的路点容器的长度不符合规定")]
            ErrCode_MotionWaypointVetorSizeError = 1022,
            [EnumDescription("服务器响应返回错误")]
            ErrCode_ResponseReturnError = 1023,
            [EnumDescription("真实机械臂不存在，因为有些接口只有在真是机械臂存在的情况下才可以被调用")]
            ErrCode_RealRobotNoExist = 1024,
            [EnumDescription("调用缓停接口失败")]
            ErrCode_moveControlSlowStopFailed = 1025,
            [EnumDescription("调用急停接口失败")]
            ErrCode_moveControlFastStopFailed = 1026,
            [EnumDescription("调用暂停接口失败")]
            ErrCode_moveControlPauseFailed = 1027,
            [EnumDescription("调用继续接口失败")]
            ErrCode_moveControlContinueFailed = 1028
        }

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

        public static CoordCalibrate GetBaseCoordCalibrate()
        {
            return new CoordCalibrate()
            {
                coordType = 0,
                methods = 9,
                jointPara = default,
                toolDesc = default
            };
        }

        /// <summary>
        /// 获取枚举上的指定名称
        /// </summary>
        /// <typeparam name="OutType">输出枚举特性属性的类型</typeparam>
        /// <typeparam name="EnumAttributeType">要查找的枚举类型</typeparam>
        /// <param name="enum">枚举对象</param>
        /// <param name="property">输出枚举特性属性名称</param>
        /// <returns></returns>
        public static OutType GetEnumCustomAttributeInfo<OutType, EnumAttributeType>(Enum @enum, string property)
        {

            var type = @enum.GetType();
            var field = type.GetField(@enum.ToString());
            if (field == null)
                return default;

            var datas = field.GetCustomAttributes(typeof(EnumAttributeType), false);
            if (datas.Length == 0)
                return default;

            var en = (EnumAttributeType)datas[0];
            var p = en.GetType().GetProperty(property);
            if (p == null)
                return default;

            return (OutType)p.GetValue(en);

        }


    }
}
