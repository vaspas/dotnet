using System.ComponentModel;
using ComparativeTest2.Models.Primitives;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Instruments
{
	/// <summary>
	/// Шрифт текста
	/// </summary>
	public class FontModel
	{
		public FontModel()
		{
			Type = "Times New Roman";
			Color = new ColorModel {A = 255, R = 255, G = 0, B = 0};
			Size = 10;
		}

		/// <summary>
		/// Тип шрифта
		/// </summary>
		[DisplayName("Тип шрифта")]
		[Description("Тип шрифта")]
		public string Type { get; set; }

		/// <summary>
		/// Цвет текста
		/// </summary>
		[DisplayName("Цвет текста")]
		[Description("Цвет текста")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorModel Color { get; set; }

		/// <summary>
		/// Размер текста
		/// </summary>
		[DisplayName("Размер текста")]
		[Description("Размер текста")]
		public int Size { get; set; }

		/// <summary>
		/// Стиль шрифта
		/// </summary>
		[DisplayName("Стиль")]
		[Description("Стиль")]
		public FontStyle Style { get; set; }

		public override string ToString()
		{
			return string.Format("({0}, {1}, size={2}, {3})", Type, Color, Size, Style);
		}
	}
}
