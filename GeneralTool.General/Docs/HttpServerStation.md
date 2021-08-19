# 使用方式

```
var httpServer = new HttpServerStation(null);
//如果想要直接处理Context则绑定下面的事件,绑定了此事件后 HandlerRequest将不起作用
//httpServer.HalderContext += HttpServer_HalderContext;
httpServer.HandlerRequest += HttpServer_HandlerRequest;
//如果以上两个事件都不绑定,则内部会使用默认的处理逻辑,在执行完相应的代码后,返回一个ServerResponse Json对象

  //示例事件处理代码
   private static void HttpServer_HandlerRequest(object sender, RequestInfo e)
   {
       var method = e.RequestRoute.MethodInfo;
       var parameters = method.GetParameters();
       var datas = new object[parameters.Length];
       int index = 0;
       foreach (var item in parameters)
       {
           var name = item.Name;
           var re = e.ServerRequest.Paramters.TryGetValue(name, out var value);
           if (re)
           {
               var obj = Convert.ChangeType(value, item.ParameterType);
               datas[index] = obj;
               index++;
           }
       }

       var result = method.Invoke(e.RequestRoute.Target, datas);
       var reStr=result.SerializeToJsonString();
       e.WriteResponse(reStr);
    }


var taskManager=new TaskManager(null, null, httpServer);

```

> 有关TaskManager使用,请查看 \Readme.md 
