using System.IO;
using SharpDX.Direct3D11;

namespace TapeDrawingSharpDx11.Cache.TextureCache
{
    /// <summary>
    /// Умеет создавать текстуру из потока. Видимо должен быть последним в списке
    /// </summary>
    class TextureFromStreamCreator : TextureFromObjectCreator<Stream>
    {
        protected override Texture2D CreateTexture(ref TextureCreatorArgs args)
        {
            ((Stream)args.Source).Position = 0;
            var texture = Texture2D.FromStream<Texture2D>(Device.DxDevice, args.Source as Stream, (int)(args.Source as Stream).Length);
            args.Width = texture.Description.Width;
            args.Height = texture.Description.Height;

            return texture;
        }
    }
}
