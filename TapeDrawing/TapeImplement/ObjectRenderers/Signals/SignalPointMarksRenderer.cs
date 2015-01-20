using System.IO;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace TapeImplement.ObjectRenderers.Signals
{
    /// <summary>
    /// Рендерер для рисования отметок точек графика.
    /// </summary>
    public class SignalPointMarksRenderer : IRenderer
    {
        /// <summary>
        /// Источник данных сигнала
        /// </summary>
        public ISignalPointSource Source;

        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        /// <summary>
        /// Границы отображения сигнала на ленте.
        /// </summary>
        public IScalePosition<float> Diapazone;

        public Stream ImageStream;

        public Alignment ImageAlignment;

        public float ImageAngle;

        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator;

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
            {
                Left = TapePosition.From,
                Right = TapePosition.To,
                Bottom = Diapazone.From,
                Top = Diapazone.To
            };
            Translator.Dst = rect;

            var shapesFactory = ShapesFactoryConfigurator.For(gr.Shapes).Translate(Translator).Result;

            using (var image = gr.Instruments.CreateImage(ImageStream))
            using (var shape = shapesFactory.CreateImage(image, ImageAlignment, ImageAngle))
            {
				// Составим отрисовываемые точки
                var point = Source.GetStartPoint(TapePosition.From, TapePosition.To);
                while (point != null && point.Value.X < TapePosition.To)
				{
                    shape.Render(point.Value);
                    point = Source.GetNextPoint();
				}
                
			}
        }

    }
}
