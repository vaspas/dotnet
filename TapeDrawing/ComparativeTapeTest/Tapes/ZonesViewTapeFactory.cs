using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ComparativeTapeTest.Generators;
using ComparativeTapeTest.Generators.CoordsGenerator;
using ComparativeTapeTest.Generators.RegionObjectsGenerator;
using ComparativeTapeTest.Generators.SignalGenerator;
using ComparativeTapeTest.Tapes.CurvePanelLayers;
using ComparativeTapeTest.Tapes.Images;
using ComparativeTapeTest.Tapes.Types;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement;
using TapeImplement.CoordGridRenderers;
using TapeImplement.MouseListenerLayers;
using TapeImplement.ObjectRenderers;
using TapeImplement.ObjectRenderers.Signals;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.ZonesView;
using TapeImplement.TapeModels.ZonesView.Extensions;
using TapeImplement.TapeModels.ZonesView.Track;
using FontStyle = TapeDrawing.Core.Primitives.FontStyle;

namespace ComparativeTapeTest.Tapes
{
    class ZonesViewTapeFactory : IMainLayerFactory
    {
        public IScalePosition<int> TapePosition { get; set; }

        public Player Player { get; set; }

        public List<object> DataSources { get; set; }
        

        public void Create(ILayer mainLayer)
        {
            var model = new TapeModel(TapePosition)
                            {
                                MainLayer = mainLayer,
                                Redraw = Player.Redraw,
                                Vertical = false
                            };

            var track=model.CreateTrack<TrackModel>(new TrackSizeRelative{Value = 1});

            var rs = new ObjectSource<Region>();
            rs.Data.Add(new Region{From = 100,To=500, Text="123"});

            track.AddRegionFillRenderer(rs, r => r.From, r => r.To,  r => new Color(255,255,0));

            track.AddRegionTextRenderer(rs, r=>r.From, r=>r.To, new FontSettings(), r=>r.Text);

            model.BuildMainLayer();
        }

    }
}
