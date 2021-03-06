#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

#endregion

namespace SigProModules
{
    unsafe public abstract class SigImport
    {
        [DllImport("Sig.dll", EntryPoint = "_CreateSignal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CreateSignal(
                                int node,// номер узла 0-31
            int nchan, // число каналов сигнала
            int ncols, // число колонок
            int nkadr, // число кадров в буфере
            int dt,// тип данных
            double f_qu, // частота квантования (гц)
            IntPtr *sig// адрес указателя на структуру сигнала
            );

        [DllImport("Sig.dll", EntryPoint = "_SetSignalScale", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetSignalScale(double x0, double dx, double qx, IntPtr sig);

        [DllImport("Sig.dll", EntryPoint = "_SetSignalWrite", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetSignalWrite(int block_size, float max_level, float zero_level, IntPtr sig);

        [DllImport("Sig.dll", EntryPoint = "_WriteSignal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void WriteSignal(IntPtr sig, IntPtr data);

        [DllImport("Sig.dll", EntryPoint = "_ResetSignalWrite", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ResetSignalWrite(IntPtr sig);

        [DllImport("Sig.dll", EntryPoint = "_CloseSignal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseSignal(int node);

        [DllImport("Sig.dll", EntryPoint = "_CloseOutSignalWait", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CloseOutSignalWait(IntPtr* sig);// адрес указателя на структуру сигнала

        [StructLayout(LayoutKind.Sequential,Pack = 4)]
        public class SharedSig
        {
            public int work, // флаг подключения процесса записи          0
                nchan, // число каналов                             1
                ncols, // число колонок                             2
                nkadr, // число кадров в буфере                     3
                ikadr, // номер очередного кадра для записи         4
                cnt,   // счетчик кадров                            5
                block_size,// размер блока записи в кадрах          6
                data_type, // код типа данных SigDataType           7
                nosign,    // числа без знака                       8
                word_size, // размер слова данных в байтах          9
                kadr_size, // размер кадра данных в байтах          10
                node;      // номер узла                            11
            public float max_level, // макс. уровень                      12
                zero_level;// уровень нуля                          13
            public double f_qu; // частота квантования                    14
            public double x0, // время-частота первого отсчета блока      16
                x1, // время-частота последнего отсчета блока   18
                dx, // шаг, если линейная сетка, else =0        20
                qx; // множ., если лог. сетка, else =0          22
            public int x_units, // шкала x = 1 - секунды, x=2 - Гц (SigXScale)     24
                y_units, // шкала y = 0 - ед., 1-мкв, 2-мв, 3-В (SigYScale) 25
                powered; // 1 - линейная, 2-квадратичная (SigYPowered)      26
            public double f_geterodine; // частота гетеродина             27
            public int time; //время в милли секундах от полуночи                  29 
            public int date; //дата, дней от 01 янв 0001 года.                     30 

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 159-31)]         
            public int[] dummy; // запасные ячейки              31

            public int users;   //                                        159
            public int loaded, // текущее заполнение буфера
                peak; // индикатор пикового заполнения буфера
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
            public OVER[] over; // индикаторы 32 каналов чтения - 33*12=0x18C байт
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
            public char[] data;

            [StructLayout(LayoutKind.Sequential)]
            public struct OVER
            {
                public int busy, // флаг работы канала чтения
                    cnt, // счетчик
                    peak;
            }
        };
    }
}
