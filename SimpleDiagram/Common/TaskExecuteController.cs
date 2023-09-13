using System;
using System.Diagnostics;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.Models;

using SimpleDiagram.BlockVIewModels;

namespace SimpleDiagram.Common
{
    public class TaskExecuteController
    {
        public ILog Log { get; set; }

        public TaskExecuteController(ILog log)
        {
            if (log == null)
            {
                log = new ConsoleLogInfo();
            }
            Log = log;
        }

        /// <summary>
        /// 启动执行
        /// </summary>
        /// <param name="current">当前开始块</param>
        /// <param name="scriptName">脚本名称</param>
        /// <param name="token">停止标识</param>
        /// <param name="isDebug">是否调试标志</param>
        /// <returns></returns>
        public Task<TaskResultInfo> Start(BaseBlockViewModel current, string scriptName, ExcuteCancelTokenSource token, bool isDebug = false)
        {

            Task<TaskResultInfo> reResult = Task.Run(async () =>
            {
                TaskResultInfo resultInfo = new TaskResultInfo()
                {
                    IsSuccess = true,
                    ErroMsg = "系统指定 - 成功"
                };

                Log.Info($"{scriptName} : 开始执行");
                if (current == null)
                {
                    Log.Error($"{scriptName} : 没有执行块,退出");
                    token.Canceld = true;
                    resultInfo.ErroMsg = "没有执行块而退出";
                    return resultInfo;
                }

                Stopwatch watch = new Stopwatch();

                watch.Start();
                bool result = await current.Execute(null, token);
                Log.Info($"执行块 - [{current.Description}] ,耗时 - [{watch.ElapsedMilliseconds}] ms");
                watch.Stop();
                if (isDebug) current.UnBreakBlock();

                //当前需要执行的块,即是当前的块的下一个
                BaseBlockViewModel executeModel = current.NextModel;
                while (result && executeModel != null)
                {
                    if (token.IsCancelNotify)
                    {
                        Log.Info($"{scriptName} : 退出执行");
                        resultInfo.IsSuccess = false;
                        resultInfo.UserEnd = null;
                        resultInfo.ErroMsg = "因停止而退出执行";
                        break;
                    }

                    if (executeModel.IsBreakPoint && isDebug)
                    {
                        Log.Info($"找到断点 -> [{executeModel.Description}]");
                        //到达下一个断点了
                        executeModel.SetBreakBlock();
                        resultInfo.IsBreak = true;
                        break;
                    }

                    Log.Info($"正在执行测试块:{executeModel.Description}");
                    if (isDebug) executeModel.SetBreakBlock();
                    else executeModel.Selected();

                    try
                    {
                        watch.Restart();
                        //执行当前块
                        result = await executeModel.Execute(current, token);
                        watch.Stop();
                        Log.Info($"执行块 - [{executeModel.Description}] ,耗时 - [{watch.ElapsedMilliseconds}] ms");

                        resultInfo.ErroMsg = "执行块指定";
                        if (!result)
                        {
                            Log.Error($"在执行块 - [{executeModel.Description} 时返回 false]");
                            resultInfo.IsSuccess = result;
                            break;
                        }

                        if (isDebug)
                            executeModel?.UnBreakBlock();
                        else
                            executeModel?.UnSelected();

                        //将当前设置为下一个块
                        current = current.NextModel;
                        if (current == null)
                            break;
                        //将要执行的块设置为当前的下一个块
                        executeModel = current.NextModel;
                    }
                    catch (Exception ex)
                    {
                        if (isDebug)
                            executeModel.UnBreakBlock();
                        else
                            executeModel.UnSelected();

                        if (token.IsCancelNotify && ex is AppDomainUnloadedException)
                        {
                            Log.Error("取消测试");
                            resultInfo.IsSuccess = false;
                            resultInfo.ErroMsg = "取消测试";
                            break;
                        }

                        resultInfo.ErroMsg = $"执行块:{executeModel.Description} 出现异常:{ex.GetInnerExceptionMessage()}";
                        Log.Error(resultInfo.ErroMsg);
                        resultInfo.IsSuccess = false;

                        break;

                    }
                    finally
                    {

                    }

                    if (watch.ElapsedMilliseconds <= 1)
                        await Task.Delay(1);

                    if (current == null)
                    {
                        Log.Waring($"{scriptName} : 没有执行块,退出执行");
                        break;
                    }

                }

                token.Canceld = true;
                if (resultInfo.IsBreak)
                    Log.Waring("暂停执行");
                else
                    Log.Info($"{scriptName} : 结束执行 - {resultInfo}");

                return resultInfo;
            });
            //ControllerManager.Controller.RaiseNotOperationDeviceImage(true);
            return reResult;
        }

    }

    /// <summary>
    /// 任务执行结果
    /// </summary>
    public struct TaskResultInfo
    {
        /// <summary>
        /// 任务是否无异常的正确结束了
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 任务是否由用户指定的结束,为null时则表示无用户指定 (如果 IsSuccess 失败,则此项无意义)
        /// </summary>
        public bool? UserEnd { get; set; }

        /// <summary>
        /// 用于调试时指定是否断点结束 
        /// </summary>
        public bool IsBreak { get; set; }

        /// <summary>
        /// 用于输出错误消息
        /// </summary>
        public string ErroMsg { get; set; }

        public override string ToString()
        {
            return $"执行结果 -> {IsSuccess} ,用户指定 -> {UserEnd} ,消息 -> {ErroMsg}";
        }
    }
}
