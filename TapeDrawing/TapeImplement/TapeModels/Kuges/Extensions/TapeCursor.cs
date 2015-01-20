
using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class TapeCursor:IExtension
    {
        public TapeCursor()
        {
            Color = new Color(255, 0, 0);
            Button = MouseButton.Left;
        }

        public MouseButton Button { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }

        public int Position
        {
            get { return CursorRenderer.Position; }
            set { CursorRenderer.Position = value; }
        }

        public Color Color { get; set; }

        public event Action PositionCursorChanged=delegate{};

        internal MouseListenerLayers.TapeCursor.TapePositionCursorRenderer CursorRenderer;

        internal void OnPositionCursorChanged()
        {
            _tapeModel.Redraw();

            if (PositionCursorChanged != null)
                PositionCursorChanged();
        }

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

            CursorRenderer = new MouseListenerLayers.TapeCursor.TapePositionCursorRenderer
            {
                TapePosition = _tapeModel.TapePosition,
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                LineColor = Color,
                LineWidth = 3,
                Position = 0
            };
            positionLayer.Add(new RendererLayer
                                  {
                                      Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                                      Settings = new RendererLayerSettings{Clip = true},
                                      Renderer = CursorRenderer
                                  });

            AddTapePositionCursor(positionLayer);
        }


        private void AddTapePositionCursor(ILayer layer)
        {
            var pressedListener = new MouseListenerLayers.PressedListener
                                      {
                                          Button = Button,
                                          Shift = Shift,
                                          Control = Control,
                                          CanStart = p => true,
                                          Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                          TapePosition = _tapeModel.TapePosition,
                                          PositionChanged = (p1, p2) =>
                                                                {
                                                                    CursorRenderer.Position = (int)p2.X;

                                                                    OnPositionCursorChanged();
                                                                },
                                          Completed = (p1, p2) =>
                                                          {
                                                              CursorRenderer.Position = (int)p2.X;
                                                              OnPositionCursorChanged();
                                                              return true;
                                                          }
                                      };

            layer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess = pressedListener
            });
            layer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener = pressedListener
            });
            layer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener =
                    new MouseListenerLayers.TapeCursor.
                    TapePositionCursorMouseWheelListener
                    {
                        Renderer = CursorRenderer,
                        PositionChanged = OnPositionCursorChanged,
                        Coeff = 1,
                        TapePosition = _tapeModel.TapePosition
                    }
            });
            layer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess =
                    new MouseListenerLayers.TapeCursor.
                    TapePositionCursorKeyboardKeyListener
                    {
                        Renderer = CursorRenderer,
                        PositionChanged = OnPositionCursorChanged,
                        DownKey = KeyboardKey.Left,
                        UpKey = KeyboardKey.Right,
                        TapePosition = _tapeModel.TapePosition
                    }
            });
        }
    }
}
