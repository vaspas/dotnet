using SharpDX.Direct3D9;

namespace TapeDrawingSharpDx.Cache.TextureCache
{
    /// <summary>
    /// Базовый класс для объектов, которые умеют создавать текстуры
    /// </summary>
    /// <typeparam name="TData">Тип объекта, который преобразуется в текстуру</typeparam>
    abstract class TextureFromObjectCreator<TData> : ICacher<Texture, TextureCreatorArgs>
    {
        public ICacher<Texture, TextureCreatorArgs> Cacher { get; set; }

        public DeviceDescriptor Device { get; set; }

        public Texture Get(ref TextureCreatorArgs args)
        {
            if (!(args.Source is TData))
                return Cacher.Get(ref args);

            return CreateTexture(ref args);
        }

        protected abstract Texture CreateTexture(ref TextureCreatorArgs args);

        public virtual void Dispose()
        {
            if (Cacher != null) Cacher.Dispose();
        }
    }
}
