using System;
using System.Collections.Generic;
using TapeDrawing.Core.Engine;

namespace TapeImplement
{
    public class Tape
    {
        public Tape()
        {
            _position.Result = new SimpleScalePosition<int>();
        }

        

        private volatile bool _drawingProcess;

        private DrawingEngine _engine;


        public DrawingEngine Engine
        {
            get { return _engine; }
            set
            {
                if (_engine == value)
                    return;

                if(_engine!=null)
                {
                    _engine.BeforeDraw -= EngineBeforeDraw;
                    _engine.AfterDraw -= EngineAfterDraw;
                }

                _engine = value;

                if (_engine != null)
                {
                    _engine.BeforeDraw += EngineBeforeDraw;
                    _engine.AfterDraw += EngineAfterDraw;
                }
            }
        }


        public IScalePosition<int> Position
        {
            set { _position.Buffer = value; }
            get { return _position; }
        }

        private readonly BufferedScalePosition<int> _position = new BufferedScalePosition<int>();


        
        private void EngineBeforeDraw(object sender, EventArgs e)
        {
            //защищаемся от повторного входа
            if (_drawingProcess)
                throw new InvalidOperationException();

            _drawingProcess = true;

            _position.Activate();
        }

        private void EngineAfterDraw(object sender, EventArgs e)
        {
            _drawingProcess = false;
        }
    }
}
