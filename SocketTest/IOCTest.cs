using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest
{
    internal class IOCTest
    {
        public void Test()
        {
            var ioc = GeneralTool.CoreLibrary.Ioc.SimpleIocSerivce.SimpleIocSerivceInstance;
            BaseCamera camera = new SubCamera();
            ioc.Inject(camera);
            ioc.Start();

            var c = ioc.Resolve<BaseCamera>();
        }
    }

    public class BaseCamera
    {

    }

    public class SubCamera : BaseCamera
    {

    }
}
