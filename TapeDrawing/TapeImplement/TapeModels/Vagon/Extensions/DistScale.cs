using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.CoordGridRenderers;
using TapeImplement.MouseListenerLayers;
using TapeImplement.ObjectRenderers;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.Vagon.Track;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    /// <summary>
    /// Модель дорожки шкалы дистанции.
    /// </summary>
    public class DistScale:IExtension<TapeModel>
    {
        internal DistScale()
        {
            _defaultFillColorAction = () =>
                                      _layer.Add(new RendererLayer
                                                     {
                                                         Area = CreateRelativeArea(0, 1, 0, 1),
                                                         Renderer = new FillRenderer(new Color(0, 185, 255))
                                                     });
            _actionsBefore.Add(_defaultFillColorAction);
        }

        private Action _defaultFillColorAction;

        private EmptyLayer _layer;

        private readonly List<Action> _actionsBefore = new List<Action>();
        private readonly List<Action> _actionsNormal=new List<Action>();
        private readonly List<Action> _actionsAfter = new List<Action>();

        private TapeModel _tapeModel;

        public void AddSource( ICoordinateSource source, FontSettings unitFont, FontSettings interruptFont)
        {
            _actionsNormal.Add(()=>AddSourceInternal( source, unitFont, interruptFont));
        }

        private void AddSourceInternal( ICoordinateSource source, FontSettings unitFont, FontSettings interruptFont)
        {
            AddInterrupt(source, interruptFont);
            AddUnits(source, unitFont);
            

            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(
                 new MouseListenerLayer
                 {
                     Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                     Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                     MouseListener =
                         new MouseListenerLayers.LinearScale.ShiftDiapazoneMouseMoveListener<int>
                         {
                             Button = MouseButton.Left,
                             Diapazone = _tapeModel.TapePosition,
                             OnChanged = _tapeModel.Redraw,
                             Translator = translator
                         }

                 });

            _layer.Add(
                new MouseListenerLayer
                {
                    Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                    Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                    MouseListener =
                        new MouseListenerLayers.LinearScale.ZoomDiapazoneMouseWheelListener<int>
                        {
                            Diapazone = _tapeModel.TapePosition,
                            OnChanged = _tapeModel.Redraw,
                            Factor = 0.1f
                        }

                });

            
        }

        private IArea<float> CreateRelativeArea(float left, float right, float bottom, float top)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateRelativeArea(1-top, 1-bottom, left, right)
                       : AreasFactory.CreateRelativeArea(left, right, bottom, top);
        }
        private IArea<float> CreateMarginsArea(float? left, float? right, float? bottom, float? top, float width, float height)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateMarginsArea(bottom, top, right, left, height, width)
                       : AreasFactory.CreateMarginsArea(left, right, top, bottom, width, height);
        }
        private IArea<float> CreateStreakArea(float position, float width)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateStreakArea(position, width, true)
                       : AreasFactory.CreateStreakArea(1-position, width, false);
        }

        private void AddUnits(ICoordinateSource source, FontSettings font)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var largeUnitTextLayer = new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0.1f, 0.3f),
                Renderer = new CoordUnitTextRenderer
                {
                    Source = source,
                    IncreaseAlignment = _tapeModel.Vertical ? Alignment.Right : Alignment.Bottom,
                    DecreaseAlignment = _tapeModel.Vertical ? Alignment.Right : Alignment.Bottom,
                    TapePosition = _tapeModel.TapePosition,
                    Translator = translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 75,
                    Angle = font.Angle,
                    FontName = font.Name,
                    Color = font.Color,
                    FontSize = font.Size,
                    FontStyle = FontStyle.None,
                    TextFormatString = string.Empty
                }
            };
            _layer.Add(largeUnitTextLayer);

            var largeUnitGridLayer = new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 0.2f),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = source,
                    LineColor = new Color(0,0,0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 3,
                    TapePosition = _tapeModel.TapePosition,
                    Translator = translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 75,
                    PriorityRenderers = new CoordUnitBaseRenderer[] { }
                }
            };
            _layer.Add(largeUnitGridLayer);

            var mediumUnitGridLayer = new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 0.1f),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = source,
                    LineColor = new Color(0, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 2,
                    TapePosition = _tapeModel.TapePosition,
                    Translator = translator,
                    Mask = new[] { 0.1f, 0.5f },
                    MinPixelsDistance = 35,
                    PriorityRenderers = new[] { largeUnitGridLayer.Renderer as CoordUnitBaseRenderer }
                }
            };
            _layer.Add(mediumUnitGridLayer);

            var smallUnitGridLayer = new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 0.05f),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = source,
                    LineColor = new Color(0, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    TapePosition = _tapeModel.TapePosition,
                    Translator = translator,
                    Mask = new [] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 7,
                    PriorityRenderers = new[] { largeUnitGridLayer.Renderer as CoordUnitBaseRenderer, mediumUnitGridLayer.Renderer as CoordUnitBaseRenderer }
                }
            };
            _layer.Add(smallUnitGridLayer);
        }

        private void AddInterrupt(ICoordinateSource source, FontSettings font)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var gridRenderer = new CoordInterruptGridRenderer
                                   {
                                       Source = source,
                                       LineColor = new Color(0, 0, 0),
                                       LineStyle = LineStyle.Solid,
                                       LineWidth = 3,
                                       TapePosition = _tapeModel.TapePosition,
                                       Translator = translator,
                                       Filter = ic => true
                                   };

            var interruptLayer = new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 0.2f),
                Renderer = gridRenderer
            };
            _layer.Add(interruptLayer);

            var textRenderer = new CoordInterruptTextRenderer
                                   {
                                       Source = source,
                                       Alignment = _tapeModel.Vertical ? Alignment.Right : Alignment.Bottom,
                                       TapePosition = _tapeModel.TapePosition,
                                       Translator = translator,
                                       Filter = ic => true,
                                       Angle = font.Angle,
                                       Color = font.Color,
                                       FontName = font.Name,
                                       FontSize = font.Size,
                                       FontStyle = font.Style,
                                       MinPixelsDistance = 30,
                                       PriorityFilter = ci => false,
                                       TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                                       TextFormatString = string.Empty
                                   };

            var interruptATextLayer = new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0.1f, 0.3f),
                Renderer = textRenderer
            };
            _layer.Add(interruptATextLayer);
        }


        public void AddRecordObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Stream image)
        {
            _actionsBefore.Add(() => AddRecordObjectRendererInternal(source, getIndex, image));
        }

        private void AddRecordObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getIndex, Stream image)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 2),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new PointObjectRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Image = image,
                    Alignment = Alignment.Top,
                    Angle = _tapeModel.Vertical ? -90 : 0,
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition
                }
            });
        }

        public void AddRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            _actionsBefore.Add(() => AddRegionObjectRendererInternal(source, getFrom, getTo, image));
        }

        private void AddRegionObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 2),
                Settings = new RendererLayerSettings{Clip = true},
                Renderer = new RegionObjectRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Image = image,
                    Alignment = Alignment.Left | Alignment.Top,
                    Angle = _tapeModel.Vertical? -90:0,
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition
                }
            });
        }

        public void AddRegionToolTip<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Func<T, string> toolTip, float startposition, float width)
            where T : class
        {
            _actionsNormal.Add(() => AddRegionToolTipInternal(source, getFrom, getTo, toolTip, startposition, width));
        }

        private void AddRegionToolTipInternal<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Func<T, string> toolTip, float startposition, float width)
            where T:class
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var myToolTipSender = new object();

            _layer.Add(new MouseListenerLayer
            {
                Area = CreateMarginsArea(0,1, startposition, null, 0,  width),
                Settings = new MouseListenerLayerSettings { ControlMouseLeave = false, MouseMoveOutside = true },
                MouseListener = new RegionMouseMoveListener<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition,
                    ClearOnMouseMoveOverside = true,
                    SelectedChanged = s =>
                    {
                        var se = _tapeModel.GetExtension<SelectedObjects>();
                        if (se != null)
                        {
                            se.RemoveMouseOverFor(myToolTipSender);
                            foreach (var o in s)
                                se.AddMouseOver(o, myToolTipSender);
                        }

                        var tte = _tapeModel.GetExtension<ToolTip>();
                        if (tte != null)
                        {
                            tte.RemoveMouseOverFor(myToolTipSender);
                            
                            foreach (var o in s)
                            {
                                var obj = o;
                                tte.AddMouseOver(obj, toolTip, myToolTipSender);
                            }
                        }
                    }
                }
            });
        }


        public void AddRegionMovableShadow<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, float startposition, float width)
            where T : class
        {
            var shadowListenerLayer = new EmptyLayer();

            _actionsBefore.Add(()=>
                                   {
                                       shadowListenerLayer.Area = CreateMarginsArea(0, 1, startposition, null, 0, width);
                                       _layer.Add(shadowListenerLayer);
                                   });

            _actionsAfter.Add(() => AddRegionMovableShadowInternal(source, getFrom, getTo, startposition, width, shadowListenerLayer));
        }

        private void AddRegionMovableShadowInternal<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, float startposition, float width,ILayer shadowListenerLayer)
            where T : class
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var mouseMove =
                new RegionMouseMoveListener<T>
                    {
                        Source = source,
                        GetFrom = getFrom,
                        GetTo = getTo,
                        Translator = translator,
                        TapePosition = _tapeModel.TapePosition,
                        ClearOnMouseMoveOverside = true,
                        SelectedChanged = s => { }
                    };

            var shadowRendererLayer = new EmptyLayer
                                         {
                                             Area = CreateMarginsArea(0, 1, startposition, null, 0, width)
                                         };
            _layer.Add(shadowRendererLayer);
            var pressedListener =
                new PressedListener
                    {
                        Button = MouseButton.Left,
                        Translator = translator,
                        TapePosition = _tapeModel.TapePosition,
                        CanStart = p => true,
                        PositionChanged = (p1, p2) => { },
                        Completed = (p1, p2) =>
                        {
                            var obj = mouseMove.Selected.FirstOrDefault();
                            if (obj == null)
                                return false;

                            var ms = _tapeModel.GetExtension<MovableShadow>();
                            if (obj == ms.TargetObject)
                                return false;

                            ms.AddObject(obj, getFrom, getTo, shadowListenerLayer, shadowRendererLayer);

                            _tapeModel.Redraw();
                            return true;
                        } 
                    };

            _layer.Add(new MouseListenerLayer
            {
                Area = CreateMarginsArea(0, 1, startposition, null, 0, width),
                Settings = new MouseListenerLayerSettings { ControlMouseLeave = false, MouseMoveOutside = true },
                MouseListener = mouseMove
            });

            _layer.Add(new MouseListenerLayer
            {
                Area = CreateMarginsArea(0, 1, startposition, null, 0, width),
                MouseListener = pressedListener
            });
        }


        public void AddTextRecordObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> objectPresentetion, bool before)
        {
            _actionsNormal.Add(() => AddTextRecordObjectRendererInternal(source, getIndex, objectPresentetion, before));
        }

        private void AddTextRecordObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> objectPresentetion, bool before)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RecordLineRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition,
                    LineColor = new Color(0, 0, 0),
                    LineWidth = 2,
                    LineStyle = LineStyle.Solid
                }
            });
            
            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 2),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RecordTextRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Alignment = (_tapeModel.Vertical ? Alignment.Top:Alignment.Top) | (before? Alignment.Right : Alignment.Left),
                    Angle = (_tapeModel.Vertical ? -90 : 0),
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition,
                    ObjectPresentation = objectPresentetion,
                    FontType = "Nina",
                    FontSize = 8,
                    FontColor = new Color(0, 0, 0),
                    FontStyle = FontStyle.None
                }
            });
        }

        public void AddFone(Color color)
        {
            _actionsBefore.Remove(_defaultFillColorAction);
            _actionsBefore.Add(() =>
                        _layer.Add(new RendererLayer
                        {
                            Area = CreateRelativeArea(0, 1, 0, 1),
                            Renderer = new FillRenderer(new Color(0, 185, 255))
                        }));
        }

        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;

            _layer = new EmptyLayer
            {
                Area = CreateMarginsArea(_tapeModel.ScaleSize, 0, 0, null, 0, _tapeModel.DistScaleSize)
            };

            _actionsBefore.ForEach(a => a());
            _actionsNormal.ForEach(a => a());
            _actionsAfter.ForEach(a => a());

            _tapeModel.MainLayer.Add(_layer);
        }
    }
}
