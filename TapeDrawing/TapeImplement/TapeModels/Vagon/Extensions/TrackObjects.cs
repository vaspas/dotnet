using System;
using System.Collections.Generic;
using System.IO;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.ObjectRenderers;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class TrackObjects:IExtension<TapeModel>
    {

        
        private TapeModel _tapeModel;

        private EmptyLayer _layer;

        private readonly List<Action> _actions = new List<Action>();

        private IArea<float> CreateMarginsArea(float left, float right, float bottom, float top)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateMarginsArea(top,bottom, right, left)
                       : AreasFactory.CreateMarginsArea(left, right,  bottom, top);
        }

        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;

            _layer = new EmptyLayer
            {
                Area = CreateMarginsArea(_tapeModel.ScaleSize, 0, 0, _tapeModel.DistScaleSize)
            };

            _actions.ForEach(a => a());

            _tapeModel.MainLayer.Add(_layer);
        }

        public void AddRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Color color, int? width)
        {
            _actions.Add(() => AddRegionObjectRendererInternal(source, getFrom, getTo, color, width));
        }

        public void AddLeftRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Color color, int width)
        {
            _actions.Add(() => AddLeftRegionObjectRendererInternal(source, getFrom, getTo, color, width));
        }

        public void AddRightRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Color color, int width)
        {
            _actions.Add(() => AddRightRegionObjectRendererInternal(source, getFrom, getTo, color, width));
        }

        private void AddRegionObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Color color, int? width)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(new RendererLayer
            {
                Area =width.HasValue? CreateMarginsArea(0, 0, null, null, 0, width.Value):CreateRelativeArea(0,1,0,1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RegionFillRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    GetColor = o => color,
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition
                }
            });
        }

        private void AddLeftRegionObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Color color, int width)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateMarginsArea(0, 0, 0, null, 0, width),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RegionFillRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    GetColor = o => color,
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition
                }
            });
        }

        private void AddRightRegionObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Color color, int width)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateMarginsArea(0, 0, null, 0, 0, width),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RegionFillRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    GetColor = o => color,
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition
                }
            });
        }


        public void AddLeftRegionObjectToolTip<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Func<T, string> toolTip, int width)
            where T : class
        {
            _actions.Add(() => AddLeftRegionObjectToolTipInternal(source, getFrom, getTo, toolTip, width));
        }

        private void AddLeftRegionObjectToolTipInternal<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Func<T, string> toolTip, float width)
            where T : class
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            var myToolTipSender = new object();

            _layer.Add(new MouseListenerLayer
            {
                Area = CreateMarginsArea(0, 0, 0, null, 0, width),
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

                        var tte = _tapeModel.GetExtension<Extensions.ToolTip>();
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


        public void AddRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            _actions.Add(() => AddRegionObjectRendererInternal(source, getFrom, getTo, image));
        }

        private void AddRegionObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 2),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RegionObjectRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Image = image,
                    Alignment = Alignment.Left | Alignment.Top,
                    Angle = _tapeModel.Vertical ? -90 : 0,
                    Translator = translator,
                    TapePosition = _tapeModel.TapePosition
                }
            });
        }

        public void AddLeftTextRecordObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> objectPresentetion, Stream image)
        {
            _actions.Add(() => AddLeftTextRecordObjectRendererInternal(source, getIndex, objectPresentetion, image));
        }

        private void AddLeftTextRecordObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> objectPresentetion, Stream image)
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

            var translator2 = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ShiftY(5).ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().ShiftY(-5).Translator;

            if(image!=null)
            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 2),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new PointObjectRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Image = image,
                    Alignment = _tapeModel.Vertical ? Alignment.Top : Alignment.Bottom,
                    Angle = _tapeModel.Vertical ? -90 : 180,
                    Translator = translator2,
                    TapePosition = _tapeModel.TapePosition
                }
            });

            

            var translator3 = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ShiftY(25).ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().ShiftY(-25).Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 2),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RecordTextRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Alignment = _tapeModel.Vertical ? Alignment.Left | Alignment.Bottom : Alignment.Right | Alignment.Bottom,
                    Angle = _tapeModel.Vertical ? 0 : -90,
                    Translator = translator3,
                    TapePosition = _tapeModel.TapePosition,
                    ObjectPresentation = objectPresentetion,
                    FontType="Nina",
                    FontSize=9,
                    FontColor=new Color(0,0,0),
                    FontStyle = FontStyle.None
                }
            });
        }

        public void AddCenterTextRecordObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> objectPresentetion, Stream image)
        {
            _actions.Add(() => AddCenterTextRecordObjectRendererInternal(source, getIndex, objectPresentetion, image));
        }

        private void AddCenterTextRecordObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> objectPresentetion, Stream image)
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

            var translator2 = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            if (image != null)
                _layer.Add(new RendererLayer
                {
                    Area = CreateRelativeArea(0, 1, 0, 1),
                    Settings = new RendererLayerSettings { Clip = true },
                    Renderer = new PointObjectRenderer<T>
                    {
                        Source = source,
                        GetIndex = getIndex,
                        Image = image,
                        Alignment = _tapeModel.Vertical ? Alignment.Bottom : Alignment.Bottom,
                        Angle = _tapeModel.Vertical ? -90 : 180,
                        Translator = translator2,
                        TapePosition = _tapeModel.TapePosition
                    }
                });



            var translator3 = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RecordTextRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Alignment = _tapeModel.Vertical ? Alignment.Left | Alignment.Bottom : Alignment.Right | Alignment.Bottom,
                    Angle = _tapeModel.Vertical ? 0 : -90,
                    Translator = translator3,
                    TapePosition = _tapeModel.TapePosition,
                    ObjectPresentation = objectPresentetion,
                    FontType = "Nina",
                    FontSize = 9,
                    FontColor = new Color(0, 0, 0),
                    FontStyle = FontStyle.None
                }
            });
        }

        public void AddRightTextRecordObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> objectPresentetion, bool isLineVisisble, Stream image)
        {
            _actions.Add(() => AddRightTextRecordObjectRendererInternal(source, getIndex, objectPresentetion,isLineVisisble, image));
        }

        private void AddRightTextRecordObjectRendererInternal<T>(IObjectSource<T> source, Func<T, int> getIndex, Func<T, string> objectPresentetion, bool isLineVisisble, Stream image)
        {
            var translator = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            if (isLineVisisble)
                _layer.Add(new RendererLayer
                               {
                                   Area = CreateRelativeArea(0, 1, 0, 1),
                                   Settings = new RendererLayerSettings {Clip = true},
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

            var translator2 = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ShiftY(-5).ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().ShiftY(5).Translator;

            if (image != null)
                _layer.Add(new RendererLayer
                               {
                                   Area = CreateRelativeArea(0, 1, -1, 1),
                                   Settings = new RendererLayerSettings {Clip = true},
                                   Renderer = new PointObjectRenderer<T>
                                                  {
                                                      Source = source,
                                                      GetIndex = getIndex,
                                                      Image = image,
                                                      Alignment = _tapeModel.Vertical ? Alignment.Bottom : Alignment.Top,
                                                      Angle = _tapeModel.Vertical ? -90 : 180,
                                                      Translator = translator2,
                                                      TapePosition = _tapeModel.TapePosition
                                                  }
                               });



            var translator3 = _tapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ShiftY(-25).ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().ShiftY(25).Translator;

            _layer.Add(new RendererLayer
            {
                Area = CreateRelativeArea(0, 1, -1, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new RecordTextRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Alignment = _tapeModel.Vertical ? Alignment.Right | Alignment.Bottom : Alignment.Left | Alignment.Bottom,
                    Angle = _tapeModel.Vertical ? 0 : -90,
                    Translator = translator3,
                    TapePosition = _tapeModel.TapePosition,
                    ObjectPresentation = objectPresentetion,
                    FontType = "Nina",
                    FontSize = 9,
                    FontColor = new Color(0, 0, 0),
                    FontStyle = FontStyle.None
                }
            });
        }

        private IArea<float> CreateRelativeArea(float left, float right, float bottom, float top)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateRelativeArea(1 - top, 1 - bottom, left, right)
                       : AreasFactory.CreateRelativeArea(left, right, bottom, top);
        }

        private IArea<float> CreateMarginsArea(float? left, float? right, float? bottom, float? top, float width, float height)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateMarginsArea(bottom, top, right, left, height, width)
                       : AreasFactory.CreateMarginsArea(left, right, top, bottom, width, height);
        }

    }
}
