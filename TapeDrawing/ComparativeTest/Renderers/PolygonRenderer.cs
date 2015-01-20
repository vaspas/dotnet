using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest.Renderers
{
    class PolygonRenderer:IRenderer
    {
        public int Count { get; set; }
        
        public Color Color { get; set; }

        public Random Random { get; set; }

        public IPointTranslator Translator { get; set; }

        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = new Rectangle<float> { Left = 0, Right = 1, Bottom = 0, Top = 1 };
            Translator.Dst = rect;

            var shapes = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;

            using (var brush=gr.Instruments.CreateSolidBrush(Color))
            using (var shape = shapes.CreatePolygon(brush))
            {
                GeneratePoints();
                
                if(_points.Length>=2)
                    shape.Render(_points);
            }
        }

        private Point<float>[] _points;

        private void GeneratePoints()
        {
            if(_points==null)
            {
                _points=new Point<float>[Count+1];

                for (int i = 0; i < _points.Length; i++)
                    _points[i] = new Point<float> { X = (float)Random.NextDouble(), Y = (float)Random.NextDouble() };
            }
            
            for (int i = 0; i < _points.Length; i++)
            {
                _points[i].X = GenerateShift(_points[i].X);
                _points[i].Y = GenerateShift(_points[i].Y);
            }
        }

        private float GenerateShift(float value)
        {
            var newValue = value + (float)Random.NextDouble() / 100 - 0.005f;

            return newValue;
        }
    }
}
