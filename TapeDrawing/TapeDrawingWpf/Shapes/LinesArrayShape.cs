using System.Collections.Generic;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingWpf.Shapes
{
	class LinesArrayShape : BaseShape, ILinesArrayShape
	{
		public System.Windows.Media.Color Color { get; set; }

		public void Render(IEnumerable<Point<float>> points)
		{
			var brush = new System.Windows.Media.SolidColorBrush(Color);
			var pen = new System.Windows.Media.Pen(brush, 1.0);

			var begin = default(Point<float>);
		    var initBegin=true;
			foreach (var point in points)
			{
                if (initBegin)
				{
					begin = point;
				    initBegin = false;
					continue;
				}
				Surface.Context.DrawLine(pen, Converter.Convert(begin), Converter.Convert(point));
			    initBegin = true;
			}
		}
	}
}
