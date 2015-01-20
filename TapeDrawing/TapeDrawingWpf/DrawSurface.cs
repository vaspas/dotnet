namespace TapeDrawingWpf
{
	/// <summary>
	/// Поверхность для рисования, на которую фигуры рендерят себя
	/// </summary>
	class DrawSurface
	{
		/// <summary>
		/// Контекст рисования wpf
		/// </summary>
		public System.Windows.Media.DrawingContext Context { get; set; }

		/// <summary>
		/// Максимальная ширина изображения
		/// </summary>
		public float Width { get; set; }
		/// <summary>
		/// Максимальная высота изображения
		/// </summary>
		public float Height { get; set; }
	}
}
