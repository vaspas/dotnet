using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IppModules.Analiz.FractionalOctaveAnalysis;

namespace TestTerzTaps
{
    class Program
    {
        static void Main(string[] args)
        {
            var QX = Math.Pow(Math.Pow(10, 0.3), (double) 1/3);

            var f = (double)1000 / 327680; // частота 1КГц относительная должна попадать в сетку
            // расчет относительной частоты нижнего фильтра верхней октавы
            // из расчета верхней частоты 0.4 (запас 5% по ФНЧ в других октавах)
            while (f * Math.Sqrt(QX) > 0.19) { f /= QX;  } // сначала спуститься по сетке вниз
            while (f * Math.Sqrt(QX) < 0.19) { f *= QX;  } // подняться вверх до границы октавы

            var m_taps = new float[3060];

            TerzTaps.CalcTerzOctTaps(ref m_taps, 17, 3, 5, 0.1f, f, QX);
        }
    }
}
