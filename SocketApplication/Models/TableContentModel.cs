using GeneralTool.CoreLibrary.WPFHelper;

namespace SocketApplication.Models
{
    public class TableContentModel : BaseNotifyModel
    {
        private string header;
        private string content;

        public string Header
        {
            get => header;
            set => RegisterProperty(ref header, value);
        }
        public string Content
        {
            get=> content;
            set => RegisterProperty(ref content, value);
        }
    }
}
