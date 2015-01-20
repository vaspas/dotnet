
using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class TapeArea:IExtension
    {
        public TapeArea()
        {
            Color =new Color(50, 0, 255, 0);
            Button = MouseButton.Left;
        }

        public MouseButton Button { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }

        public int AreaFrom
        {
            get { return AreaRenderer.PositionFrom; }
            set { AreaRenderer.PositionFrom = value; }
        }

        public int AreaTo
        {
            get { return AreaRenderer.PositionTo; }
            set { AreaRenderer.PositionTo = value; }
        }

        public Color Color { get; set; }

        public event Action AreaChanged=delegate{};

        internal MouseListenerLayers.TapeArea.TapeAreaRenderer AreaRenderer;


        
        private TapeModel _tapeModel;
        
        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;
            
            var positionLayer = new EmptyLayer
            {
                Area = AreasFactory.CreateMarginsArea(_tapeModel.ScaleSize, 0, 0, 0)
            };
            _tapeModel.MainLayer.Add(positionLayer);

            AreaRenderer = new MouseListenerLayers.TapeArea.TapeAreaRenderer
            {
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                Color = Color,
                TapePosition = tapeModel.TapePosition
            };

            positionLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = AreaRenderer
            });

            AddTapeArea(positionLayer);

        }


        private void AddTapeArea(ILayer layer)
        {
            var pressedListener = new MouseListenerLayers.PressedListener
                                      {
                                          Button = Button,
                                          Control = Control,
                                          Shift = Shift,
                                          CanStart = p => true,
                                          Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                          TapePosition = _tapeModel.TapePosition,
                                          PositionChanged = (p1, p2) =>
                                                                {
                                                                    AreaRenderer.PositionFrom = (int)p1.X;
                                                                    AreaRenderer.PositionTo = (int)p2.X;

                                                                    _tapeModel.Redraw();

                                                                    AreaChanged();
                                                                },
                                          Completed = (p1, p2) => p1!=null &&
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
