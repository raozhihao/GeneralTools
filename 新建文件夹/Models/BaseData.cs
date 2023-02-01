using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.WPFHelper;

namespace SimpleDiagram.Models
{
    public class BaseData : BaseNotifyModel
    {
        public string ScriptId { get; set; }

        public string BlockId { get; set; }
    }
}
