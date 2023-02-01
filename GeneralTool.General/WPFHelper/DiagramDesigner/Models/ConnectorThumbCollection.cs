using System.Collections.Generic;
using System.Linq;

using GeneralTool.General.WPFHelper.DiagramDesigner.Thumbs;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectorThumbCollection : List<ConnectorThumb>
    {
        /// <summary>
        /// 
        /// </summary>
        public ConnectorThumb this[Direction direction]
        {
            get
            {
                return this.FirstOrDefault(s => s.Direction == direction);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnectorThumbCollection()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public ConnectorThumbCollection(IEnumerable<ConnectorThumb> connectors) : base(connectors)
        {

        }
    }
}
