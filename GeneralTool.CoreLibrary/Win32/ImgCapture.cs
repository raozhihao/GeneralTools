using System;
using System.Drawing;

namespace GeneralTool.CoreLibrary.Win32
{
    public static class ImgCapture
    {
        private static readonly IntPtr _desktopWindow = IntPtr.Zero;
        private static IntPtr WindowDeviceContext;
        private static IntPtr CompatibleDeviceContext;
        private static IntPtr CompatibleBitmap;
        private static IntPtr _oldBitmap;
        private static Size size;
        private static CopyPixelOperation PixelOperations;
        private static bool inited;
        static ImgCapture()
        {

        }

        public static Bitmap CaptureScreen()
        {

            if (!inited)
                Init();
            bool success = Win32Helper.StretchBlt(CompatibleDeviceContext, 0, 0, size.Width, size.Height, WindowDeviceContext, 0, 0, size.Width, size.Height, PixelOperations);
            if (!success)
            {
                Console.WriteLine("no bitmap:" + inited);

                //lock (locker)
                //{ 
                //    Dispose();
                //    Init();
                //    Console.WriteLine("Reset init");
                //}
            }
            Bitmap img = Image.FromHbitmap(CompatibleBitmap);

            return img;
        }

        public static void Init()
        {
            size = DesktopHelper.DesktopSize;
            WindowDeviceContext = Win32Helper.GetWindowDC(_desktopWindow);
            CompatibleDeviceContext = Win32Helper.CreateCompatibleDC(WindowDeviceContext);
            CompatibleBitmap = Win32Helper.CreateCompatibleBitmap(WindowDeviceContext, size.Width, size.Height);
            _oldBitmap = Win32Helper.SelectObject(CompatibleDeviceContext, CompatibleBitmap);

            PixelOperations = CopyPixelOperation.SourceCopy;
            inited = true;
        }

        public static void Dispose()
        {
            inited = false;
            _ = Win32Helper.SelectObject(CompatibleDeviceContext, _oldBitmap);
            _ = Win32Helper.DeleteObject(CompatibleBitmap);
            _ = Win32Helper.DeleteDC(CompatibleDeviceContext);
            _ = Win32Helper.ReleaseDC(_desktopWindow, WindowDeviceContext);
        }
    }
}
