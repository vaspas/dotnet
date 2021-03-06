﻿using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest2.Renderers
{
	class FillRectangleRenderer : ICurrentRenderer, INeedPointTranslatorRenderer
	{
		public BaseModel Model { get; set; }

		public IPointTranslator Translator { get; set; }

		public void Draw(IGraphicContext gr, Rectangle<float> rect)
		{
			var model = (FillRectangleModel) Model;

			Translator.Src = rect;
			Translator.Dst = rect;

			var shapes = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;
			using (var brush = gr.Instruments.CreateSolidBrush(model.Brush.Color.Target))
			using (var shape = shapes.CreateFillRectangle(brush))
			{
				shape.Render(model.Rectangle.Target);
			}
		}
	}
}
