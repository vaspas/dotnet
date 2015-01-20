using SharpDX.Direct3D9;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawingSharpDx.Cache;
using TapeDrawingSharpDx.Cache.LineCache;
using TapeDrawingSharpDx.Cache.TextureCache;

namespace TapeDrawingSharpDx.Instruments
{
	/// <summary>
	/// Фабрика инструментов
	/// </summary>
	class InstrumentsFactory : IInstrumentsFactory
	{
		/// <summary>
		/// Спрайт DirectX для вывода текста
		/// </summary>
		public Sprite TextSprite { get; set; }

		/// <summary>
		/// Спрайт DirectX для вывода изображений
		/// </summary>
		public Sprite ImageSprite { get; set; }

		/// <summary>
		/// Кэш шрифтов
		/// </summary>
        public ICacher<SharpDX.Direct3D9.Font, System.Drawing.Font> FontCacher { get; set; }

		/// <summary>
		/// Кэш текстур
		/// </summary>
        public ICacher<Texture, TextureCreatorArgs> TextureCacher { get; set; }

		/// <summary>
		/// Кэш линий
		/// </summary>
        public ICacher<Line, LineCreatorArgs> LineCacher { get; set; }

		public IBrush CreateSolidBrush(Color color)
		{
            return new Brush { Argb =  Converter.Convert(color)};
		}

        public IPen CreatePen(Color color, float width, LineStyle style)
        {
            var lineArgs = new LineCreatorArgs {Width = width, Style = style};
            return new Pen
                       {
                           HLine = LineCacher.Get(ref lineArgs),
                           Argb = Converter.ConvertBGRA(color)
                       };
        }

	    public IFont CreateFont(string type, int size, Color color, FontStyle style)
		{
			return new Font
			       	{
			       	};
		}

        public IImage CreateImage<T>(T data)
        {
            return CreateImagePortion(data, default(Rectangle<float>));
        }

        public IImage CreateImagePortion<T>(T data, Rectangle<float> roi)
        {
            var args = new TextureCreatorArgs { Source = data };

            return new Image
                       {
                           Width = args.Width, 
                           Height = args.Height
                       };
        }

	}
}