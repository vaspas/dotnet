using SharpDX.Direct3D11;

namespace TapeDrawingSharpDx11.Cache.TextureCache
{
    /// <summary>
    /// Базовый класс для объектов, которые умеют создавать текстуры
    /// </summary>
    /// <typeparam name="TData">Тип объекта, который преобразуется в текстуру</typeparam>
    abstract class TextureFromObjectCreator<TData> : ICacher<Texture2D, TextureCreatorArgs>
    {
        public ICacher<Texture2D, TextureCreatorArgs> Cacher { get; set; }

        public DeviceDescriptor Device { get; set; }

        public Texture2D Get(ref TextureCreatorArgs args)
        {
            if (!(args.Source is TData))
                return Cacher.Get(ref args);

            return CreateTexture(ref args);
        }

        protected abstract Texture2D CreateTexture(ref TextureCreatorArgs args);

        public virtual void Dispose()
        {
            if (Cacher != null) Cacher.Dispose();
        }
    }
}
