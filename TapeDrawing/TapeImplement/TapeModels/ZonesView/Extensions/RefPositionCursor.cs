
using System;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;

namespace TapeImplement.TapeModels.ZonesView.Extensions
{
    public class RefPositionCursor:IExtension<TapeModel>
    {
        internal RefPositionCursor()
        {
            Color = new Color(150, 255, 0, 0);
        }

        public float Position { get; set; }

        public int AbsolutePosition
        {
            get
            {
                return (int) Math.Round(_tapeModel.TapePosition.From +
                              Position*(_tapeModel.TapePosition.To - _tapeModel.TapePosition.From));
            }
        }

        public Color Color { get; set; }

        public event Action RefPositionCursorChanged=delegate{};
        
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
                                        Area = CreateMarginsArea(0, 0, 0, 0)
                                    };
            _tapeModel.MainLayer.Add(positionLayer);
            

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
                                                                    //Position = p2.X;

                                                                    //RefPositionCursorChanged();
                                                                    //_tapeModel.Redraw();
                                                                },
                                          Completed = (p1, p2) =>
                                                          {
                                                              Position = p2.X;
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
        }
    }
}
