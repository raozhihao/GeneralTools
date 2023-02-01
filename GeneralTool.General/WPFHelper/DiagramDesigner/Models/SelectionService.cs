using System.Collections.Generic;

using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectionService
    {
        /// <summary>
        /// 
        /// </summary>
        public List<BlockItem> SelectionBlockItems { get; private set; } = new List<BlockItem>();

        /// <summary>
        /// 
        /// </summary>
        public void AddItem(BlockItem item)
        {
            if (!this.SelectionBlockItems.Contains(item))
            {
                this.SelectionBlockItems.Add(item);
            }
            item.IsSelected = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveItem(BlockItem item)
        {
            item.IsSelected = false;
            if (this.SelectionBlockItems.Contains(item))
            {
                this.SelectionBlockItems.Remove(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() => this.SelectionBlockItems.Clear();
    }
}
