using System.IO;
using SharpDX.Direct3D9;

namespace TapeDrawingSharpDx.Cache.TextureCache
{
    /// <summary>
    /// Умеет создавать текстуру из Битпама
    /// </summary>
    class TextureFromBitmapCreator : TextureFromObjectCreator<System.Drawing.Bitmap>
    {
        public TextureFromStreamCreator TextureFromStreamCreator { get; set; }

        protected override Texture CreateTexture(ref TextureCreatorArgs args)
        {
           Texture texture;
            using (var ms = new MemoryStream())
            {
                ((System.Drawing.Bitmap)args.Source).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Position = 0;
                texture = TextureFromStreamCreator.Get(ref args);
            }
            return texture;
        }
    }
}
