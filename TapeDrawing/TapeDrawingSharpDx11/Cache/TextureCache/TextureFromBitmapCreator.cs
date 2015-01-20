using System.IO;
using SharpDX.Direct3D11;

namespace TapeDrawingSharpDx11.Cache.TextureCache
{
    /// <summary>
    /// Умеет создавать текстуру из Битпама
    /// </summary>
    class TextureFromBitmapCreator : TextureFromObjectCreator<System.Drawing.Bitmap>
    {
        public TextureFromStreamCreator TextureFromStreamCreator { get; set; }

        protected override Texture2D CreateTexture(ref TextureCreatorArgs args)
        {
            Texture2D texture;
            using (var ms = new MemoryStream())
            {
                ((System.Drawing.Bitmap)args.Source).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Position = 0;
                args.Source = ms;
                texture = TextureFromStreamCreator.Get(ref args);
            }
            return texture;
        }
    }
}
