using System.ComponentModel;
using ComparativeTest2.Models.Instruments;
using ComparativeTest2.Models.Primitives;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Shapes
{
	/// <summary>
	/// Текст
	/// </summary>
	public class TextShapeModel : BaseModel
	{
		public TextShapeModel()
		{
			Font = new FontModel();
			Text = "";
			Point = new PointModel();
		}

		/// <summary>
		/// Шрифт текста
		/// </summary>
		[DisplayName("Шрифт")]
		[Description("Шрифт")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public FontModel Font { get; set; }

		/// <summary>
		/// Тип выравнивания
		/// </summary>
		[DisplayName("Выравнивание")]
		[Description("Выравнивание")]
		[Editor(typeof(Ui.FlagEnumUiEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public Alignment Alignment { get; set; }

		/// <summary>
		/// Угол поворота
		/// </summary>
		[DisplayName("Угол поворота")]
		[Description("Угол поворота")]
		public float Angle { get; set; }

		/// <summary>
		/// Отображаемый текст
		/// </summary>
		[DisplayName("Текст")]
		[Description("Текст")]
		public string Text { get; set; }

		/// <summary>
		/// Координаты отображения текста
		/// </summary>
		[DisplayName("Координаты текста")]
		[Description("Координаты текста")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PointModel Point { get; set; }

		/// <summary>
		/// Имя фигуры
		/// </summary>
		public override string GetShapeName()
		{
			return "Текст";
		}

		/// <summary>
		/// Позволяет получить описание фигуры
		/// </summary>
		/// <returns></returns>
		public override string GetDescription()
		{
			return string.Format("\"{0}\" {1}, {2}deg, {3}, {4}", Text, Point, Angle, Font, Alignment);
		}
	}
}
