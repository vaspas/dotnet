using System;
using System.Collections.Generic;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.ObjectRenderers.LinearScale
{
    public abstract class ScaleBase
    {
        /// <summary>
        /// Минимальное расстояние между обозначениями в пикселах.
        /// </summary>
        public int MinPixelsDistance { get; set; }

        /// <summary>
        /// Маска со значениями в пределах (0:1], например если указано {0,2;0,5;1} 
        /// то отображаться могут значения …1, 2,5,10,20,50,100,200,500,1000,2000…
        /// </summary>
        public float[] Mask { get; set; }
        
        /// <summary>
        /// Диапазон.
        /// </summary>
        public IScalePosition<float> Diapazone { get; set; }

        /// <summary>
        /// Транслятор точек
        /// </summary>
        public IPointTranslator Translator { get; set; }

        protected float GetMinCodeDistance()
        {
            var pointFrom = Translator.Translate(new Point<float> { X = Diapazone.From, Y = 0.0f });
            var pointTo = Translator.Translate(new Point<float> { X = Diapazone.To, Y = 0.0f });
            return
                (float)
                (MinPixelsDistance * (Diapazone.To - Diapazone.From) /
                 Math.Sqrt(Math.Pow(pointTo.X - pointFrom.X, 2) + Math.Pow(pointTo.Y - pointFrom.Y, 2)));
        }

        protected float CreateStep()
        {
            // Рассчитаем минимальное расстояние в индексах
            var minCodeDistance = GetMinCodeDistance();
            
            // Далее по маске нужно найти какие значения отображать
            var step = float.MaxValue;
            foreach (var m in Mask)
            {
                var mark = m * (float)Math.Pow(10, Math.Ceiling(Math.Log10(minCodeDistance / m)));
                if (mark < step) step = mark;
            }

            return step;
        }

        protected IEnumerable<float> GetCodes()
        {
            // Нужно узнать с каким шагом рисовать штрихи
            var step = CreateStep();


            // Найдем первую точку шкалы
            float currentCoord = Diapazone.From- Diapazone.From % step;
            if (Diapazone.From >= 0)
                currentCoord += step;
            
            // С шагом step будем рисовать штрихи
            int counter = 0;
            var first = currentCoord;
            while (currentCoord <= Diapazone.To)
            {
                yield return currentCoord;

                // Именно так, иначе ошибка накапливается
                counter++;
                currentCoord = first + counter * step;
            }
        }
    }
}
