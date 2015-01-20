using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Renderers
{
	class FillAllRenderer : ICurrentRenderer
	{
		public BaseModel Model { get; set; }

		/// <summary>
		/// Метод для рисования на слое.
		/// </summary>
		/// <param name="gr">Объект для рисования.</param>
		/// <param name="rect">Область рисования.</param>
		public void Draw(IGraphicContext gr, Rectangle<float> rect)
		{
			var model = (FillAllModel)Model;

			using (var shape = gr.Shapes.CreateFillAll(model.Color.Target))
				shape.Render();
		}
	}
}
