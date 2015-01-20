using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeDrawingWinFormsDx;

namespace WinFormsTest
{
    public partial class TestUserControl : UserControl
    {
        public TestUserControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint|ControlStyles.Opaque, true);

            InitializeComponent();

            var _tm = new ControlTapeModel();
            _tm.Control = this;

            _tm.Engine.MainLayer = new RendererLayer
                                    {
                                        Area = AreasFactory.CreateRelativeArea(0.1f,0.9f,0.1f,0.9f),
                                        Renderer = new FillAllRenderer()
                                    };
            _tm.Engine.MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(10,10,10,null,60,60),
                /*Area = new RelativeArea
                {
                    CurrentRelative = new TapeDrawing.Core.Primitives.RectangleF
                    {
                        Left = 0.1f,
                        Top = 0.1f,
                        Right = 0.8f,
                        Bottom = 0.8f
                    }
                },*/
                Renderer = new FillRectangleRenderer()
            });

            var ml = new MouseHoldListener
                         {
                             OnRedraw = ()=>Invalidate()
                         };
            _tm.Engine.MainLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateMarginsArea(10, 10, 10, null, 60, 60),
                MouseListener = ml
            });
            _tm.Engine.MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(10, 10, 10, null, 60, 60),
                Renderer= ml
            });

            _tm.Engine.MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(10,10,null,10,60,60),
                Renderer = new TranslatorTestRenderer
                               {
                                   Translator = PointTranslatorConfigurator.CreateLinear()
                                   .MirrorX()
                                   //.MirrorY()
                                   //.ChangeAxels()
                                   //.MirrorX()
                                   //.MirrorY()
                                   .Translator,
                                   TextAlignment = Alignment.Right,
                                   TextAngle = 180
                               },
                 
            });

            _tm.Engine.MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0,1,0,1),
                Renderer = new ImageRenderer(),

            });

            var clipLayer = new RendererLayer
                                {
                                    Area = AreasFactory.CreateRelativeArea(0.2f, 0.7f, 0.2f, 0.7f),
                                    Settings = new RendererLayerSettings {Clip = true},
                                    Renderer = new FillAllRenderer()
                                };
            _tm.Engine.MainLayer.Add(clipLayer);

            _scrollArea = new VerticalScrollArea {Height = 200};
            var scrollLayer = new EmptyLayer
                                  {
                                      Area = _scrollArea
                                  };
            clipLayer.Add(scrollLayer);

            scrollLayer.Add(new RendererLayer
                                {
                                    Area = AreasFactory.CreateMarginsArea(0, 0, 0, null, 0, 50),
                                    Renderer = new TextRenderer{SomeText = "botton text"}
                                });
            scrollLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 150, null, 0, 50),
                Renderer = new TextRenderer { SomeText = "top text" }
            });

            //_printModel.MainLayer = _tm.Engine.MainLayer;
            //_printModel.Document=new PrintDocument();
            //_printModel.OnNextPage = () => false;
        }

        private VerticalScrollArea _scrollArea;

       //private PrintTapeModel _printModel=new PrintTapeModel();

        private void button1_Click(object sender, EventArgs e)
        {
            _scrollArea.ScrollValue = (_scrollArea.ScrollValue + 0.1f)%1;
            Invalidate();
            //using (var previewDialog = new PrintPreviewDialog())
            //{
            ////    previewDialog.Document = _printModel.Document;
            //    previewDialog.ShowDialog();
            //}
        }
    }
}
