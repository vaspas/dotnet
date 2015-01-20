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
using TapeImplement.ObjectRenderers;
using TapeImplement.ObjectRenderers.LinearScale;
using TapeImplement.ObjectRenderers.Signals;
using TapeImplement.SimpleRenderers;

namespace TapeImplement.TapeModels.Kuges.Track
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
        
        public void InitScale(FontSettings font, Color lineColor)
        {
            ScaleLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0.7f, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new ScaleLinesRenderer
                {
                    Diapazone = Position,
                    LineColor = lineColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    Mask = new[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 30,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                }
            });

            ScaleLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 0.7f, 0, 1),
                Renderer = new ScaleTextRenderer
                {
                    Diapazone = Position,
                    FontColor = font.Color,
                    FontSize = font.Size,
                    FontStyle = font.Style,
                    FontName = font.Name,
                    Angle = font.Angle,
                    Mask = new[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 30,
                    ScalePresentation = v=>v.ToString(),
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                }
            });

            ScaleLayer.Add(
                new RendererLayer
                    {
                        Area = AreasFactory.CreateRelativeArea(0.9f, 1, 0, 1),
                        Settings = new RendererLayerSettings {Clip = true},
                        Renderer =
                            new ScaleLinesRenderer
                                {
                                    Diapazone = Position,
                                    LineColor = lineColor,
                                    LineStyle = LineStyle.Solid,
                                    LineWidth = 1,
                                    Mask = new[] {0.1f, 0.2f, 0.5f},
                                    MinPixelsDistance = 6,
                                    Translator =
                                        PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                                }
                    });

            ScaleLayer.Add(
                new MouseListenerLayer
                    {
                        Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                        Settings = new MouseListenerLayerSettings{ MouseMoveOutside = true },
                        MouseListener =
                            new MouseListenerLayers.LinearScale.ShiftDiapazoneMouseMoveListener<float>
                                {
                                    Button = MouseButton.Left,
                                    Diapazone = Position,
                                    OnChanged = TapeModel.Redraw,
                                    Translator =
                                        PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                                }

                    });

            ScaleLayer.Add(
                new MouseListenerLayer
                {
                    Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                    Settings = new MouseListenerLayerSettings { MouseMoveOutside = true },
                    MouseListener =
                        new MouseListenerLayers.LinearScale.ZoomDiapazoneMouseWheelListener<float>
                        {
                            Control = true,
                            Diapazone = Position,
                            OnChanged = TapeModel.Redraw,
                            Factor = 0.1f
                        }

                });
        }


        public void AddPointObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Stream image)
        {
            DataLayer.Add(new RendererLayer
                             {
                                 Area = AreasFactory.CreateMarginsArea(0,0,0,0),
                                 Renderer = new PointObjectRenderer<T>
                                                {
                                                    Source = source,
                                                    GetIndex = getIndex,
                                                    Image = image,
                                                    Alignment = Alignment.None,
                                                    Angle = 0,
                                                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                                    TapePosition = TapeModel.TapePosition
                                                }
                             });
        }

        public void AddRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new RegionObjectRenderer<T>
                {
                    Source = source,
                    GetFrom = getTo,
                    GetTo = getTo,
                    Image = image,
                    Alignment = Alignment.None,
                    Angle = 0,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }


        public void AddSignal(ISignalSource source, LineSettings lineSettings)
        {
            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new SignalRenderer
                {
                    Source = source,
                    Diapazone = Position,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
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
            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new SwitchRenderer<int>
                               {
                                   ScalePosition = TapeModel.TapePosition,
                                   IsHorizontal = true,
                                   SwitchValue = 1f,
                                   OriginalStep = source.Step,
                                   BeforeDraw = source.SetWindowSize,
                                   LowDensityRenderer = new SignalRenderer
                                   {
                                       Source = source,
                                       Diapazone = Position,
                                       Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                       TapePosition = TapeModel.TapePosition,
                                       LineColor = lineSettings.Color,
                                       LineStyle = lineSettings.Style,
                                       LineWidth = lineSettings.Width
                                   },
                                   HighDensityRenderer = new SignalRenderer
                                   {
                                       Source = source,
                                       Diapazone = Position,
                                       Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                       TapePosition = TapeModel.TapePosition,
                                       LineColor = lineSettings.Color,
                                       LineStyle = lineSettings.Style,
                                       LineWidth = lineSettings.Width
                                   }
                               }
                
                
            });
            _settings.Add(source, lineSettings);
        }

        public LineSettings GetSettingsFor(ISignalSource source)
        {
            return _settings[source];
        }

        public void AddSignal(ISignalPointSource source, LineSettings lineSettings)
        {
            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new SignalPointRenderer
                {
                    Source = source,
                    Diapazone = Position,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    TapePosition = TapeModel.TapePosition,
                    LineColor = lineSettings.Color,
                    LineStyle = lineSettings.Style,
                    LineWidth = lineSettings.Width
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
