
namespace TapeDrawing.Core.Translators
{
    class FakeLinearTranslator : ILinearTranslator
    {
        public float SrcFrom { get; set; }
        public float SrcTo { get; set; }

        public float DstFrom { get; set; }
        public float DstTo { get; set; }

        public float Translate(float val)
        {
            return val;
        }
    }
}
