using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public ConnectionArgs(Connection connection, ConnectionChangeType connectionChangeType)
        {
            Connection = connection;
            ConnectionChangeType = connectionChangeType;
        }

        /// <summary>
        /// 
        /// </summary>
        public Connection Connection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ConnectionChangeType ConnectionChangeType { get; set; }


    }

    /// <summary>
    /// 
    /// </summary>
    public enum ConnectionChangeType
    {
        /// <summary>
        /// 
        /// </summary>
        Add,
        /// <summary>
        /// 
        /// </summary>
        Remove,
    }
}
