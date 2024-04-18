using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.IniHelpers;

namespace SocketTest.IniCommon
{
    public class WindowNode : Category
    {
        public WindowNode(string sectionName) : base(sectionName)
        {
            this.Width = new Node<int>(sectionName, nameof(this.Width), 1, true);
        }

        public Node<int> Width { get; set; }
        public Node<int> Height { get; set; }
    }
}
