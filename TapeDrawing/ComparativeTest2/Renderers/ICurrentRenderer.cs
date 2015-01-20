using ComparativeTest2.Models;
using TapeDrawing.Core;

namespace ComparativeTest2.Renderers
{
	/// <summary>
	/// Интерфейс рендереров программы
	/// </summary>
	interface ICurrentRenderer : IRenderer
	{
		BaseModel Model { get; set; }
	}
}
