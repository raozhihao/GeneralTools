using GeneralTool.General.Attributes;
using System;

namespace Libs
{
    public class MethodClass : IMethodClass
    {
        public bool Link(string ip, int port)
        {
            Console.WriteLine(nameof(Link));
            return true;
        }

        [Route(nameof(MoveRotate), "进行姿态移动")]
        public void MoveRotate([WaterMark("要移动的轴(1,0,0 表示RX轴)")] string axis = "1,0,0", [WaterMark("要移动的度数(deg)")] double rotateStepValue = 5)
        {
            Console.WriteLine(nameof(MoveRotate));
        }
    }
}
