using TapeImplement;
using TapeImplementTest.SourceImplement;

namespace TapeImplementTest.Factories
{
    static class FactoryCreator
    {
        public static ILayerFactory GetFactory(bool vertical, TestCoordinateSource source, IScalePosition<int> tapePosition, TestParams testParams)
        {
            if (!vertical)return new LayerFactory {Source = source, TapePosition = tapePosition, TestParams = testParams};
            return new LayerFactoryVertical {Source = source, TapePosition = tapePosition, TestParams = testParams};
        }
    }
}
