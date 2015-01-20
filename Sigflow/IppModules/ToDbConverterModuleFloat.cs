using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class ToDbConverterModuleFloat : IExecuteModule
    {
        private float[] _data;

        public ISignalReader<float> In { get; set; }
        public ISignalWriter<float> Out { get; set; }

        public int Value { get; set; }

        public float ThresholdGtValue { get; set; }

        public float ThresholdLtValue { get; set; }

        public unsafe bool? Execute()
        {
            if (!In.NextBlockSize.HasValue)
                return false;

            var blockSize = In.NextBlockSize.Value;

            if (_data == null || _data.Length != blockSize)
                _data = new float[blockSize];

            if (!In.ReadTo(_data))
                return false;

            fixed (float* pData = _data)
            {
                //берем модуль, для случая с отрицательными значениями
                ipp.sp.ippsAbs_32f_I(pData, _data.Length);
                //Переводим значения y в лог.
                ipp.sp.ippsLn_32f_I(pData, _data.Length);
                ipp.sp.ippsMulC_32f_I((float) (Value/Math.Log(10)), pData, _data.Length);
                //Обрезаем сверху и снизу
                ipp.sp.ippsThreshold_GT_32f_I(pData, _data.Length, ThresholdGtValue);
                ipp.sp.ippsThreshold_LT_32f_I(pData, _data.Length, ThresholdLtValue);
            }

            Out.Write(_data);

            return true;
        }
    }
}
