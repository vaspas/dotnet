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
    public class SignalOverloadDetectorModuleFloat : IExecuteModule, IMasterModule
    {
        public ISignalReader<float> In { get; set; }

        /// <summary>
        /// Предельное значение.
        /// </summary>
        public float MaxValue { get; set; }

        
        public float CurrentOverload { get; private set; }
        
        public float MaxOverload { get; private set; }
        
        public unsafe bool? Execute()
        {
            var maxValue = MaxValue;

            if(In.Available==0 || maxValue<=0)
                return false;
            
            var block = In.Take();

            float min, max;

            fixed (float* srcData = block)
                ipp.sp.ippsMinMax_32f(srcData, block.Length, &min, &max);

            var overloadValue = Math.Max(Math.Abs(min), Math.Abs(max))/maxValue;
            CurrentOverload= overloadValue;
            MaxOverload= Math.Max(overloadValue, MaxOverload);

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
