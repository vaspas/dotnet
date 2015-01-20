using System.ComponentModel;
using ComparativeTest2.Models.Primitives;

namespace ComparativeTest2.Models.Instruments
{
	/// <summary>
	/// Кисть для закрашивания фигур
	/// </summary>
	public class BrushModel
	{
		public BrushModel()
		{
			Color = new ColorModel();
		}

		/// <summary>
		/// Цвет
		/// </summary>
		[DisplayName("Цвет заливки")]
		[Description("Цвет заливки")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ColorModel Color { get; set; }

		public override string ToString()
		{
			return string.Format("{0}", Color);
		}
	}
}
