using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GeneralTool.CoreLibrary.Win32
{
    public static class DesktopHelper
    {
        /// <summary>
        /// 获取真实设置的桌面分辨率大小
        /// </summary>
        public static Size DesktopSize
        {
            get
            {
                IntPtr hdc = Win32Helper.GetDC(IntPtr.Zero);
                Size size = new Size
                {
                    Width = Win32Helper.GetDeviceCaps(hdc, Win32Helper.DESKTOPHORZRES),
                    Height = Win32Helper.GetDeviceCaps(hdc, Win32Helper.DESKTOPVERTRES)
                };
                _ = Marshal.Release(hdc);
                return size;
            }
        }

        public static Bitmap CaptureScreen(Size bounds)
        {

            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds);
            }

            return bitmap;
        }

        /// <summary>
        /// 将窗口置前
        /// </summary>
        /// <param name="hWnd"></param>
        public static void SetForeground(this IntPtr hWnd)
        {
            if (Win32Helper.IsIconic(hWnd))
            {
                Win32Helper.ShowWindow(hWnd, 9);
            }

            // 将窗口置于前端
            Win32Helper.SetForegroundWindow(hWnd);
        }

    }
}
