
using System.Collections.Generic;
using TapeDrawing.Core.Area;

namespace TapeDrawing.Core.Layer
{
    /// <summary>
    /// Интерфейс для слоев.
    /// </summary>
    public interface ILayer:IList<ILayer>
    {
        /// <summary>
        /// Возвращает область слоя.
        /// </summary>
        /// <returns></returns>
        IArea<float> Area{ get;}
    }
}
