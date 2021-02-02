using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.Models;

namespace GeneralTool.General.Interfaces
{
    public interface IClientHelper
    {
        ResponseCommand SendCommand(RequestCommand cmd);


        void Close();

    }
}
