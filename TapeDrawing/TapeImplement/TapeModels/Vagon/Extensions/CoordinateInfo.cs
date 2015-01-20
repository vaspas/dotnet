
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.SimpleRenderers;
using Color = TapeDrawing.Core.Primitives.Color;
using FontStyle = TapeDrawing.Core.Primitives.FontStyle;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class CoordinateInfo:IExtension<TapeModel>
    {
        internal CoordinateInfo()
        {
            
        }

        public Func<int, string> GetCursorPosition { get; set; }
        
        private TextRenderer _textRenderer;
        
        private TapeModel _tapeModel;

        private IArea<float> CreateMarginsArea(float? left, float? right, float? bottom, float? top, float width, float height)
        {
            return _tapeModel.Vertical
                       ? AreasFactory.CreateMarginsArea(bottom, top, right, left, height, width)
                       : AreasFactory.CreateMarginsArea(left, right, top, bottom, width, height);
        }

        public void Build(TapeModel tapeModel)
        {
            _tapeModel = tapeModel;

            _tapeModel.MainLayer.Add(new RendererLayer
            {
                Area = CreateMarginsArea(0, null, 0, null, _tapeModel.ScaleSize, _tapeModel.DistScaleSize),
                Settings = new RendererLayerSettings{Clip = true},
                Renderer = new ImageRenderer<Bitmap>
                {
                    Image = Properties.Resources.fadedBar,
                    Angle = _tapeModel.Vertical?0:90
                }
            });

            _tapeModel.MainLayer.Add(new RendererLayer
            {
                Area = CreateMarginsArea(0, null, 0, null, _tapeModel.ScaleSize, _tapeModel.DistScaleSize),
                Renderer = new BorderRenderer
                               {
                                   Bottom = true,
                                   Top = true,
                                   Left = true,
                                   Right = true,
                                   LineStyle = LineStyle.Solid,
                                   Color = new Color(0,0,0),
                                   LineWidth = 2
                               }
            });

            _textRenderer =  new TextRenderer
                                          {
                                              Angle = _tapeModel.Vertical ? 0 : -90,
                                              Color = new Color(0, 0, 0),
                                              FontName = "Nina",
                                              LayerAlignment = Alignment.None,
                                              Size = 16,
                                              Style = FontStyle.None,
                                              TextAlignment = Alignment.None,
                                              Text =""
                                          };

            var layer = new RendererLayer
                            {
                                Area = CreateMarginsArea(0, null, 0, null, _tapeModel.ScaleSize, _tapeModel.DistScaleSize),
                               Renderer =  _textRenderer
                            };
            _tapeModel.MainLayer.Add(layer);

            _tapeModel.CursorPositionChanged +=
                i =>
                    {
                        if (GetCursorPosition != null)
                            _textRenderer.Text = GetCursorPosition(i);
                    };
        }
    }
}
