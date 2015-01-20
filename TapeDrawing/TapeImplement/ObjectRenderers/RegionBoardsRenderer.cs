using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.ObjectRenderers
{
    /// <summary>
    /// Рендерер протяженных объектов
    /// Если объект длиннее чем изображение, изображение копируется последовательно по всей длине объекта, 
    /// а последняя копия обрезается по границе объекта или слоя.
    /// </summary>
    /// <typeparam name="T">Тип объектов, отображаемых рендерером</typeparam>
    public class RegionBoardsRenderer<T> : IRenderer
    {
        /// <summary>
        /// Ширина линии
        /// </summary>
        public float LineWidth;

        /// <summary>
        /// Цвет линии
        /// </summary>
        public Color LineColor;

        /// <summary>
        /// Стиль линии
        /// </summary>
        public LineStyle LineStyle;

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

            using (var pen = gr.Instruments.CreatePen(LineColor, LineWidth, LineStyle))
            using (var rectShape = shapes.CreateDrawRectangle(pen))
            {
                foreach (var r in Source.GetData(TapePosition.From,TapePosition.To))
                    rectShape.Render(new Rectangle<float>
                    {
                        Left = Math.Max(GetFrom(r), TapePosition.From),
                        Right = Math.Min(GetTo(r), TapePosition.To),
                        Bottom = 0,
                        Top = 1
                    });
            }
        }
    }
}
