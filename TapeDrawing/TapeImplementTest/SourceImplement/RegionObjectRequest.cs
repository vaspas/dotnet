using System;
using System.Collections.Generic;

namespace TapeImplementTest.SourceImplement
{
    /// <summary>
    /// Класс умеет выполнять запросы о протяженных объектах
    /// </summary>
    internal class RegionObjectRequest : List<Region<RegionObject>>, IObjectRequest<Region<RegionObject>>
    {
        public TestCoordinateSource Source { get; set; }

        public List<Region<RegionObject>> Get(int from, int to)
        {
            // Вторая половина ленты - это регион
            var list = new List<Region<RegionObject>>();

            var sourceIndexes = (int)Math.Abs(Math.Round(Math.Abs(Source.Min - Source.Max) / Source.CoordinateStep));
            if (from > sourceIndexes) return list;
            if (to < (sourceIndexes / 2)) return list;

            list.Add(new Region<RegionObject> { From = sourceIndexes / 2, To = sourceIndexes, Target = new RegionObject() });
            return list;
        }
    }
}
