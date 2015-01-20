using TapeDrawing.Core.Translators;

namespace ComparativeTest2.Renderers
{
	interface INeedPointTranslatorRenderer
	{
		IPointTranslator Translator { get; set; }
	}
}
