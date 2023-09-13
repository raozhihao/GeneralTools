using GeneralTool.CoreLibrary.MVS;

namespace SimpleDiagram
{
    internal class MyCamera : MVSCamera
    {

        public void SetFrame(int frame)
        {
            int code = M_MyCamera.MV_CC_SetFrameRate_NET(frame);
            if (code == 0) return;
            _ = ErrorCode.ErrorCodeInstance[code];//"0"
            ErrorCode.ErrorCodeInstance.IfErrorThrowExecption(code);//""
        }

    }
}
