using System.Collections.Generic;
using System.Windows.Forms;

namespace ComparativeTest2.Ui
{
	/// <summary>
	/// Интерфейс объекта, который может быть элементом списка
	/// </summary>
	public interface IListViewObject
	{
		/// <summary>
		/// Список названий колонок объекта
		/// </summary>
		List<ColumnHeader> LvHeaderList { get; }
		/// <summary>
		/// Элемент списка, который соответствует объекту
		/// </summary>
		ListViewItem LvItem { get; }
	}
}