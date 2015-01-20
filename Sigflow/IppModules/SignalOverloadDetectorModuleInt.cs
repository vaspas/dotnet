using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <summary>
    /// Модуль детектор перегрузки сигнала.
    /// </summary>
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class SignalOverloadDetectorModuleInt : IExecuteModule, IMasterModule
    {
        public ISignalReader<int> In { get; set; }

        /// <summary>
        /// Предельное значение.
        /// </summary>
        public int MaxValue { get; set; }

        public float CurrentOverload { get; private set; }

        public float MaxOverload { get; private set; }

        private volatile bool _dropMaxOverload;
        public void DropMaxOverload()
        {
            _dropMaxOverload = true;
        }

        public unsafe bool? Execute()
        {
            var maxValue = MaxValue;

            if(In.Available==0 || maxValue<=0)
                return false;
            
            var block = In.Take();

            int min, max;

            fixed (int* srcData = block)
                ipp.sp.ippsMinMax_32s(srcData, block.Length, &min, &max);

            var overloadValue = (float)Math.Max(Math.Abs(min), Math.Abs(max))/maxValue;
            CurrentOverload = overloadValue;
            if (_dropMaxOverload)
            { 
                MaxOverload = overloadValue;
                _dropMaxOverload = false;
            }
            else
                MaxOverload = Math.Max(overloadValue, MaxOverload);

            In.Put(block);

            return true;
        }

        public bool Start()
        {
            MaxOverload = 0;
            CurrentOverload = 0;

            return true;
        }

        public void BeforeStop()
        {

        }
        public void AfterStop()
        {

        }
    }
}
