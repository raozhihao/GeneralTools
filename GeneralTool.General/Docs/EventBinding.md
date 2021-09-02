#### 事件扩展标记
使用说明:可以将事件重定向到ViewModel层中 <br>
示例代码 XAML 引用命名空间: <br>
xmlns:a="clr-namespace:GeneralTool.General.WPFHelper;assembly=GeneralTool.General"<br>
>
```
<Button x:Name="btn"
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