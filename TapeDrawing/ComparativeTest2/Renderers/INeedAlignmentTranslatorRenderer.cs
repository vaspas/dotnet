using TapeDrawing.Core.Translators;

namespace ComparativeTest2.Renderers
{
	/// <summary>
	/// Рендерер, которому нужен транслятор положения для работы
	/// </summary>
	interface INeedAlignmentTranslatorRenderer
	{
		IAlignmentTranslator AlignmentTranslator { get; set; }
	}
}
