using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class TrackInfo : IExtension
    {
        public TrackInfo()
        {
            Size = 12;
            Height = 14;
            Alignment = Alignment.Right | Alignment.Top;
            Style = FontStyle.Bold;
        }

        private DataTrackModel _trackModel;
        
        public int Size { get; set; }

        public int Height { get; set; }
        
        public Alignment Alignment { get; set; }

        public FontStyle Style { get; set; }

        public TrackInfo Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);

            return this;
        }

        private int _counter;
        public TrackInfo Add(string text, Color color)
        {
            var position = _counter*Height;

            var renderer = new SimpleRenderers.TextRenderer
            {
                Color = color,
                Angle = 0,
                FontName = "Arial",
                LayerAlignment = Alignment,
                Size = Size,
                Style = Style,
                Text = text,
                TextAlignment = Alignment
            };
            
            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 
                (Alignment & Alignment.Bottom) == Alignment.Bottom?position:0, 
                (Alignment & Alignment.Top) == Alignment.Top?position:0),
                Renderer = renderer
            });

            _counter++;

            return this;
        }
    }
}
