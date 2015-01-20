using System.ComponentModel;
using ComparativeTest2.Models.Primitives;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Instruments
{
	/// <summary>
	/// Кисть рисования линий
	/// </summary>
	public class PenModel
	{
		public PenModel()
		{
			Color = new ColorModel();
			Width = 1;
		}

		/// <summary>
		/// Цвет
		/// </summary>
		[DisplayName("Цвет линий")]
		[Description("Цвет линий")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorModel Color { get; set; }

		/// <summary>
		/// Толщина линии
		/// </summary>
		[DisplayName("Толщина линии")]
		[Description("Толщина линии")]
		public int Width { get; set; }

		/// <summary>
		/// Стиль линии
		/// </summary>
		[DisplayName("Стиль линии")]
		[Description("Стиль линии")]
		public LineStyle Style { get; set; }

		public override string ToString()
		{
			return string.Format("{0}, Толщ={1}, {2}", Color, Width, Style);
		}
	}
}
