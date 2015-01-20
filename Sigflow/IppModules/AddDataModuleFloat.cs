using System.Threading;
using IppWrapper;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public class AddDataModuleFloat:IExecuteModule
    {
        public ISignalReader<float> In { get; set; }

        public ISignalWriter<float> Out { get; set; }


        public float[] Data { get; set; }
        
        private float[] _buffer=new float[0];

        public unsafe bool? Execute()
        {
            var data = In.Take();

            if (data == null)
                return false;
            
            var localData = Data;

            //при отсутствии поправок или при некорректной длине поправок
            if (localData == null || localData.Length!=data.Length)
                Out.Write(data);
            else
            {
                if (data.Length != _buffer.Length)
                    _buffer = new float[data.Length];

                fixed (float* pSrc1 = data, pSrc2 = localData, pDst = _buffer)
                    IppHelper.Do(ipp.sp.ippsAdd_32f(pSrc1, pSrc2, pDst, data.Length));
                
                Out.Write(_buffer);
            }

            In.Put(data);

            return true;
        }
    }
}
