using System;
using System.IO;
using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest2.Renderers
{
	internal class ImageRenderer : ICurrentRenderer, INeedPointTranslatorRenderer, INeedAlignmentTranslatorRenderer
	{
		public BaseModel Model { get; set; }
		public IPointTranslator Translator { get; set; }
		public IAlignmentTranslator AlignmentTranslator { get; set; }

		public void Draw(IGraphicContext gr, Rectangle<float> rect)
		{
			var model = (ImageModel)Model;

			Translator.Src = rect;
            Translator.Dst = rect;

            var shapes = ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator)
                .Translate(AlignmentTranslator)
                .Result;

			var bytes = model.ImageSerialized;
			using (var ms = new MemoryStream(bytes))
            using (var image = gr.Instruments.CreateImagePortion(ms, model.Roi.Target))
			using (var shape = shapes.CreateImage(image, model.Alignment, model.Angle))
			{
				shape.Render(model.Point.Target);
			}
		}
	}
}
