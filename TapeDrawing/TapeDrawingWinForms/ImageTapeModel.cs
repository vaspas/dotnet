
using System.Drawing;
using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinForms
{
    public class ImageTapeModel
    {
        public ImageTapeModel(float scaleFactor)
        {
            Engine = new DrawingEngine
                          {
                              GraphicContext = _graphicContext,
                              Area = default(Rectangle<float>)
                          };
            _scaleFactor = scaleFactor;
        }

        private Image _buffer;

        public DrawingEngine Engine { private set; get; }

        private readonly GraphicContext _graphicContext = new GraphicContext();

        private readonly  float _scaleFactor;

        public Image Buffer
        {
            get { return _buffer; }
            set
            {
                if (_buffer == value)
                    return;

                if (_buffer != null)
                    _graphicContext.Graphics = null;

                _buffer = value;

                if (_buffer != null)
                {
                    _graphicContext.Graphics = Graphics.FromImage(_buffer);
                    Engine.Area = new Rectangle<float>
                                      {
                                          Right = _buffer.Width / _scaleFactor,
                                          Bottom = _buffer.Height / _scaleFactor
                                      };

                    _graphicContext.ImageHorizontalDpi = _scaleFactor * 100f;
                    _graphicContext.ImageHorizontalScaleFactor = 1f / _scaleFactor;
                    _graphicContext.ImageVerticalDpi = _scaleFactor * 100f;
                    _graphicContext.ImageVerticalScaleFactor = 1f / _scaleFactor;

                    Engine.BeforeDraw += (e, a) => _graphicContext.Graphics.ScaleTransform(_scaleFactor, _scaleFactor);
                    Engine.AfterDraw += (e, a) => _graphicContext.Graphics.ScaleTransform(1f / _scaleFactor, 1f / _scaleFactor);
                }
            }
        }

        public void Redraw()
        {
            Engine.Draw();
        }
    }
}
