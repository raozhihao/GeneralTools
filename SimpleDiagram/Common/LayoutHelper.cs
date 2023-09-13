using System.Linq;
using System.Windows.Controls;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

using SimpleDiagram.Blocks;

using SimpleDiagram.BlockVIewModels;

namespace SimpleDiagram.Common
{
    public class LayoutHelper
    {
        public ILog Log { get; set; }
        public LayoutHelper(ILog log = null)
        {
            if (log == null) log = new ConsoleLogInfo();
            Log = log;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="start"></param>
        public void LoadBlocks(UIElementCollection arg, BlockItem start)
        {
            System.Collections.Generic.IEnumerable<BlockItem> blocks = arg.OfType<BlockItem>();

            if (start == null)
                return;

            foreach (BlockItem item in blocks)
            {
                (item as BaseBlock).Log = Log;
                BaseBlockViewModel model = item.DataContext as BaseBlockViewModel;
                model.Log = Log;
                model.NextModel = GetSinkModel(item, arg);// item.SinkItems.FirstOrDefault()?.DataContext as BaseBlockViewModel;
                System.Collections.Generic.IEnumerable<BaseBlockViewModel> sinkModels = item.SinkItems.Select(s => s.DataContext as BaseBlockViewModel);
                model.SetSinkModels(sinkModels.ToList());
                model.NextModel?.SourceBlockModels.Add(model);

                item.SetProperty();

            }
        }

        private BaseBlockViewModel GetSinkModel(BlockItem source, UIElementCollection arg)
        {
            //获取当前块的下一个块
            System.Collections.Generic.List<BlockItem> sinkItems = source.SinkItems;
            System.Collections.Generic.IEnumerable<Connection> connections = arg.OfType<Connection>();
            foreach (BlockItem sink in sinkItems)
            {
                Connection first = connections.FirstOrDefault(b => b.SourceBlock == source && b.SinkBlock == sink);
                if (first == null)
                    continue;

                if (first.SourceThumb.Direction == Direction.Bottom)
                    return sink.DataContext as BaseBlockViewModel;
            }

            return null;
        }
    }
}
