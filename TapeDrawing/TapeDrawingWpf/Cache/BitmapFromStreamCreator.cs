using System.IO;
using System.Windows.Media.Imaging;

namespace TapeDrawingWpf.Cache
{
    class BitmapFromStreamCreator : IBitmapSource<Stream>
    {
        public BitmapImage Get(Stream data)
        {
            data.Position = 0;
            var bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            bmpImage.CacheOption = BitmapCacheOption.OnLoad;
            bmpImage.CreateOptions = BitmapCreateOptions.None;
            bmpImage.StreamSource = data;
            bmpImage.EndInit();
            bmpImage.Freeze();
            return bmpImage;
        }
    }
}
