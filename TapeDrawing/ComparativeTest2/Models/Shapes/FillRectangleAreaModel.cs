using System.ComponentModel;
using ComparativeTest2.Models.Instruments;
using ComparativeTest2.Models.Primitives;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Shapes
{
	/// <summary>
	/// Закрашенный прямоугольник
	/// </summary>
	public class FillRectangleAreaModel : BaseModel
	{
		public FillRectangleAreaModel()
		{
			Brush = new BrushModel();
			Point = new PointModel();
            Size = new SizeModel();
		}

		/// <summary>
		/// Кисть рисования
		/// </summary>
		[DisplayName("Кисть заливки")]
		[Description("Кисть заливки")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BrushModel Brush { get; set; }

		/// <summary>
        /// Координата
		/// </summary>
		[DisplayName("Координата")]
		[Description("Координата")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PointModel Point { get; set; }

        /// <summary>
        /// Размер
        /// </summary>
        [DisplayName("Размер")]
        [Description("Размер")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SizeModel Size { get; set; }

        /// <summary>
        /// Тип выравнивания
        /// </summary>
        [DisplayName("Выравнивание")]
        [Description("Выравнивание")]
        [Editor(typeof(Ui.FlagEnumUiEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Alignment Alignment { get; set; }

		/// <summary>
		/// Имя фигуры
		/// </summary>
		public override string GetShapeName()
		{
			return "Область закрашенный прямоугольник";
		}

		/// <summary>
		/// Позволяет получить описание фигуры
		/// </summary>
		/// <returns></returns>
		public override string GetDescription()
		{
            return string.Format("{0}, {1}, {2}, {3}", Point, Size, Brush, Alignment);
		}
	}
}
