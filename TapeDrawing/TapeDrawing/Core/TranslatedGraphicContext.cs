using TapeDrawing.Core.Translators;
using TapeDrawing.ShapesDecorators;

namespace TapeDrawing.Core
{
    class TranslatedGraphicContext : IGraphicContext
    {
        public IGraphicContext Target { get; set; }

        public IPointTranslator Translator { get; set; }

        public Instruments.IInstrumentsFactory Instruments
        {
            get { return Target.Instruments; }
        }

        public Shapes.IShapesFactory Shapes
        {
            get { return ShapesFactoryConfigurator.For(Target.Shapes).Translate(Translator).Result; }
        }

        public IClip CreateClip()
        {
            return new TranslatedClip
                       {
                           Target = Target.CreateClip(),
                           Translator = Translator
                       };
        }
    }
}
