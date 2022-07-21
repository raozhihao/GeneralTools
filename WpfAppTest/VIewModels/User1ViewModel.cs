using System.Threading.Tasks;

namespace WpfAppTest.VIewModels
{
    public class User1ViewModel : GeneralTool.General.WPFHelper.BaseNotifyModel
    {
        private string te;
        public string Te { get => this.te; set => this.RegisterProperty(ref this.te, value); }

        public void ClickM(string ss)
        {

        }

        public async Task<string> ClickE(string ss)
        {
            await Task.Delay(1);
            return ss + "aaa";
        }


    }
}
