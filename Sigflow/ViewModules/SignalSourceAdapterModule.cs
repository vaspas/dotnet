using System;
using Sigflow.Dataflow;
using Sigflow.Module;
using TdGraphsParts.Renderers.Graph;

namespace ViewModules
{
    /// <remarks>
    /// INSPECTED 02/05/2013
    /// </remarks>
    public class SignalSourceAdapterModule<T> : IExecuteModule, ISignalSource<T>, ISignalInfoSource<T>
        where T:struct 
    {
        public bool? Execute()
        {
             if (!In.NextBlockSize.HasValue)
                return false;

            var blockSize = In.NextBlockSize.Value;

            if (In.Available >= blockSize * 2)
            {
                In.TrySkip(blockSize);
                return true;
            }

            lock (_sync)
            {
                if (_primary.Length != blockSize)
                    _primary = new T[blockSize];

                if (In.ReadTo(_primary))
                    _newDataAvailable = true;
            }
                    
            return false;
        }

        private readonly object _sync=new object();

        public ISignalReader<T> In { get; set; }

        private T[] _primary=new T[0];
        private T[] _secondary = new T[0];
        private volatile bool _newDataAvailable;

               
        private int _current;
        public float GetStartIndex(float fromIndex)
        {
            if (_newDataAvailable)
            {
                lock (_sync)
                {
                    if (_secondary.Length != _primary.Length)
                        _secondary = new T[_primary.Length];

                    var temp = _primary;
                    _primary = _secondary;
                    _secondary = temp;

                    _from= From;
                    _step = Step;

                    _newDataAvailable = false;
                }                
            }

            if (_from > fromIndex)
                _current = 0;
            else
            {
                _current = (int)((fromIndex - _from) / _step);
                //Note:под вопросом
                while ((_from + _current * _step) > fromIndex)
                    _current--;
            }
            return _from + _current * _step;
        }

        public float From { get; set; }


        private float _from;

        private float _step=1;

        private float _newStep = 1;
        public float Step 
        {
            get
            {
                return _newStep;
            }
            set 
            {
                lock (_sync)
                {
                    _primary = new T[0];
                    _newStep = value;
                    _newDataAvailable = true;
                }
            }
        }

        public T GetNextValue()
        {
            return _secondary[_current++];
        }

        public bool IsDataAvailable
        {
            get { return _secondary.Length > _current; }
        }

        public T GetValueFor(float index)
        {
            var dataIndex = (int)Math.Round((index - _from) / _step);
            if (dataIndex < 0 || dataIndex >= _secondary.Length)
                return default(T);
            return _secondary[dataIndex];
        }

        public float GetNearestIndexFor(float index)
        {
            return (int)Math.Round((index - _from) / _step) * _step + _from;
        }
    }
}
