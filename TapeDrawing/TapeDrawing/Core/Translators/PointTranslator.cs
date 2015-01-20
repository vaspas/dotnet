using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Translators
{
    class PointTranslator:IPointTranslatorInternal
    {
        private Rectangle<float> _src;
        public Rectangle<float> Src
        {
            get { return _src; }
            set { _src = value; }
        }

        private Rectangle<float> _dst;
        public Rectangle<float> Dst
        {
            get { return _dst; }
            set { _dst = value; }
        }

        private ILinearTranslator _translatorX;
        public ILinearTranslator TranslatorX
        {
            get { return _translatorX; }
            set { _translatorX = value; }
        }

        private ILinearTranslator _translatorY;
        public ILinearTranslator TranslatorY
        {
            get { return _translatorY; }
            set { _translatorY = value; }
        }


        public Point<float> Translate(Point<float> val)
        {
            _translatorX.SrcFrom = _src.Left;
            _translatorX.SrcTo = _src.Right;

            _translatorX.DstFrom = _dst.Left;
            _translatorX.DstTo = _dst.Right;

            _translatorY.SrcFrom = _src.Bottom;
            _translatorY.SrcTo = _src.Top;

            _translatorY.DstFrom = _dst.Bottom;
            _translatorY.DstTo = _dst.Top;

            return new Point<float>
                       {
                           X = _translatorX.Translate(val.X),
                           Y = _translatorY.Translate(val.Y)
                       };
        }
    }
}
