using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeImplement;

namespace ComparativeTapeTest.Renderers
{
    class TestPlayerRenderer : IRenderer
    {
        public IScalePosition<int> TapePosition { get; set; }

        public IPointTranslator PointTranslator { get; set; }
        public IAlignmentTranslator AlignmentTranslator { get; set; }
     
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            PointTranslator.Src = new Rectangle<float> { Left = -1, Right = 1, Bottom = -1, Top = 1 };
            PointTranslator.Dst = rect;

            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(PointTranslator)
                .Translate(AlignmentTranslator)
                .Result;

            using(var font=gr.Instruments.CreateFont("Arial",16,new Color(0,0,0),FontStyle.None))
            {
                using (var shape = shapes.CreateText(font, Alignment.Left, 0))
                    shape.Render(TapePosition.From.ToString(), new Point<float>{X=-1,Y=0});
                using (var shape = shapes.CreateText(font, Alignment.Right, 0))
                    shape.Render(TapePosition.To.ToString(), new Point<float> { X = 1, Y = 0 });
            }
        }
    }
}
