using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawingSharpDx11.Cache;
using TapeDrawingSharpDx11.Cache.TextureCache;
using TapeDrawingSharpDx11.Sprites.TextSprite;
using Color = TapeDrawing.Core.Primitives.Color;
using FontStyle = TapeDrawing.Core.Primitives.FontStyle;

namespace TapeDrawingSharpDx11.Instruments
{
	/// <summary>
	/// Фабрика инструментов
	/// </summary>
	class InstrumentsFactory : IInstrumentsFactory
	{
        public TextSprite TextSprite;

        /// <summary>
        /// Кэш текстур
        /// </summary>
        public ICacher<Texture2D, TextureCreatorArgs> TextureCacher { get; set; }

        public ICacher<Sprites.TextSprite.Font, Cache.FontCache.FontDescription> FontCacher { get; set; }

	    public DeviceDescriptor Device;

		public IBrush CreateSolidBrush(Color color)
		{
            return new Brush { Argb =  Converter.ConvertToVertex(color)};
		}

        public IPen CreatePen(Color color, float width, LineStyle style)
        {
            var p= new Pen
                       {
                           Argb = Converter.ConvertToVertex(color),
                           Width = width
                       };
            if(style==LineStyle.Dash)
            {
                p.Dash1 = 3;
                p.Dash2 = 1;
                p.Dash3 = 3;
                p.Dash4 = 1;
            }
            else if (style == LineStyle.Dot)
            {
                p.Dash1 = 1;
                p.Dash2 = 1;
                p.Dash3 = 1;
                p.Dash4 = 1;
            }
            return p;
        }

	    public IFont CreateFont(string type, int size, Color color, FontStyle style)
	    {
            var fontDesc = new Cache.FontCache.FontDescription
            {
                Name = type,
                Size = size,
                Stretch = FontStretch.Normal,
                Style = style == FontStyle.Italic ? SharpDX.DirectWrite.FontStyle.Italic : SharpDX.DirectWrite.FontStyle.Normal,
                Weight = style == FontStyle.Bold ? FontWeight.Bold : FontWeight.Normal
            };
            
			return new Font
			       	{
                        TextBlock = FontCacher.Get(ref fontDesc),
                        Color = Converter.Convert(color)
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
                correctedRoi.Left = (int)(roi.Left);
                correctedRoi.Right = (int)(roi.Right);
                correctedRoi.Bottom = (int)(roi.Bottom);
                correctedRoi.Top = (int)(roi.Top);
            }

            var args = new TextureCreatorArgs { Source = data };
            var texture = TextureCacher.Get(ref args);

            return new Image
            {
                Width = args.Width,
                Height = args.Height,
                TextureShaderResourceView = new ShaderResourceView(Device.DxDevice, texture),
                Roi = correctedRoi
            };
        }

	}
}