namespace TapeDrawingWpf.Cache
{
    interface IBitmapSource<in T>
    {
        System.Windows.Media.Imaging.BitmapImage Get(T data);
    }
}
