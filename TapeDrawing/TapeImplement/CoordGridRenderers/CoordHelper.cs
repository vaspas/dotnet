using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Данные для отображения штриха. Структура нужна для того, чтобы убрать погрешность в отображении координат
    /// </summary>
    struct CoordinateData
    {
        /// <summary>
        /// Индекс сигнала
        /// </summary>
        public float Index;
        /// <summary>
        /// Реальная координата
        /// </summary>
        public float Coordinate;
    }

    /// <summary>
    /// Содержит статические методы, которые могут использоваться не один раз
    /// </summary>
    static class CoordHelper
    {
        private const double EqualFactor = 0.0001;

        /// <summary>
        /// Позволяет по маске получить шаг, с которым ставятся штрихи
        /// </summary>
        /// <param name="source">Источник координат</param>
        /// <param name="unit">Непрерывный отрезок</param>
        /// <param name="mask">Маска</param>
        /// <param name="minPixelsDistance">Минимальное расстояние между штрихами в пикселах</param>
        /// <param name="tapePosition">Позиция на ленте</param>
        /// <param name="translator">Транслятор точек</param>
        /// <returns>Шаг для шкалы</returns>
        public static float CreateStep(ICoordinateSource source, IScalePosition<int> tapePosition, Unit unit, float[] mask, int minPixelsDistance, IPointTranslator translator)
        {
            // Рассчитаем минимальное расстояние в индексах
            var minIndexDistance = GetMinIndexDistance(minPixelsDistance, tapePosition, translator);

            // Рассчитаем минимальное расстояние в единицах длины
            var minDistance = (minIndexDistance*(unit.EndCoordinate - unit.BeginCoordinate))/
                              ((float) unit.EndIndex - unit.BeginIndex);

            // Уберем знак если значение отрицательное
            if (minDistance < -0.0f) minDistance = -minDistance;

            // Далее по маске нужно найти какие значения отображать
            var step = float.MaxValue;
            foreach (var m in mask)
            {
                var mark = m*(float) Math.Pow(10, Math.Ceiling(Math.Log10(minDistance/m)));
                if (mark < step) step = mark;
            }

            return step;
        }

        /// <summary>
        /// Позволяет перечислить все коордианаты, где нужно ставить штрихи
        /// </summary>
        /// <param name="source">Источник координат</param>
        /// <param name="tapePosition">Позиция ленты</param>
        /// <param name="minPixelsDistance">Минимальное расстояние в пикселах</param>
        /// <param name="renderer">Рендерер, который выполняет рисование</param>
        /// <param name="priorityRenderers">Рендереры, которые имеют приоритет</param>
        /// <param name="unit">Юнит, в котором осуществляется рисование</param>
        /// <param name="translator">Транслятор точек</param>
        /// <returns>Перечисление CoordinateData</returns>
        public static IEnumerable GetCoordinateData(ICoordinateSource source, IScalePosition<int> tapePosition, int minPixelsDistance, CoordUnitBaseRenderer renderer, CoordUnitBaseRenderer[] priorityRenderers, Unit unit, IPointTranslator translator)
        {
            // Нужно узнать с каким шагом рисовать штрихи
            var step = CreateStep(source, tapePosition, unit, renderer.Mask, renderer.MinPixelsDistance,translator);

            // Посчитаем минимальное расстояние в индексах между прерываниями
            var minIndexDistance = GetMinIndexDistance(minPixelsDistance, tapePosition, translator);

            // Для каждого из высокоприоритетных рендереров посчитаем шаг
            // и минимальное расстояние для каждого более важного слоя
            var steps = new List<float>();
            var minIndexDistances = new List<float>();
            if (priorityRenderers != null)
            {
                steps.AddRange(
                    priorityRenderers.Select(
                        priorityRenderer =>
                        CreateStep(source, tapePosition, unit, priorityRenderer.Mask,
                                   priorityRenderer.MinPixelsDistance,translator)));

                minIndexDistances.AddRange(priorityRenderers.Select(priorityRenderer => GetMinIndexDistance(priorityRenderer.MinPixelsDistance, tapePosition, translator)));
            }


            // Если шаг шкалы не кратен более важным шкалам, то не будем рисовать эту шкалу вообще
            bool toDraw = steps.Select(f => f/step).All(tmp => Math.Abs(tmp - Math.Round(tmp)) <= EqualFactor);
            if (toDraw)
            {
                // Найдем первую точку шкалы
                float currentCoord;

                if (IsMultiple(unit.BeginCoordinate, step))
                {
                    // Если начальная координата кратна шагу, то это первый штрих
                    currentCoord = unit.BeginCoordinate;
                }
                else
                {
                    currentCoord = unit.BeginCoordinate - unit.BeginCoordinate%step;
                    if (unit.BeginCoordinate >= 0) currentCoord += step;
                }

                // Постоянный коэффициент преобразования координат в индексы
                var k = (unit.EndIndex - (double)unit.BeginIndex) / ((double)unit.EndCoordinate - unit.BeginCoordinate);

                // С шагом step будем рисовать штрихи
                int counter = 0;
                var first = currentCoord;
                while (currentCoord <= unit.EndCoordinate)
                {
                    // Найдем индекс штриха
                    var index = (float)(k * (currentCoord - unit.BeginCoordinate) + unit.BeginIndex);

                    // Найдем можно ли рисовать этот штрих или текст
                    var coord = currentCoord;

                    // Чтобы рисовать штрих, его значение должно быть некратно ни одному из отображаемых значений в других слоях
                    // если они при этом отображаются
                    var drawHatch = steps.All(f => !IsMultiple(coord, f)) ||
                                    minIndexDistances.All(t => !IsOverMinDistance(index, unit, t));


                    if (drawHatch)
                    {
                        // Если расстояние больше или равно минимальному
                        if (IsOverMinDistance(index, unit, minIndexDistance/2))
                        {
                            // Вернем индекс очередного штриха
                            yield return new CoordinateData {Index = index, Coordinate = currentCoord};
                        }
                    }

                    // Именно так, иначе ошибка накапливается
                    counter++;
                    currentCoord = first + counter*step;
                }
            }
        }

        /// <summary>
        /// Позволяет получить все участки без прерываний
        /// </summary>
        /// <param name="source">Источник координат</param>
        /// <param name="tapePosition">Участок ленты</param>
        /// <returns>Перечисление Юнитов для foreach</returns>
        public static IEnumerable GetUnits(ICoordinateSource source, IScalePosition<int> tapePosition)
        {
            // Рисуемый отрезок делится прерываниями. В начало и конец добавляются прерывания
            var interrupts = GetInterrupts(source, tapePosition);

            // Между каждыми двумя прерываниями будем рисовать шкалу
            for (var i = 0; i < interrupts.Length - 1; i++)
            {
                if ((interrupts[i + 1].Index - interrupts[i].Index) < 2) continue;

                // Найдем начальную и конечную координату участка между прерываниями
                var beginCoord = source.GetUnitValue(interrupts[i].Index + 1);
                var endCoord = source.GetUnitValue(interrupts[i + 1].Index - 1);

                // Если координата на участке не менялась, то пропустим такой участок
                if (beginCoord == endCoord) continue;

            	yield return
            		new Unit
            			{
            				BeginCoordinate = beginCoord,
            				BeginIndex = interrupts[i].Index + 1,
            				EndCoordinate = endCoord,
            				EndIndex = interrupts[i + 1].Index - 1,
            				LeftInterrupt = i!=0 ,
            				RightInterrupt = i + 1 != interrupts.Count()-1
            			};
            }
        }

        /// <summary>
        /// Определяет кратно ли одно значение другому
        /// </summary>
        /// <param name="value"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        private static bool IsMultiple(float value, float step)
        {
            // return (value % step == 0);
            var d = value / step;
            return d == Math.Round(d);
        }
        /// <summary>
        /// Определяет, не слишком ли близко штрих находится в краям юнита
        /// </summary>
        /// <param name="index"></param>
        /// <param name="unit"></param>
        /// <param name="minIndexDistance"></param>
        /// <returns>true если штрих удален на приемлемое расстояние от краев</returns>
        private static bool IsOverMinDistance(float index, Unit unit, float minIndexDistance)
        {
        	return unit.EndIndex > unit.BeginIndex
        	       	? ((unit.LeftInterrupt && (Math.Abs(index - (unit.BeginIndex - 1)) >= minIndexDistance)) ||
        	       	   !unit.LeftInterrupt) &&
        	       	  ((unit.RightInterrupt && (Math.Abs(index - (unit.EndIndex + 1)) >= minIndexDistance)) ||
        	       	   !unit.RightInterrupt)
        	       	: ((unit.LeftInterrupt && (Math.Abs(index - (unit.BeginIndex + 1)) >= minIndexDistance)) ||
        	       	   !unit.LeftInterrupt) &&
        	       	  ((unit.RightInterrupt && (Math.Abs(index - (unit.EndIndex - 1)) >= minIndexDistance)) ||
        	       	   !unit.RightInterrupt);
        }

    	/// <summary>
        /// Превращяет расстояние в пикселах в расстояние в индексах
        /// </summary>
        /// <param name="minPixelsDistance"></param>
        /// <param name="tapePosition"></param>
        /// <param name="translator"></param>
        /// <returns></returns>
        private static float GetMinIndexDistance(int minPixelsDistance, IScalePosition<int> tapePosition, IPointTranslator translator)
    	{
    	    var pointFrom = translator.Translate(new Point<float> {X = tapePosition.From, Y = 0.0f});
    	    var pointTo = translator.Translate(new Point<float> {X = tapePosition.To, Y = 0.0f});
        	return
        		(float)
        		(minPixelsDistance*(tapePosition.To - tapePosition.From)/
        		 Math.Sqrt(Math.Pow(pointTo.X - pointFrom.X, 2) + Math.Pow(pointTo.Y - pointFrom.Y, 2)));
        }

    	/// <summary>
        /// Позволяет получить список прерываний. Начало и конец участка ленты тоже являются прерываниями
        /// </summary>
        /// <param name="source">Источник координат</param>
        /// <param name="tapePosition">Участок ленты, для которого нужно получить прерывания</param>
        /// <returns>Список прерываний на участке</returns>
        private static ICoordInterrupt[] GetInterrupts(ICoordinateSource source, IScalePosition<int> tapePosition)
        {
            var interBuf = source.GetCoordInterrupts(tapePosition.From, tapePosition.To);
            var interrupts = new ICoordInterrupt[interBuf.Length + 2];

			interrupts[0] = new CoordInterrupt { Index = tapePosition.From < 0 ? 0 : tapePosition.From };
            interBuf.CopyTo(interrupts, 1);
			interrupts[interrupts.Length - 1] = new CoordInterrupt { Index = tapePosition.To };

            return interrupts;
        }
    }
}
