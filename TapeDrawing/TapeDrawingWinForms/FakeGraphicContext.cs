using TapeDrawing.Core;
using TapeDrawing.Core.Shapes;

namespace TapeDrawingWinForms
{
    /// <summary>
    /// 
    /// </summary>
    internal class FakeGraphicContext:GraphicContext
    {
        public override IShapesFactory Shapes
        {
            get { return new FakeShapesFactory(); }
        }

        public override IClip CreateClip()
        {
            return new FakeClip();
        }
    }
}
