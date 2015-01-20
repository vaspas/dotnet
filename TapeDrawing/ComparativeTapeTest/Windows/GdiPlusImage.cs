using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeDrawingWinForms;

namespace ComparativeTapeTest.Windows
{
    class GdiPlusImage : Form, IWindow
    {
        
        public TapeDrawing.Core.Engine.DrawingEngine Engine
        {
            get { return _model.Engine; }
        }

        private readonly ImageTapeModel _model = new ImageTapeModel(1);
        

        public void Open()
        {
            _model.Buffer = new Bitmap(600,400);
            _model.Engine.MainLayer = new EmptyLayer
                                   {
                                       Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
                                   };
            WindowState = FormWindowState.Maximized;
            
            Show();
        }


        public void Redraw()
        {
            _model.Redraw();
            CreateGraphics().DrawImage(_model.Buffer, 0, 0);
            _model.Buffer.Save("D:\\testTape.jpg", ImageFormat.Jpeg);
        }


        public string Title { get; set; }

    }
}
