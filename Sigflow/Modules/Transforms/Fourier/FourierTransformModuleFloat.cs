using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules.Transforms.Fourier
{
    public class FourierTransformModuleFloat : IExecuteModule
    {
        public ISignalReader<float> InRe { get; set; }

        public ISignalReader<float> InIm { get; set; }

        public ISignalWriter<float> OutRe { get; set; }

        public ISignalWriter<float> OutIm { get; set; }

        private float[] _outReBuffer = new float[0];
        private float[] _outImBuffer = new float[0];

        public bool? Execute()
        {
            var blockSize = InRe.NextBlockSize;
            if (blockSize==null || blockSize != InIm.NextBlockSize)
                return false;

            var re = InRe.Take();
            var im = InIm.Take();

            if(_outReBuffer.Length!=blockSize)
            {
                _outReBuffer = new float[blockSize.Value];
                _outImBuffer = new float[blockSize.Value];
            }

            for(var k=0; k<blockSize; k++)
            {
                _outReBuffer[k] = 0;
                _outImBuffer[k] = 0;

                for (var n = 0; n < blockSize; n++)
                {
                    //степень комплексной экпоненты
                    var imValue = -(float)(2 * Math.PI * n * k) / blockSize.Value;
                    //комплекстная экспонента Re=0 в степени
                    var eRe= Math.Cos(imValue);
                    var eIm = Math.Sin(imValue);

                    //перемножаем комплексные числа, суммируем результат
                    _outReBuffer[k] += (float)(eRe * re[n] - eIm * im[n]);
                    _outImBuffer[k] += (float)(eRe * im[n] + eIm * re[n]);
                }
            }

            InRe.Put(re);
            InIm.Put(im);

            if (OutRe != null)
                OutRe.Write(_outReBuffer);
            if(OutIm!=null)
                OutIm.Write(_outImBuffer);

            return true;
        }
    }
}
