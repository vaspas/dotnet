using System;
using System.Drawing;
using System.IO;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest.Renderers
{
    class ImageRenderer : IRenderer
    {
        public int Count { get; set; }
        
        public Random Random { get; set; }

        public Bitmap Image { get; set; }

        public Stream ImageStream { get; set; }

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

            //using (var image = gr.Instruments.CreateImage(Image))
            using (var image = gr.Instruments.CreateImage(ImageStream))
            using (var shape = shapes.CreateImage(image, Alignment, -(DateTime.Now.Ticks / 100000) % 360))
            {
                GeneratePoints();

                Array.ForEach(_points, shape.Render);
            }
        }

        private Point<float>[] _points;

        private void GeneratePoints()
        {
            if (_points == null)
            {
                _points = new Point<float>[Count + 1];

                for (int i = 0; i < _points.Length; i++)
                    _points[i] = new Point<float> { X = (float)Random.NextDouble(), Y = (float)Random.NextDouble() };
            }

        }
    }
}
