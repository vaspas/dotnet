using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class RectangleZoom:IExtension
    {
        public RectangleZoom()
        {
            Color =new Color(0, 0, 0);
            Button = MouseButton.Left;
        }

        private DataTrackModel _trackModel;

        public Color Color { get; set; }

        public MouseButton Button { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }

        /// <summary>
        /// Событие вызывается до процесса изменения размера.
        /// </summary>
        public event Action BeforeZoom = delegate { };
        /// <summary>
        /// Событие вызывается после процесса изменения размера.
        /// </summary>
        public event Action AfterZoom = delegate { };

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
                                   CanStart = p=>true,
                                   Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                   PositionChanged = OnPositionChanged,
                                   Completed = OnCompleted
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

        private void OnPositionChanged(Point<float> p1, Point<float> p2)
        {
            _renderer.Position.Left = p1.X;
            _renderer.Position.Right = p2.X;
            _renderer.Position.Bottom = p1.Y;
            _renderer.Position.Top = p2.Y;

            _trackModel.TapeModel.Redraw();
        }

        private bool OnCompleted(Point<float>? p1, Point<float> p2)
        {
            _renderer.Position.Left = 0;
            _renderer.Position.Right = 0;
            _renderer.Position.Bottom = 0;
            _renderer.Position.Top = 0;

            if (p1==null || (p1.Value.X == p2.X && p1.Value.Y == p2.Y))
                return false;

            BeforeZoom();

            _trackModel.Position.Set(
                _trackModel.Position.From +
                System.Math.Min(p1.Value.Y, p2.Y) *
                (_trackModel.Position.To - _trackModel.Position.From),
                _trackModel.Position.From +
                System.Math.Max(p1.Value.Y, p2.Y) *
                (_trackModel.Position.To - _trackModel.Position.From)
                );

            _trackModel.TapeModel.TapePosition.Set(
                (int)(_trackModel.TapeModel.TapePosition.From +
                       System.Math.Min(p1.Value.X, p2.X) *
                       (_trackModel.TapeModel.TapePosition.To -
                        _trackModel.TapeModel.TapePosition.From)),
                (int)(_trackModel.TapeModel.TapePosition.From +
                       System.Math.Max(p1.Value.X, p2.X) *
                       (_trackModel.TapeModel.TapePosition.To -
                        _trackModel.TapeModel.TapePosition.From))
                );

            _trackModel.TapeModel.Redraw();

            AfterZoom();

            return true;
        }
    }
}
