using System.Collections.Generic;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;

namespace TapeDrawing.Layers
{
    public class EmptyLayer : List<ILayer>, ILayer
    {
        /// <summary>
        /// Возвращает область слоя.
        /// </summary>
        /// <returns></returns>
        public IArea<float> Area { get; set; }
    }
}
