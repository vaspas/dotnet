using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace TapeImplement.ObjectRenderers
{
    /// <summary>
    /// Рендерер точечных объектов
    /// </summary>
    public class RecordLineRenderer<T> : IRenderer
    {
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

        public Color LineColor;

        public float LineWidth;

        public LineStyle LineStyle;

        /// <summary>
        /// Функция получения положения данных.
        /// </summary>
        public Func<T, int> GetIndex;


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
                                 {Left = TapePosition.From, Right = TapePosition.To, Bottom = -1, Top = 1};
            Translator.Dst = rect;

            var shapesFactory = ShapesFactoryConfigurator.For(gr.Shapes)
                .Translate(Translator).Result;

            using (var pen = gr.Instruments.CreatePen(LineColor, LineWidth, LineStyle))
            using (var linesShape = shapesFactory.CreateLines(pen))
            {
                // Запросим данные
                var dataList = Source.GetData(TapePosition.From, TapePosition.To);

                // Отобразим объекты
                foreach (var r in dataList)
                {
                    linesShape.Render(new[]
                                          {
                                              new Point<float>{X=GetIndex(r), Y=-1}, 
                                              new Point<float>{X=GetIndex(r), Y=1}
                                          });
                }
            }
        }
    }
}
