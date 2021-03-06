#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

#endregion

namespace SoundBlasterModules.WaveApi
{
    /// <summary>
    /// Класс содержит буфер для SB и структуру-заголок его описывающую, 
    /// которые фиксируются в памяти и не перемещаються сборщиком мусора.
    /// </summary>
    class SBBuffer
    {        
        /// <summary>
        /// Структура заголовок буфера.
        /// </summary>
        public WaveAPI.WAVEHDR WaveHdr;
        /// <summary>
        /// Хэндл структуры, чтобы она не перемещалась в памяти.
        /// </summary>
        public GCHandle WaveHdrHdl;

        /// <summary>
        /// Буфер для SB.
        /// </summary>
        private Array buf_;
        /// <summary>
        /// Handle для того чтобы сборщик мусора не перемещал наш буфер в памяти.
        /// </summary>
        private GCHandle bufHandle_;

        /// <summary>
        /// Инициализация ресурсов.
        /// </summary>
        /// <param name="block_size">Размер буфера.</param>
        /// <param name="float_type">Тип данных float, иначе Int16.</param>
        public void Alloc(int block_size, bool float_type)
        {
            int data_type_size = 2;
            if (float_type) data_type_size = 4;
            if (float_type)
                buf_ = Array.CreateInstance(typeof(float), block_size );
            else buf_ = Array.CreateInstance(typeof(Int16), block_size );

            bufHandle_ = GCHandle.Alloc(buf_, GCHandleType.Pinned);
            WaveHdr = new WaveAPI.WAVEHDR();
            WaveHdr.lpData = bufHandle_.AddrOfPinnedObject();
            WaveHdr.dwBufferLength = block_size * data_type_size;
            WaveHdr.dwFlags = 0;
            WaveHdr.dwLoops = 0;
            WaveHdr.dwUser = (IntPtr)GCHandle.Alloc(this);

            WaveHdrHdl = GCHandle.Alloc(WaveHdr, GCHandleType.Pinned);

            WaveHdr = (WaveAPI.WAVEHDR)WaveHdrHdl.Target;

        }

        /// <summary>
        /// Освобождение ресурсов.
        /// </summary>
        public void Free()
        {
            GCHandle h = (GCHandle)WaveHdr.dwUser;
            h.Free();
            bufHandle_.Free();
            WaveHdr.lpData = IntPtr.Zero;

            WaveHdrHdl.Free();
        }
    }
}
