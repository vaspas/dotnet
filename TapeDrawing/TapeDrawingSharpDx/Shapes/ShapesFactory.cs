using SharpDX.Direct3D9;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx.Instruments;

namespace TapeDrawingSharpDx.Shapes
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

		public IFillAllShape CreateFillAll(Color color)
		{
			return new FillAllShape {Device = Device, Color = Converter.Convert(color)};
		}

		public ILinesShape CreateLines(IPen pen)
		{
			return new LinesShape {Device = Device, Pen = (Pen) pen};
		}

		public ILinesArrayShape CreateLinesArray(Color color)
		{
			return new LinesArrayShape { Device = Device };
		}

		public IPolygonShape CreatePolygon(IBrush brush)
        {
            return new PolygonShape { Device = Device};
        }

		public IDrawRectangleShape CreateDrawRectangle(IPen pen)
		{
            return new DrawRectangleShape { Device = Device, Pen = (Pen)pen };
		}

        public IDrawRectangleAreaShape CreateDrawRectangleArea(IPen pen, Alignment alignment)
        {
            return new DrawRectangleAreaShape { Device = Device, Pen = (Pen)pen, Alignment = alignment};
        }

		public IFillRectangleShape CreateFillRectangle(IBrush brush)
		{
			return new FillRectangleShape {Device = Device, Brush = (Brush)brush};
		}

        public IFillRectangleAreaShape CreateFillRectangleArea(IBrush brush, Alignment alignment)
        {
            return new FillRectangleAreaShape 
            { 
                Device = Device
            };
        }

		public ITextShape CreateText(IFont font, Alignment alignment, float angle)
		{
			return new TextShape {Device = Device};
		}

		public IImageShape CreateImage(IImage image, Alignment alignment, float angle)
		{
			return new ImageShape {Device = Device};
		}

        /*public IPixelShape CreatePixel()
        {
            return null;
        }*/
	}
}