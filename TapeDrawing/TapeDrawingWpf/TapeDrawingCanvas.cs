using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace TapeDrawingWpf
{
    /// <summary>
    /// Элемент, на который может выводиться графика TapeDrawing
    /// </summary>
    public class TapeDrawingCanvas : FrameworkElement
    {
        public Action<DrawingContext> OnDraw { get; set; }

        protected override void OnRender(DrawingContext dc)
        {
            OnDraw(dc);

            //эксперимент с быстродействием рисования линий
            /*var pg = new PathGeometry();
            var pathFig = new PathFigure();

            var r = new Random();

            pathFig.StartPoint = new Point(r.Next(500), r.Next(500));

            for (int i = 0; i < 1000; i++)
                pathFig.Segments.Add(new LineSegment(new Point(r.Next(500), r.Next(500)), true));

            pg.Figures.Add(pathFig);

            dc.DrawGeometry(null, 
                new System.Windows.Media.Pen(new System.Windows.Media.SolidColorBrush(Color.FromRgb(0,0,0)), 1) 
                , pg);*/

            base.OnRender(dc);
        }

    }
}
