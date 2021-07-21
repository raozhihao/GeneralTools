### GeneralTool.General 部分使用指南

#### TaskManager任务管理器
使用说明
> 1.创建任务类,继承自BaseTaskInvoke <br>
2.任务类打上Route标记,该标记指定类路径 <br>
3.对要开放的方法打上Route标记,指定方法路径 <br>
4.可选的属性WaterMark标记 <br>

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


#### 事件扩展标记
使用说明:可以将事件重定向到ViewModel层中 <br>
示例代码 XAML 引用命名空间: <br>
xmlns:a="clr-namespace:GeneralTool.General.WPFHelper;assembly=GeneralTool.General"<br>
>
```<Button x:Name="btn"
                Grid.Row="1"
                Content="Btn"
                MouseDown="{a:EventBinding MouseDownMethod,RoutedEvent={x:Static UIElement.MouseDownEvent}}" />
```
示例代码 ViewModel <br>
```
 public void MouseDownMethod(object sender, MouseButtonEventArgs eventArgs)
        {
            
        }
``` 

#### ImageViewControl 图片查看控件
使用说明:可以对加载的图片进行缩放,平移,在图片上添加右键菜单,截取区域等 <br>
示例代码 XAML 引用命名空间: <br>
xmlns:imgView="clr-namespace:GeneralTool.General.WPFHelper.WPFControls;assembly=GeneralTool.General" <br>
xmlns:a="clr-namespace:GeneralTool.General.WPFHelper;assembly=GeneralTool.General"<br>
```
 <imgView:ImageViewControl Grid.Row="1"
                                      Background="Yellow"
                                      ImageSource="{Binding ImageSource}"
                                      CanImageDraw="True"
                                      SliderStyle="{StaticResource MahApps.Styles.Slider.Flat}"
                                      ToolExpanderStyle="{StaticResource MahApps.Styles.Expander}"
                                      ToolCutButtonStyle="{StaticResource CutStyle}"
                                      MenuOkStyle="{StaticResource MenuOk}"
                                      MenuCancelStyle="{StaticResource MenuCancel}"
                                      CutImageDownEvent="{a:EventBinding CutImageOkMethod}"
                                      CutImageEvent="{a:EventBinding CutRectMethod}"
                                      ImageMouseMoveEvent="{a:EventBinding ImageMouseMove}"
                                      CutPanelVisibleChanged="{a:EventBinding CutPanelRectMethod}">

            </imgView:ImageViewControl>
```

#### PropertyGridControl 属性编辑器
使用说明:类Winform的PropertyGrid控件,目前只支持解析 字符串,数值,枚举类型,Brush类型也可以,但是纯字符串的 <br>
如果需要解析其它复杂类型,请修改源码 ObjectExpandeUIEditor 或在自定义类型上打上[UIEditor(typeof(ObjectExpandeUIEditor))]标记,此标记为最基础的解析类,会将打上此标记的类型中的所有属性解析出来
<br>
示例代码 XAML 引用命名空间: <br>
 xmlns:wpf="clr-namespace:GeneralTool.General.WPFHelper.WPFControls;assembly=GeneralTool.General"<br>
 ```
  <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="auto" />
        </Grid.RowDefinitions>
        <wpf:PropertyGridControl SelectedObject="{Binding ElementName=btn}" />
        <Button Content="Test"
                x:Name="btn" 
                Grid.Row="1"/>
    </Grid>
 ```



