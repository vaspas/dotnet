using System;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWpf.Instruments
{
	/// <summary>
	/// Рисунок для отображения
	/// </summary>
	class Image : IImage
	{
		/// <summary>
		/// Рисунок wpf для отображения
		/// </summary>
		public System.Windows.Media.Imaging.BitmapImage ConcreteInstrument { get; set; }

        public float Width
        {
            get 
            { 
                if(Roi.IsEmpty())
                    return ConcreteInstrument.PixelWidth;

                return Math.Abs(Roi.Right - Roi.Left);
            }
        }
        public float Height
        {
            get 
            {
                if (Roi.IsEmpty())
                    return ConcreteInstrument.PixelHeight;

                return Math.Abs(Roi.Top - Roi.Bottom);
            }
        }

	    public Rectangle<int> Roi;

		#region Implementation of IDisposable
		public void Dispose()
		{}
		#endregion
	}
}
