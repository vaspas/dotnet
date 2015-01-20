
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.VagonPrint.Track;

namespace TapeImplement.TapeModels.VagonPrint.Table
{
    /// <summary>
    /// Модель таблицы.
    /// </summary>
    public class TableModel
    {
        public TapeModel TapeModel { get; set; }

        public int RowHeight { get; set; }

        private readonly List<Column> _columns=new List<Column>();

        public void AddColumn(int size, string title)
        {
            _columns.Add(new Column {Size = size, Title = title});
        }

        public int Size
        {
            get { return _columns.Sum(c=>c.Size); }
        }

        public void BuildInfoLayer(ILayer infoLayer)
        {
            infoLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new BorderRenderer
                {
                    Bottom = true,
                    Top = true,
                    Left = true,
                    Right = true,
                    Color = TapeModel.Settings.DefaultColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = TapeModel.Settings.DefaultLineWidth
                }
            });

            float currentValue = 0;
            foreach (var column in _columns)
            {
                var layer = new EmptyLayer { Area = AreasFactory.CreateMarginsArea(currentValue, null, 0, 0, column.Size, 0) };
                infoLayer.Add(layer);


                layer.Add(new RendererLayer
                {
                    Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                    Renderer = new BorderRenderer
                    {
                        Right = true,
                        Color = TapeModel.Settings.DefaultColor,
                        LineStyle = LineStyle.Solid,
                        LineWidth = TapeModel.Settings.DefaultLineWidth
                    }
                });
                layer.Add(new RendererLayer
                {
                    Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                    Renderer = new TextRenderer
                    {
                        Angle = 0,
                        Color = TapeModel.Settings.DefaultColor,
                        FontName = TapeModel.Settings.FontName,
                        Size = 8,
                        Style = FontStyle.None,
                        Text = column.Title
                    }
                });


                currentValue += column.Size;
            }
        }

        private float _scaleFactor = 1;
        public float ScaleFactor
        {
            get { return _scaleFactor; }
            set
            {
                _scaleFactor = value;
                if(_renderer!=null)
                    _renderer.ScaleFactor = _scaleFactor; 
            }
        }

        private Renderer _renderer;

        public void BuildLayer(ILayer layer)
        {
            _renderer = new Renderer
                            {
                                FontColor = TapeModel.Settings.DefaultColor,
                                FontName = TapeModel.Settings.FontName,
                                FontSize = 8,
                                PrintSource = TapeModel.PrintSource,
                                RowHeight = RowHeight,
                                TapePosition = TapeModel.TapePosition,
                                Columns = _columns,
                                LineColor = TapeModel.Settings.DefaultColor,
                                LineWidth = TapeModel.Settings.DefaultLineWidth,
                                CursorLineColor = TapeModel.Settings.LightColor,
                                CursorLineWidth = TapeModel.Settings.DefaultLineWidth,
                                CursorLineSize = TapeModel.Tracks.FindAll(tr => tr.Model is DataTrackModel)
                                                      .Sum(tr => tr.Size + TapeModel.Settings.GraphsSplitWidth)
                                                  + TapeModel.Settings.GraphsSplitWidth,
                                ScaleFactor = _scaleFactor
                            };

            layer.Add(new RendererLayer
                          {
                              Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                              Renderer = _renderer
                          });
        }

        public bool IsRowOverflow
        {
            get { return _renderer.OverflowRows != null; }
        }

        public void DropOverflowRows()
        {
            _renderer.DropOverflowRows();
        }
    }
}
