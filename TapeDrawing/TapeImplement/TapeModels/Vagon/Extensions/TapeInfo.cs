using System;
using System.Collections.Generic;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.Vagon.Track;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    /// <summary>
    /// Добавление текстовой информации для дорожки.
    /// </summary>
    public class TapeInfo:IExtension<TapeModel>
    {
        private readonly List<Action> _actions = new List<Action>();

        public TapeInfo AddInfo(Alignment alignment, Func<string> getInfo, FontSettings fs)
        {
            _actions.Add(() => AddInfoInternal(alignment, alignment, getInfo, fs));
            return this;
        }


        private void AddInfoInternal(Alignment alignment, Alignment textAlignment, Func<string> getInfo, FontSettings fs)
        {
            _tapeModel.MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new TextRenderer
                {
                    Angle = 0,
                    Color = fs.Color,
                    FontName = fs.Name,
                    GetText = getInfo,
                    LayerAlignment = alignment,
                    Size = fs.Size,
                    Style = fs.Style,
                    TextAlignment = textAlignment,
                    BackgroundColor = new Color(255, 255, 255),
                    BorderLineWidth = 1,
                    Margin = 3
                }
            });
        }

        private TapeModel _tapeModel;


        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;
            
            _actions.ForEach(a => a());
        }
    }
}
