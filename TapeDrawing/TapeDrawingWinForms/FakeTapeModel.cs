using System.Drawing;
using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinForms
{
    public class FakeTapeModel
    {
        public FakeTapeModel(int width, int heigth)
        {
            var fakeImage = new Bitmap(width, heigth);
            _graphicContext = new FakeGraphicContext {Graphics = Graphics.FromImage(fakeImage)};
            Engine = new DrawingEngine
                         {
                             GraphicContext = _graphicContext,
                             Area = new Rectangle<float>
                                        {
                                            Left = 0,
                                            Right = 0,
                                            Bottom = width,
                                            Top = heigth
                                        }
            };
        }

        
        public DrawingEngine Engine { private set; get; }

        private readonly FakeGraphicContext _graphicContext;

        public ILayer MainLayer
        {
            get { return Engine.MainLayer; }
            set { Engine.MainLayer = value; }
        }

    }
}
