using GeneralTool.General.Models;

namespace GeneralTool.General.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClientHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        ResponseCommand SendCommand(RequestCommand cmd);

        /// <summary>
        /// 
        /// </summary>
        void Close();

    }
}
