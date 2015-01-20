using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest2.Renderers
{
    class FillRectangleAreaRenderer : ICurrentRenderer, INeedPointTranslatorRenderer, INeedAlignmentTranslatorRenderer
	{
		public BaseModel Model { get; set; }

        public IPointTranslator Translator { get; set; }
        public IAlignmentTranslator AlignmentTranslator { get; set; }

		public void Draw(IGraphicContext gr, Rectangle<float> rect)
		{
			var model = (FillRectangleAreaModel) Model;

			Translator.Src = rect;
			Translator.Dst = rect;

            var shapes = ShapesFactoryConfigurator.For(gr.Shapes)
               .Translate(Translator)
               .Translate(AlignmentTranslator)
               .Result;

			using (var brush = gr.Instruments.CreateSolidBrush(model.Brush.Color.Target))
			using (var shape = shapes.CreateFillRectangleArea(brush,model.Alignment))
			{
				shape.Render(model.Point.Target, model.Size.Target);
			}
		}
	}
}
