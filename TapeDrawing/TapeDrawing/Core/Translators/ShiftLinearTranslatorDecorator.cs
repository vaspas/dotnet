
namespace TapeDrawing.Core.Translators
{
    class ShiftLinearTranslatorDecorator : ILinearTranslator
    {
        private ILinearTranslator _internal;
        public ILinearTranslator Internal
        {
            get { return _internal; }
            set { _internal = value; }
        }

        private float _shift;
        public float Shift
        {
            get { return _shift; }
            set { _shift = value; }
        }

        public float SrcFrom
        {
            get { return Internal.SrcFrom; }
            set { Internal.SrcFrom = value; }
        }
        public float SrcTo
        {
            get { return Internal.SrcTo; }
            set { Internal.SrcTo = value; }
        }

        public float DstFrom
        {
            get { return Internal.DstFrom; }
            set { Internal.DstFrom = value; }
        }
        public float DstTo
        {
            get { return Internal.DstTo; }
            set { Internal.DstTo = value; }
        }

        public float Translate(float val)
        {
            return _internal.Translate(val)+_shift;
        }
    }
}
