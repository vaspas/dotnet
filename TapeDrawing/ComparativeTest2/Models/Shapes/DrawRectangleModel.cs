using System.ComponentModel;
using ComparativeTest2.Models.Instruments;
using ComparativeTest2.Models.Primitives;

namespace ComparativeTest2.Models.Shapes
{
	/// <summary>
	/// Незакрашенный прямоугольник
	/// </summary>
	public class DrawRectangleModel : BaseModel
	{
		public DrawRectangleModel()
		{
			Pen = new PenModel();
			Rectangle = new RectangleModel();
		}

		/// <summary>
		/// Кисть рисования
		/// </summary>
		[DisplayName("Кисть рисования")]
		[Description("Кисть рисования")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PenModel Pen { get; set; }

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
			return "Незакрашенный прямоугольник";
		}

		/// <summary>
		/// Позволяет получить описание фигуры
		/// </summary>
		/// <returns></returns>
		public override string GetDescription()
		{
			return string.Format("{0}, {1}", Rectangle, Pen);
		}
	}
}
