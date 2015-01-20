
using System;
using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.MouseListenerLayers;
using TapeImplement.ObjectRenderers;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class MovableShadow : IExtension<TapeModel>, IKeyProcess, IMouseButtonListener, IMouseButtonHandler
    {
        public Color Color { get; set; }

        public void AddObject<T>(T target, Func<T, int> getFrom, Func<T, int> getTo, ILayer listenerLayer, ILayer rendererLayer)
        {
            Clear();

            TargetObject = target;
            _rendererLayer = rendererLayer;
            _listenerLayer = listenerLayer;
            ShadowObject = new Shadow(getFrom(target), getTo(target));

            BuildLayer();

            Changed();
        }

        private void BuildLayer()
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            //отображение тени
            _rendererLayer.Add(
                new RendererLayer
                    {
                        Area = AreasFactory.CreateFullArea(),
                        Renderer =
                            new RegionFillRenderer<Shadow>
                                {
                                    Source = ShadowObject,
                                    GetFrom = r => r.From,
                                    GetTo = r => r.To,
                                    GetColor = o => Color,
                                    Translator = translator,
                                    TapePosition = _tapeModel.TapePosition
                                }
                    });

            //отображение тени
            _rendererLayer.Add(
                new RendererLayer
                    {
                        Area = AreasFactory.CreateFullArea(),
                        Renderer =
                            new RegionBoardsRenderer<Shadow>
                                {
                                    Source = ShadowObject,
                                    GetFrom = r => r.From,
                                    GetTo = r => r.To,
                                    LineColor = new Color(0, 0, 0),
                                    LineStyle = LineStyle.Dot,
                                    LineWidth = 1,
                                    Translator = translator,
                                    TapePosition = _tapeModel.TapePosition
                                }
                    });

            //отображение границ на всю ширину ленты
            _onMainRendererLayer.Add(
                 new RendererLayer
                    {
                        Area = _tapeModel.CreateMarginsArea(_tapeModel.ScaleSize, 0,0,0),
                        Renderer =
                            new RegionBoardsRenderer<Shadow>
                                {
                                    Source = ShadowObject,
                                    GetFrom = r => r.From,
                                    GetTo = r => r.To,
                                    LineColor = new Color(0, 0, 0),
                                    LineStyle = LineStyle.Dot,
                                    LineWidth = 1,
                                    Translator = translator,
                                    TapePosition = _tapeModel.TapePosition
                                }
                    });

            //перемещение верхней границы тени
            var selectedTo =
                new RecordMouseMoveListener<Shadow>
                    {
                        Translator = translator,
                        TapePosition = _tapeModel.TapePosition,
                        ClearOnMouseMoveOverside = true,
                        GetIndex = s => s.To,
                        SelectedAreaPixels = 5,
                        Source = ShadowObject
                    };
            _listenerLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateFullArea(),
                MouseListener = selectedTo
            });
            _listenerLayer.Add(
                new MouseListenerLayer
                {
                    Area = AreasFactory.CreateFullArea(),
                    MouseListener =
                        new PressedListener
                        {
                            Button = MouseButton.Left,
                            Translator = translator,
                            TapePosition = _tapeModel.TapePosition,
                            CanStart = p => selectedTo.Selected.Count>0,
                            PositionChanged = (p1, p2) =>
                            {
                                ShadowObject.ToShift = (int)(p2.X - p1.X);
                                _tapeModel.Redraw();
                            },
                            Completed = (p1, p2) => p1!=null && ShadowObject.Fix()
                        }
                });


            //перемещение нижней границы тени
            var selectedFrom =
                new RecordMouseMoveListener<Shadow>
                {
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition,
                    ClearOnMouseMoveOverside = true,
                    GetIndex = s => s.From,
                    SelectedAreaPixels = 5,
                    Source = ShadowObject
                };
            _listenerLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateFullArea(),
                MouseListener = selectedFrom
            });
            _listenerLayer.Add(
                new MouseListenerLayer
                {
                    Area = AreasFactory.CreateFullArea(),
                    MouseListener =
                        new PressedListener
                        {
                            Button = MouseButton.Left,
                            Translator = translator,
                            TapePosition = _tapeModel.TapePosition,
                            CanStart = p => selectedFrom.Selected.Count > 0,
                            PositionChanged = (p1, p2) =>
                            {
                                ShadowObject.FromShift = (int)(p2.X - p1.X);
                                _tapeModel.Redraw();
                            },
                            Completed = (p1, p2) => p1 != null && ShadowObject.Fix()
                        }
                });


            //перемещение тени
            _listenerLayer.Add(
                new MouseListenerLayer
                    {
                        Area = AreasFactory.CreateFullArea(),
                        MouseListener =
                            new PressedListener
                                {
                                    Button = MouseButton.Left,
                                    Translator = translator,
                                    TapePosition = _tapeModel.TapePosition,
                                    CanStart = p => p.X>=ShadowObject.From && p.X<=ShadowObject.To,
                                    PositionChanged = (p1, p2) =>
                                                          {
                                                              ShadowObject.Shift = (int)(p2.X - p1.X);
                                                              _tapeModel.Redraw();
                                                          },
                                    Completed = (p1, p2) => p1 != null && ShadowObject.Fix()
                                }
                    });

            //подключение обработки кнопок
            _listenerLayer.Add(
                new KeyboardListenerLayer
                    {
                        Area = AreasFactory.CreateFullArea(),
                        KeyboardProcess = this
                    });
        }

        public Shadow ShadowObject { get; private set; }
        public object TargetObject { get; private set; }
        private ILayer _listenerLayer;
        private ILayer _rendererLayer;
        private ILayer _onMainRendererLayer;

        public void Clear()
        {
            if (_rendererLayer == null)
                return;

            _rendererLayer.Clear();
            _rendererLayer = null;
            _listenerLayer.Clear();
            _listenerLayer = null;

            _onMainRendererLayer.Clear();

            TargetObject = null;
            ShadowObject = null;

            Changed();
        }

        private TapeModel _tapeModel;

        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;

            _tapeModel.MainLayer.Add(
                new MouseListenerLayer
                    {
                        Area = AreasFactory.CreateFullArea(),
                        MouseListener = this
                    });

            _onMainRendererLayer = new EmptyLayer { Area = AreasFactory.CreateFullArea() };
            _tapeModel.MainLayer.Add(_onMainRendererLayer);
        }

        void IKeyProcess.OnKeyDown(KeyboardKey key)
        {
            if (key != KeyboardKey.Escape || ShadowObject == null)
                return;
            
            Clear();

            _tapeModel.Redraw();
        }

        void IKeyProcess.OnKeyUp(KeyboardKey key)
        {
        }


        void IMouseButtonListener.OnMouseDown(MouseButton button)
        {
            if (button != MouseButton.Left)
                return;

            if (TargetObject == null)
                return;

            (this as IMouseButtonHandler).HandleMouseDown();
        }

        void IMouseButtonListener.OnMouseUp(MouseButton button)
        {
            if(button!=MouseButton.Left)
                return;

            if (TargetObject == null)
                return;

            Clear();
            _tapeModel.Redraw();

            (this as IMouseButtonHandler).HandleMouseUp();
        }

        Action IMouseButtonHandler.HandleMouseDown { get; set; }
        Action IMouseButtonHandler.HandleMouseUp { get; set; }

        public event Action Changed = delegate { };
    }

    public class Shadow : IObjectSource<Shadow>
    {
        public Shadow (int from, int to)
        {
            From = from;
            To = to;
        }
        
        public IEnumerable<Shadow> GetData(int from, int to)
        {
            if (From > to || To < from)
                return new Shadow[] { };

            return new [] { this };
        }

        public IEnumerable<Shadow> GetData()
        {
            return new[] { this };
        }

        public int Shift;
        public int FromShift;
        public int ToShift;

        public bool Fix()
        {
            if (Shift==0 && FromShift==0 && ToShift==0)
                return false;

            From = From;
            To = To;

            Shift = 0;
            FromShift = 0;
            ToShift = 0;

            return true;
        }
        
        private int _from;
        public int From
        {
            get { return _from + Shift+FromShift; }
            private set { _from = value; }
        }
        private int _to;
        public int To
        {
            get { return _to + Shift+ToShift; }
            private set { _to = value; }
        }
    }
}
