using System;
using System.Collections.Generic;
using System.Text;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.TapeModels.Vagon.ToolTip
{
    class ToolTipRenderer : IRenderer, IMouseMoveListener
    {
        public Action Redraw { get; set; }


        public IList<Func<string>> Objects
        {
            get { return _objects; }
        }
        private readonly List<Func<string>> _objects = new List<Func<string>>();

        public IPointTranslator Translator { get; set; }

        public Alignment TextAlignment { get; set; }

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            Translator.Src = rect;
            Translator.Dst = rect;

            if (Objects.Count==0 || _lastPoint==null)
            {
                _lastShow = false;
                return;
            }

            var sb = new StringBuilder();
            _objects.ForEach(o => sb.AppendLine(o()));
            sb.Remove(sb.Length - 1, 1);

            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator
                .For(gr.Shapes).Translate(Translator).Result;

            var text = sb.ToString();

           using (var font = gr.Instruments.CreateFont("Nina", 9, new Color(0,0,0), FontStyle.None))
           using (var foneBrush = gr.Instruments.CreateSolidBrush(new Color(255, 255, 255)))
           using (var pen = gr.Instruments.CreatePen(new Color(0,0,0),1,LineStyle.Solid))
           using (var shapeText = shapes.CreateText(font, TextAlignment, 0))
           using (var shapeRect = shapes.CreateFillRectangleArea(foneBrush, TextAlignment))
           using (var shapeBorder = shapes.CreateDrawRectangleArea(pen, TextAlignment))
           {
               var margin = 3;

               var sz = shapeText.Measure(text);
               sz.Width += margin*2;
               sz.Height += margin * 2;

               shapeRect.Render(_lastPoint.Value, sz, margin, margin);
               shapeText.Render(text, _lastPoint.Value);
               shapeBorder.Render(_lastPoint.Value, sz, margin, margin);
           }

            _lastShow = true;
        }

        private bool _lastShow;


        private Point<float>? _lastPoint;
        public void OnMouseMove(Point<float> point, Rectangle<float> rect)
        {
            _lastPoint = point;

            if (Objects.Count > 0 || _lastShow)
                Redraw();
        }

        public void OnMouseLeave()
        {
            if (_lastPoint == null)
                return;

            _lastPoint = null;

            Redraw();
        }

        public void OnMouseEnter()
        {
        }
    }
}
