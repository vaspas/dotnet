using System.Collections.Generic;
using System.ComponentModel;
using ComparativeTest2.Models.Instruments;
using ComparativeTest2.Models.Primitives;

namespace ComparativeTest2.Models.Shapes
{
	/// <summary>
	/// Полигон
	/// </summary>
	public class PolygonModel : BaseModel
	{
		public PolygonModel()
		{
			Brush = new BrushModel();
			Points = new List<PointModel>();
		}

		/// <summary>
		/// Кисть рисования
		/// </summary>
		[DisplayName("Кисть заливки")]
		[Description("Кисть заливки")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BrushModel Brush { get; set; }

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
			return "Полигон";
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

			return string.Format("{0} points, {1}. {2}", Points.Count, Brush, pointsDesc);
		}
	}
}
