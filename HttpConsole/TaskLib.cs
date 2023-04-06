using GeneralTool.General;
using GeneralTool.General.Attributes;
using GeneralTool.General.TaskLib;

namespace HttpConsole
{
    [Route(nameof(TaskLib) + "/")]
    public class TaskLib : BaseTaskInvoke
    {
        [Route(nameof(Test), Method = GeneralTool.General.NetHelper.HttpMethod.POST, ReReponse = true)]
        public string Test(string str, int count = 2)
        {
            return new
            {
                code = 1,
                message = "wq"
            }.SerializeToJsonString();
        }

        [Route(nameof(TT), ReReponse = true, ReReponseErroFomartString = "{\"code\":\"0\",\"data\":[\"erroMsg\":\"{0}\"]}")]
        public void TT()
        {
            throw new System.Exception("1112312");
        }

        [Route(nameof(Json), ReReponse = true)]
        public string Json([WaterMark("", IsJson = true)] ComplexTaskParams taskParams)
        {
            return new
            {
                code = 0,
                message = "试验框架调用",
                data = new
                {
                    order = taskParams.Order
                }
            }.SerializeToJsonString();
        }
    }

    /// <summary>
    /// 基础任务参数
    /// </summary>
    public class BasicTaskParams
    {
        public string Order { get; set; }
    }

    /// <summary>
    /// 带回调函数的任务参数
    /// </summary>
    public class CallbackTaskParams : BasicTaskParams
    {
        public CallbacksParams callbacks = new CallbacksParams();
        public CallbacksParams Callbacks { get => callbacks; set => callbacks = value; }
    }

    /// <summary>
    /// 带完整参数的任务参数
    /// </summary>
    public class ComplexTaskParams : CallbackTaskParams
    {
        private ScriptParams _params = new ScriptParams();
        public ScriptParams Params { get => _params; set => _params = value; }
    }

    /// <summary>
    /// 脚本参数
    /// </summary>
    public class ScriptParams
    {
        private string inputPath = "";
        public string InputPath { get { return inputPath; } set { inputPath = value; } }
        private string outputFolder = "";
        public string OutputFolder { get { return outputFolder; } set { outputFolder = value; } }
    }

    /// <summary>
    /// 回调参数
    /// </summary>
    public class CallbacksParams
    {
        private string upload = "";
        public string Upload { get => upload; set => upload = value; }

        private string finish = "";
        public string Finish { get => finish; set => finish = value; }
    }


}
