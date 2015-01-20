using System.ComponentModel;
using ComparativeTest2.Models.Instruments;
using ComparativeTest2.Models.Primitives;

namespace ComparativeTest2.Models.Shapes
{
	/// <summary>
	/// Закрашенный прямоугольник
	/// </summary>
	public class FillRectangleModel : BaseModel
	{
		public FillRectangleModel()
		{
			Brush = new BrushModel();
			Rectangle = new RectangleModel();
		}

		/// <summary>
		/// Кисть рисования
		/// </summary>
		[DisplayName("Кисть заливки")]
		[Description("Кисть заливки")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BrushModel Brush { get; set; }

		/// <summary>
		/// Координаты прямоугольника
		/// </summary>
		[DisplayName("Координаты прямоугольника")]
		[Description("Координаты прямоугольника")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public RectangleModel Rectangle { get; set; }

		/// <summary>
		/// Имя фигуры
		/// </summary>
		public override string GetShapeName()
		{
			return "Закрашенный прямоугольник";
		}

		/// <summary>
		/// Позволяет получить описание фигуры
		/// </summary>
		/// <returns></returns>
		public override string GetDescription()
		{
			return string.Format("{0}, {1}", Rectangle, Brush);
		}
	}
}
