
namespace TapeDrawing.Core.Translators
{
     class InverseLinearTranslatorDecorator : ILinearTranslator
    {
        private ILinearTranslator _internal;
        public ILinearTranslator Internal
        {
            get { return _internal; }
            set { _internal = value; }
        }

        public float SrcFrom
        {
            get { return _internal.SrcTo; }
            set { _internal.SrcTo = value; }
        }
        public float SrcTo
        {
            get { return _internal.SrcFrom; }
            set { _internal.SrcFrom = value; }
        }

        public float DstFrom
        {
            get { return _internal.DstFrom; }
            set { _internal.DstFrom = value; }
        }
        public float DstTo
        {
            get { return _internal.DstTo; }
            set { _internal.DstTo = value; }
        }

        public float Translate(float val)
        {
            return _internal.Translate(val);
        }
    }
}
