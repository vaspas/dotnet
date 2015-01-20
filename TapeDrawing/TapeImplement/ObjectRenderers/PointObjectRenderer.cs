using System;
using System.IO;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace TapeImplement.ObjectRenderers
{
    /// <summary>
    /// Рендерер точечных объектов
    /// </summary>
    public class PointObjectRenderer<T> : IRenderer
    {
        /// <summary>
        /// Рисунок точечного объекта
        /// </summary>
        public Stream Image;

        /// <summary>
        /// Угол наклона изображения
        /// </summary>
        public float Angle;

        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Источник данных для отображения
        /// </summary>
        public IObjectSource<T> Source;

        /// <summary>
        /// Функция получения положения данных.
        /// </summary>
        public Func<T, int> GetIndex;

        /// <summary>
        /// Объект для преобразования точек
        /// </summary>
        public IPointTranslator Translator;

        /// <summary>
        /// Выравнивание рисунка
        /// </summary>
        public Alignment Alignment;

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

            using (var image = gr.Instruments.CreateImage(Image))
            using (var imageShape = shapesFactory.CreateImage(image, Alignment, Angle))
            {
                // Запросим данные
                var dataList = Source.GetData(TapePosition.From, TapePosition.To);
                
                // Отобразим объекты
                foreach (var r in dataList)
                {
                    imageShape.Render(new Point<float> {X = GetIndex(r), Y = 0});
                }
            }
        }
    }
}
