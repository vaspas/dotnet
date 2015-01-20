using System;
using System.Collections.Generic;
using TapeImplement.ObjectRenderers.Signals;

namespace ComparativeTapeTest.Generators.SignalGenerator
{
    class SignalSource : ISignalSource, ISourceId
    {
        public SignalSource()
        {
            Data = new List<float>();
        }

        public string Id { get; set; }

        public List<float> Data { get; private set; }


        private int _currentIndex;
        public float GetStartIndex(int fromIndex)
        {
            _currentIndex = fromIndex;
            return fromIndex;
        }

        public float Step
        {
            get { return 1; }
        }
        public float GetNextValue()
        {
            return Data[_currentIndex++];
        }

        public bool IsDataAvailable
        {
            get { return !(_currentIndex >= Data.Count); }
        }
    }
}
