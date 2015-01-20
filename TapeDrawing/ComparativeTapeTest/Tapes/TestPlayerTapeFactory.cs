using System.Collections.Generic;
using ComparativeTapeTest.Renderers;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement;
using TapeDrawing.Core.Translators;

namespace ComparativeTapeTest.Tapes
{
    class TestPlayerTapeFactory : IMainLayerFactory
    {
        public IScalePosition<int> TapePosition { get; set; }

        public Player Player { get; set; }

        public List<object> DataSources { get; set; }

        public void Create(ILayer mainLayer)
        {
            mainLayer.Add(CreateBackgroundLayer());
            mainLayer.Add(CreateTestLayer());
        }

        private ILayer CreateBackgroundLayer()
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new TapeImplement.SimpleRenderers.FillAllRenderer(
                    new Color(255, 255, 255))
            };
        }

        private ILayer CreateTestLayer()
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new TestPlayerRenderer
                {
                    TapePosition = TapePosition,
                    AlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                    PointTranslator = PointTranslatorConfigurator.CreateLinear().Translator
                }
            };
        }
    }
}
