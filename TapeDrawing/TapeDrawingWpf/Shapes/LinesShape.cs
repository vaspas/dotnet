using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using Pen = TapeDrawingWpf.Instruments.Pen;

namespace TapeDrawingWpf.Shapes
{
    /// <summary>
    /// Фигура рисует линии
    /// </summary>
    class LinesShape : BaseShape, ILinesShape
    {
        /// <summary>
        /// Карандаш для рисования
        /// </summary>
        public Pen Pen;

        public void Render(IEnumerable<Point<float>> points)
        {
            /* Вариант 1
            System.Windows.Point startpoint;
            var segments = Converter.Convert(points, out startpoint);

            Surface.Context.DrawGeometry(null, Pen.ConcreteInstrument,
                                         new System.Windows.Media.PathGeometry(new List<System.Windows.Media.PathFigure>
                                                                                {
                                                                                    new System.Windows.Media.PathFigure(startpoint,
                                                                                                                        segments,
                                                                                                                        false)
                                                                                }));
             * */

            var pathFig = new PathFigure();

            System.Windows.Point startpoint;
            Converter.Convert(points, out startpoint)
                .ToList().ForEach(pathFig.Segments.Add);
            pathFig.StartPoint = startpoint;


            Surface.Context.DrawGeometry(null, Pen.ConcreteInstrument, new PathGeometry(new List<PathFigure> { pathFig }));

            /*IPoint<float> begin = null;
            foreach (var point in points)
            {
                if (begin == null)
                {
                    begin = point;
                    continue;
                }
                Surface.Context.DrawLine(Pen.ConcreteInstrument, Converter.Convert(begin), Converter.Convert(point));
                begin = point;
            }*/

        }
    }
}

