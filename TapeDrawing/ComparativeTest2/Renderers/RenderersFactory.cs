using System;
using System.Collections.Generic;
using ComparativeTest2.Models;
using ComparativeTest2.Models.Shapes;
using TapeDrawing.Core;
using TapeDrawing.Core.Translators;

namespace ComparativeTest2.Renderers
{
	/// <summary>
	/// Фабрика создания рендереров
	/// </summary>
	static class RenderersFactory
	{
		static RenderersFactory()
		{
			// Заполним таблицу соответствия
			Renderers = new Dictionary<Type, Type>
			             	{
			             		{typeof (FillAllModel), typeof (FillAllRenderer)},
			             		{typeof (DrawRectangleModel), typeof (DrawRectangleRenderer)},
                                {typeof (DrawRectangleAreaModel), typeof (DrawRectangleAreaRenderer)},
								{typeof (FillRectangleModel), typeof (FillRectangleRenderer)},
                                {typeof (FillRectangleAreaModel), typeof (FillRectangleAreaRenderer)},
								{typeof (TextShapeModel), typeof (TextRenderer)},
								{typeof (LinesModel), typeof (LinesRenderer)},
								{typeof (LinesArrayModel), typeof (LinesArrayRenderer)},
								{typeof (PolygonModel), typeof (PolygonRenderer)},
								{typeof (ImageModel), typeof (ImageRenderer)}
			             	};
		}

		/// <summary>
		/// Создает рендерер для отображения модели
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static IRenderer Create(BaseModel model)
		{
			var renderer =  (ICurrentRenderer)Activator.CreateInstance(Renderers[model.GetType()]);
			renderer.Model = model;
			if (renderer is INeedPointTranslatorRenderer)
				((INeedPointTranslatorRenderer) renderer).Translator = PointTranslatorConfigurator.CreateLinear().MirrorY().Translator;
			if (renderer is INeedAlignmentTranslatorRenderer)
				((INeedAlignmentTranslatorRenderer) renderer).AlignmentTranslator =
					AlignmentTranslatorConfigurator.Create().Translator;
			return renderer;
		}

		/// <summary>
		/// Словать рендереров
		/// </summary>
		private static readonly Dictionary<Type, Type> Renderers;
	}
}
