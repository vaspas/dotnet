using System;
using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace ComparativeTest.Renderers
{
    class PointsRenderer : IRenderer
    {
        public Random Random { get; set; }

        public IPointTranslator Translator { get; set; }

        public void Draw(IGraphicContext gr, IRectangle<float> rect)
        {
            Translator.Dst = rect;
            Translator.Src = rect;

            var shapes = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;

            using (var shape = shapes.CreatePoints())
            {
                var size = (rect.Right - rect.Left + 1)*(rect.Top - rect.Bottom + 1);

                if (_points == null || _points.Count != size)
                {
                    GeneratePoints(rect);
                    GenerateColors(rect);
                }

                shape.Render(_points, _colors);
            }
        }

        private List<IPoint<float>> _points;
        private List<IColor> _colors;

        private void GeneratePoints(IRectangle<float> rect)
        {
            _points = new List<IPoint<float>>();
            
                for(float x=rect.Left;x<=rect.Right;x++)
                    for(float y=rect.Bottom;y<=rect.Top;y++)
                        _points.Add(PrimitivesFactory.CreatePoint(x, y));
            
        }

        private void GenerateColors(IRectangle<float> rect)
        {
            _colors = new List<IColor>();

                for (float x = rect.Left; x <= rect.Right; x++)
                    for (float y = rect.Bottom; y <= rect.Top; y++)
                        _colors.Add(PrimitivesFactory.CreateColor(
                            (byte) (Random.Next()%byte.MaxValue),
                            (byte) (Random.Next()%byte.MaxValue),
                            (byte) (Random.Next()%byte.MaxValue),
                            (byte) (Random.Next()%byte.MaxValue)));
            
        }
    }
}
