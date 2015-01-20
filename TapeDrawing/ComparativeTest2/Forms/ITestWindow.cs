using System;
using System.Drawing;

namespace ComparativeTest2.Forms
{
	/// <summary>
	/// Интерфейс тестового окна
	/// </summary>
	interface ITestWindow
	{
		/// <summary>
		/// Фабрика создания слоев
		/// </summary>
		MainLayerFactory Factory { set; }

		/// <summary>
		/// Расположение окна
		/// </summary>
		Point FormLocation { get; set; }

		/// <summary>
		/// Размер окна
		/// </summary>
		Size FormSize { get; set; }

		/// <summary>
		/// Открывает окно
		/// </summary>
		void Open();

		/// <summary>
		/// Закрывает окно
		/// </summary>
		void Close();

		/// <summary>
		/// Заставляет перерисовать окно
		/// </summary>
		void Redraw();

		/// <summary>
		/// Активировать окно
		/// </summary>
		void ActivateForm();

		/// <summary>
		/// Событие того, что окно закрыто
		/// </summary>
		event EventHandler Closed; 
	}
}
