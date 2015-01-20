
namespace TapeDrawing.Core.Translators
{
    public interface ILinearTranslator
    {
        float SrcFrom { get; set; }
        float SrcTo { get; set; }

        float DstFrom { get; set; }
        float DstTo { get; set; }

        float Translate(float val);
    }
}
