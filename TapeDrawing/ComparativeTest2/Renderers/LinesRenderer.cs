using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest2.Renderers
{
	class LinesRenderer : ICurrentRenderer, INeedPointTranslatorRenderer
	{
		public BaseModel Model { get; set; }

		public IPointTranslator Translator { get; set; }

		public void Draw(IGraphicContext gr, Rectangle<float> rect)
		{
			var model = (LinesModel) Model;

			Translator.Src = rect;
            Translator.Dst = rect;

            var shapes = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;

            using (var pen=gr.Instruments.CreatePen(model.Pen.Color.Target, model.Pen.Width, model.Pen.Style))
            using (var shape = shapes.CreateLines(pen))
            {
				shape.Render(model.Points.ConvertAll(p => p.Target));
            }
		}
	}
}
