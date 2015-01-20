using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.ObjectRenderers
{
    /// <summary>
    /// Рендерер протяженных объектов
    /// </summary>
    /// <typeparam name="T">Тип объектов, отображаемых рендерером</typeparam>
    public class RegionFillRenderer<T> : IRenderer
    {
        /// <summary>
        /// Цвет
        /// </summary>
        public Func<T, Color> GetColor;

        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Источник данных для отображения
        /// </summary>
        public IObjectSource<T> Source;

        /// <summary>
        /// Объект для преобразования точек
        /// </summary>
        public IPointTranslator Translator;

        public Func<T, int> GetFrom;
        public Func<T, int> GetTo;

        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if (TapePosition.From >= TapePosition.To)
                return;

            Translator.Src = new Rectangle<float>
                                 {Left = TapePosition.From, Right = TapePosition.To, Bottom = 0f, Top = 1f};
            Translator.Dst = rect;
            
            var shapes = TapeDrawing.ShapesDecorators.ShapesFactoryConfigurator
                .For(gr.Shapes).Translate(Translator).Result;

            //using (var brush = gr.Instruments.CreateSolidBrush(GetColor()))
            //using (var rectShape = shapes.CreateFillRectangle(brush))

            foreach (var r in Source.GetData(TapePosition.From, TapePosition.To))
                using (var brush = gr.Instruments.CreateSolidBrush(GetColor(r)))
                using (var rectShape = shapes.CreateFillRectangle(brush))
                    rectShape.Render(new Rectangle<float>
                                         {
                                             Left = Math.Max(GetFrom(r), TapePosition.From),
                                             Right = Math.Min(GetTo(r), TapePosition.To),
                                             Bottom = 0f,
                                             Top = 1f
                                         });
            
        }
    }
}
