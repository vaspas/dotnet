using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using SharpDX.Direct3D11;
using TapeDrawing.Core;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx11.Cache;
using TapeDrawingSharpDx11.Cache.FontCache;
using TapeDrawingSharpDx11.Cache.TextureCache;
using TapeDrawingSharpDx11.Instruments;
using TapeDrawingSharpDx11.Shapes;
using TapeDrawingSharpDx11.Sprites;
using TapeDrawingSharpDx11.Sprites.TextSprite;

namespace TapeDrawingSharpDx11
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

        public bool Antialias 
        {
            get { return Graphics.Antialias; }
            set { Graphics.Antialias = value; }
        }

		internal DirectxGraphics Graphics { get; private set; }

		public IInstrumentsFactory Instruments
		{
			get
			{
				if (!_cacheCreated) CreateCacheObjects();

				return new InstrumentsFactory
				       	{
                            Device = Graphics.Device,
                            TextureCacher = _textureCacher,
                            TextSprite = _textSprite,
                            FontCacher=_fontCacher
				       	};
			}
		}

		public IShapesFactory Shapes
		{
			get
            {
                if (!_cacheCreated) CreateCacheObjects();
                return new ShapesFactory { Device = Graphics.Device, Sprite = _sprite, TextureSprite = _textureSprite, 
                    LineSprite = _linesprite,
                                           TextSprite = _textSprite,
                                           GpaaSprite=_gpaaSprite
                }; 
            }
		}

        public IClip CreateClip()
        {
            return new Clip(Graphics);
        }

	    private void CreateCacheObjects()
		{
            _textSprite = new TextSprite(Graphics.Device.DxDevice);
            _sprite=new Sprite(Graphics.Device);
            _linesprite = new LineSprite(Graphics.Device);
            _textureSprite = new TextureSprite(Graphics.Device);
            if(Antialias)
                _gpaaSprite = new GpaaSprite(Graphics.Device);
            _gbaaSprite = new GbaaSprite(Graphics.Device);
	        Graphics.GbaaSprite = _gbaaSprite;

            // Двухуровневый кэш шрифтов
            _fontCacher = new FontFromDescriptionCreator { Sprite = _textSprite};
            _fontCacher = new FontCacherDecorator<string, FontDescription> { Cacher = _fontCacher, HashFunction = s => s.GetHash(), MaxSize = 100 };

            // Многоуровневый кэш текстур. 
            // 1. По указателю потока
            // 2. по указателю Bitmap
            // 3. По хэшу потока
            // 4. По хэшу Bitmap
            // 5. Создание из Bitmap
            // 6. Создание из потока
            _textureCacher = new TextureFromStreamCreator { Device = Graphics.Device };
            _textureCacher = new TextureFromBitmapCreator
            {
                Device = Graphics.Device,
                Cacher = _textureCacher,
                TextureFromStreamCreator = (TextureFromStreamCreator)_textureCacher
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

        private TextSprite _textSprite;
	    private Sprite _sprite;
        private LineSprite _linesprite;
        private TextureSprite _textureSprite;
        private GpaaSprite _gpaaSprite;
        private GbaaSprite _gbaaSprite;
        /// <summary>
        /// Кэш текстур
        /// </summary>
        private ICacher<Texture2D, TextureCreatorArgs> _textureCacher;

        private ICacher<Sprites.TextSprite.Font, FontDescription> _fontCacher;

		#region Implementation of IDisposable

		public void Dispose()
		{
			if(_cacheCreated)
			{
                if(_gpaaSprite!=null)
                    _gpaaSprite.Dispose();
                if (_gbaaSprite != null)
                    _gbaaSprite.Dispose();
                _textureCacher.Dispose();
                _fontCacher.Dispose();
                _sprite.Dispose();
                _linesprite.Dispose();
                _textureSprite.Dispose();
			}
			Graphics.Dispose();
		}

		#endregion
	}
}
