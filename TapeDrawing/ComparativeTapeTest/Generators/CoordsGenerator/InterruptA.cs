using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TapeImplement.CoordGridRenderers;

namespace ComparativeTapeTest.Generators.CoordsGenerator
{
    class InterruptA:ICoordInterrupt
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
