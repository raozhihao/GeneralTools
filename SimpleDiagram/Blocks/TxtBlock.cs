using SimpleDiagram.BlockVIewModels;
using SimpleDiagram.Models;
using SimpleDiagram.Windows;

namespace SimpleDiagram.Blocks
{
    public class TxtBlock : TrueFalseBaseBlock
    {
        public override BaseBlockViewModel BlockViewModel { get; set; } = new TxtBlockViewModel();

        public override WindowResult OpenWindow()
        {
            if (!(BlockViewModel is TxtBlockViewModel t))
            {
                return WindowResult.False;
            }
            TxtWindow window = new TxtWindow(t.Txt?.Txt)
            {
                Owner = MainWindow
            };

            bool? re = window.ShowDialog();
            if (re.Value)
            {

                {
                    t.Txt = new TxtModel()
                    {
                        Txt = window.ResultTxt,
                        BlockId = t.BlockId,
                        ScriptId = LayoutId
                    };
                }
                SetContent();
            }
            return GetResult(re);
        }

        private void SetContent()
        {
            if (BlockViewModel is TxtBlockViewModel t)
            {
                SetShowText(t.Txt.Txt);
            }
        }

        protected override void OnDispose()
        {

        }

        public override void SetShow()
        {
            SetContent();
        }
    }
}
