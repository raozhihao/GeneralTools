using System.Collections.Generic;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models
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
            if (!SelectionBlockItems.Contains(item))
            {
                SelectionBlockItems.Add(item);
            }
            item.IsSelected = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveItem(BlockItem item)
        {
            item.IsSelected = false;
            if (SelectionBlockItems.Contains(item))
            {
                _ = SelectionBlockItems.Remove(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() => SelectionBlockItems.Clear();
    }
}
