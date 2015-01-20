using TapeImplement.CoordGridRenderers;

namespace ComparativeTapeTest.Generators.CoordsGenerator
{
    class BeginEndInterrupt : ICoordInterrupt
    {
        /// <summary>
        /// Обозначение отметки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Положение на ленте в индексах
        /// </summary>
        public int Index { get; set; }
    }
}
