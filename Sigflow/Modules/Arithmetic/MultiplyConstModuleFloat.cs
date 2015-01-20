using Sigflow.Dataflow;
using Sigflow.Module;

namespace Modules.Arithmetic
{
    /// <remarks>
    /// Умножение на константу.
    /// </remarks>
    public class MultiplyConstModuleFloat:IExecuteModule
    {
        public MultiplyConstModuleFloat()
        {
            Const = 1;
        }

        public unsafe bool? Execute()
        {
            var srcData = In.Take();
            if (srcData==null)
                return false;

            if(_data.Length!=srcData.Length)
                _data = new float[srcData.Length];

            fixed (float* pSrcData = srcData)
            fixed (float* pDstData = _data)
            {
                for(var i=0;i<srcData.Length;i++)
                    pDstData[i] = Const*pSrcData[i];
            }
            
            Out.Write(_data);

            In.Put(srcData);

            return true;
        }

        public float Const { get; set; }

        private float[] _data=new float[0];

        public ISignalReader<float> In { get; set; }

        public ISignalWriter<float> Out { get; set; }
    }
}
