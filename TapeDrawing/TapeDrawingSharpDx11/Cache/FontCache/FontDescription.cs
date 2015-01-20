using SharpDX.DirectWrite;

namespace TapeDrawingSharpDx11.Cache.FontCache
{
    class FontDescription
    {
        public string Name;
        public FontWeight Weight;
        public FontStyle Style;
        public FontStretch Stretch;
        public float Size;

        public string GetHash()
        {
            return string.Format("name{0}weigth{1}style{2}stretch{3}size{4}",
                                 Name, Weight, Style, Stretch, Size);
        }
    }
}
