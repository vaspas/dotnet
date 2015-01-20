using System.Collections.Generic;
using ComparativeTest2.Models;
using ComparativeTest2.Renderers;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Layers;

namespace ComparativeTest2
{
	/// <summary>
	/// Фабрика создает все слои из выбранных объектов
	/// </summary>
	public class MainLayerFactory
	{
		public List<BaseModel> Shapes { get; set; }

		public void Create(ILayer mainLayer)
		{
			foreach (var shape in Shapes)
			{
				mainLayer.Add(CreateLayer(shape));
			}
		}

		private ILayer CreateLayer(BaseModel model)
		{
			return new RendererLayer
			{
				Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
				Renderer = RenderersFactory.Create(model)
			};
		}
	}
}
