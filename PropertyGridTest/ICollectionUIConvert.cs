using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using GeneralTool.General.Interfaces;

namespace PropertyGridTest
{
    public class ICollectionUIConvert : IUIEditorConvert
    {
        public void ConvertTo(Grid gridParent, object instance, PropertyInfo propertyInfo, bool? sortAsc, ref int Row, string header = null)
        {
            
        }
    }
}
