
using System.Collections.Generic;
using TapeDrawing.Core.Layer;
using TapeImplement;

namespace ComparativeTapeTest
{
    interface IMainLayerFactory
    {
        IScalePosition<int> TapePosition { get; set; }

        Player Player { get; set; }

        List<object> DataSources { get; set; }

        void Create(ILayer mainLayer);
    }
}
