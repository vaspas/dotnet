
using System.Runtime.InteropServices;
using System.Security;

namespace IppModules.Analiz.FractionalOctaveAnalysis
{
    unsafe static class CalcBandPassFiltersWrapper
    {
        internal const string libname = "CalcBandPassFilters.dll";

        /// <summary>
        ///Расчет коэффициентов b0,b1,b2,1,a1,a2 ячеек 2-го порядка
        /// для 1/3-октавных (или октавного) фильтров
        /// Выдает коэффициент уменьшения полосы, выбираемый
        /// автоматически для коррекции шумовой полосы пропускания
        /// </summary>
        /// <param name="taps"></param>
        /// <param name="n_Oct">число октав</param>
        /// <param name="nf_per_oct">число фильтров 12, 3 или 1</param>
        /// <param name="nzv">число звеньев фильтра</param>
        /// <param name="ripple">пульсации в полосе пропускания (дБ)</param>
        /// <param name="f">средняя частота первого фильтра</param>
        /// <param name="qx"></param>
        /// <returns></returns>
        [DllImport(libname, EntryPoint = "_CalcTerzOctTaps", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern
            float CalcTerzOctTaps(float[] taps,int n_Oct, int nf_per_oct, int nzv, float ripple, double f, double qx);

        /// <summary>
        /// Расчет коэффициентов b0,b1,b2,1,a1,a2 ячеек 2-го порядка
        /// полосового БИХ-фильтра Чебышева
        /// </summary>
        /// <param name="taps">результаты</param>
        /// <param name="m">число звеньев</param>
        /// <param name="ripple">пульсации в полосе пропускания (дБ)</param>
        /// <param name="band1">нижняя граничная частота</param>
        /// <param name="band2">верхняя граничная частота</param>
        [DllImport(libname, EntryPoint = "_bp_cheb", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void bp_cheb(float[] taps, int m, float ripple, float band1, float band2);
    }
}
