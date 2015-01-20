using System;
using System.IO;
using TapeDrawing.Core;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawing.Core.Translators;

namespace TapeImplement.ObjectRenderers
{
    /// <summary>
    /// Рендерер протяженных объектов
    /// Если объект длиннее чем изображение, изображение копируется последовательно по всей длине объекта, 
    /// а последняя копия обрезается по границе объекта или слоя.
    /// </summary>
    /// <typeparam name="T">Тип объектов, отображаемых рендерером</typeparam>
    public class RegionObjectRenderer<T> : IRenderer
    {
        /// <summary>
        /// Рисунок протяженного объекта
        /// </summary>
        public Stream Image;

        /// <summary>
        /// Угол наклона изображения
        /// </summary>
        public float Angle;

        /// <summary>
        /// Выравнивание изображений относительно точек
        /// </summary>
        public Alignment Alignment;

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
                                 {Left = TapePosition.From, Right = TapePosition.To, Bottom = -1, Top = 1};
            Translator.Dst = rect;

            using (var image = gr.Instruments.CreateImage(Image))
            using (var shape = gr.Shapes.CreateImage(image, Alignment, Angle))
            {
                foreach (var t in Source.GetData(TapePosition.From, TapePosition.To))
                    DrawRegion(gr, t, image, shape);

            }
        }

        private void DrawRegion(IGraphicContext gr, T region, IImage image, IImageShape shape)
        {
            var from = Translator.Translate(new Point<float>
                                                {
                                                    X =
                                                        GetFrom(region) > TapePosition.From
                                                            ? GetFrom(region)
                                                            : TapePosition.From,
                                                    Y = 0
                                                });
            var to = Translator.Translate(new Point<float>
                                              {
                                                  X = GetTo(region) < TapePosition.To ? GetTo(region) : TapePosition.To,
                                                  Y = 0
                                              });

            var length = (float) Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2));

            var size = image.Width;
            var kStep = size / length;

            for (var i = 0; i < length / size; i++)
            {
                var x = from.X + (to.X - from.X)*kStep*i;
                var y = from.Y + (to.Y - from.Y)*kStep*i;

                var lastLength = (float) Math.Sqrt(Math.Pow(to.X - x, 2) + Math.Pow(to.Y - y, 2));

                if (lastLength >= size)
                    shape.Render(new Point<float>{X=x,Y=y});
                else
                {
                    var roi = new Rectangle<float>
                                  {
                                      Left = image.Width/2 - lastLength/2,
                                      Right = image.Width/2 + lastLength/2,
                                      Bottom = 0,
                                      Top = image.Height
                                  };
                    if ((Alignment & Alignment.Left) == Alignment.Left)
                    {
                        roi.Left = 0;
                        roi.Right = lastLength;
                    }
                    if ((Alignment & Alignment.Right) == Alignment.Right)
                    {
                        roi.Left = image.Width-lastLength;
                        roi.Right = image.Width;
                    }

                    using (var imagePortion = gr.Instruments.CreateImagePortion(Image, roi))
                    using (var shapePortion = gr.Shapes.CreateImage(imagePortion, Alignment, Angle))
                        shapePortion.Render(new Point<float> { X = x, Y = y });
                }
            }
        }
    }
}
