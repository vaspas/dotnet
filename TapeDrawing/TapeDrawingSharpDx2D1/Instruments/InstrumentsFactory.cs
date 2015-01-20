using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx2D1.Instruments
{
	/// <summary>
	/// Фабрика инструментов
	/// </summary>
	class InstrumentsFactory : IInstrumentsFactory
	{

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
            return null;
        }

	}
}