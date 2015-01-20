using System.Collections.Generic;
using System.Linq;
using TapeImplement.CoordGridRenderers;

namespace ComparativeTapeTest.Generators.CoordsGenerator
{
    class CoordSource:ICoordinateSource
    {
        public CoordSource()
        {
            Data=new List<object>();
        }

        public List<object> Data { get; private set; }

        /// <summary>
        /// Позволяет получить все отметки для указанного диапазона ленты
        /// </summary>
        /// <param name="from">Начальный индекс диапазона</param>
        /// <param name="to">Конечный индекс диапазона</param>
        /// <returns>Массив отметок (прерываний ленты)
        /// !ВНИМАНИЕ! В начало и конец ленты должны быть добавлены прерывания. Тоесть на ленте обязательно
        /// должны быть 2 прерывания</returns>
        public ICoordInterrupt[] GetCoordInterrupts(int from, int to)
        {
            return Data.FindAll(i => i is ICoordInterrupt)
                .ConvertAll(i => (ICoordInterrupt) i)
                .FindAll(i => i.Index >= from && i.Index <= to)
                .OrderBy(i=>i.Index)
                .ToArray();
        }

        /// <summary>
        /// Возвращает значение координаты в единицах длины для указанного индекса
        /// </summary>
        /// <param name="index">Индекс сигнала</param>
        /// <returns>Координата на пути. Если индекс вне диапазона, то должен всегда возвращаться 0
        /// !ВНИМАНИЕ! 0 не может использоваться как код ошибки, потомучто это валидное значение
        /// Использовать так: если на участке координата начала и конца совпала, то не обрабатывать</returns>
        public float GetUnitValue(int index)
        {
            var unitRegion = Data.FindAll(o => (o is UnitRegion))
                .ConvertAll(i => (UnitRegion) i)
                .FirstOrDefault(r => r.From <= index && r.To >= index);

            return unitRegion!=null
                ? unitRegion.ValueFrom + (unitRegion.ValueTo-unitRegion.ValueFrom)* ((float)(index-unitRegion.From)/(unitRegion.To-unitRegion.From))
                : 0;
        }
    }
}
