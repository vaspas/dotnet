using System;
using TdGraphsParts.Renderers.Graph;

namespace WindowsFormsGenerator
{
    class IntegratedMinMaxSignalSource:IIntegratedSignalSource<float>
    {
        public ISignalSource<float> Internal { get; set; }

        private float _winSize;
        public void SetWindowSize(float windowSize)
        {
            _winSize = windowSize;
        }

        public float GetStartIndex(float fromIndex)
        {
            _remainder = _winSize;
            _min = null;
            return Internal.GetStartIndex(fromIndex);
        }

        public float Step 
        { 
            get { return _winSize <= Internal.Step ? Internal.Step : _winSize/2; }
        }

        private float _remainder;
        private float? _min;
        public float GetNextValue()
        {
            if (_winSize <= Internal.Step)
                return Internal.GetNextValue();

            if (_min.HasValue)
            {
                var val = _min.Value;
                _min = null;
                return val;
            }

            var max = float.MinValue;
            _min = float.MaxValue;

            while (Internal.IsDataAvailable && _remainder-Internal.Step>0)
            {
                var val = Internal.GetNextValue();

                max = Math.Max(max, val);
                _min = Math.Min(_min.Value, val);

                _remainder -= Internal.Step;
            }

            _remainder += _winSize;

            return max;
        }

        public bool IsDataAvailable { get { return Internal.IsDataAvailable || _min.HasValue; } }
    }
}
