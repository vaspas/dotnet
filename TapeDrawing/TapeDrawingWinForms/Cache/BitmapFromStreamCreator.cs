using System.IO;

namespace TapeDrawingWinForms.Cache
{
    class BitmapFromStreamCreator:IBitmapSource<Stream>
    {
        public GraphicContext Context { get; set; }

        public System.Drawing.Bitmap Get(Stream data)
        {
            var bmp= new System.Drawing.Bitmap(data);
            if (Context.ImageHorizontalDpi.HasValue && Context.ImageVerticalDpi.HasValue)
                bmp.SetResolution(Context.ImageHorizontalDpi.Value, Context.ImageVerticalDpi.Value);
            return bmp;
        }
    }
}
