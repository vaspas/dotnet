using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Shapes;
using TapeDrawingWinFormsDx.Cache;
using TapeDrawingWinFormsDx.Cache.FontCache;
using TapeDrawingWinFormsDx.Cache.LineCache;
using TapeDrawingWinFormsDx.Cache.TextureCache;
using TapeDrawingWinFormsDx.Instruments;
using TapeDrawingWinFormsDx.Shapes;

namespace TapeDrawingWinFormsDx
{
	/// <summary>
	/// Графический контекст
	/// </summary>
	public class GraphicContext : IGraphicContext,IDisposable
	{
		public GraphicContext()
		{
			Graphics = new DirectxGraphics();
			MaxCacheSize = 100;
			_cacheCreated = false;
		}

		public int MaxCacheSize { get; set; }

        public bool Antialias { get; set; }

		internal DirectxGraphics Graphics { get; private set; }

		public IInstrumentsFactory Instruments
		{
			get
			{
				if (!_cacheCreated) CreateCacheObjects();

				return new InstrumentsFactory
				       	{
				       		FontCacher = _fontCacher,
				       		ImageSprite = _imageSprite,
				       		LineCacher = _lineCacher,
				       		TextSprite = _textSprite,
				       		TextureCacher = _textureCacher
				       	};
			}
		}

		public IShapesFactory Shapes
		{
			get { return new ShapesFactory {Device = Graphics.Device}; }
		}

        public IClip CreateClip()
        {
            return new Clip(Graphics);
        }

	    private void CreateCacheObjects()
		{
            _line = new Line(Graphics.Device.DxDevice) { Antialias = Antialias, PatternScale = 1.0f };
			_textSprite = new Sprite(Graphics.Device.DxDevice);
			_imageSprite = new Sprite(Graphics.Device.DxDevice);
			
            // Двухуровневый кэш линий
            _lineCacher = new LineFromArgsCreator { Device = Graphics.Device, Antialias = Antialias };
		    _lineCacher = new LineCacherDecorator<LineCreatorArgs, LineCreatorArgs>
		                      {Cacher = _lineCacher, HashFunction = s => s, MaxSize = 100};

            // Двухуровневый кэш шрифтов
		    _fontCacher = new FontFromGdiCreator {Device = Graphics.Device, Antialias = Antialias};
		    _fontCacher = new FontCacherDecorator<System.Drawing.Font, System.Drawing.Font>
		                      {Cacher = _fontCacher, HashFunction = s => s, MaxSize = 100};

            // Многоуровневый кэш текстур. 
            // 1. По указателю потока
            // 2. по указателю Bitmap
            // 3. По хэшу потока
            // 4. По хэшу Bitmap
            // 5. Создание из Bitmap
            // 6. Создание из потока
		    _textureCacher = new TextureFromStreamCreator {Device = Graphics.Device};
		    _textureCacher = new TextureFromBitmapCreator
		                         {
		                             Device = Graphics.Device,
		                             Cacher = _textureCacher,
		                             TextureFromStreamCreator = (TextureFromStreamCreator) _textureCacher
		                         };
		    _textureCacher = new TextureCacherDecorator<string, System.Drawing.Bitmap>
		                         {
		                             Cacher = _textureCacher,
		                             HashFunction =
		                                 s =>
		                                 Convert.ToBase64String((new MD5CryptoServiceProvider()).ComputeHash(GetBytes(s))),
		                             MaxSize = 100
		                         };
		    _textureCacher = new TextureCacherDecorator<string, System.IO.Stream>
		                         {
		                             Cacher = _textureCacher,
		                             HashFunction = s =>
		                                                {
		                                                    s.Position = 0;
		                                                    return Convert.ToBase64String(
		                                                        (new MD5CryptoServiceProvider()).ComputeHash(s));
		                                                },
		                             MaxSize = 100
		                         };
            _textureCacher = new TextureCacherDecorator<System.Drawing.Bitmap, System.Drawing.Bitmap>
		                         {
		                             Cacher = _textureCacher,
		                             HashFunction = s => s,
		                             MaxSize = 100
		                         };
		    _textureCacher = new TextureCacherDecorator<System.IO.Stream, System.IO.Stream>
		                         {
		                             Cacher = _textureCacher,
		                             HashFunction = s => s,
		                             MaxSize = 100
		                         };

            _cacheCreated = true;
		}

        /// <summary>
        /// Преобразует изображение в массив байтов
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <returns>Массив байтов изображения</returns>
        public static byte[] GetBytes(System.Drawing.Bitmap image)
        {
            var bmp = image;
            var bmpd = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bytes = new byte[Math.Abs(bmpd.Stride)*bmp.Height];
            Marshal.Copy(bmpd.Scan0, bytes, 0, bytes.Length);
            bmp.UnlockBits(bmpd);

            return bytes;
        }

	    private bool _cacheCreated;
		/// <summary>
		/// Объект DirectX для рисования линий
		/// </summary>
		private Line _line;
		/// <summary>
		/// Спрайт DirectX для вывода текста
		/// </summary>
		private Sprite _textSprite;
		/// <summary>
		/// Спрайт DirectX для вывода изображений
		/// </summary>
		private Sprite _imageSprite;
		/// <summary>
		/// Кэш шрифтов
		/// </summary>
        private ICacher<Microsoft.DirectX.Direct3D.Font, System.Drawing.Font> _fontCacher;
		/// <summary>
		/// Кэш текстур
		/// </summary>
        private ICacher<Texture, TextureCreatorArgs> _textureCacher;
	    /// <summary>
	    /// Кэш линий
	    /// </summary>
	    private ICacher<Line, LineCreatorArgs> _lineCacher;

		#region Implementation of IDisposable

		public void Dispose()
		{
			if(_cacheCreated)
			{
				_line.Dispose();
				_textSprite.Dispose();
				_imageSprite.Dispose();
				_fontCacher.Dispose();
				_textureCacher.Dispose();
				_lineCacher.Dispose();
			}
			Graphics.Dispose();
		}

		#endregion
	}
}
