using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.SimpleRenderers;

namespace ComparativeTapeTest.Tapes.CurvePanelLayers
{
    class InfoTableLayerFactory
    {
        /// <summary>
        /// Текст для отображения
        /// </summary>
        public string[,] TableData { get; set; }

        public int TextSize { get; set; }

        public ILayer Create()
        {
            var mainLayer = new EmptyLayer {Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0)};

            mainLayer.Add(new RendererLayer
                              {
                                  Area =
                                      AreasFactory.CreateRelativeArea(0f, 1f, 0f, 1f),
                                  Renderer = new FillRenderer(new Color(255, 255, 255))
                              });

            mainLayer.Add(new RendererLayer
                              {
                                  Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                                  Renderer = new TableTextRenderer
                                                 {
                                                     TableData = TableData,
                                                     Color = new Color(0, 0, 0),
                                                     FontName = "Arial",
                                                     Size = TextSize
                                                 }
                              });

            var columnsCount = TableData.GetLength(1);

            for (int c = 0; c < columnsCount; c++)
                mainLayer.Add(new RendererLayer
                                  {
                                      Area =
                                          AreasFactory.CreateRelativeArea((float) c/columnsCount,
                                                                          (float) (c + 1)/columnsCount, 0, 1),
                                      Renderer = new BorderRenderer
                                                     {
                                                         Bottom = true,
                                                         Top = true,
                                                         Left = true,
                                                         Right = true,
                                                         Color = new Color(0, 0, 0),
                                                         LineWidth = 1
                                                     }
                                  });


            return mainLayer;
        }

    }
}
