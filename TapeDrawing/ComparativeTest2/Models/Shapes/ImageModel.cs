using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using ComparativeTest2.Models.Primitives;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Shapes
{
	public class ImageModel : BaseModel
	{
		public ImageModel()
		{
			Image = new Bitmap(1, 1);
			Point = new PointModel();
            Roi=new RectangleModel();
		}

		/// <summary>
		/// Изображение
		/// </summary>
		[DisplayName("Изображение")]
		[Description("Изображение")]
		[XmlIgnore]
		public Bitmap Image { get; set; }

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
		/// Координаты отображения
		/// </summary>
		[DisplayName("Координаты")]
		[Description("Координаты")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public PointModel Point { get; set; }

        /// <summary>
        /// Координаты отображения
        /// </summary>
        [DisplayName("Сегмент текстуры")]
        [Description("Сегмент текстуры")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RectangleModel Roi { get; set; }

		/// <summary>
		/// Изображение для сохранения в XML
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[XmlElement("Image")]
		public byte[] ImageSerialized
		{
			get
			{
				if (Image == null) return null;
				using (var ms = new MemoryStream())
				{
					Image.Save(ms, ImageFormat.Png);
					return ms.ToArray();
				}
			}
			set
			{
				if (value == null)
				{
					Image = null;
				}
				else
				{
					using (var ms = new MemoryStream(value))
					{
						Image = new Bitmap(ms);
					}
				}
			}
		}

		/// <summary>
		/// Имя фигуры
		/// </summary>
		public override string GetShapeName()
		{
			return "Рисунок";
		}

		/// <summary>
		/// Позволяет получить описание фигуры
		/// </summary>
		/// <returns></returns>
		public override string GetDescription()
		{
			return string.Format("Size {0}x{1}, {2}, {3}deg, {4}, {5}", Image.Width, Image.Height, Point, Angle, Alignment, Roi);
		}
	}
}
