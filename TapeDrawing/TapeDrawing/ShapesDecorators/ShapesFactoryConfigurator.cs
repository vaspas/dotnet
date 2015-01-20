using TapeDrawing.Core.Shapes;
using TapeDrawing.Core.Translators;

namespace TapeDrawing.ShapesDecorators
{
    public static class ShapesFactoryConfigurator
    {
        public static ShapesFactoryConfigurator<TFactory> For<TFactory>(TFactory factory) where TFactory : class
        {
            return new ShapesFactoryConfigurator<TFactory>
            {
                Result = factory
            };
        }
    }

    public class ShapesFactoryConfigurator<T> where T:class
    {
        public T Result { get; internal set; }

        public ShapesFactoryConfigurator<T> Translate(IPointTranslator translator)
        {
            Result = DecoratorsFactory.CreateFor(Result, translator);

            return this;
        }

        public ShapesFactoryConfigurator<T> Translate(IAlignmentTranslator translator)
        {
            Result = DecoratorsFactory.CreateFor(Result, translator);

            return this;
        }

    }
}
