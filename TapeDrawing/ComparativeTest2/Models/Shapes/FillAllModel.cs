using System.ComponentModel;
using ComparativeTest2.Models.Primitives;

namespace ComparativeTest2.Models.Shapes
{
	/// <summary>
	/// Модель сплошной заливки
	/// </summary>
	public class FillAllModel : BaseModel
	{
		public FillAllModel()
		{
			Color = new ColorModel {R = 255, G = 255, B = 255};
		}

		/// <summary>
		/// Цвет
		/// </summary>
		[DisplayName("Цвет заливки")]
		[Description("Цвет заливки")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorModel Color { get; set; }

		/// <summary>
		/// Имя фигуры
		/// </summary>
		public override string GetShapeName()
		{
			return "Сплошная заливка";
		}

		/// <summary>
		/// Позволяет получить описание фигуры
		/// </summary>
		/// <returns></returns>
		public override string GetDescription()
		{
			return string.Format("Фон {0}", Color);
		}
	}
}
