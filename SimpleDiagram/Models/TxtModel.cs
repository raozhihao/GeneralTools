namespace SimpleDiagram.Models
{
    public class TxtModel : BaseData
    {
        private string txt = "";
        public string Txt
        {
            get => txt;
            set => RegisterProperty(ref txt, value);
        }

    }
}
