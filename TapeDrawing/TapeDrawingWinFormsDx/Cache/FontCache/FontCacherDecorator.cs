using System;
using System.Collections.Generic;

namespace TapeDrawingWinFormsDx.Cache.FontCache
{
    class FontCacherDecorator<THash, TData> : ICacher<Microsoft.DirectX.Direct3D.Font, TData>
    {
        /// <summary>
        /// Объект, у кого будем просить шрифт если в нашем кэше нет
        /// </summary>
        public ICacher<Microsoft.DirectX.Direct3D.Font, TData> Cacher { get; set; }
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
        private readonly Dictionary<THash, Microsoft.DirectX.Direct3D.Font> _cache = new Dictionary<THash, Microsoft.DirectX.Direct3D.Font>();

        public Microsoft.DirectX.Direct3D.Font Get(ref TData args)
        {
            if (_cache.Count > MaxSize) ClearCache();

            var hash = HashFunction(args);

            if (!_cache.ContainsKey(hash))
                _cache.Add(hash, Cacher.Get(ref args));

            return _cache[hash];
        }

        protected void ClearCache()
        {
            foreach (var key in _cache.Keys)
            {
                if (!_cache[key].Disposed) _cache[key].Dispose();
            }
            _cache.Clear();
        }

        public void Dispose()
        {
            if (Cacher != null) Cacher.Dispose();
            ClearCache();
        }
    }
}
