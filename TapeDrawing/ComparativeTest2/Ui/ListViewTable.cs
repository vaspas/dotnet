using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using ComparativeTest2.Models;

namespace ComparativeTest2.Ui
{
	/// <summary>
	/// Делегат метода с двумя аргументами
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <param name="arg1"></param>
	/// <param name="arg2"></param>
	public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);

	/// <summary>
	/// Делегат метода с тремя аргументами
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <param name="arg1"></param>
	/// <param name="arg2"></param>
	/// <param name="arg3"></param>
	public delegate void Action<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);

	/// <summary>
	/// Это ListView для отображения объектов BaseModel
	/// </summary>
	public class ListViewTable : ListViewTable<BaseModel>
	{
	}

	/// <summary>
	/// Это ListView для отображения объектов IListViewObject
	/// </summary>
	public class ListViewTable<T> : ListView where T : class, IListViewObject, new()
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public ListViewTable()
		{
			SetDefaultStyle();
			SelectedIndexChanged += ListViewTableSelectedIndexChanged;
			MouseDown += ListViewTableMouseDown;
			MouseUp += ListViewTableMouseUp;
			ItemActivate += ListViewTableItemActivate;
		}

		/// <summary>
		/// Список объектов, которые должны отображаться в списке
		/// </summary>
		public List<T> ObjectList { get; set; }

		/// <summary>
		/// Обновляет список
		/// </summary>
		public void UpdateList()
		{
			BeginUpdate();

			// Очистим содержимое
			Items.Clear();
			if (ObjectList == null)
			{
				EndUpdate();
				return;
			}

			ChangeColumns();

			// Заполним новое содержимое
			foreach (var listViewObject in ObjectList)
				Items.Add(listViewObject.LvItem);

			EndUpdate();
		}

		/// <summary>
		/// Сбрасывает выделение
		/// </summary>
		public void ResetSelection()
		{
			SelectedItems.Clear();
		}
		/// <summary>
		/// Выбирает в списке объект
		/// </summary>
		/// <param name="obj">Объект, который нужно выбрать</param>
		public void SelectObject(object obj)
		{
			foreach (ListViewItem item in Items)
			{
				if (item.Tag != obj) continue;

				item.EnsureVisible();
				item.Selected = true;

				break;
			}
		}
		/// <summary>
		/// Выбирает в списке объекты
		/// </summary>
		/// <param name="objects">Объекты, которые нужно выбрать</param>
		public void SelectObjects(List<object> objects)
		{
			foreach (var obj in objects)
			{
				foreach (ListViewItem item in Items)
				{
					if (item.Tag != obj) continue;

					item.EnsureVisible();
					item.Selected = true;

					break;
				}
			}
		}
		/// <summary>
		/// Позволяет узать текущий выбранный объект в листбоксе
		/// </summary>
		public object SelectedObject
		{
			get
			{
				return SelectedItems.Count == 0 ? null : SelectedItems[0].Tag;
			}
		}
		/// <summary>
		/// Позволяет создать новый список объектов из текущего состояния списка
		/// </summary>
		/// <returns>Список объектов</returns>
		public List<T> GetListObjects()
		{
			return CreateListViewObjects();
		}

		/// <summary>
		/// Событие вызывается когда пользователь выбирает объект
		/// </summary>
		public event Action<object> ObjectSеlected;
		/// <summary>
		/// Событие вызывается когда пользователь выбирает объекты
		/// </summary>
		public event Action<object[]> ObjectsSеlected;
		/// <summary>
		/// По объекту щелкнули 2 раза или нажали Enter
		/// </summary>
		public event Action<object, string> ObjectInAction;
		/// <summary>
		/// Событие "Пользователь нажал кнопку мыши"
		/// </summary>
		public event Action<object, MouseButtons, Point> ObjectMouseDown;
		/// <summary>
		/// Событие "Пользователь отпустил кнопку мыши"
		/// </summary>
		public event Action<MouseButtons> ObjectItemMouseUp;

		public void AddListItem(T obj)
		{
			Items.Add(obj.LvItem);
		}
		public void UpSelectedListItem()
		{
			if (SelectedItems.Count == 0) return;

			// Получим индекс
			int index = Items.IndexOf(SelectedItems[0]);

			// Если это первый элемент, то некуда двигать
			if (index == 0) return;

			// Иначе меняем с предыдущим элементом
			BeginUpdate();

			var buf = Items[index - 1];
			var buf2 = Items[index];

			Items.RemoveAt(index - 1);
			Items.RemoveAt(index - 1);

			Items.Insert(index - 1, buf2);
			Items.Insert(index, buf);

			buf2.Selected = true;

			EndUpdate();
		}
		public void DownSelectedItem()
		{
			// Если ничего не выбрано
			if (SelectedItems.Count == 0) return;

			// Получим индекс
			int index = Items.IndexOf(SelectedItems[0]);

			// Если это крайний элемент, то некуда двигать
			if (index == (Items.Count - 1)) return;

			// Иначе меняем с предыдущим элементом
			BeginUpdate();

			var buf = Items[index + 1];
			var buf2 = Items[index];

			Items.RemoveAt(index);
			Items.RemoveAt(index);

			Items.Insert(index, buf);
			Items.Insert(index + 1, buf2);

			buf2.Selected = true;

			EndUpdate();
		}
		public void RemoveSeletedItem()
		{
			if (SelectedItems.Count == 0) return;

			// Получим индекс
			int index = Items.IndexOf(SelectedItems[0]);

			BeginUpdate();

			ListViewItem next;
			if (index < (Items.Count - 1)) next = Items[index + 1];
			else if (index > 0) next = Items[index - 1];
			else next = null;
			
			Items.RemoveAt(index);

			if (next == null)
			{
				if (ObjectSеlected != null) ObjectSеlected(null);
			}
			else
			{
				next.Selected = true;
			}

			EndUpdate();
		}
		public void ChangeProperty(T obj)
		{
			var oldItem = FindObject( obj);
			if (oldItem == null) return;

			var item = obj.LvItem;
			oldItem.Text = item.Text;
			oldItem.ImageIndex = item.ImageIndex;

			// Очистим все SubItems кроме первого
			int count = oldItem.SubItems.Count;
			for (int i = 0; i < count;i++ )
			{
				oldItem.SubItems[i] = item.SubItems[i];
			}
			//foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
			//{
			//	oldItem.SubItems.Add(subItem);
			//}
		}

		#region - Закрытые методы -
		/// <summary>
		/// Выполняет поиск объекта в узлах
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private ListViewItem FindObject(T obj)
		{
			return Items.Cast<ListViewItem>().FirstOrDefault(item => item.Tag == obj);
		}

		/// <summary>
		/// Крайняя информация о нажатии мыши
		/// </summary>
		private ListViewHitTestInfo _lastInfo;
		/// <summary>
		/// Устанавливает стиль по умолчанию
		/// </summary>
		private void SetDefaultStyle()
		{
			GridLines = true;
			ShowGroups = false;
			ShowItemToolTips = true;
			UseCompatibleStateImageBehavior = false;
			View = View.Details;
			FullRowSelect = true;
			//MultiSelect = false;

		}
		/// <summary>
		/// Вызывается при выборе пользователем элемента
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListViewTableSelectedIndexChanged(object sender, EventArgs e)
		{
			object obj = null;
			if (SelectedItems.Count != 0) obj = SelectedItems[0].Tag;

			// Вызовим метод для одного объекта
			if (ObjectSеlected != null) ObjectSеlected(obj);

			// Запустим метод для группы объектов
			if (ObjectsSеlected != null)
			{
				if (SelectedItems.Count == 0)
				{
					ObjectsSеlected(null);
					return;
				}
				var array = new object[SelectedItems.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = SelectedItems[i].Tag;
				}
				ObjectsSеlected(array);
			}
		}
		/// <summary>
		/// Применяет колонки
		/// </summary>
		private void ChangeColumns()
		{
			if (ObjectList == null) return;
			if (ObjectList.Count == 0) return;
			if (Columns.Count > 0) return;

			foreach (var header in ObjectList[0].LvHeaderList)
				Columns.Add(header);
		}
		/// <summary>
		/// Вызывается когда пользователь отпускает кнопку мыши
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListViewTableMouseUp(object sender, MouseEventArgs e)
		{
			if (ObjectItemMouseUp != null) ObjectItemMouseUp(e.Button);
		}
		/// <summary>
		/// Вызывается когда пользователь нажимает кнопку мыши
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListViewTableMouseDown(object sender, MouseEventArgs e)
		{
			_lastInfo = HitTest(e.X, e.Y);

			var lvItem = GetItemAt(e.Location.X, e.Location.Y);
			if (lvItem == null) return;

			if (ObjectMouseDown != null) ObjectMouseDown(lvItem.Tag, e.Button, PointToScreen(e.Location));
		}
		/// <summary>
		/// Элемент выбран и над ним производится действие
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListViewTableItemActivate(object sender, EventArgs e)
		{
			// Определим по какой колонке щелкнули
			var columnName = "";
			if (_lastInfo != null)
			{
				if (_lastInfo.SubItem != null)
					columnName = _lastInfo.SubItem.Name;
				_lastInfo = null;
			}

			if (SelectedItems.Count == 0) return;
			if (ObjectInAction != null) ObjectInAction(SelectedItems[0].Tag, columnName);
		}
		/// <summary>
		/// Позволяет получить список объектов, которые сейчас в контроле
		/// </summary>
		/// <returns>Список объектов контрола</returns>
		private List<T> CreateListViewObjects()
		{
			return (from ListViewItem item in Items select (T)item.Tag).ToList();
		}

		#endregion
	}
}