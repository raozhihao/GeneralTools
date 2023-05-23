using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.TaskLib;

namespace ConsoleFram.TaskTests
{
    [Route(nameof(TestTask))]
    public class TestTask:BaseTaskInvoke
    {
        [Route(nameof(Test))]
        public string Test()
        {
            return Guid.NewGuid() + "";
        }
    }
}
