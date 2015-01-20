using System;
using System.IO;
using System.Security.Cryptography;
using TapeDrawing.Core;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Shapes;
using TapeDrawingWpf.Instruments;
using TapeDrawingWpf.Shapes;

namespace TapeDrawingWpf
{
	/// <summary>
	/// Графический контекст
	/// </summary>
	class GraphicContext : IGraphicContext
	{
        public GraphicContext()
        {
            _bitmapSource = new Cache.BitmapFromStreamCreator();
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
        }

	    /// <summary>
		/// Поверхность для рисования
		/// </summary>
		public DrawSurface Surface { get; set; }

        private readonly Cache.IBitmapSource<Stream> _bitmapSource;

		public IInstrumentsFactory Instruments
		{
            get { return new InstrumentsFactory {BitmapFromStreamSource = _bitmapSource}; }
		}

		public IShapesFactory Shapes
		{
			get { return new ShapesFactory { Surface = Surface }; }
		}

        public IClip CreateClip()
        {
            return new Clip(Surface);
        }
	}
}
