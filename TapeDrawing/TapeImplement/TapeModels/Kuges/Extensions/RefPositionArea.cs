using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class RefPositionArea:IExtension
    {
        public RefPositionArea()
        {
            Color = new Color(50, 0, 255, 0);
            Button = MouseButton.Left;
        }

        public MouseButton Button { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }

        public float RefAreaFrom
        {
            get { return AreaRenderer.PositionFrom; }
            set { AreaRenderer.PositionFrom = value; }
        }

        public float RefAreaTo
        {
            get { return AreaRenderer.PositionTo; }
            set { AreaRenderer.PositionTo = value; }
        }

        public Color Color { get; set; }

        public event Action RefAreaChanged;

        internal MouseListenerLayers.TapeArea.TapeRefAreaRenderer AreaRenderer;


        
        private TapeModel _tapeModel;

        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;
            _tapeModel.AddExtension(this);

            var positionLayer = new EmptyLayer
            {
                Area = AreasFactory.CreateMarginsArea(_tapeModel.ScaleSize, 0, 0, 0)
            };
            _tapeModel.MainLayer.Add(positionLayer);

            AreaRenderer = new MouseListenerLayers.TapeArea.TapeRefAreaRenderer
            {
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                Color = Color
            };

            positionLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = AreaRenderer
            });

            AddTapeRefArea(positionLayer);

        }


        private void AddTapeRefArea(ILayer layer)
        {
            var pressedListener = new MouseListenerLayers.PressedListener
                                      {
                                          Button = Button,
                                          Control = Control,
                                          Shift = Shift,
                                          CanStart = p => true,
                                          Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                          PositionChanged = (p1, p2) =>
                                                                {
                                                                    AreaRenderer.PositionFrom = p1.X;
                                                                    AreaRenderer.PositionTo = p2.X;

                                                                    _tapeModel.Redraw();

                                                                    if (RefAreaChanged != null)
                                                                        RefAreaChanged();
                                                                },
                                          Completed = (p1, p2) => p1 != null && 
                                              (p1.Value.X != p2.X || p1.Value.Y != p2.Y)
                                      };

            layer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess = pressedListener
            });
            layer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                MouseListener = pressedListener
            });
        }
    }
}
