using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.CoordGridRenderers;
using TapeImplement.ObjectRenderers;
using TapeImplement.ObjectRenderers.LinearScale;
using TapeImplement.ObjectRenderers.Signals;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.Vagon.Extensions;
using ScaleTextRenderer = TapeImplement.ObjectRenderers.LinearScale.ScaleTextRenderer;

namespace TapeImplement.TapeModels.Vagon.Track
{
    /// <summary>
    /// Модель дорожки с данными.
    /// </summary>
    public class DataTrackModel:BaseTrackModel
    {
        public DataTrackModel()
        {
            _diapazone.Min = -100;
            _diapazone.Max = 100;
            _diapazone.MinWidth = 10;
            _diapazone.MaxWidth = 500;
            _diapazone.Set(-100, 100);
        }

        private readonly BoundedScalePosition<float> _diapazone = new BoundedScalePosition<float>();

        /// <summary>
        /// Соотвествие настроек и источников данных.
        /// </summary>
        private readonly Dictionary<object, LineSettings> _settings = new Dictionary<object, LineSettings>();

        public IScalePosition<float> Position
        {
            get { return _diapazone; }
        }

        public IScaleDiapazone<float> Diapazone
        {
            get { return _diapazone; }
        }

        public Func<int, string> GetValue { get; set; }

        private IArea<float> CreateRelativeArea(float left, float right, float bottom, float top)
        {
            return TapeModel.Vertical
                       ? AreasFactory.CreateRelativeArea(bottom, top, 1 - right, 1 - left)
                       : AreasFactory.CreateRelativeArea(left, right, bottom, top);
        }

        private IArea<float> CreateMarginsArea(float? left, float? right, float? bottom, float? top, float width, float height)
        {
            return TapeModel.Vertical
                       ? AreasFactory.CreateMarginsArea(bottom, top, right, left, height, width)
                       : AreasFactory.CreateMarginsArea(left, right, top, bottom, width, height);
        }

        private Alignment CreateAlignment (Alignment alignment)
        {
            if(!TapeModel.Vertical)
                return alignment;

            var newAlignment = Alignment.None;

            if ((alignment & Alignment.Right) != 0)
                newAlignment |= Alignment.Bottom;

            if ((alignment & Alignment.Left) != 0)
                newAlignment |= Alignment.Top;

            if ((alignment & Alignment.Top) != 0)
                newAlignment |= Alignment.Right;

            if ((alignment & Alignment.Bottom) != 0)
                newAlignment |= Alignment.Left;

            return newAlignment;
        }

        public void Init(string headerText)
        {
            Init(headerText, Alignment.None);
        }

        public void InitFixed(string headerText, Alignment textAlignment, float[] values, Func<float, string> valuePresenter)
        {
            InitScale(values, valuePresenter);
            InitInternal(headerText, textAlignment);
        }

        public void Init(string headerText, Alignment textAlignment)
        {
            InitAutoScale();
            InitInternal(headerText, textAlignment);
        }

        private void InitInternal(string headerText, Alignment textAlignment)
        {
            InitHeader(headerText, textAlignment);
            InitMouseWheelScale();
            InitMouseWheelShift();

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 1, 0, 1),
                Renderer = new BorderRenderer
                {
                    Bottom = false,
                    Color = new Color(0, 0, 0),
                    Left = TapeModel.Vertical,
                    LineStyle = LineStyle.Solid,
                    Right = false,
                    Top = !TapeModel.Vertical,
                    LineWidth = 1
                }
            });
        }
        
        private void InitAutoScale()
        {
            var translator= TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().Translator
                       : PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator;
            
            ScaleLayer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0.8f, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new ScaleLinesRenderer
                {
                    Diapazone = Position,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    LineColor = new Color(0,0,0),
                    Mask = new[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 30,
                    Translator = translator
                }
            });
            
            ScaleLayer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0.5f, 0.8f, 0, 1),
                Renderer = new ScaleTextRenderer
                {
                    Diapazone = Position,
                    FontSize = 7,
                    FontName = "Nina",
                    FontColor = new Color(0, 0, 0),
                    Mask = new[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 30,
                    ScalePresentation = v=>v.ToString(),
                    Translator = translator
                }
            });

            ScaleLayer.Add(
                new RendererLayer
                    {
                        Area = CreateRelativeArea(0.95f, 1, 0, 1),
                        Settings = new RendererLayerSettings {Clip = true},
                        Renderer =
                            new ScaleLinesRenderer
                                {
                                    Diapazone = Position,
                                    LineStyle = LineStyle.Solid,
                                    LineWidth = 1,
                                    LineColor = new Color(0, 0, 0),
                                    Mask = new[] {0.1f, 0.2f, 0.5f},
                                    MinPixelsDistance = 6,
                                    Translator = translator
                                }
                    });

            ScaleLayer.Add(
                new MouseListenerLayer
                    {
                        Area = AreasFactory.CreateFullArea(),
                        Settings = new MouseListenerLayerSettings{ MouseMoveOutside = true },
                        MouseListener =
                            new MouseListenerLayers.LinearScale.ShiftDiapazoneMouseMoveListener<float>
                                {
                                    Button = MouseButton.Left,
                                    Diapazone = Position,
                                    OnChanged = TapeModel.Redraw,
                                    Translator = translator
                                }

                    });

            ScaleLayer.Add(
                new MouseListenerLayer
                {
                    Area = AreasFactory.CreateFullArea(),
                    Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                    MouseListener =
                        new MouseListenerLayers.LinearScale.ZoomDiapazoneMouseWheelListener<float>
                        {
                            Diapazone = Position,
                            OnChanged = TapeModel.Redraw,
                            Factor = 0.1f
                        }
                });

            ScaleLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 1, 0, 1),
                Renderer = new BorderRenderer
                {
                    Bottom = TapeModel.Vertical,
                    Color = new Color(0, 0, 0),
                    Left = false,
                    LineStyle = LineStyle.Solid,
                    Right = !TapeModel.Vertical,
                    Top = false,
                    LineWidth = 1
                }
            });
        }

        private void InitScale(float[] values, Func<float, string> valuePresenter)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            ScaleLayer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0.5f, 0.8f, 0, 1),
                Settings = new RendererLayerSettings { Clip = false },
                Renderer = new CoordGridRenderers.ScaleTextRenderer
                {
                    Angle = -45,
                    GetMax = () => Position.To,
                    GetMin = () => Position.From,
                    Color = new Color(0,0,0),
                    FontName = "Nina",
                    FontStyle = FontStyle.None,
                    FontSize = 6,
                    Translator = translator,
                    Values = values,
                    ValuePresentation = valuePresenter,
                    TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator
                }
            });

            ScaleLayer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0.95f, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new ScaleGridRenderer
                {
                    GetMax = () => Position.To,
                    GetMin = () => Position.From,
                    LineColor = new Color(0, 0, 0),
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    Values = values,
                    Translator = translator
                }
            });

            ScaleLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 1, 0, 1),
                Renderer = new BorderRenderer
                {
                    Bottom = TapeModel.Vertical,
                    Color = new Color(0, 0, 0),
                    Left = false,
                    LineStyle = LineStyle.Solid,
                    Right = !TapeModel.Vertical,
                    Top = false,
                    LineWidth = 1
                }
            });
        }

        private void InitHeader(string headerText, Alignment alignment)
        {
            var al = CreateAlignment(alignment);

            var r = new TextRenderer
                        {
                            Size = 10,
                            Style = FontStyle.None,
                            FontName = "Nina",
                            Color = new Color(0, 0, 0),
                            Angle = TapeModel.Vertical ? 0 : -90,
                            TextAlignment = al,
                            LayerAlignment = al,
                            Text = headerText
                        };

            ScaleLayer.Add(new RendererLayer
            {
                Area = CompositeArea<float>.Create()
                .Add(CreateRelativeArea(0f, 0.5f, 0, 1))
                .Add(CreateMarginsArea(2,2,2,2,0,0)),
                Settings = new RendererLayerSettings{Clip = true},
                Renderer = r
            });

            TapeModel.CursorPositionChanged +=
                i =>
                    {
                        if (GetValue != null)
                            r.Text = string.Format("{0}: {1}", headerText, GetValue(i));
                    };
        }

        private void InitMouseWheelShift()
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener =
                    new MouseListenerLayers.LinearScale.
                    ShiftDiapazoneMouseWheelListener<int>
                    {
                        Button = MouseButton.Left,
                        Diapazone = TapeModel.TapePosition,
                        OnChanged = TapeModel.Redraw,
                        Factor = 0.1f,
                        Translator = translator
                    }
            });
        }

        private void InitMouseWheelScale()
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var mouseListener = new MouseListenerLayers.LinearScale.ZoomDiapazoneMouseWheelListener<int>
                             {
                                 Control = true,
                                 Diapazone = TapeModel.TapePosition,
                                 OnChanged = TapeModel.Redraw,
                                 Factor = 0.1f
                             };

            DataLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener = mouseListener
            });


            DataLayer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess = mouseListener
            });
        }

        public void AddLeftPointObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Stream image)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, -1, 1),
                Renderer = new PointObjectRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Image = image,
                    Alignment = Alignment.Bottom,
                    Angle = TapeModel.Vertical?90: 0,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }

        public void AddPointObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Stream image)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
                             {
                                 Area = AreasFactory.CreateMarginsArea(0,0,0,0),
                                 Renderer = new PointObjectRenderer<T>
                                                {
                                                    Source = source,
                                                    GetIndex = getIndex,
                                                    Image = image,
                                                    Alignment = Alignment.None,
                                                    Angle = TapeModel.Vertical ? 90 : 0,
                                                    Translator = translator,
                                                    TapePosition = TapeModel.TapePosition
                                                }
                             });
        }

        public void AddRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new RegionObjectRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Image = image,
                    Alignment = Alignment.None,
                    Angle = 0,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }

        public void AddDeviationsRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Color color, float width)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;
            
            DataLayer.Add(new RendererLayer
            {
                Area = CreateMarginsArea(0, 0, null, null, 0, width),
                Renderer = new RegionFillRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    GetColor =o=> color,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }

        public void AddToolTip<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Func<T, string> toolTip, float width)
            where T:class
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var myToolTipSender = new object();

            DataLayer.Add(new MouseListenerLayer
            {
                Area = CreateMarginsArea(0, 0, null, null, 0, width),
                Settings = new MouseListenerLayerSettings { ControlMouseLeave = false, MouseMoveOutside = true },
                MouseListener = new RegionMouseMoveListener<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition,
                    ClearOnMouseMoveOverside = true,
                    SelectedChanged = s =>
                    {
                        var se = TapeModel.GetExtension<SelectedObjects>();
                        if (se != null)
                        {
                            se.RemoveMouseOverFor(myToolTipSender);
                            foreach (var o in s)
                                se.AddMouseOver(o, myToolTipSender);
                        }

                        var tte = TapeModel.GetExtension<Extensions.ToolTip>();
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

        public void AddSignal(ISignalSource source, LineSettings lineSettings)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new SignalRenderer
                {
                    Source = source,
                    Diapazone = Position,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition,
                    LineColor = lineSettings.Color,
                    LineStyle = lineSettings.Style,
                    LineWidth = lineSettings.Width
                }
            });
            _settings.Add(source, lineSettings);
        }

        public void AddSignal(IIntegratedSignalSource source, LineSettings lineSettings)
        {
            DataLayer.Add(CreateRendererLayer(source, lineSettings));
            _settings.Add(source, lineSettings);
        }

        public void AddSignalToBack(IIntegratedSignalSource source, LineSettings lineSettings)
        {
            DataLayer.Insert(0, CreateRendererLayer(source, lineSettings));
            _settings.Add(source, lineSettings);
        }

        private RendererLayer CreateRendererLayer(IIntegratedSignalSource source, LineSettings lineSettings)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            return new RendererLayer
                       {
                           Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                           Renderer = new SwitchRenderer<int>
                                          {
                                              ScalePosition = TapeModel.TapePosition,
                                              IsHorizontal = !TapeModel.Vertical,
                                              SwitchValue = 1f,
                                              OriginalStep = source.Step,
                                              BeforeDraw = source.SetWindowSize,
                                              LowDensityRenderer = new SignalRenderer
                                                                       {
                                                                           Source = source,
                                                                           Diapazone = Position,
                                                                           Translator = translator,
                                                                           TapePosition = TapeModel.TapePosition,
                                                                           LineColor = lineSettings.Color,
                                                                           LineStyle = lineSettings.Style,
                                                                           LineWidth = lineSettings.Width
                                                                       },
                                              HighDensityRenderer = new SignalRenderer
                                                                        {
                                                                            Source = source,
                                                                            Diapazone = Position,
                                                                            Translator = translator,
                                                                            TapePosition = TapeModel.TapePosition,
                                                                            LineColor = lineSettings.Color,
                                                                            LineStyle = lineSettings.Style,
                                                                            LineWidth = lineSettings.Width
                                                                        }
                                          }
                       };
        }

        public LineSettings GetSettingsFor(ISignalSource source)
        {
            return _settings[source];
        }

        

        public void AddSignal(ISignalPointSource source, LineSettings lineSettings, bool renderAsRegions)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new SignalPointRenderer
                {
                    Source = source,
                    Diapazone = Position,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition,
                    LineColor = lineSettings.Color,
                    LineStyle = lineSettings.Style,
                    LineWidth = lineSettings.Width,
                    RenderAsRegions = renderAsRegions
                }
            });
            _settings.Add(source, lineSettings);
        }

        public LineSettings GetSettingsFor(ISignalPointSource source)
        {
            return _settings[source];
        }

        public void UpdateSignalSettings()
        {
            foreach (var src in _settings.Keys)
            {
                var layer = DataLayer.OfType<RendererLayer>()
                    .First(l => GetSource(l.Renderer) == src);

                SetSettings(layer.Renderer, _settings[src]);
            }
        }

        private static void SetSettings(IRenderer renderer, LineSettings settings)
        {
            if (renderer is SwitchRenderer<int>)
            {
                SetSettings((renderer as SwitchRenderer<int>).LowDensityRenderer, settings);
                SetSettings((renderer as SwitchRenderer<int>).HighDensityRenderer, settings);
            }

            if (renderer is SignalRenderer)
            {
                (renderer as SignalRenderer).LineColor = settings.Color;
                (renderer as SignalRenderer).LineStyle = settings.Style;
                (renderer as SignalRenderer).LineWidth = settings.Width;
            }

            if(renderer is SignalPointRenderer)
            {
                (renderer as SignalPointRenderer).LineColor = settings.Color;
                (renderer as SignalPointRenderer).LineStyle = settings.Style;
                (renderer as SignalPointRenderer).LineWidth = settings.Width;
            }
        }

        private static object GetSource(IRenderer renderer)
        {
            if (renderer is SwitchRenderer<int>)
                return GetSource((renderer as SwitchRenderer<int>).LowDensityRenderer);

            if (renderer is SignalRenderer)
                return (renderer as SignalRenderer).Source;

            if (renderer is SignalPointRenderer)
                return (renderer as SignalPointRenderer).Source;

            return null;
        }


    }
}
