using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.WPFHelper;

namespace PropertyGridTest
{
    class PanelViewModel:BaseNotifyModel
    {
        private ObservableCollection<KeyValuePair<int,string>> infos = new ObservableCollection<KeyValuePair<int, string>>();
        public ObservableCollection<KeyValuePair<int, string>> Infos
        {
            get => this.infos;
            set => this.RegisterProperty(ref this.infos, value);
        }

        
    }
}
