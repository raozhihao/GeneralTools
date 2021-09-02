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