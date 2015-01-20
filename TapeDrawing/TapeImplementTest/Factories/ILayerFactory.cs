using TapeDrawing.Core.Layer;
using TapeImplement;
using TapeImplementTest.SourceImplement;

namespace TapeImplementTest.Factories
{
    public interface ILayerFactory
    {
        /// <summary>
        /// Источник данных координат
        /// </summary>
        TestCoordinateSource Source { get; set; }
        /// <summary>
        /// Диапазон рисования
        /// </summary>
        IScalePosition<int> TapePosition { get; set; }
        /// <summary>
        /// Параметры теста
        /// </summary>
        TestParams TestParams { get; set; }
        /// <summary>
        /// Заполняет слой другими слоями
        /// </summary>
        /// <param name="mainLayer">Главный слой</param>
        void Create(ILayer mainLayer);
    }
}
