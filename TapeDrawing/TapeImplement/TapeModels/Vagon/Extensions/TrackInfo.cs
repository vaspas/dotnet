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
    public class TrackInfo:IExtension<BaseTrackModel>
    {
        private BaseTrackModel _trackModel;
        
        private readonly List<Action> _actions = new List<Action>();

        public TrackInfo AddInfo(Alignment alignment, Func<string> getInfo, FontSettings fs)
        {
            _actions.Add(() => AddInfoInternal(alignment, getInfo, fs));
            return this;
        }

        //добавляет информационную строку с дорожке ленты
        private void AddInfoInternal(Alignment alignment, Func<string> getInfo, FontSettings fs)
        {
            _trackModel.DataLayer.Add(new RendererLayer
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
                    TextAlignment = alignment,
                    BackgroundColor = new Color(255, 255, 255),
                    BorderLineWidth = 1,
                    Margin = 3
                }
            });
        }

        public void Build(BaseTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);
            
            _actions.ForEach(a => a());
        }
    }
}
