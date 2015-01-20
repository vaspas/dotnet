using System;
using System.Collections.Generic;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.CoordGridRenderers;
using TapeImplement.ObjectRenderers.LinearScale;
using TapeImplement.TapeModels.Vagon.Track;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class ScaleGrid:IExtension<DataTrackModel>
    {
        private DataTrackModel _trackModel;
        
        private readonly List<Action> _actions = new List<Action>();

        public ScaleGrid AddGridLines(Color color)
        {
            _actions.Add(()=>AddGridLinesInternal(color));
            return this;
        }

        private void AddGridLinesInternal(Color color)
        {
            var translator = _trackModel.TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().Translator
                       : PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator;

            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new ScaleLinesRenderer
                {
                    Diapazone = _trackModel.Position,
                    LineColor = color,
                    LineStyle = LineStyle.Solid,
                    LineWidth = 1,
                    Mask = new[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 30,
                    Translator = translator
                }
            });

            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new ScaleLinesRenderer
                {
                    Diapazone = _trackModel.Position,
                    LineColor = color,
                    LineStyle = LineStyle.Dot,
                    LineWidth = 1,
                    Mask = new[] { 0.1f, 0.2f, 0.5f },
                    MinPixelsDistance = 6,
                    Translator = translator
                }
            });
        }

        public ScaleGrid AddFixedLines(Color color, LineStyle style, float[] values)
        {
            _actions.Add(() => AddFixedLinesInternal(color,style, values));
            return this;
        }

        private void AddFixedLinesInternal(Color color, LineStyle style, float[] values)
        {
            var translator = _trackModel.TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new ScaleGridRenderer
                {
                    GetMax = ()=>_trackModel.Position.To,
                    GetMin = () => _trackModel.Position.From,
                    LineColor = color,
                    LineStyle = style,
                    LineWidth = 1,
                    Values = values,
                    Translator = translator
                }
            });
        }

        public void Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);
            
            _actions.ForEach(a => a());
        }
    }
}
