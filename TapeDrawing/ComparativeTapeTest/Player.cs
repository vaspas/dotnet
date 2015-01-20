using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using TapeImplement;

namespace ComparativeTapeTest
{
    class Player
    {
        public Player()
        {
            _timer=new Timer();
            _timer.Elapsed += _timer_Elapsed;

            TapePosition = new SimpleScalePosition<int>
                                {
                                    From = 0,To = 1000
                                };

            Tapes=new List<Tape>();
        }

        public Action _timerAction;

        public List<Tape> Tapes { get; private set; }

        public Action Redraw { get; set; }

        public int Min { get; set; }
        public int Max { get; set; }

        public IScalePosition<int> TapePosition { get; private set; }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(_timerAction!=null)
                _timerAction();

            if (Redraw != null)
                Redraw();
        }

        private readonly Timer _timer;

        public void StartShift(float shift, int periodMs)
        {
            if(periodMs<=0)
            {
                Stop();
            }

            _timer.Start();
            _timer.Interval = periodMs;

            _timerAction = () =>
                               {
                                   if (TapePosition.From <= Min && TapePosition.To >= Max)
                                       return;

                                   var shiftIndexes = (int)(shift * (TapePosition.To - TapePosition.From));
                                   TapePosition.Set(TapePosition.From + shiftIndexes,
                                                    TapePosition.To + shiftIndexes);

                                   CheckBeginEnd();

                                   Tapes.ForEach(t => t.Position.Set(TapePosition.From, TapePosition.To));
                               };
        }

        private void CheckBeginEnd()
        {
            if (TapePosition.From < Min)
            {
                var shiftIndexes = Min - TapePosition.From;
                TapePosition.Set(TapePosition.From + shiftIndexes,
                                                    TapePosition.To + shiftIndexes);
            }

            if (TapePosition.To > Max)
            {
                var shiftIndexes = TapePosition.To - Max;
                TapePosition.Set(TapePosition.From + shiftIndexes,
                                                    TapePosition.To + shiftIndexes);
            }
        }

        public void GoTo(int index)
        {
            var dist = TapePosition.To - TapePosition.From;

            TapePosition.Set(index - dist / 2, index + dist / 2);
            
            CheckBeginEnd();

            Tapes.ForEach(t => t.Position.Set(TapePosition.From, TapePosition.To));

            if (Redraw != null)
                Redraw();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
