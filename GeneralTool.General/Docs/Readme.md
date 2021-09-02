> nuget地址   Install-Package GeneralTool.General

##### 支持功能
1.AdbHelper <br>
 > 命名空间:GeneralTool.General.Adb <br>
 > 可使用此类访问adb功能,发送命令格式,例如 adb shell getprop ro.product.brand 

2.AttributeExtensions <br>
> 命名空间 GeneralTool.General.Attributes
> 扩展方法,可获取任意类型上的自定义特性

3.RobotAdepter <br>
> 命名空间: GeneralTool.General.AuboSixAxisMechanicalArm
> 本类主要是用于 敖博六轴机械臂 的控制库

4.DataTableExtensions <br>
> 命名空间: GeneralTool.General.DataSetExtensions
> 本类主要用于 DataTable 的各类扩展方法

5.DbManager <br>
> 命名空间: GeneralTool.General.DbHelper
> 本类主要用于 通用底层的数据库访问类,不用自己管理数据库的连接,其中为短连接

6.EnumExtension <br>
> 命名空间: GeneralTool.General.Enums
> 本类为枚举的扩展类,用于获取指定枚举类型上的特性

7.ExceptionExtensions <br>
> 命名空间: GeneralTool.General.ExceptionHelper
> 本类为异常的扩展类,可以获取所有内部的异常消息

8.IniHelper <br>
> 命名空间: GeneralTool.General.IniHelpers
> 本类为读写Ini配置文件的帮助类,可配合Node类型进行使用

9.SimpleIocSerivce <br>
> 命名空间: GeneralTool.General.Ioc
> 本类为IOC容器,使用方式与autofac等差不多,但不同于autofac的是
> 后者对于构造函数的调用时,属性仍未注入,但前者在调用构造函数时,属性已经成功注入
> 前者的逻辑为先初始化属性后调用构造函数,与后者刚好相反
> 所以在有继承关系的类型注入时,可能会达不到预期

10.PathExtensions <br>
> 命名空间: GeneralTool.General.IOExtensions
> 本类为string的扩展方法,主要是将多个字符串组合成一个新的路径

11.IPCClientHelper 与 IPCServerHelper <br>
> 命名空间: GeneralTool.General.IPCHelper
> 用于IPC管道通信

12.ArraryExtensions, IEnumerableExtensions, StringExtensions <br>
> 命名空间: GeneralTool.General.LinqExtensions
> 主要是三类扩展方法

13.ConsoleLogInfo, FileInfoLog <br>
> 命名空间: GeneralTool.General.Logs
> 前者只在调试窗口打印日志,后者会记录到本地文件中

14.HttpHelper <br>
> 命名空间: GeneralTool.General.NetHelper
> 发送Http Get Post命令并获取返回

15.ProcessEngine, ProcessHelper <br>
> 命名空间: GeneralTool.General.ProcessHelpers
> 前者为长连接的Process任务,但只支持子进程一对一的返回,后者为短连接的Process任务

16.SerialControl <br>
> 命名空间: GeneralTool.General.SerialPortEx
> SerialPort的扩展类

17.TaskManager <br>
> 命名空间: GeneralTool.General.TaskLib
> 具体参阅 TaskManager.md

18.ByteExtensions, EnumExtensions, Int32Extensions, StringExtensions <br>
> 命名空间: GeneralTool.General.ValueTypeExtensions
> 各种类型的扩展方法

19.PageHelper, QueryHelpers <br>
> 命名空间: GeneralTool.General.WebExtensioins
> 用于http中计算分页和参数的扩展方法

20.WaitDialogHelper <br>
> 命名空间: GeneralTool.General.WinForm
> 用于在Winform中加载等待框

21.WPFHelper <br>
> 在GeneralTool.General.WPFHelper 有各种关于WPF的扩展类及帮助类,转换类等

22.ObjectPool <br>
> 命名空间: GeneralTool.General
> 简单管理的对象池类

22.RandomEx <br>
> 命名空间: GeneralTool.General
> 随机数获取扩展类

23.SerializeExtensions <br>
> 命名空间: GeneralTool.General
> 对象的序列化及反序列化扩展

24.SerializeHelpers <br>
> 命名空间: GeneralTool.General
> 将.net对象序列化成二进制或反序列化,只支持.net对象之间的序列化(不用额外添加Serialize特性)

25.WindowManager <br>
> 命名空间: GeneralTool.General
> 可以管理Winform/Wpf所有打开窗体的状态,及获取/关闭最后一个活跃窗体









 