using System.IO;

namespace TapeDrawingWinForms.Cache
{
    interface IBitmapSource<in T>
    {
        System.Drawing.Bitmap Get(T data);
    }
}
