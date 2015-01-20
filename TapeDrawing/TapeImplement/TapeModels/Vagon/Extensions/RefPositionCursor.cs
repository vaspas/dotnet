
using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class RefPositionCursor:IExtension<TapeModel>
    {
        internal RefPositionCursor()
        {
            Color = new Color(150, 255, 0, 0);
        }

        public float Position
        {
            get { return _cursorRenderer.Position; }
            set { _cursorRenderer.Position = value; }
        }

        private int? _absolutePosition;
        public int AbsolutePosition
        {
            get
            {
                if (_absolutePosition.HasValue)
                    return _absolutePosition.Value;

                return (int) Math.Round(_tapeModel.TapePosition.From +
                              Position*(_tapeModel.TapePosition.To - _tapeModel.TapePosition.From));
            }
            set 
            {
                _absolutePosition = value;
                RefPositionCursorChanged();
            }
        }

        public Color Color { get; set; }

        public event Action RefPositionCursorChanged=delegate{};

        private MouseListenerLayers.TapeCursor.TapeRefPositionCursorRenderer _cursorRenderer;
        
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

            var positionLayer = new EmptyLayer
                                    {
                                        Area = CreateMarginsArea(_tapeModel.ScaleSize, 0, 0, 0)
                                    };
            _tapeModel.MainLayer.Add(positionLayer);

            var translator = tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _cursorRenderer = new MouseListenerLayers.TapeCursor.TapeRefPositionCursorRenderer
            {
                Translator = translator,
                LineColor = Color,
                LineWidth = 2,
                Position = 0.5f
            };
            positionLayer.Add(new RendererLayer
                                  {
                                      Area = AreasFactory.CreateFullArea(),
                                      Renderer = new RendererDecorator
                                                     {
                                                         Before = ()=>
                                                                      {
                                                                          if (!_absolutePosition.HasValue)
                                                                              return;

                                                                          Position =
                                                                              (float)(_absolutePosition.Value -
                                                                               _tapeModel.TapePosition.From)/
                                                                              (_tapeModel.TapePosition.To -
                                                                               _tapeModel.TapePosition.From);
                                                                          
                                                                          _absolutePosition = null;
                                                                      },
                                                         Internal = _cursorRenderer
                                                     }
                                  });

            AddTapeRefPositionCursor(positionLayer);
        }


        private void AddTapeRefPositionCursor(ILayer layer)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var pressedListener = new MouseListenerLayers.PressedListener
                                      {
                                          Button = MouseButton.Left,
                                          Shift = false,
                                          Control = false,
                                          CanStart = p => true,
                                          Translator = translator,
                                          PositionChanged = (p1, p2) =>
                                                                {
                                                                    _cursorRenderer.Position = p2.X;

                                                                    RefPositionCursorChanged();
                                                                    _tapeModel.Redraw();
                                                                },
                                          Completed = (p1, p2) =>
                                                          {
                                                              _cursorRenderer.Position = p2.X;
                                                              RefPositionCursorChanged();
                                                              _tapeModel.Redraw();
                                                              return true;
                                                          }
                                      };

            layer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateFullArea(),
                KeyboardProcess = pressedListener
            });
            layer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateFullArea(),
                Settings = new MouseListenerLayerSettings{MouseUpOutside = false},
                MouseListener = pressedListener
            });
            /*layer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener =
                    new MouseListenerLayers.TapeRefPositionCursor.
                    TapeRefPositionCursorMouseWheelListener
                    {
                        Renderer = CursorRenderer,
                        PositionChanged = OnRefPositionCursorChanged,
                        Coeff = 1,
                        TapePosition = _tapeModel.TapePosition
                    }
            });*/
            layer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateFullArea(),
                KeyboardProcess =
                    new MouseListenerLayers.TapeCursor.
                    TapeRefPositionCursorKeyboardKeyListener
                    {
                        Renderer = _cursorRenderer,
                        PositionChanged = ()=>
                                              {
                                                  RefPositionCursorChanged();
                                                  _tapeModel.Redraw();
                                              },
                        DownKey = _tapeModel.Vertical?KeyboardKey.Down: KeyboardKey.Left,
                        UpKey = _tapeModel.Vertical ? KeyboardKey.Up : KeyboardKey.Right,
                        TapePosition = _tapeModel.TapePosition
                    }
            });
        }
    }
}
