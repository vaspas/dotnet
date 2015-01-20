using System;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules.Transforms
{
    public class ComplexArgModuleFloat : IExecuteModule
    {
        public ISignalReader<float> InRe { get; set; }

        public ISignalReader<float> InIm { get; set; }

        public ISignalWriter<float> Out { get; set; }

        private float[] _outBuffer = new float[0];

        public bool? Execute()
        {
            var blockSize = InRe.NextBlockSize;
            if (blockSize==null || blockSize != InIm.NextBlockSize)
                return false;

            var re = InRe.Take();
            var im = InIm.Take();

            if(_outBuffer.Length!=blockSize)
                _outBuffer = new float[blockSize.Value];

            for(var k=0; k<blockSize; k++)
                _outBuffer[k] = (float)Math.Atan2(im[k], re[k]);
            

            InRe.Put(re);
            InIm.Put(im);

            if (Out != null)
                Out.Write(_outBuffer);

            return true;
        }
    }
}
