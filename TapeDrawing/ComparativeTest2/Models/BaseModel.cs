using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using ComparativeTest2.Models.Shapes;
using ComparativeTest2.Ui;

namespace ComparativeTest2.Models
{
	/// <summary>
	/// Базовая модель
	/// </summary>
	[XmlInclude(typeof(FillAllModel))]
	[XmlInclude(typeof(DrawRectangleModel))]
    [XmlInclude(typeof(DrawRectangleAreaModel))]
	[XmlInclude(typeof(FillRectangleModel))]
    [XmlInclude(typeof(FillRectangleAreaModel))]
	[XmlInclude(typeof(TextShapeModel))]
	[XmlInclude(typeof(LinesModel))]
	[XmlInclude(typeof(LinesArrayModel))]
	[XmlInclude(typeof(PolygonModel))]
	[XmlInclude(typeof(ImageModel))]
	public class BaseModel : IListViewObject
	{
		/// <summary>
		/// Позволяет получить название фигуры. Если пользователь задал ей свое имя,
		/// то отображается оно. Если не задал - то стандартное имя
		/// </summary>
		/// <returns></returns>
		public string GetName()
		{
			if (string.IsNullOrEmpty(CustomName))
				return GetShapeName();
			return CustomName;
		}

		/// <summary>
		/// Позволяет получить описание фигуры
		/// </summary>
		/// <returns></returns>
		public virtual string GetDescription()
		{
			return null;
		}

		/// <summary>
		/// Имя фигуры
		/// </summary>
		public virtual string GetShapeName()
		{
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		[DisplayName(" Пользовательское имя")]
		[Description("Пользовательское имя")]
		public string CustomName { get; set; }

		#region Implementation of IListViewObject

		/// <summary>
		/// Список названий колонок объекта
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		public List<ColumnHeader> LvHeaderList
		{
			get
			{
				var headers = new List<ColumnHeader>();

				var header = new ColumnHeader { Text = "Фигура", Width = 200, Name = "Name" };
				headers.Add(header);
				header = new ColumnHeader { Text = "Описание", Width = 400, Name = "Description" };
				headers.Add(header);

				return headers;
			}
		}

		/// <summary>
		/// Элемент списка, который соответствует объекту
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		public ListViewItem LvItem
		{
			get
			{
				// Создадим элемент
				var listViewItem = new ListViewItem
				{
					Text = GetName(),
					ImageIndex = 0
				};

				// Создадим колонки
				listViewItem.SubItems[listViewItem.SubItems.Count - 1].Name = "Name";

				listViewItem.SubItems.Add(GetDescription());
				listViewItem.SubItems[listViewItem.SubItems.Count - 1].Name = "Description";

				// Заполним тэг
				listViewItem.Tag = this;

				return listViewItem;
			}
		}

		#endregion
	}
}
