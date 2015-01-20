using System;
using System.Collections.Generic;

namespace TapeDrawingWinFormsDx.Cache.TextureCache
{
    /// <summary>
    /// Кэш текстур, полученных из объектов определенного типа
    /// </summary>
    /// <typeparam name="THash">Тип данных хэш-суммы</typeparam>
    /// <typeparam name="TData">Тип объектов, из которых создаются текстуры</typeparam>
    class TextureCacherDecorator<THash, TData> : ICacher<Microsoft.DirectX.Direct3D.Texture, TextureCreatorArgs>
    {
        /// <summary>
        /// Объект кэша, у которого просить текстуру, если в нашем кэше нет
        /// </summary>
        public ICacher<Microsoft.DirectX.Direct3D.Texture, TextureCreatorArgs> Cacher { get; set; }
        /// <summary>
        /// Функция вычисления хэш-суммы
        /// </summary>
        public Func<TData, THash> HashFunction { get; set; }
        /// <summary>
        /// Максимальный размер кэша
        /// </summary>
        public int MaxSize { get; set; }

        /// <summary>
        /// Словарь, в котором хранятся кэшированные данные
        /// </summary>
        private readonly Dictionary<THash, DxTexture> _cache = new Dictionary<THash, DxTexture>();

        private class DxTexture
        {
            public float Width { get; set; }
            public float Height { get; set; }
            public Microsoft.DirectX.Direct3D.Texture Texture { get; set; }
        }

        public Microsoft.DirectX.Direct3D.Texture Get(ref TextureCreatorArgs args)
        {
            // Если этот кэш не хранит эти данные, то попросим у следующего
            if (!(args.Source is TData))
                return Cacher.Get(ref args);

            if (_cache.Count > MaxSize) ClearCache();

            var hash = HashFunction((TData)args.Source);

            if (_cache.ContainsKey(hash))
            {
                var dxTexture = _cache[hash];
                if (dxTexture.Texture.Disposed) _cache.Remove(hash);
                else
                {
                    args.Width = dxTexture.Width;
                    args.Height = dxTexture.Height;
                    return dxTexture.Texture;
                }
            }

            // Нужно создать новую текстуру. Как это сделать, кто-то дальше должен знать :)
            var texture = Cacher.Get(ref args);
            _cache.Add(hash, new DxTexture { Texture = texture, Width = args.Width, Height = args.Height });

            return texture;
        }

        protected void ClearCache()
        {
            foreach (var key in _cache.Keys)
            {
                if (!_cache[key].Texture.Disposed) _cache[key].Texture.Dispose();
            }
            _cache.Clear();
        }

        /// <summary>
        /// Очищает ресурсы сначала во вложенном кэше, потом у себя
        /// </summary>
        public void Dispose()
        {
            if (Cacher != null) Cacher.Dispose();
            ClearCache();
        }
    }
}
