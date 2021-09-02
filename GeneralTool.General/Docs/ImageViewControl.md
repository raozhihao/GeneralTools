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