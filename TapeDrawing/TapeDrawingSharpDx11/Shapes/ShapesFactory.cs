using SharpDX.DirectWrite;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx11.Instruments;
using TapeDrawingSharpDx11.Sprites;
using TapeDrawingSharpDx11.Sprites.TextSprite;

namespace TapeDrawingSharpDx11.Shapes
{
	/// <summary>
	/// Фабрика фигур
	/// </summary>
	class ShapesFactory : IShapesFactory
	{
		/// <summary>
		/// Контекст устройства
		/// </summary>
		public DeviceDescriptor Device { get; set; }

	    public Sprite Sprite;
        public LineSprite LineSprite;
	    public TextSprite TextSprite;
        public TextureSprite TextureSprite;
	    public GpaaSprite GpaaSprite;

		public IFillAllShape CreateFillAll(Color color)
		{
		    return new FillAllShape {Device = Device, Color = Converter.Convert(color)};
		}

		public ILinesShape CreateLines(IPen pen)
		{
            return new LinesShape { Device = Device, Pen = (Pen)pen, Sprite = Sprite, LineSprite = LineSprite, GpaaSprite = GpaaSprite};
		}

		public ILinesArrayShape CreateLinesArray(Color color)
		{
			return new LinesArrayShape { Device = Device, Color = Converter.ConvertToVertex(color), Sprite = Sprite};
		}

		public IPolygonShape CreatePolygon(IBrush brush)
        {
            return new PolygonShape { Device = Device, Brush = (Brush)brush, Sprite = Sprite };
        }

		public IDrawRectangleShape CreateDrawRectangle(IPen pen)
		{
            return new DrawRectangleShape { Device = Device, Pen = (Pen)pen, Sprite = Sprite, LineSprite = LineSprite };
		}

        public IDrawRectangleAreaShape CreateDrawRectangleArea(IPen pen, Alignment alignment)
        {
            return new DrawRectangleAreaShape { Device = Device, Pen = (Pen)pen, Alignment = alignment, Sprite = Sprite, LineSprite = LineSprite };
        }

		public IFillRectangleShape CreateFillRectangle(IBrush brush)
		{
			return new FillRectangleShape {Device = Device, Brush = (Brush)brush,Sprite = Sprite};
		}

        public IFillRectangleAreaShape CreateFillRectangleArea(IBrush brush, Alignment alignment)
        {
            return new FillRectangleAreaShape 
            { 
                Device = Device,
                Brush = (Brush)brush,
                Sprite = Sprite,
                Alignment = alignment
            };
        }

		public ITextShape CreateText(IFont font, Alignment alignment, float angle)
		{
		    return new TextShape
		               {
		                   Device = Device,
		                   Sprite = TextSprite,
		                   Font = (Instruments.Font) font,
		                   Alignment = alignment,
                           Angle = angle
            };
		}

		public IImageShape CreateImage(IImage image, Alignment alignment, float angle)
		{
			return new ImageShape {Device = Device, Sprite=TextureSprite,Image = (Image)image,Alignment = alignment, Angle = angle};
		}

        /*public IPixelShape CreatePixel()
        {
            return null;
        }*/
	}
}