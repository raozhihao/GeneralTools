using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

using GeneralTool.General.Interfaces;

namespace PropertyGridTest
{
    public class MyExpandeUIEditor : IUIEditorConvert
    {
        public void ConvertTo(Grid gridParent, object instance, PropertyInfo propertyInfo, bool? sortAsc, ref int Row, string header = null)
        {
            gridParent.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });

        }
    }
}