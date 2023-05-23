using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IClientHelper
    {
        #region Public 方法

        /// <summary>
        /// </summary>
        void Close();

        /// <summary>
        /// </summary>
        /// <param name="cmd">
        /// </param>
        /// <returns>
        /// </returns>
        ResponseCommand SendCommand(RequestCommand cmd);

        #endregion Public 方法
    }
}