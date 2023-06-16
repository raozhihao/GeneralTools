using System;

namespace GeneralTool.CoreLibrary.AuboSixAxisMechanicalArm
{
    /// <summary>
    /// 全局通用配置类
    /// </summary>
    public class Util
    {
        /// <summary>
        /// 
        /// </summary>
        public const int RSERR_SUCC = 0;
        /// <summary>
        /// 关节个数
        /// </summary>
        public const int ARM_DOF = 6;
        /// <summary>
        /// M_PI
        /// </summary>
        public const double M_PI = Math.PI;

        //接口板用户DI地址
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_F1 = 30;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_F2 = 31;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_F3 = 32;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_F4 = 33;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_F5 = 34;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_F6 = 35;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_00 = 36;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_01 = 37;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_02 = 38;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_03 = 39;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_04 = 40;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_05 = 41;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_06 = 42;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_07 = 43;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_10 = 44;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_11 = 45;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_12 = 46;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_13 = 47;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_14 = 48;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_15 = 49;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_16 = 50;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DI_17 = 51;
        /// <summary>
        /// 
        /// </summary>

        //接口板用户DO地址
        public const int ROBOT_IO_U_DO_00 = 32;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_01 = 33;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_02 = 34;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_03 = 35;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_04 = 36;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_05 = 37;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_06 = 38;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_07 = 39;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_10 = 40;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_11 = 41;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_12 = 42;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_13 = 43;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_14 = 44;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_15 = 45;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_16 = 46;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_U_DO_17 = 47;
        /// <summary>
        /// 
        /// </summary>

        //接口板用户AI地址
        public const int ROBOT_IO_VI0 = 0;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_VI1 = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_VI2 = 2;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_VI3 = 3;
        /// <summary>
        /// 
        /// </summary>

        //接口板用户AO地址
        public const int ROBOT_IO_VO0 = 0;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_VO1 = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_CO0 = 2;
        /// <summary>
        /// 
        /// </summary>
        public const int ROBOT_IO_CO1 = 3;

        //接口板IO类型
        //
        /// <summary>
        /// 接口板用户DI(数字量输入)　可读可写
        /// </summary>
        public const int Robot_User_DI = 4;
        /// <summary>
        /// 接口板用户DO(数字量输出)  可读可写
        /// </summary>
        public const int Robot_User_DO = 5;
        /// <summary>
        /// 接口板用户AI(模拟量输入)  可读可写
        /// </summary>
        public const int Robot_User_AI = 6;
        /// <summary>
        /// 接口板用户AO(模拟量输出)  可读可写
        /// </summary>
        public const int Robot_User_AO = 7;

        //工具端IO类型
        //
        /// <summary>
        /// 工具端DI
        /// </summary>
        public const int Robot_Tool_DI = 8;
        /// <summary>
        /// 工具端DO
        /// </summary>
        public const int Robot_Tool_DO = 9;
        /// <summary>
        /// 工具端AI
        /// </summary>
        public const int Robot_Tool_AI = 10;
        /// <summary>
        /// 工具端AO
        /// </summary>
        public const int Robot_Tool_AO = 11;
        /// <summary>
        /// 工具端DI
        /// </summary>
        public const int Robot_ToolIoType_DI = Robot_Tool_DI;
        /// <summary>
        /// 工具端DO
        /// </summary>
        public const int Robot_ToolIoType_DO = Robot_Tool_DO;

        /// <summary>
        /// 工具端IO名称
        /// </summary>
        public const string TOOL_IO_0 = ("T_DI/O_00");
        /// <summary>
        /// 
        /// </summary>
        public const string TOOL_IO_1 = ("T_DI/O_01");
        /// <summary>
        /// 
        /// </summary>
        public const string TOOL_IO_2 = ("T_DI/O_02");
        /// <summary>
        /// 
        /// </summary>
        public const string TOOL_IO_3 = ("T_DI/O_03");

        //
        /// <summary>
        /// 工具端数字IO输入
        /// </summary>
        public const int TOOL_IO_IN = 0;
        /// <summary>
        /// 工具端数字IO输出
        /// </summary>
        public const int TOOL_IO_OUT = 0;

        //工具端电源类型
        /// <summary>
        /// 
        /// </summary>
        public const int OUT_0V = 0;

        /// <summary>
        /// 
        /// </summary>
        public const int OUT_12V = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int OUT_24V = 2;

        /// <summary>
        /// IO状态-无效
        /// </summary>
        public const double IO_STATUS_INVALID = 0.0;
        /// <summary>
        /// IO状态-有效
        /// </summary>
        public const double IO_STATUS_VALID = 1.0;

        /// <summary>
        /// 坐标系枚举
        /// </summary>
        public const int BaseCoordinate = 0;
        /// <summary>
        /// 
        /// </summary>
        public const int EndCoordinate = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int WorldCoordinate = 2;
        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 坐标系标定方法
        /// </summary>
        public static string[] CoordMethodName = { "xOy", "yOz", "zOx", "xOxy", "xOxz", "yOyz", "yOyx", "zOzx", "zOzy" };

        //运动轨迹类型
        //
        /// <summary>
        /// 运动轨迹类型圆
        /// </summary>
        public const int ARC_CIR = 2;
        /// <summary>
        /// 运动轨迹类型圆弧
        /// </summary>
        public const int CARTESIAN_MOVEP = 3;

        /// <summary>
        /// 机械臂状态
        /// </summary>
        public const int RobotStopped = 0;
        /// <summary>
        /// 
        /// </summary>
        public const int RobotRunning = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int RobotPaused = 2;
        /// <summary>
        /// 
        /// </summary>
        public const int RobotResumed = 3;

        //机械臂工作模式
        /// <summary>
        /// 机械臂仿真模式
        /// </summary>
        public const int RobotModeSimulator = 0; //
        /// <summary>
        /// 机械臂真实模式
        /// </summary>
        public const int RobotModeReal = 1; //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static int GetCoordMothodByName(string methodName)
        {
            int method = -1;

            for (int i = 0; i < CoordMethodName.Length; i++)
            {
                if (methodName == CoordMethodName[i])
                {
                    method = i;
                    break;
                }
            }
            return method;
        }

    }
}
