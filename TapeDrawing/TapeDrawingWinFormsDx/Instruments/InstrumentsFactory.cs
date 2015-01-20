using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawingWinFormsDx.Cache;
using TapeDrawingWinFormsDx.Cache.LineCache;
using TapeDrawingWinFormsDx.Cache.TextureCache;

namespace TapeDrawingWinFormsDx.Instruments
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
        public ICacher<Microsoft.DirectX.Direct3D.Font, System.Drawing.Font> FontCacher { get; set; }

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
		    return new Brush {Argb = Converter.ConvertToInt(color), A = color.A};
		}

        public IPen CreatePen(Color color, float width, LineStyle style)
        {
            var lineArgs = new LineCreatorArgs {Width = width, Style = style};
            return new Pen
                       {
                           HLine = LineCacher.Get(ref lineArgs),
                           Argb = Converter.ConvertToInt(color)
                       };
        }

	    public IFont CreateFont(string type, int size, Color color, FontStyle style)
		{
		    var gdiFont = new System.Drawing.Font(type, size, Converter.Convert(style));
			return new Font
			       	{
			       		Color = Converter.Convert(color),
                        FontD3D = FontCacher.Get(ref gdiFont),
			       		TextSprite = TextSprite
			       	};
		}

        public IImage CreateImage<T>(T data)
        {
            return CreateImagePortion(data, default(Rectangle<float>));
        }

        public IImage CreateImagePortion<T>(T data, Rectangle<float> roi)
        {
            var correctedRoi = default(Rectangle<int>);
            if (!roi.IsEmpty())
            {
                correctedRoi.Left = (int) (roi.Left);
                correctedRoi.Right = (int) (roi.Right);
                correctedRoi.Bottom = (int) (roi.Bottom);
                correctedRoi.Top = (int) (roi.Top);
            }

            var args = new TextureCreatorArgs { Source = data };
            var texture = TextureCacher.Get(ref args);

            return new Image
                       {
                           Width = args.Width, 
                           Height = args.Height, 
                           ImageSprite = ImageSprite, 
                           Texture = texture,
                           Roi = correctedRoi
                       };
        }

	}
}