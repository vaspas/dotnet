
using TapeDrawingSharpDx11.Sprites.TextSprite;

namespace TapeDrawingSharpDx11.Cache.FontCache
{
    
    class FontFromDescriptionCreator : ICacher<Font, FontDescription>
    {
        public TextSprite Sprite { get; set; }

        public bool Antialias { get; set; }

        public Font Get(ref FontDescription fontDescription)
        {
            return new Font(Sprite, fontDescription.Name, fontDescription.Weight,
                                       fontDescription.Style, fontDescription.Stretch,
                                       fontDescription.Size);
        }

        public void Dispose()
        { }
    }
}
