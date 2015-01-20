using System.Drawing.Printing;
using TapeDrawing.Core.Engine;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinForms
{
    public class PrintTapeModel
    {
        public PrintTapeModel(int dpi)
        {
            _engine = new DrawingEngine
                          {
                              GraphicContext = _graphicContext
                          };
            _graphicContext.ImageHorizontalDpi = dpi;
            _graphicContext.ImageHorizontalScaleFactor = 100f / dpi;
            _graphicContext.ImageVerticalDpi = dpi;
            _graphicContext.ImageVerticalScaleFactor = 100f / dpi;
        }

        private PrintDocument _document;

        private readonly DrawingEngine _engine;

        private readonly GraphicContext _graphicContext = new GraphicContext();


        private void Connect(PrintDocument document)
        {
            document.PrintPage += PrintPage;
        }

        private void Disconnect(PrintDocument document)
        {
            document.PrintPage -= PrintPage;
        }


        public PrintDocument Document
        {
            get { return _document; }
            set
            {
                if (_document == value)
                    return;

                if (_document != null)
                    Disconnect(_document);

                _document = value;

                if (_document != null)
                    Connect(_document);
            }
        }

        public ILayer MainLayer
        {
            get { return _engine.MainLayer; }
            set { _engine.MainLayer=value; }
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            _graphicContext.Graphics = e.Graphics;

            float left = _document.DefaultPageSettings.Margins.Left;
            float top = _document.DefaultPageSettings.Margins.Top;
            float width = _document.DefaultPageSettings.PaperSize.Width - left - _document.DefaultPageSettings.Margins.Right;
            float height = _document.DefaultPageSettings.PaperSize.Height - top - _document.DefaultPageSettings.Margins.Bottom;

            if (_document.DefaultPageSettings.Landscape)
            {
                float w = width;
                width = height;
                height = w;
            }

            _engine.Area = new Rectangle<float>
                               {
                                   Left = left,
                                   Right = left + width,
                                   Bottom = top + height,
                                   Top = top
                               };
            _engine.Draw();

            e.HasMorePages = OnNextPage();
        }

        public delegate bool NextPageDelegate();

        public NextPageDelegate OnNextPage { get; set; }
    }
}
