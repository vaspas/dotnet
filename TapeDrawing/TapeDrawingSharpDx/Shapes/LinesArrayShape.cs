using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingSharpDx.Shapes
{
	/// <summary>
	/// Фигура для рисования отдельных линий
	/// </summary>
	class LinesArrayShape : BaseShape, ILinesArrayShape
	{
		public void Render(IEnumerable<Point<float>> points)
		{
			
		}
	}
}
