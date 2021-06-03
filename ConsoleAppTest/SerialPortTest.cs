namespace ConsoleAppTest
{
    public class SerialPortTest
    {
        public void Test()
        {
            var serial = new GeneralTool.General.SerialPortEx.SerialControl()
            {
                PortName = "COM4",
                BaudRate = 9600,
                Parity = System.IO.Ports.Parity.None,
                StopBits = System.IO.Ports.StopBits.One,
                DataBits = 8,
                ReadTimeout = 3000,
                WriteTimeout = 3000,
                Head = 0xee,
                End = 0xff
            };
            serial.Open();
            var request = serial.CreateRequest();
            //填入关键字和数据集合
            request.SetData(0x00, new byte[] { });
            //发送数据并返回
            var reponse = serial.Send(request);
            //获取返回数据
            var data = reponse.UserDatas;
        }
    }
}
