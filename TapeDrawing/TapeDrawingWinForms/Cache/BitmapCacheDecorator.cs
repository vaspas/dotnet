using System;
using System.Collections.Generic;
using System.Drawing;

namespace TapeDrawingWinForms.Cache
{
    class ImageCacheDecorator<THash, TData>:IBitmapSource<TData>
    {
        public IBitmapSource<TData> Internal { get; set; }

        public Func<TData, THash> HashFunction { get; set; }

        public int MaxSize { get; set; }

        private readonly Dictionary<THash, Bitmap> _cache=new Dictionary<THash, Bitmap>();


        public Bitmap Get(TData data)
        {
            if(_cache.Count>MaxSize)
                _cache.Clear();

            var hash = HashFunction(data);

            if (!_cache.ContainsKey(hash))
                _cache.Add(hash, Internal.Get(data));

            return _cache[hash];
        }
    }
}
