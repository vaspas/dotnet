using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Translators
{
    class ChangeAxelsPointTranslatorDecorator : IPointTranslatorInternal
    {
        private IPointTranslatorInternal _internal;
        public IPointTranslatorInternal Internal
        {
            get { return _internal; }
            set { _internal = value; }
        }


        public Rectangle<float> Src
        {
            get { return _internal.Src; }
            set { _internal.Src = value; }
        }

        public Rectangle<float> Dst
        {
            get { return _internal.Dst; }
            set { _internal.Dst = value; }
        }


        public ILinearTranslator TranslatorX
        {
            get { return _internal.TranslatorY; }
            set { _internal.TranslatorY = value; }
        }

        public ILinearTranslator TranslatorY
        {
            get { return _internal.TranslatorX; }
            set { _internal.TranslatorX = value; }
        }


        public Point<float> Translate(Point<float> val)
        {
            var tx = _internal.TranslatorY;
            var ty = _internal.TranslatorX;
            var src = _internal.Src;
            var dst = _internal.Dst;

            tx.SrcFrom = src.Bottom;
            tx.SrcTo = src.Top;
            tx.DstFrom = dst.Left;
            tx.DstTo = dst.Right;

            ty.SrcFrom = src.Left;
            ty.SrcTo = src.Right;
            ty.DstFrom = dst.Bottom;
            ty.DstTo = dst.Top;

            return new Point<float>
            {
                X = tx.Translate(val.Y),
                Y = ty.Translate(val.X)
            };
        }
    }
}
