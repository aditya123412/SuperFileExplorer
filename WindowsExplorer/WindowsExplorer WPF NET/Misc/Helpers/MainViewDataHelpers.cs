using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WindowsExplorer_WPF.Misc.Helpers
{
    internal static class MainViewDataHelpers
    {
        public static BitmapSource Bitmap2BitmapImage(Bitmap bitmap, bool MakeTransparent = true)
        {
            if (MakeTransparent == true)
                bitmap.MakeTransparent();

            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;

            try
            {
                retval = Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
                retval.Freeze();
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}