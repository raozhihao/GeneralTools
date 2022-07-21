using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.Attributes;
using GeneralTool.General.Models;
using GeneralTool.General.TaskLib;
using GeneralTool.General.WPFHelper;

namespace WpfAppTest.VIewModels
{
    public class PropertyViewModel : BaseNotifyModel
    {
        public ObservableCollection<object> TaskItems { get; set; }

        public PropertyViewModel()
        {
            this.TaskItems = new ObservableCollection<object>();
            var manager = new TaskManager();
            var re = manager.OpenWithoutServer(new TestTask());
            var dic = manager.GetInterfaces();
            foreach (var item in manager.TaskModels)
            {
                foreach (var model in item.DoTaskModels)
                {
                    foreach (var p in model.DoTaskParameterItem.Paramters)
                    {
                        this.TaskItems.Add(p);
                    }
                }
            }
        }
    }

    [Route("TestTask/")]
    public class TestTask : BaseTaskInvoke
    {
        [Route(nameof(TestString))]
        public void TestString([WaterMark("String1")] string test)
        {
            Console.WriteLine(test);
        }
    }
}
