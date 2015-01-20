﻿using System;
using System.Windows.Forms;
using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeDrawingWinFormsDx;

namespace ComparativeTapeTest.Windows
{
    class WinFormsDx : Form, IWindow
    {
        public WinFormsDx()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            ContextMenu=new ContextMenu();
            ContextMenu.MenuItems.Add(new MenuItem("123"));
        }

        public TapeDrawing.Core.Engine.DrawingEngine Engine
        {
            get { return _model.Engine; }
        }

        private readonly ControlTapeModel _model=new ControlTapeModel{Antialias = true};

        public void Open()
        {
            _model.Control = this;
            _model.Engine.MainLayer = new EmptyLayer
                                   {
                                       Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
                                   };

            WindowState = FormWindowState.Maximized;
            Show();
        }


        public void Redraw()
        {
            Invalidate();
        }


        public string Title { get; set; }
        
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _model.Control = null;

            base.OnClosing(e);
        }
    }
}
