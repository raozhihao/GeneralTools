using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.Attributes;
using GeneralTool.General.TaskLib;

namespace PropertyGridTest
{
    [Route(nameof(StationTask)+"/")]
    public class StationTask:BaseTaskInvoke
    {
        [Route(nameof(Test))]
        public string Test()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
