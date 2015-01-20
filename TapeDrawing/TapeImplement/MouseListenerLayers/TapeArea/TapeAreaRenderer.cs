using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.MouseListenerLayers.TapeArea
{
    
    public class TapeAreaRenderer:IRenderer
    {
        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        public int PositionFrom;
        public int PositionTo;

        public Color Color;

        public IPointTranslator Translator;
        
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Dst = rect;
            Translator.Src = new Rectangle<float>{Left = TapePosition.From, Right = TapePosition.To, Bottom = 0f, Top = 1f};

            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator).Result;

            using (var pen = gr.Instruments.CreateSolidBrush(Color))
            using (var shape = shapes.CreateFillRectangle(pen))
            {
                shape.Render(new Rectangle<float> 
                {
                    Left = Math.Max(PositionFrom,TapePosition.From) , 
                    Right = Math.Min(PositionTo, TapePosition.To),
                    Bottom = 0f, Top = 1f
                });
            }
        }
    }
}
