
using System;
using System.IO;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using FontStyle = TapeDrawing.Core.Primitives.FontStyle;

namespace TapeDrawingWinForms.Instruments
{
    class InstrumentsFactory : IInstrumentsFactory
    {
        internal GraphicContext Context { get; set; }

        public Cache.IBitmapSource<Stream> BitmapFromStreamSource { get; set; }

        public IPen CreatePen(Color color, float width, LineStyle style)
        {
            var gdipPen = new System.Drawing.Pen(Converter.Convert(color), width);
            gdipPen.DashStyle = Converter.Convert(style);

            return new Pen
                       {
                           ConcreteInstrument = gdipPen
                       };
        }

        public IBrush CreateSolidBrush(Color color)
        {
            return new Brush
            {
                ConcreteInstrument = new System.Drawing.SolidBrush(Converter.Convert(color))
            };
        }

        public IFont CreateFont(string type, int size, Color color, FontStyle style)
        {
            return new Font
            {
                ConcreteInstrument = new System.Drawing.Font(type, size, Converter.Convert(style)),
                Brush = new System.Drawing.SolidBrush(Converter.Convert(color))
            };
        }

        public IImage CreateImage<T>(T data)
        {
            return CreateImagePortion(data, default(Rectangle<float>));
        }

        public IImage CreateImagePortion<T>(T data, Rectangle<float> roi)
        {
            var correctedRoi=default(Rectangle<int>);
            if (!roi.IsEmpty())
            {
                correctedRoi.Left = (int) (roi.Left/Context.ImageHorizontalScaleFactor);
                correctedRoi.Right = (int) (roi.Right/Context.ImageHorizontalScaleFactor);
                correctedRoi.Bottom = (int) (roi.Bottom/Context.ImageVerticalScaleFactor);
                correctedRoi.Top = (int) (roi.Top/Context.ImageVerticalScaleFactor);
            }

            if (data is System.Drawing.Bitmap)
            {
                return new Image
                {
                    HorizontalScaleFactor = Context.ImageHorizontalScaleFactor,
                    VerticalScaleFactor = Context.ImageVerticalScaleFactor,
                    ConcreteInstrument = data as System.Drawing.Bitmap,
                    Roi = correctedRoi
                };
            }

            if (data is Stream)
            {
                return new Image
                {
                    HorizontalScaleFactor = Context.ImageHorizontalScaleFactor,
                    VerticalScaleFactor = Context.ImageVerticalScaleFactor,
                    ConcreteInstrument = BitmapFromStreamSource.Get(data as Stream),
                    Roi = correctedRoi
                };
            }

            throw new ArgumentException();
        }
    }
}
