
namespace TapeDrawing.Core.Translators
{
    class LinearTranslator:ILinearTranslator
    {
        private float _srcFrom;
        public float SrcFrom
        {
            get { return _srcFrom; }
            set { _srcFrom = value; }
        }
        private float _srcTo;
        public float SrcTo
        {
            get { return _srcTo; }
            set { _srcTo = value; }
        }

        private float _dstFrom;
        public float DstFrom
        {
            get { return _dstFrom; }
            set { _dstFrom = value; }
        }
        private float _dstTo;
        public float DstTo
        {
            get { return _dstTo; }
            set { _dstTo = value; }
        }

        public float Translate(float val)
        {
            var k = _srcFrom == _srcTo ? 0 : (double) (val - _srcFrom)/(_srcTo - _srcFrom);
            return (float)(k*(_dstTo - _dstFrom) + _dstFrom);
        }
    }
}
