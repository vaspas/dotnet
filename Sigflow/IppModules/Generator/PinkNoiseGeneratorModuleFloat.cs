
using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Generator
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public unsafe class PinkNoiseGeneratorModuleFloat : IExecuteModule
    {
        /// <summary>
        /// Вспомогательная переменная для генерации шума.
        /// </summary>
        private readonly uint _seedNoise = (uint)(new Random()).Next();

        private float _value;
        /// <summary>
        /// Эффективное значение.
        /// </summary>
        public float Value 
        { 
            get { return _value; }
            set 
            { 
                _value = value;
                _reAllocateRequest = true;
            }
        }

        private ipp.IppsRandGaussState_32f* _rndState;
        private ipp.IppsFIRState_64f* _fltrState;
        private bool _allocated;

        private volatile bool _reAllocateRequest;

        private float[] _workArr1;
        private double[] _workArr2;

        public int BlockSize { get; set; }


        public ISignalWriter<float> Out { get; set; }


        private void Alloc()
        {
            if (_allocated)
                return;

            fixed (ipp.IppsRandGaussState_32f** r = &_rndState)
                ipp.sp.ippsRandGaussInitAlloc_32f(r, 0, _value, _seedNoise);


            const int order = 10 + 1 + 1;
            const int ntaps = 1024 * 2 * 2;

            var tmpc = new ipp.Ipp64fc[ntaps];
            var tmpc1 = new ipp.Ipp64fc[ntaps];
            ipp.sp.ippsZero_64fc(tmpc, tmpc.Length);
            for (var i = 0; i < (ntaps / 2); i++)
                tmpc[i].re = 1.0 / Math.Sqrt(1.0 * i + 1);
            for (var i = 1; i < (ntaps / 2); i++)
                tmpc[ntaps - i].re = tmpc[i].re;


            ipp.IppsFFTSpec_C_64fc* fft;
            ipp.sp.ippsFFTInitAlloc_C_64fc(&fft, order, 2, ipp.IppHintAlgorithm.ippAlgHintNone);//  ipp.IppHintAlgorithm.ippAlgHintAccurate);
            ipp.sp.ippsFFTInv_CToC_64fc(tmpc, tmpc1, fft, null);
            ipp.sp.ippsCopy_64fc(tmpc1, tmpc, ntaps);
            ipp.sp.ippsFFTFree_C_64fc(fft);

            var taps = new double[ntaps];

            fixed (double* t = taps)
            {
                ipp.sp.ippsReal_64fc(tmpc, t, ntaps);
                var t2 = new double[ntaps];
                for (var i = 0; i < ntaps / 2; i++)
                    t2[ntaps / 2 + i] = t2[ntaps / 2 - i] = taps[i];
                t2[0] = 0;// taps[ntaps / 2];
                for (var i = 0; i < ntaps; i++)
                    taps[i] = t2[i];
                fixed (ipp.IppsFIRState_64f** f = &_fltrState)
                    ipp.sp.ippsFIRInitAlloc_64f(f, t, ntaps, null);
            }

            _allocated = true;
        }

        private void Free()
        {
            if (!_allocated)
                return;

            ipp.sp.ippsRandGaussFree_32f(_rndState);
            ipp.sp.ippsFIRFree_64f(_fltrState);

            _allocated = false;
        }

        ~PinkNoiseGeneratorModuleFloat()
        {
            Free();
        }
        
        

        private float[] _data;

        public bool? Execute()
        {
            if(_reAllocateRequest)
            {
                Free();
                _reAllocateRequest = false;
            }
            Alloc();

            var blockSize = BlockSize;

            if (_data == null || _data.Length != blockSize)
            {
                _data=new float[blockSize];
                _workArr1=new float[blockSize];
                _workArr2=new double[blockSize];
            }

            //формируем новый блок
            fixed (float* wArr1 = _workArr1)
            {
                ipp.sp.ippsRandGauss_32f(wArr1, _workArr1.Length, _rndState);

                fixed (double* wArr2 = _workArr2)
                {
                    ipp.sp.ippsConvert_32f64f(wArr1, wArr2, _workArr2.Length);
                    ipp.sp.ippsFIR_64f_I(wArr2, _workArr2.Length, _fltrState);
                }
            }

            fixed (double* pNext = _workArr2)
            fixed (float* pBlock = _data)
            {
                ipp.sp.ippsConvert_64f32f(pNext, pBlock, _data.Length);
            }

            Out.Write(_data);

            return null;
        }
    }
}
