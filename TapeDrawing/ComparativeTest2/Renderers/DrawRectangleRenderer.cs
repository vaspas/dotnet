using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest2.Renderers
{
	class DrawRectangleRenderer : ICurrentRenderer, INeedPointTranslatorRenderer
	{
		public BaseModel Model { get; set; }

		public IPointTranslator Translator { get; set; }

		/// <summary>
		/// Метод для рисования на слое.
		/// </summary>
		/// <param name="gr">Объект для рисования.</param>
		/// <param name="rect">Область рисования.</param>
		public void Draw(IGraphicContext gr, Rectangle<float> rect)
		{
			var model = (DrawRectangleModel) Model;

			Translator.Src = rect;
			Translator.Dst = rect;

			var shapes = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;

			using (var pen = gr.Instruments.CreatePen(model.Pen.Color.Target, model.Pen.Width, model.Pen.Style))
			using (var shape = shapes.CreateDrawRectangle(pen))
			{
				shape.Render(model.Rectangle.Target);
			}
		}
	}
}
