using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.IniHelpers;

namespace HttpConsole.Inis
{
    public class WindowNode : Category
    {
        public WindowNode(string sectionName) : base(sectionName)
        {
            this.Str = new Node<string>(sectionName, nameof(this.Str), "aa", true,"Configs\\aa.ini");
            this.Level = new Node<int>(sectionName, nameof(this.Level), 1, true, "Configs\\bb.ini");
        }

        public Node<string> Str { get; set; }

        public Node<int> Level { get; set; }
    }
}
