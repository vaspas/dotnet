using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class RefPositionRectangle:IExtension
    {
        public RefPositionRectangle()
        {
            Color = new Color(100,0, 0, 0);
            Button = MouseButton.Left;
        }

        public MouseButton Button { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }

        public Color Color { get; set; }

        public Rectangle<float> Position
        {
            get { return _renderer.Position; }
        }

        public Action PositionChanged { get; set; }

        private DataTrackModel _trackModel;

        private MouseListenerLayers.SelectedRectangle.SelectedRectangleRenderer _renderer;

        public void Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);
            
            _renderer = new MouseListenerLayers.SelectedRectangle.SelectedRectangleRenderer
            {
                FillColor = Color,
                LineColor = new Color(255, Color.R, Color.G, Color.B),
                LineWidth = 1,
                LineStyle = LineStyle.Solid,
                Translator = PointTranslatorConfigurator.CreateLinear().Translator
            };
            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = _renderer
            });

            var listener = new MouseListenerLayers.PressedListener
                               {
                                   Button = Button,
                                   Shift = Shift,
                                   Control = Control,
                                   CanStart = p => true,
                                   Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                   PositionChanged = (p1, p2) =>
                                                         {
                                                             if (p1.X == p2.X && p1.Y == p2.Y)
                                                                 return;

                                                             _renderer.Position.Left = p1.X;
                                                             _renderer.Position.Right = p2.X;
                                                             _renderer.Position.Bottom = p1.Y;
                                                             _renderer.Position.Top = p2.Y;

                                                             _trackModel.TapeModel.Redraw();

                                                             if (PositionChanged != null)
                                                                 PositionChanged();
                                                         },
                                   Completed = (p1, p2) =>
                                                   {
                                                       var worked = p1 != null && 
                                                                    (p1.Value.X != p2.X || p1.Value.Y != p2.Y);

                                                       if (p1!=null && !worked
                                                           && (_renderer.Position.Left != _renderer.Position.Right
                                                               || _renderer.Position.Bottom != _renderer.Position.Top))
                                                       {
                                                           _renderer.Position.Left = _renderer.Position.Right
                                                                                     =
                                                                                     _renderer.Position.Bottom =
                                                                                     _renderer.Position.Top = 0;
                                                           _trackModel.TapeModel.Redraw();
                                                           return true;
                                                       }

                                                       return worked;
                                                   }
                               };
            _trackModel.DataLayer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess = listener
            });
            _trackModel.DataLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener = listener
            });
        }
    }
}
