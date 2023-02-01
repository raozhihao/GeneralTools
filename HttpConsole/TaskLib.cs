using GeneralTool.General.Attributes;
using GeneralTool.General.TaskLib;

namespace HttpConsole
{
    [Route(nameof(TaskLib) + "/")]
    public class TaskLib : BaseTaskInvoke
    {
        [Route(nameof(Test), Method = GeneralTool.General.NetHelper.HttpMethod.POST)]
        public string Test(string str, int count)
        {
            return str;
        }

        [Route(nameof(TT))]
        public void TT()
        {

        }
    }
}
