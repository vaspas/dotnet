using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace TapeDrawingWpf.Cache
{
    class ImageCacheDecorator<THash, TData> : IBitmapSource<TData>
    {
        public IBitmapSource<TData> Internal { get; set; }

        public Func<TData, THash> HashFunction { get; set; }

        public int MaxSize { get; set; }

        private readonly Dictionary<THash, BitmapImage> _cache = new Dictionary<THash, BitmapImage>();


        public BitmapImage Get(TData data)
        {
            if (_cache.Count > MaxSize)
                _cache.Clear();

            var hash = HashFunction(data);

            if (!_cache.ContainsKey(hash))
                _cache.Add(hash, Internal.Get(data));

            return _cache[hash];
        }
    }
}
