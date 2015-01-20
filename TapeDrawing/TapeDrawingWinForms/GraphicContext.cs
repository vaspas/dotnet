using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using TapeDrawing.Core;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingWinForms
{
    internal class GraphicContext:IGraphicContext
    {
        public GraphicContext()
        {
            //собираем кеш
            _bitmapSource = new Cache.BitmapFromStreamCreator
                                {
                                    Context = this
                                };
            _bitmapSource = new Cache.ImageCacheDecorator<string, Stream>
                                {
                                    HashFunction = s =>
                                    {
                                        s.Position = 0;
                                        return Convert.ToBase64String(
                                            (new MD5CryptoServiceProvider()).ComputeHash(s));
                                    },
                                    Internal = _bitmapSource,
                                    MaxSize = 100
                                };
            _bitmapSource = new Cache.ImageCacheDecorator<Stream, Stream>
                                {
                                    HashFunction = s => s,
                                    Internal = _bitmapSource,
                                    MaxSize = 100
                                };


            ImageHorizontalScaleFactor = 1;
            ImageVerticalScaleFactor = 1;
        }

        public Graphics Graphics { get; set; }

        private readonly Cache.IBitmapSource<Stream> _bitmapSource;

        public IInstrumentsFactory  Instruments
        {
            get
            {
                return new Instruments.InstrumentsFactory
                           {
                               Context = this,
                               BitmapFromStreamSource = _bitmapSource
                           };
            }
        }

        public virtual IShapesFactory Shapes
        {
            get { return new Shapes.ShapesFactory {GraphicContext = this}; }
        }

        public virtual IClip CreateClip()
        {
            return new Clip(Graphics);
        }

        internal float? ImageHorizontalDpi { get; set; }
        internal float? ImageVerticalDpi { get; set; }

        internal float ImageHorizontalScaleFactor { get; set; }
        internal float ImageVerticalScaleFactor { get; set; }
    }
}
