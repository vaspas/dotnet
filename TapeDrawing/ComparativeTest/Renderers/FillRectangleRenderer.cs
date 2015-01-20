using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest.Renderers
{
    class FillRectangleRenderer : IRenderer
    {
        public int Count { get; set; }
        
        public Color Color { get; set; }

        public Random Random { get; set; }

        public IPointTranslator Translator { get; set; }

        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = new Rectangle<float> {Left = 0, Right = 1, Bottom = 0, Top = 1};
            Translator.Dst = rect;

            var shapes = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;

            using (var brush=gr.Instruments.CreateSolidBrush(Color))
            using (var shape = shapes.CreateFillRectangle(brush))
            {
                GenerateRectangles();

                Array.ForEach(_rectangles, shape.Render);
            }
        }

        private Rectangle<float>[] _rectangles;

        private void GenerateRectangles()
        {
            if (_rectangles == null)
            {
                _rectangles = new Rectangle<float>[Count];
                for (int i = 0; i < _rectangles.Length; i++)
                    _rectangles[i] = new Rectangle<float>
                                         {
                                             Left = (float) Random.NextDouble(),
                                             Right = (float) Random.NextDouble(),
                                             Bottom = (float) Random.NextDouble(),
                                             Top = (float) Random.NextDouble()
                                         };
            }

            for (int i = 0; i < _rectangles.Length; i++)
            {
                _rectangles[i].Bottom -= GenerateShift();
                _rectangles[i].Top += GenerateShift();
                _rectangles[i].Left -= GenerateShift();
                _rectangles[i].Right += GenerateShift();
            }
        }

        private float GenerateShift()
        {
            return ((DateTime.Now.Millisecond % 1000) / 1000f - 0.5f) / 100f;
        }
    }
}
