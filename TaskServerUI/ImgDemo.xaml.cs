using System.Windows;
using System.Windows.Input;

namespace TaskServerUI
{
    /// <summary>
    /// ImgDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ImgDemo : Window
    {
        public ImgDemo()
        {
            InitializeComponent();

            this.ImgControl.PreviewMouseMove += this.ImgControl_PreviewMouseMove;
        }

        private void ImgControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            this.RectInfoTxt.Text = this.ImgControl.GetChooseRect().ToString();
        }

        private void ImgControl_ImageMouseMoveEvent(GeneralTool.CoreLibrary.Models.ImageMouseEventArgs obj)
        {
            
        }
    }
}
