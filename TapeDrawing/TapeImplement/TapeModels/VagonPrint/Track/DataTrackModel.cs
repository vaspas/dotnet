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
using TapeImplement.ObjectRenderers.Signals;
using TapeImplement.SimpleRenderers;

namespace TapeImplement.TapeModels.VagonPrint.Track
{
    /// <summary>
    /// Модель дорожки с данными.
    /// </summary>
    public class DataTrackModel:BaseTrackModel
    {
        public DataTrackModel()
        {
            _diapazone.Min = -10000;
            _diapazone.Max = 10000;
            _diapazone.MinWidth = 1;
            _diapazone.MaxWidth = 5000;
            _diapazone.Set(-100, 100);
        }

        private readonly BoundedScalePosition<float> _diapazone = new BoundedScalePosition<float>();

        private float _center;

        /// <summary>
        /// Соотвествие настроек и источников данных.
        /// </summary>
        private readonly Dictionary<object, LineSettings> _settings = new Dictionary<object, LineSettings>();
        
        public void InitScale(float[] values, float[] dataValues, float center, float max, float min)
        {
            _diapazone.Set(min, max);
            _center = center;

            ScaleLayer.Add(new RendererLayer
                               {
                                   Area = AreasFactory.CreateMarginsArea(0,0,null,0, 0, 4),
                                   Settings = new RendererLayerSettings {Clip = true},
                                   Renderer = new CoordGridRenderers.ScaleGridRenderer
                                                  {
                                                      Values = values,
                                                      LineColor = TapeModel.Settings.DefaultColor,
                                                      LineStyle = LineStyle.Solid,
                                                      LineWidth = TapeModel.Settings.DefaultLineWidth,
                                                      Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                                                      GetMin =()=> min,
                                                      GetMax =()=> max
                                                  }
                               });

            ScaleLayer.Add(new RendererLayer
                               {
                                   Area = AreasFactory.CreateMarginsArea(0, 0, 3, 0),
                                   Renderer = new CoordGridRenderers.ScaleTextRenderer
                                                  {
                                                      Values = values,
                                                      FontSize = 7,
                                                      FontStyle = FontStyle.None,
                                                      FontName = TapeModel.Settings.FontName,
                                                      Angle = 90,
                                                      Color = TapeModel.Settings.DefaultColor,
                                                      Translator =
                                                          PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                                                      GetMin = ()=> min,
                                                      GetMax = ()=> max,
                                                      TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                                                      Alignment = Alignment.Right,
                                                      LayerAlignment = 0
                                                  }
                               });

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordGridRenderers.ScaleGridRenderer
                {
                    Values = new []{center},
                    LineColor = TapeModel.Settings.DefaultColor,
                    LineStyle = LineStyle.Dash,
                    LineWidth = TapeModel.Settings.DefaultLineWidth,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    GetMin =()=> min,
                    GetMax =()=> max
                }
            });

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordGridRenderers.ScaleGridRenderer
                {
                    Values = dataValues,
                    LineColor = TapeModel.Settings.LightColor,
                    LineStyle = LineStyle.Dot,
                    LineWidth = TapeModel.Settings.DefaultLineWidth,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    GetMin =()=> min,
                    GetMax =()=> max
                }
            });
        }


        public void AddLeftPointObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Stream image)
        {
            DataLayer.Add(new RendererLayer
                             {
                                 Area = AreasFactory.CreateRelativeArea(-1,1,0,1),
                                 Renderer = new PointObjectRenderer<T>
                                                {
                                                    Source = source,
                                                    GetIndex = getIndex,
                                                    Image = image,
                                                    Alignment = Alignment.Bottom,
                                                    Angle = 90,
                                                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                                                    TapePosition = TapeModel.TapePosition
                                                }
                             });
        }


        public void AddRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            var center = (_center - _diapazone.From) / (_diapazone.To - _diapazone.From);

            var layer = new EmptyLayer
            {
                Area = AreasFactory.CreateRelativeArea(center - 0.1f, center + 0.1f, 0, 1)
            };
            DataLayer.Add(layer);

            layer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new RegionObjectRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Image = image,
                    Alignment = Alignment.Right,
                    Angle = 90,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }


        public void AddDeviationsRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Color color, float width)
        {
            var center = (_center - _diapazone.From)/(_diapazone.To - _diapazone.From);

            var layer = new EmptyLayer
                            {
                                Area = AreasFactory.CreateRelativeArea(center-0.1f, center+0.1f, 0,1)
                            };
            DataLayer.Add(layer);

            layer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(null, null, 0, 0, width, 0),
                Renderer = new RegionFillRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    GetColor = o => color,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
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
                    Diapazone = _diapazone,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    TapePosition = TapeModel.TapePosition,
                    LineColor = lineSettings.Color,
                    LineStyle = lineSettings.Style,
                    LineWidth = lineSettings.Width
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
                    Diapazone = _diapazone,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
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
