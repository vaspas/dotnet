using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest2.Renderers
{
	class TextRenderer : ICurrentRenderer, INeedPointTranslatorRenderer, INeedAlignmentTranslatorRenderer
	{
		public BaseModel Model { get; set; }

		public IPointTranslator Translator { get; set; }

		public IAlignmentTranslator AlignmentTranslator { get; set; }

		public void Draw(IGraphicContext gr, Rectangle<float> rect)
		{
			var model = (TextShapeModel)Model;

			Translator.Src = rect;
			Translator.Dst = rect;

			var shapes = ShapesFactoryConfigurator.For(gr.Shapes)
				.Translate(Translator)
				.Translate(AlignmentTranslator)
				.Result;

			using (var font = gr.Instruments.CreateFont(model.Font.Type, model.Font.Size, model.Font.Color.Target, model.Font.Style))
            using (var shape = shapes.CreateText(font, model.Alignment, model.Angle))
            {
				shape.Render(model.Text, model.Point.Target);
            }
		}
	}
}
