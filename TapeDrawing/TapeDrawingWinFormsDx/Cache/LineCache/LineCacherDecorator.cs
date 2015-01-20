using System;
using System.Collections.Generic;
using Microsoft.DirectX.Direct3D;

namespace TapeDrawingWinFormsDx.Cache.LineCache
{
    class LineCacherDecorator<THash, TData> : ICacher<Line, TData>
    {
        /// <summary>
        /// Объект, у кого будем просить линию
        /// </summary>
        public ICacher<Line, TData> Cacher { get; set; }
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
        private readonly Dictionary<THash, Line> _cache = new Dictionary<THash, Line>();


        public Line Get(ref TData args)
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
