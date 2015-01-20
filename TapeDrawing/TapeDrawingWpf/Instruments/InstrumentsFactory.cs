using System;
using System.IO;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWpf.Instruments
{
	/// <summary>
	/// Фабрика инструментов
	/// </summary>
	class InstrumentsFactory : IInstrumentsFactory
	{
        public Cache.IBitmapSource<Stream> BitmapFromStreamSource { get; set; }

		public IBrush CreateSolidBrush(Color color)
		{
			return new Brush
			{
				ConcreteInstrument = new System.Windows.Media.SolidColorBrush(Converter.Convert(color))
			};
		}

		public IPen CreatePen(Color color, float width, LineStyle style)
		{
			return new Pen
			       	{
			       		ConcreteInstrument =
			       			new System.Windows.Media.Pen(new System.Windows.Media.SolidColorBrush(Converter.Convert(color)), width)
			       				{DashStyle = Converter.Convert(style)}
			       	};
		}

		public IFont CreateFont(string type, int size, Color color, FontStyle style)
		{
			return new Font
			       	{
			       		ConcreteInstrument =
			       			new System.Windows.Media.Typeface(new System.Windows.Media.FontFamily(type),
			       			                                  Converter.ConvertStyle(style),
			       			                                  Converter.ConvertWeight(style),
			       			                                  System.Windows.FontStretches.Normal),
			       		Brush = new System.Windows.Media.SolidColorBrush(Converter.Convert(color)),
			       		Size = size*1.35f
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
                correctedRoi.Right = (int)(roi.Right);
                correctedRoi.Bottom = (int)(roi.Bottom);
                correctedRoi.Top = (int)(roi.Top);
            }

            if (data is System.Drawing.Bitmap)
            {
                var bmpImage = new System.Windows.Media.Imaging.BitmapImage();
                bmpImage.BeginInit();
                bmpImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bmpImage.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.None;
                var ms = new MemoryStream();
                var bmp = data as System.Drawing.Bitmap;
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bmpImage.StreamSource = ms;
                bmpImage.EndInit();
                bmpImage.Freeze();
                ms.Close();
                ms.Dispose();

                return new Image
                {
                    ConcreteInstrument = bmpImage,
                    Roi = correctedRoi
                };
            }

            if (data is System.Windows.Media.Imaging.BitmapImage)
            {
                return new Image
                           {
                               ConcreteInstrument = data as System.Windows.Media.Imaging.BitmapImage,
                               Roi = correctedRoi
                           };
            }

            if (data is Stream)
            {
                return new Image
                {
                    ConcreteInstrument = BitmapFromStreamSource.Get(data as Stream),
                    Roi = correctedRoi
                };
            }

            throw new ArgumentException();
        }
	}
}
