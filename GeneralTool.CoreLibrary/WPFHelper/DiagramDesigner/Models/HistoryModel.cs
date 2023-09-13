using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models
{
    public class HistoryModel
    {
        public BlockItem Item { get; set; }

        public HistoryType HistoryType { get; set; }
    }

    public enum HistoryType
    {
        Add,
        Delete
    }
}
