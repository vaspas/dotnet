using System.Collections.Generic;
using System.Linq;
using TapeImplement;

namespace TapeImplementTest.SourceImplement
{
    /// <summary>
    /// Класс умеет выполнять запросы о точечных объектах
    /// </summary>
    internal class PointObjectRequest : IObjectRequest<PointObject>
    {
        public TestCoordinateSource Source { get; set; }

        public List<PointObject> Get(int from, int to)
        {
            return
                Source.GetCoordInterrupts(from, to).Select(
                    interrupt => new PointObject { Index = interrupt.Index }).ToList();
        }
    }
}
