using System.Linq;
using System.Windows.Controls;

using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.General.WPFHelper.DiagramDesigner.Models;

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
            this.Log = log;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="start"></param>
        public void LoadBlocks(UIElementCollection arg, BlockItem start)
        {
            var blocks = arg.OfType<BlockItem>();

            if (start == null)
                return;

            foreach (var item in blocks)
            {
                (item as BaseBlock).Log = this.Log;
                var model = item.DataContext as BaseBlockViewModel;
                model.Log = this.Log;
                model.NextModel = this.GetSinkModel(item, arg);// item.SinkItems.FirstOrDefault()?.DataContext as BaseBlockViewModel;
                var sinkModels = item.SinkItems.Select(s => s.DataContext as BaseBlockViewModel);
                model.SetSinkModels(sinkModels.ToList());
                model.NextModel?.SourceBlockModels.Add(model);

                item.SetProperty();

            }
        }

        private BaseBlockViewModel GetSinkModel(BlockItem source, UIElementCollection arg)
        {
            //获取当前块的下一个块
            var sinkItems = source.SinkItems;
            var connections = arg.OfType<Connection>();
            foreach (var sink in sinkItems)
            {
                var first = connections.FirstOrDefault(b => b.SourceBlock == source && b.SinkBlock == sink);
                if (first == null)
                    continue;

                if (first.SourceThumb.Direction == Direction.Bottom)
                    return sink.DataContext as BaseBlockViewModel;
            }

            return null;
        }
    }
}
