using System;
using System.Runtime.InteropServices;

namespace GeneralTool.General.AuboSixAxisMechanicalArm
{
    partial class RobotAdepter
    {
        /// <summary>
        /// 机械臂控制上下文句柄
        /// </summary>
        public UInt16 Rshd = 0xffff;

        const string Robot_Library = "libserviceinterface.dll";

        /// <summary>
        /// 初始化机械臂控制库
        /// </summary>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_initialize", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int rs_initialize();

        /// <summary>
        /// 反初始化机械臂控制库
        /// </summary>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_uninitialize", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private static extern int rs_uninitialize();

        /// <summary>
        /// 创建机械臂控制上下文句柄
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_create_context", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_create_context(ref UInt16 rshd);

        /// <summary>
        /// 注销机械臂控制上下文句柄
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_destory_context", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_destory_context(UInt16 rshd);

        /// <summary>
        /// 注销机械臂控制上下文句柄
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="addr">机械臂服务器的IP地址</param>
        /// <param name="port">机械臂服务器的端口号</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_login", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_login(UInt16 rshd, [MarshalAs(UnmanagedType.LPStr)] string addr, int port);

        /// <summary>
        /// 断开机械臂服务器链接
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_logout", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_logout(UInt16 rshd);

        /// <summary>
        /// 初始化全局的运动属性
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_init_global_move_profile", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_init_global_move_profile(UInt16 rshd);

        /// <summary>
        /// 设置六个关节轴动的最大速度（最大为180度/秒），注意如果没有特殊需求，6个关节尽量配置成一样！
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_velc">六个关节的最大加速度，单位(rad/ss)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_global_joint_maxvelc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_global_joint_maxvelc(UInt16 rshd, double[] max_velc);

        /// <summary>
        /// 获取六个关节轴动的最大速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_velc">返回六个关节的最大加度单位(rad/s)(rad/s)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_global_joint_maxvelc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_global_joint_maxvelc(UInt16 rshd, ref MetaData.JointVelcAccParam max_velc);

        /// <summary>
        /// 设置六个关节轴动的最大加速度 （十倍的最大速度），注意如果没有特殊需求，6个关节尽量配置成一样！
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_acc">六个关节的最大加速度，单位(rad/ss)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_global_joint_maxacc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_global_joint_maxacc(UInt16 rshd, double[] max_acc);

        /// <summary>
        /// 获取六个关节轴动的最大加速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_acc">返回六个关节的最大加速度单位(rad/s^2)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_global_joint_maxacc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_global_joint_maxacc(UInt16 rshd, ref MetaData.JointVelcAccParam max_acc);

        /// <summary>
        /// 设置机械臂末端最大线加速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_acc">末端最大加线速度，单位(m/s^2)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_global_end_max_line_acc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_global_end_max_line_acc(UInt16 rshd, double max_acc);

        /// <summary>
        /// 设置机械臂末端最大线速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_velc">末端最大线速度，单位(m/s)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_global_end_max_line_velc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_global_end_max_line_velc(UInt16 rshd, double max_velc);

        /// <summary>
        /// 获取机械臂末端最大线加速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_acc">机械臂末端最大线加速度，单位(m/s^2)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_global_end_max_line_acc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_global_end_max_line_acc(UInt16 rshd, ref double max_acc);

        /// <summary>
        /// 获取机械臂末端最大线速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_velc">机械臂末端最大线速度，单位(m/s)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_global_end_max_line_velc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_global_end_max_line_velc(UInt16 rshd, ref double max_velc);

        /// <summary>
        /// 设置机械臂末端最大角加速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_acc">末端最大角加速度，单位(rad/s^2)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_global_end_max_angle_acc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_global_end_max_angle_acc(UInt16 rshd, double max_acc);

        /// <summary>
        /// 设置机械臂末端最大角速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_velc">末端最大速度，单位(rad/s)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_global_end_max_angle_velc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_global_end_max_angle_velc(UInt16 rshd, double max_velc);

        /// <summary>
        /// 获取机械臂末端最大角加速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_acc">机械臂末端最大角加速度，单位(m/s^2)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_global_end_max_angle_acc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_global_end_max_angle_acc(UInt16 rshd, ref double max_acc);

        /// <summary>
        /// 获取机械臂末端最大角加速度
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="max_velc">机械臂末端最大角速度，单位(m/s)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_global_end_max_angle_velc", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_global_end_max_angle_velc(UInt16 rshd, ref double max_velc);

        /// <summary>
        /// 设置用户坐标系
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="user_coord">用户坐标系</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_user_coord", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_user_coord(UInt16 rshd, ref MetaData.CoordCalibrate user_coord);

        /// <summary>
        /// 设置基座坐标系
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_base_coord", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_base_coord(UInt16 rshd);

        /// <summary>
        /// 机械臂轴动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="joint_radia">六个关节的关节角，单位(rad)</param>
        /// <param name="isblock">isblock==true  代表阻塞，机械臂运动直到到达目标位置或者出现故障后返回。
        /// isblock==false 代表非阻塞，立即返回，运动指令发送成功就返回，函数返回后机械臂开始运动。</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_joint", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_joint(UInt16 rshd, double[] joint_radia, bool isblock);

        /// <summary>
        /// 机械臂直线运动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="joint_radia">六个关节的关节角，单位(rad)</param>
        /// <param name="isblock">isblock==true  代表阻塞，机械臂运动直到到达目标位置或者出现故障后返回。
        ///             isblock==false 代表非阻塞，立即返回，运动指令发送成功就返回，函数返回后机械臂开始运动。</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_line", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_line(UInt16 rshd, double[] joint_radia, bool isblock);

        /// <summary>
        /// 保持当前位置变换姿态做旋转运动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="user_coord">用户坐标系</param>
        /// <param name="rotate_axis">转轴(x,y,z) 例如：(1,0,0)表示沿Y轴转动</param>
        /// <param name="rotate_angle">旋转角度 单位（rad）</param>
        /// <param name="isblock">isblock==true  代表阻塞，机械臂运动直到到达目标位置或者出现故障后返回。
        ///             isblock==false 代表非阻塞，立即返回，运动指令发送成功就返回，函数返回后机械臂开始运动。</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_rotate", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_rotate(UInt16 rshd, ref MetaData.CoordCalibrate user_coord, ref MetaData.MoveRotateAxis rotate_axis, double rotate_angle, bool isblock);

        /// <summary>
        /// 清除所有已经设置的全局路点
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_remove_all_waypoint", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_remove_all_waypoint(UInt16 rshd);

        /// <summary>
        /// 添加全局路点用于轨迹运动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="joint_radia">六个关节的关节角，单位(rad)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_add_waypoint", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_add_waypoint(UInt16 rshd, double[] joint_radia);

        /// <summary>
        /// 设置交融半径
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="radius">交融半径，单位(m)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_blend_radius", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_blend_radius(UInt16 rshd, double radius);

        /// <summary>
        /// 设置圆运动圈数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="times">当times大于0时，机械臂进行圆运动times次,当times等于0时，机械臂进行圆弧轨迹运动</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_circular_loop_times", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_circular_loop_times(UInt16 rshd, int times);

        /// <summary>
        /// 检查用户坐标系参数设置是否合理
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="user_coord">用户坐标系</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_check_user_coord", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_check_user_coord(UInt16 rshd, ref MetaData.CoordCalibrate user_coord);

        /// <summary>
        /// 设置基于基座标系运动偏移量
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="relative">相对位移(x, y, z) 单位(m)</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_relative_offset_on_base", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_relative_offset_on_base(UInt16 rshd, ref MetaData.MoveRelative relative);

        /// <summary>
        /// 设置基于用户标系运动偏移量
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="relative">相对位移(x, y, z) 单位(m)</param>
        /// <param name="user_coord">用户坐标系</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_relative_offset_on_user", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_relative_offset_on_user(UInt16 rshd, ref MetaData.MoveRelative relative, ref MetaData.CoordCalibrate user_coord);

        /// <summary>
        /// 取消提前到位设置
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_no_arrival_ahead", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_no_arrival_ahead(UInt16 rshd);

        /// <summary>
        /// 设置距离模式下的提前到位距离
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="distance">提前到位距离 单位（米）</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_arrival_ahead_distance", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_arrival_ahead_distance(UInt16 rshd, double distance);

        /// <summary>
        ///  设置时间模式下的提前到位时间
        /// </summary>

        [DllImport(Robot_Library, EntryPoint = "rs_set_arrival_ahead_time", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_arrival_ahead_time(UInt16 rshd, double sec);

        /// <summary>
        /// 轨迹运动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="sub_move_mode">轨迹类型:2:圆弧,3:轨迹</param>
        /// <param name="isblock">isblock==true  代表阻塞，机械臂运动直到到达目标位置或者出现故障后返回。isblock==false 代表非阻塞，立即返回，运动指令发送成功就返回，函数返回后机械臂开始运动。</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_track", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_track(UInt16 rshd, int sub_move_mode, bool isblock);

        /// <summary>
        /// 保持当前位姿通过直线运动的方式运动到目标位置
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="target">基于用户平面表示的目标位置</param>
        /// <param name="tool">工具参数</param>
        /// <param name="isblock">isblock==true  代表阻塞，机械臂运动直到到达目标位置或者出现故障后返回。isblock==false 代表非阻塞，立即返回，运动指令发送成功就返回，函数返回后机械臂开始运动。</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_line_to", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_line_to(UInt16 rshd, ref MetaData.Pos target, ref MetaData.ToolInEndDesc tool, bool isblock);

        /// <summary>
        /// 保持当前位姿通过关节运动的方式运动到目标位置
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="target">基于用户平面表示的目标位置</param>
        /// <param name="tool">工具参数</param>
        /// <param name="isblock">isblock==true  代表阻塞，机械臂运动直到到达目标位置或者出现故障后返回。isblock==false 代表非阻塞，立即返回，运动指令发送成功就返回，函数返回后机械臂开始运动。</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_joint_to", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_joint_to(UInt16 rshd, ref MetaData.Pos target, ref MetaData.ToolInEndDesc tool, bool isblock);

        /// <summary>
        /// 设置示教坐标系
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="user_coord">示教坐标系</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_teach_coord", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_teach_coord(UInt16 rshd, ref MetaData.CoordCalibrate user_coord);

        /// <summary>
        /// 开始轴动示教 mode 示教关节:JOINT1~6,   位置示教:MOV_X,MOV_Y,MOV_Z   姿态示教:ROT_X,ROT_Y,ROT_Z  dir 运动方向 正方向true 反方向false
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="teach_mode">示教关节:JOINT1,JOINT2,JOINT3, JOINT4,JOINT5,JOINT6,   位置示教:MOV_X,MOV_Y,MOV_Z   姿态示教:ROT_X,ROT_Y,ROT_Z</param>
        /// <param name="dir">运动方向   正方向true  反方向false</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_teach_move_start", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_teach_move_start(UInt16 rshd, MetaData.TeachMode teach_mode, bool dir);

        /// <summary>
        /// 结束示教
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_teach_move_stop", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_teach_move_stop(UInt16 rshd);

        /// <summary>
        /// 获取机械臂当前位置信息
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="waypoint">关节位置信息</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_current_waypoint", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_current_waypoint(UInt16 rshd, ref MetaData.WayPoint_S waypoint);

        /// <summary>
        /// 获取机械臂诊断信息
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="info">机械臂诊断信息</param>
        /// <returns>成功 其他失败</returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_diagnosis_info", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_diagnosis_info(UInt16 rshd, ref MetaData.RobotDiagnosis info);

        /// <summary>
        /// 正解,此函数为正解函数，已知关节角求对应位置的位置和姿态。
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="joint_radia">六个关节的关节角，单位(rad)</param>
        /// <param name="waypoint">六个关节角,位置,姿态</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_forward_kin", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_forward_kin(UInt16 rshd, double[] joint_radia, ref MetaData.WayPoint_S waypoint);

        /// <summary>
        /// 逆解 此函数为机械臂逆解函数，根据位置信息(x,y,z)和对应位置的参考姿态(w,x,y,z)得到对应位置的关节角信息。
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="joint_radia">参考关节角（通常为当前机械臂位置）单位(rad)</param>
        /// <param name="pos">目标路点的位置 单位:米</param>
        /// <param name="ori">目标路点的参考姿态</param>
        /// <param name="waypoint">目标路点信息</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_inverse_kin", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_inverse_kin(UInt16 rshd, double[] joint_radia, ref MetaData.Pos pos, ref MetaData.Ori ori, ref MetaData.WayPoint_S waypoint);

        /// <summary>
        /// 欧拉角转四元素
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="rpy">姿态的欧拉角表示方法</param>
        /// <param name="ori">姿态的四元素表示方法</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_rpy_to_quaternion", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_rpy_to_quaternion(UInt16 rshd, ref MetaData.Rpy rpy, ref MetaData.Ori ori);

        /// <summary>
        /// 四元素转欧拉角
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="ori">姿态的四元素表示方法</param>
        /// <param name="rpy">姿态的欧拉角表示方法</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_quaternion_to_rpy", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_quaternion_to_rpy(UInt16 rshd, ref MetaData.Ori ori, ref MetaData.Rpy rpy);

        /// <summary>
        /// 基座坐标系转用户坐标系
        /// 概述:  将法兰盘中心基于基座标系下的位置和姿态　转成　工具末端基于用户座标系下的位置和姿态。
        /// 扩展1:  法兰盘中心可以看成是一个特殊的工具，即工具的位置为(0,0,0) 因此当工具为(0,0,0)时，相当于将法兰盘中心基于基座标系下的位置和姿态　转成　法兰盘中心基于用户座标系下的位置和姿态。
        /// 扩展2:  用户坐标系也可以选择成基座标系，　　即：userCoord.coordType = BaseCoordinate
        ///  因此当用户平面为基座标系时，相当于将法兰盘中心基于基座标系下的位置和姿态　转成　工具末端基于基座标系下的位置和姿态，即在基座标系加工具。
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="pos_onbase">基于基座标系的法兰盘中心位置信息（x,y,z）  单位(m)</param>
        /// <param name="ori_onbase">基于基座标系的姿态信息(w, x, y, z)</param>
        /// <param name="user_coord">用户坐标系</param>
        /// <param name="tool_pos">工具信息</param>
        /// <param name="pos_onuser">基于用户座标系的工具末端位置信息,输出参数</param>
        /// <param name="ori_onuser">基于用户座标系的工具末端姿态信息,输出参数</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_base_to_user", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_base_to_user(UInt16 rshd, ref MetaData.Pos pos_onbase, ref MetaData.Ori ori_onbase, ref MetaData.CoordCalibrate user_coord, ref MetaData.ToolInEndDesc tool_pos, ref MetaData.Pos pos_onuser, ref MetaData.Ori ori_onuser);

        /// <summary>
        /// 用户坐标系转基座坐标系
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="pos_onuser">基于用户座标系的工具末端位置信息</param>
        /// <param name="ori_onuser">基于用户座标系的工具末端姿态信息</param>
        /// <param name="user_coord">用户坐标系</param>
        /// <param name="tool_pos">工具信息</param>
        /// <param name="pos_onbase">基于基座标系的法兰盘中心位置信息</param>
        /// <param name="ori_onbase">基于基座标系的姿态信息</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_user_to_base", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_user_to_base(UInt16 rshd, ref MetaData.Pos pos_onuser, ref MetaData.Ori ori_onuser, ref MetaData.CoordCalibrate user_coord, ref MetaData.ToolInEndDesc tool_pos, ref MetaData.Pos pos_onbase, ref MetaData.Ori ori_onbase);

        /// <summary>
        /// 基坐标系转基座标得到工具末端点的位置和姿态
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="flange_center_pos_onbase">基于基座标系的工具末端位置信息</param>
        /// <param name="flange_center_ori_onbase">基于基座标系的工具末端姿态信息</param>
        /// <param name="tool_pos">工具信息</param>
        /// <param name="tool_end_pos_onbase">基于基座标系的工具末端位置信息</param>
        /// <param name="tool_end_ori_onbase">基于基座标系的工具末端姿态信息</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_base_to_base_additional_tool", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_base_to_base_additional_tool(UInt16 rshd, ref MetaData.Pos flange_center_pos_onbase, ref MetaData.Ori flange_center_ori_onbase, ref MetaData.ToolInEndDesc tool_pos, ref MetaData.Pos tool_end_pos_onbase, ref MetaData.Ori tool_end_ori_onbase);

        /// <summary>
        /// 设置工具的运动学参数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="tool">工具的运动学参数</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_tool_end_param", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_tool_end_param(UInt16 rshd, ref MetaData.ToolInEndDesc tool);

        /// <summary>
        /// 设置无工具的动力学参数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_none_tool_dynamics_param", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_none_tool_dynamics_param(UInt16 rshd);

        /// <summary>
        /// 根据接口板IO类型和地址设置IO状态
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="io_type">IO类型</param>
        /// <param name="addr">IO状态</param>
        /// <param name="val">IO状态</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_board_io_status_by_addr", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_board_io_status_by_addr(UInt16 rshd, MetaData.RobotIoType io_type, int addr, double val);

        /// <summary>
        /// 根据接口板IO类型和地址获取IO状态
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="io_type">IO类型</param>
        /// <param name="addr">IO地址</param>
        /// <param name="val"></param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_board_io_status_by_addr", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_board_io_status_by_addr(UInt16 rshd, MetaData.RobotIoType io_type, int addr, ref double val);

        /// <summary>
        /// 设置工具端IO状态
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="name">IO名称</param>
        /// <param name="status">工具端IO状态</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_tool_do_status", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_tool_do_status(UInt16 rshd, string name, MetaData.IO_STATUS status);

        /// <summary>
        /// 获取工具端IO状态
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="name">IO名称</param>
        /// <param name="val">工具端IO状态</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_tool_io_status", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_tool_io_status(UInt16 rshd, string name, ref double val);

        /// <summary>
        /// 设置工具端电源电压类型
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="type">ower_type:电源类型 0:.OUT_0V 1:.OUT_12V 2:.OUT_24V</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_tool_power_type", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_tool_power_type(UInt16 rshd, MetaData.ToolPowerType type);

        /// <summary>
        /// 获取工具端电源电压类型
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="type">电源类型</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_tool_power_type", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_tool_power_type(UInt16 rshd, ref MetaData.ToolPowerType type);

        /// <summary>
        /// 设置工具端数字量IO的类型
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="addr">地址</param>
        /// <param name="type">类型  0:输入 1:输出</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_tool_io_type", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_tool_io_type(UInt16 rshd, MetaData.ToolDigitalIOAddr addr, MetaData.ToolPowerType type);

        /// <summary>
        /// 设置工具的动力学参数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="tool">工具的动力学参数</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_tool_dynamics_param", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_tool_dynamics_param(UInt16 rshd, ref MetaData.ToolDynamicsParam tool);

        /// <summary>
        /// 获取工具的动力学参数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="tool">工具的动力学参数</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_tool_dynamics_param", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_tool_dynamics_param(UInt16 rshd, ref MetaData.ToolDynamicsParam tool);

        /// <summary>
        /// 设置无工具运动学参数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_none_tool_kinematics_param", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_none_tool_kinematics_param(UInt16 rshd);

        /// <summary>
        /// 设置工具的运动学参数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="tool">工具的动力学参数</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_tool_kinematics_param", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_tool_kinematics_param(UInt16 rshd, ref MetaData.ToolInEndDesc tool);

        /// <summary>
        /// 获取工具的运动学参数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="tool">工具的动力学参数</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_tool_kinematics_param", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_tool_kinematics_param(UInt16 rshd, ref MetaData.ToolInEndDesc tool);

        /// <summary>
        /// 启动机械臂
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="tool">动力学参数</param>
        /// <param name="colli_class">碰撞等级</param>
        /// <param name="read_pos">是否允许读取位置</param>
        /// <param name="static_colli_detect">是否允许侦测静态碰撞</param>
        /// <param name="board_maxacc">接口板允许的最大加速度</param>
        /// <param name="state">机械臂启动状态</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_robot_startup", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_robot_startup(UInt16 rshd, ref MetaData.ToolDynamicsParam tool, byte colli_class, bool read_pos, bool static_colli_detect, int board_maxacc, ref MetaData.ROBOT_SERVICE_STATE state);

        /// <summary>
        /// 关闭机械臂
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_robot_shutdown", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_robot_shutdown(UInt16 rshd);

        /// <summary>
        /// 停止机械臂运动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_stop", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_stop(UInt16 rshd);

        /// <summary>
        /// 停止机械臂运动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_fast_stop", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_fast_stop(UInt16 rshd);

        /// <summary>
        /// 暂停机械臂运动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_pause", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_pause(UInt16 rshd);

        /// <summary>
        /// 暂停后回复机械臂运动
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_move_continue", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_move_continue(UInt16 rshd);

        /// <summary>
        /// 机械臂碰撞后恢复
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_collision_recover", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_collision_recover(UInt16 rshd);

        /// <summary>
        /// 获取机械臂当前状态
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="state">机械臂当前状态</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_robot_state", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_robot_state(UInt16 rshd, ref MetaData.RobotState state);

        /// <summary>
        /// 设置机械臂服务器工作模式
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="state">机械臂服务器工作模式</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_work_mode", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_work_mode(UInt16 rshd, MetaData.RobotWorkMode state);

        /// <summary>
        /// 获取机械臂服务器当前工作模式
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="state">机械臂服务器工作模式</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_work_mode", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_work_mode(UInt16 rshd, ref MetaData.RobotWorkMode state);

        /// <summary>
        /// 设置机械臂碰撞等级
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="grade">碰撞等级 范围（0～10）</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_set_collision_class", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_set_collision_class(UInt16 rshd, int grade);

        /// <summary>
        /// 获取socket链接状态
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="connected">connected true：已连接 false：未连接</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_get_socket_status", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_get_socket_status(UInt16 rshd, ref byte connected);

        /// <summary>
        /// 设置是否允许实时路点信息推送
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="enable">true表示允许 false表示不允许</param>
        /// <returns></returns>
        [DllImport(Robot_Library, EntryPoint = "rs_enable_push_realtime_roadpoint", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_enable_push_realtime_roadpoint(UInt16 rshd, bool enable);

        /// <summary>
        /// 实时路点回调函数
        /// </summary>
        /// <param name="waypoint"></param>
        /// <param name="arg"></param>
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate void REALTIME_ROADPOINT_CALLBACK(ref MetaData.WayPoint_S waypoint, IntPtr arg);

        /// <summary>
        /// 实时关节状态回调函数
        /// </summary>
        /// <param name="jointStatus"></param>
        /// <param name="size"></param>
        /// <param name="arg"></param>
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate void REALTIME_JOINT_STATUS_CALLBACK(ref MetaData.JointStatus jointStatus, int size, IntPtr arg);

        /// <summary>
        /// 设置是否允许实时关节状态推送
        /// </summary>
        /// <param name="rshd"></param>
        /// <param name="enable">true表示允许 false表示不允许</param>
        /// <returns></returns>

        [DllImport(Robot_Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_enable_push_realtime_joint_status(UInt16 rshd, bool enable);


        /// <summary>
        /// 注册用于获取实时关节状态回调函数
        /// </summary>
        /// <param name="rshd"></param>
        /// <param name="realTimeJointStatusCallback"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        [DllImport(Robot_Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rs_setcallback_realtime_joint_status(UInt16 rshd, [MarshalAs(UnmanagedType.FunctionPtr)] REALTIME_JOINT_STATUS_CALLBACK realTimeJointStatusCallback, IntPtr arg);

        /// <summary>
        /// 注册用于获取实时路点的回调函数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="CurrentPositionCallback">获取实时路点信息的函数委托</param>
        /// <param name="arg">这个参数系统不做任何处理，只是进行了缓存，当回调函数触发时该参数会通过回调函数的参数传回</param>
        [DllImport(Robot_Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rs_setcallback_realtime_roadpoint(UInt16 rshd, [MarshalAs(UnmanagedType.FunctionPtr)] REALTIME_ROADPOINT_CALLBACK CurrentPositionCallback, IntPtr arg);

        /// <summary>
        /// 机械臂事件
        /// </summary>
        /// <param name="rs_event"></param>
        /// <param name="arg"></param>
        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate void ROBOT_EVENT_CALLBACK(ref MetaData.RobotEventInfo rs_event, IntPtr arg);

        /// <summary>
        /// 注册用于获取机械臂事件信息的回调函数
        /// </summary>
        /// <param name="rshd">械臂控制上下文句柄</param>
        /// <param name="RobotEventCallback">获取机械臂事件信息的函数</param>
        /// <param name="arg">个参数系统不做任何处理，只是进行了缓存，当回调函数触发时该参数会通过回调函数的参数传回</param>
        [DllImport(Robot_Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern void rs_setcallback_robot_event(UInt16 rshd, [MarshalAs(UnmanagedType.FunctionPtr)] ROBOT_EVENT_CALLBACK RobotEventCallback, IntPtr arg);
    }
}
