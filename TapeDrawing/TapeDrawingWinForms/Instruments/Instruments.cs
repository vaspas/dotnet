using TapeDrawing.Core.Instruments;
using System;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinForms.Instruments
{
    class Pen:Instrument<System.Drawing.Pen>, IPen
    { }

    class Brush : Instrument<System.Drawing.Brush>, IBrush
    { }

    class Font: Instrument<System.Drawing.Font>, IFont
    {
        public System.Drawing.Brush Brush { get; set; }

        public override void Dispose()
        {
            if (Brush is IDisposable)
                (Brush as IDisposable).Dispose();

            base.Dispose();
        }
    }

    class Image : IImage
    {
        public Image()
        {
            HorizontalScaleFactor = 1;
            VerticalScaleFactor = 1;
        }

        internal float HorizontalScaleFactor { get; set; }
        internal float VerticalScaleFactor { get; set; }

        public System.Drawing.Bitmap ConcreteInstrument { get; set; }

        public float Width
        {
            get 
            { 
                if(Roi.IsEmpty())
                    return ConcreteInstrument.Width * HorizontalScaleFactor;

                return Math.Abs(Roi.Right - Roi.Left) * HorizontalScaleFactor;
            }
        }
        public float Height
        {
            get 
            {
                if (Roi.IsEmpty())
                    return ConcreteInstrument.Height * VerticalScaleFactor;

                return Math.Abs(Roi.Top - Roi.Bottom) * VerticalScaleFactor;
            }
        }

        public Rectangle<int> Roi;

        public virtual void Dispose()
        {
        }
    }
}
