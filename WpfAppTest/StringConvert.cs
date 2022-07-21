using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfAppTest
{
    public static class StringConvert
    {
        public static IValueConverter StringToObjectConverter => new GeneralTool.General.WPFHelper.Extensions.CoverterEx().ObjectToStringConverter;
    }
}
