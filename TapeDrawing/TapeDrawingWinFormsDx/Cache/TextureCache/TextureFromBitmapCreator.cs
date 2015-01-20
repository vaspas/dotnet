using System.IO;

namespace TapeDrawingWinFormsDx.Cache.TextureCache
{
    /// <summary>
    /// Умеет создавать текстуру из Битпама
    /// </summary>
    class TextureFromBitmapCreator : TextureFromObjectCreator<System.Drawing.Bitmap>
    {
        public TextureFromStreamCreator TextureFromStreamCreator { get; set; }

        protected override Microsoft.DirectX.Direct3D.Texture CreateTexture(ref TextureCreatorArgs args)
        {
           Microsoft.DirectX.Direct3D.Texture texture;
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
