using System;
using System.Runtime.InteropServices;
using Sigflow.Dataflow;
using Sigflow.Module;

namespace SigProModules
{

    /// <summary>
    /// Передает сигнал в вольтах в SigPro.
    /// </summary>
    public class SignalToNodeModuleFloat : IExecuteModule, IMasterModule
    {
        public bool? Execute()
        {
            if (_buffer==null || !_arrH.IsAllocated)
                return false;

            if (!In.ReadTo(_buffer))
                return false;

            SigImport.WriteSignal(_sig, _arrH.AddrOfPinnedObject());

            return true;
        }
        
        public ISignalReader<float> In { get; set;}


        private float[] _buffer;
        /// <summary>
        /// Массив для обмена данными м/у узлами.
        /// </summary>
        // Array arr;
        private GCHandle _arrH;

        /// <summary>
        /// Указатель на SharedSig структуру в неупарвляемой памяти.
        /// </summary>
        private IntPtr _sig;


        public int NodeNumber { get; set; }

        public int ChannelsCount { get; set; }

        public float Frequency { get; set; }

        public int ChannelBlockSize { get; set; }

        public int SigProBufferBlocksCount { get; set; }

        public Action<string> OnMessage { get; set; }

        public bool StartDemand { get; set; }
    


        public bool Start()
        {
            var pSig = IntPtr.Zero;
            unsafe
            {
                var createSignalResult = SigImport.CreateSignal(NodeNumber, ChannelsCount,
                                                             1,
                                                             SigProBufferBlocksCount*ChannelBlockSize,
                                                             4, //тип float
                                                             Frequency,
                                                             &pSig);

                //http://stackoverflow.com/questions/8357106/stackoverflowexception-in-wpf-when-call-method-from-c-library
                try
                {
                    throw new Exception("Fpu reset intended");
                }
                catch (Exception)
                {
                }

                if (createSignalResult)
                {
                    if (OnMessage != null)
                        OnMessage("Невозможна запись в SigPro узел № " + NodeNumber);

                    return StartDemand;
                }
            }



            if (SigImport.SetSignalScale(0, 1f / Frequency, 0, pSig))
            {
                if (OnMessage != null)
                    OnMessage("Невозможна запись в SigPro узел № " + NodeNumber);

                return StartDemand;
            }
            
            var ss = new SigImport.SharedSig();
            Marshal.PtrToStructure(pSig, ss);
            ss.powered = 1;
            ss.nosign = 0;
            ss.x_units = 1;
            ss.y_units = 3;
            Marshal.StructureToPtr(ss, pSig, false);

            if(SigImport.SetSignalWrite(ChannelBlockSize,32678, 0, pSig))
            {
                if (OnMessage != null)
                    OnMessage("Невозможна запись в SigPro узел № " + NodeNumber);

                return StartDemand;
            }
            
            _sig = pSig;
            _buffer=new float[ChannelBlockSize*ChannelsCount];
            _arrH = GCHandle.Alloc(_buffer, GCHandleType.Pinned);

            return true;
        }

        public void BeforeStop()
        {
        }
        public void AfterStop()
        {
            if (_buffer == null)
                return;

            _arrH.Free();
            _buffer = null;

            SigImport.ResetSignalWrite(_sig);
            var p = _sig;
            unsafe
            {
                SigImport.CloseOutSignalWait(&p);
            }

            _sig = IntPtr.Zero;
        }
        
        
    }
}
