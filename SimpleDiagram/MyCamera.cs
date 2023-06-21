

using System.Drawing;

using GeneralTool.CoreLibrary.MVS;

namespace SimpleDiagram
{
    internal class MyCamera : MVSCamera
    {

        public void SetFrame(int frame)
        {
            var code = this.M_MyCamera.MV_CC_SetFrameRate_NET(frame);
            if (code == 0) return;

            var msg = ErrorCode.ErrorCodeInstance[code];//"0"
            ErrorCode.ErrorCodeInstance.IfErrorThrowExecption(code);//""
        }

        
    }
}
