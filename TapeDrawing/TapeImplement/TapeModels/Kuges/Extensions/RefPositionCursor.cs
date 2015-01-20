
using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class RefPositionCursor:IExtension
    {
        public RefPositionCursor()
        {
            Color = new Color(255, 0, 0);
            Button = MouseButton.Left;
        }

        public MouseButton Button { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }

        public float Position
        {
            get { return CursorRenderer.Position; }
            set { CursorRenderer.Position = value; }
        }

        public Color Color { get; set; }

        public event Action RefPositionCursorChanged;

        internal MouseListenerLayers.TapeCursor.TapeRefPositionCursorRenderer CursorRenderer;

        internal void OnRefPositionCursorChanged()
        {
            _tapeModel.Redraw();

            if (RefPositionCursorChanged != null)
                RefPositionCursorChanged();
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

            CursorRenderer = new MouseListenerLayers.TapeCursor.TapeRefPositionCursorRenderer
            {
                Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                LineColor = Color,
                LineWidth = 3,
                Position = 0.5f
            };
            positionLayer.Add(new RendererLayer
                                  {
                                      Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                                      Renderer = CursorRenderer
                                  });

            AddTapeRefPositionCursor(positionLayer);
        }


        private void AddTapeRefPositionCursor(ILayer layer)
        {
            var pressedListener = new MouseListenerLayers.PressedListener
                                      {
                                          Button = Button,
                                          Shift = Shift,
                                          Control = Control,
                                          CanStart = p => true,
                                          Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                          PositionChanged = (p1, p2) =>
                                                                {
                                                                    CursorRenderer.Position = p2.X;

                                                                    OnRefPositionCursorChanged();
                                                                },
                                          Completed = (p1, p2) =>
                                                          {
                                                              CursorRenderer.Position = p2.X;
                                                              OnRefPositionCursorChanged();
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
                    TapeRefPositionCursorMouseWheelListener
                    {
                        Renderer = CursorRenderer,
                        PositionChanged = OnRefPositionCursorChanged,
                        Coeff = 1,
                        TapePosition = _tapeModel.TapePosition
                    }
            });
            layer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess =
                    new MouseListenerLayers.TapeCursor.
                    TapeRefPositionCursorKeyboardKeyListener
                    {
                        Renderer = CursorRenderer,
                        PositionChanged = OnRefPositionCursorChanged,
                        DownKey = KeyboardKey.Left,
                        UpKey = KeyboardKey.Right,
                        TapePosition = _tapeModel.TapePosition
                    }
            });
        }
    }
}
