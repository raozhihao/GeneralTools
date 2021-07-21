using GeneralTool.General.WPFHelper;
using GeneralTool.General.WPFHelper.Events;
using System;
using System.Windows;
using System.Windows.Input;

namespace WpfAppTest.VIewModels
{
    class MainViewModel : BaseNotifyModel
    {
        private string resultText = "AAA";
        public string ResultText { get => this.resultText; set => this.RegisterProperty(ref this.resultText, value); }

        private object code=1;
        public object Code { get => this.code; set => this.RegisterProperty(ref this.code, value); }


        public EventCommand<MouseEventArgs> MoveCommand { get; set; }
        public EventCommand<MouseButtonEventArgs> RightClickCommand { get; set; }
        public EventCommand<RoutedEventArgs> LoadCommand { get; set; }
        public EventCommand<MouseButtonEventArgs> TextClickCommand { get; set; }
        public EventCommand<MouseButtonEventArgs> ClickCommand { get; set; }


        private bool eventEnable = true;
        public bool EventEnable { get => this.eventEnable; set => this.RegisterProperty(ref this.eventEnable, value); }

        private object selectedObject;
        public object SelectedObject { get => this.selectedObject; set => this.RegisterProperty(ref this.selectedObject, value); }

        private bool sort = true;
        public bool Sort { get => this.sort; set => this.RegisterProperty(ref this.sort, value); }

        private string header = "自定义属性编辑器";
        public string Header { get => this.header; set => this.RegisterProperty(ref this.header, value); }

        public User1ViewModel User1ViewModel { get; set; }
        public void MouseDownMethod(object sender, MouseButtonEventArgs eventArgs)
        {
            this.SelectedObject = sender;
        }

        public void ClickM()
        {

        }

        public void CutPanelVisibleMethod(object sender, GeneralTool.General.Models.ImageCutRectEventArgs e)
        {

        }
        public MainViewModel()
        {
            this.MoveCommand = new EventCommand<MouseEventArgs>(DockPanelMouseMoveEventMethod);
            this.RightClickCommand = new EventCommand<MouseButtonEventArgs>(DockPanelMouseRightClickEventMethod);
            this.LoadCommand = new EventCommand<RoutedEventArgs>(WindowLoadedEventMethod);
            this.TextClickCommand = new EventCommand<MouseButtonEventArgs>(TextClickEventMethod);
            this.ClickCommand = new EventCommand<MouseButtonEventArgs>(ClickEventMethod);
            this.User1ViewModel = new User1ViewModel();
        }

        private void ClickEventMethod(object sender, MouseButtonEventArgs e)
        {
            this.ResultText = Guid.NewGuid().ToString();
        }

        private void TextClickEventMethod(object sender, MouseButtonEventArgs e)
        {
            this.TextClickCommand.RemoveEvent();
        }

        private void WindowLoadedEventMethod(object sender, RoutedEventArgs e)
        {
            (sender as Window).Title = "Window Loaded";
        }

        private void DockPanelMouseRightClickEventMethod(object sender, MouseButtonEventArgs e)
        {
            this.ResultText = $"{sender} | {e.RightButton} | args : {this.RightClickCommand.CommandParameter}";
        }

        private void DockPanelMouseMoveEventMethod(object sender, MouseEventArgs e)
        {

            this.ResultText = $"{sender} | {e.GetPosition((IInputElement)sender)} | args : {this.MoveCommand.CommandParameter}";
        }
    }
}
