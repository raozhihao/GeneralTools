using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace Clinet
{
    public class Test2
    {
        GeneralTool.General.TaskLib.SocketClient client;
        public void Test()
        {
            client = new GeneralTool.General.TaskLib.SocketClient();
            client.InitSocket("127.0.0.1", 8899);
            client.RecDataEvent += this.Server_RecDataEvent;
            client.IsAutoSize = true;
            client.IsReciverForAll = true;
            client.Connect();
            Thread.Sleep(1000);
            this.Send();
        }

        private void Server_RecDataEvent(GeneralTool.General.TaskLib.RecDataObject obj)
        {

        }

        public void Send()
        {
            var image = this.ImageToBase64(@"C:\Users\raozh\Pictures\Saved Pictures\carmera.bmp");
            var obj = new
            {
                ide = "aaa",
                max = 1,
                pic = image,
                ismast = true
            };

            var str = JsonConvert.SerializeObject(obj);
            this.client.Send(str);
        }

        /// <summary>
        /// Image 转成 base64
        /// </summary>
        /// <param name="fileFullName"></param>
        public string ImageToBase64(string fileFullName)
        {
            try
            {
                Bitmap bmp = new Bitmap(fileFullName);
                MemoryStream ms = new MemoryStream();
                var suffix = fileFullName.Substring(fileFullName.LastIndexOf('.') + 1,
                    fileFullName.Length - fileFullName.LastIndexOf('.') - 1).ToLower();
                var suffixName = suffix == "png"
                    ? ImageFormat.Png
                    : suffix == "jpg" || suffix == "jpeg"
                        ? ImageFormat.Jpeg
                        : suffix == "bmp"
                            ? ImageFormat.Bmp
                            : suffix == "gif"
                                ? ImageFormat.Gif
                                : ImageFormat.Jpeg;

                bmp.Save(ms, suffixName);
                byte[] arr = new byte[ms.Length]; ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length); ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
