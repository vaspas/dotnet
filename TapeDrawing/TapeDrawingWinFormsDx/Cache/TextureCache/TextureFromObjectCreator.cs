namespace TapeDrawingWinFormsDx.Cache.TextureCache
{
    /// <summary>
    /// Базовый класс для объектов, которые умеют создавать текстуры
    /// </summary>
    /// <typeparam name="TData">Тип объекта, который преобразуется в текстуру</typeparam>
    abstract class TextureFromObjectCreator<TData> : ICacher<Microsoft.DirectX.Direct3D.Texture, TextureCreatorArgs>
    {
        public ICacher<Microsoft.DirectX.Direct3D.Texture, TextureCreatorArgs> Cacher { get; set; }

        public DeviceDescriptor Device { get; set; }

        public Microsoft.DirectX.Direct3D.Texture Get(ref TextureCreatorArgs args)
        {
            if (!(args.Source is TData))
                return Cacher.Get(ref args);

            return CreateTexture(ref args);
        }

        protected abstract Microsoft.DirectX.Direct3D.Texture CreateTexture(ref TextureCreatorArgs args);

        public virtual void Dispose()
        {
            if (Cacher != null) Cacher.Dispose();
        }
    }
}
