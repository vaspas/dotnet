using System.Collections.Generic;
using System.ComponentModel;
using ComparativeTest2.Models.Primitives;

namespace ComparativeTest2.Models.Shapes
{
	/// <summary>
	/// Отдельные линии
	/// </summary>
	public class LinesArrayModel : BaseModel
	{
		public LinesArrayModel()
		{
			Color = new ColorModel();
			Points = new List<PointModel>();
		}

		/// <summary>
		/// Цвет
		/// </summary>
		[DisplayName("Цвет линий")]
		[Description("Цвет линий")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorModel Color { get; set; }

		/// <summary>
		/// Точки
		/// </summary>
		[DisplayName("Точки")]
		[Description("Точки")]
		public List<PointModel> Points { get; set; }

		/// <summary>
		/// Имя фигуры
		/// </summary>
		public override string GetShapeName()
		{
			return "Отдельные линии";
		}

		/// <summary>
		/// Позволяет получить описание фигуры
		/// </summary>
		/// <returns></returns>
		public override string GetDescription()
		{
			// Список точек
			var pointsDesc = Points.Count > 0 ? Points[0].ToString() : "";
			for (var index = 1; index < Points.Count; index++)
				pointsDesc += ("-" + Points[index]);

			return string.Format("{0} points, {1}. {2}", Points.Count, Color, pointsDesc);
		}
	}
}
