using System.IO;
using Microsoft.DirectX.Direct3D;

namespace TapeDrawingWinFormsDx.Cache.TextureCache
{
    /// <summary>
    /// Умеет создавать текстуру из потока. Видимо должен быть последним в списке
    /// </summary>
    class TextureFromStreamCreator : TextureFromObjectCreator<Stream>
    {
        protected override Texture CreateTexture(ref TextureCreatorArgs args)
        {
            var info = new ImageInformation();
            ((Stream)args.Source).Position = 0;
            var texture = TextureLoader.FromStream(Device.DxDevice, (Stream)args.Source, 0, 0, 0, Usage.RenderTarget, Format.A8R8G8B8,
                                                       Pool.Default, Filter.None, Filter.None, 0, ref info);
            args.Width = info.Width;
            args.Height = info.Height;

            return texture;
        }
    }
}
