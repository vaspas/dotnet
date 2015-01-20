using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest.Renderers
{
    class TextRenderer : IRenderer
    {
        public int Count { get; set; }

        public int Size { get; set; }

        public string Text { get; set; }

        public Color Color { get; set; }

        public FontStyle Style { get; set; }

        public Random Random { get; set; }

        public IPointTranslator Translator { get; set; }

        public Alignment Alignment { get; set; }

        public IAlignmentTranslator AlignmentTranslator { get; set; }

        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = new Rectangle<float> { Left = 0, Right = 1, Bottom = 0, Top = 1 };
            Translator.Dst = rect;

            var shapes = ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator)
                .Translate(AlignmentTranslator)
                .Result;

            using (var font=gr.Instruments.CreateFont("Arial", Size, Color, Style))
            using (var shape = shapes.CreateText(font, Alignment, (DateTime.Now.Ticks / 100000) % 360))
            {
                GeneratePoints();

                Array.ForEach(_points, p=> shape.Render(Text, p));
            }
        }

        private Point<float>[] _points;

        private void GeneratePoints()
        {
            if (_points == null)
            {
                _points = new Point<float>[Count];

                for (int i = 0; i < _points.Length; i++)
                    _points[i] = new Point<float> { X = (float)Random.NextDouble(), Y = (float)Random.NextDouble() };
            }

        }

    }
}
