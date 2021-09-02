### nuget
> Install-Package GeneralTool.General
> 
### GeneralTool.General 部分使用指南

#### TaskManager任务管理器
使用说明
> 1.创建任务类,继承自BaseTaskInvoke <br>
2.任务类打上Route标记,该标记指定类路径 <br>
3.对要开放的方法打上Route标记,指定方法路径 <br>
4.在方法上使用Route标记时,可选属性为 HttpMethod,该属性指示外部进行web调用时的方法<br>
  但只有当在实例化 TaskManager 中传入 HttpServerStation 对象或实现 ServerStationBase(自主处理)时才有用<br>
5.可选的属性WaterMark标记 <br>

任务类示例:
> 
	[Route(nameof(TestLib2) + "/", "测试2")]
    public class TestLib2 : BaseTaskInvoke
    {
        [Route(nameof(TestHello), "测试SayHello")]
        public string TestHello([WaterMark("名字")] string name, [WaterMark("年龄")] double age = 18)
        {
            return $"Hello {name} , your age is {age}";
        }

        [Route(nameof(ErrorLog), "错误日志测试")]
        public void ErrorLog()
        {
            try
            {
                this.Log.Error("Test Error");
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                //异常信息交给 this.erroMsg 后外部通过socket调用能够获取此信息
                this.erroMsg = ex.Message;
                this.Log.Error(this.erroMsg);
                throw;
            }
        }

        [Route(nameof(WaringLog), "警告日志测试")]
        public void WaringLog()
        {
            this.Log.Waring("警告测试");
        }
    }


> 5.使用 <br>
```
  		  //实例化任务对象
            var taskManager = new TaskManager();
            //开启Socket接口
            taskManager.Open("127.0.0.1", 8899, new TestLib2());
            //加载获取所有任务
            taskManager.GetInterfaces();
            //获取所有任务对象
            var taskModels = taskManager.TaskModels;
```




