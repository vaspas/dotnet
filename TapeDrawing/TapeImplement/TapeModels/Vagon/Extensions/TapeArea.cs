
using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class TapeArea:IExtension<TapeModel>
    {
        internal TapeArea()
        {
            Color = new Color(80, 255, 200, 0);
        }
        
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

        private IArea<float> CreateMarginsArea(float left, float right, float bottom, float top)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateMarginsArea(bottom, top, right, left)
                       : AreasFactory.CreateMarginsArea(left, right, top, bottom);
        }

        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;

            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var positionLayer = new EmptyLayer
            {
                Area = CreateMarginsArea(_tapeModel.ScaleSize, 0, 0, 0)
            };
            _tapeModel.MainLayer.Add(positionLayer);

            AreaRenderer = new MouseListenerLayers.TapeArea.TapeAreaRenderer
            {
                Translator = translator,
                Color = Color,
                TapePosition = tapeModel.TapePosition
            };

            positionLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Settings = new RendererLayerSettings{Clip = true},
                Renderer = AreaRenderer
            });

            AddTapeArea(positionLayer);

        }

        private float _zone = 0.05f;

        private void AddTapeArea(ILayer layer)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;
            
            var pressedListener = new MouseListenerLayers.PressedListener
                                      {
                                          Button = MouseButton.Left,
                                          Control = false,
                                          Shift = false,
                                          CanStart = p => true,
                                          Translator = translator,
                                          TapePosition = _tapeModel.TapePosition,
                                          PositionChanged = (p1, p2) =>
                                                                {
                                                                    AreaRenderer.PositionFrom = (int)p1.X;
                                                                    AreaRenderer.PositionTo = (int)p2.X;
                                                                    
                                                                    var k = (p2.X - _tapeModel.TapePosition.From)/
                                                                            (_tapeModel.TapePosition.To -
                                                                             _tapeModel.TapePosition.From);

                                                                    if (k < _zone)
                                                                        AreaRenderer.PositionTo =
                                                                            _tapeModel.TapePosition.From;
                                                                    if (k > 1 - _zone)
                                                                        AreaRenderer.PositionTo =
                                                                            _tapeModel.TapePosition.To;

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

            var clickListener = new MouseListenerLayers.PressedListener
            {
                Button = MouseButton.Left,
                Control = false,
                Shift = true,
                CanStart = p => true,
                Translator = translator,
                TapePosition = _tapeModel.TapePosition,
                PositionChanged = (p1, p2) =>
                {
                    AreaRenderer.PositionTo = (int)p2.X;

                    _tapeModel.Redraw();

                    AreaChanged();
                },
                Completed = (p1, p2) => p1 != null
            };

            layer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess = clickListener
            });
            layer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                MouseListener = clickListener
            });
        }
    }
}
