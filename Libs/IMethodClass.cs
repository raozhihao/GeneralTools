using GeneralTool.General.Attributes;

namespace Libs
{
    public interface IMethodClass
    {
        bool Link(string ip, int port);
        void MoveRotate([WaterMark("要移动的轴(1,0,0 表示RX轴)")] string axis = "1,0,0", [WaterMark("要移动的度数(deg)")] double rotateStepValue = 5);
    }
}