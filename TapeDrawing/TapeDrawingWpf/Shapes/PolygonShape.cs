using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Brush = TapeDrawingWpf.Instruments.Brush;

namespace TapeDrawingWpf.Shapes
{
	/// <summary>
	/// Фигура рисует линии
	/// </summary>
	class PolygonShape : BaseShape, IPolygonShape
	{
		public Brush Brush { get; set; }

        public void Render(IList<Point<float>> points)
		{
            var pathFig = new PathFigure();

            System.Windows.Point startpoint;
            Converter.Convert(points, out startpoint)
                .ToList().ForEach(pathFig.Segments.Add);
            pathFig.StartPoint = startpoint;

            Surface.Context.DrawGeometry(Brush.ConcreteInstrument, null,
                                         new PathGeometry(new List<PathFigure> {pathFig}));

		}
	}
}
