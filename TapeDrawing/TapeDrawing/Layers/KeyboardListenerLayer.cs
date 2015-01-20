using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;

namespace TapeDrawing.Layers
{
    public class KeyboardListenerLayer : List<ILayer>, ILayer, IKeyProcessLayer
    {
        /// <summary>
        /// Возвращает область слоя.
        /// </summary>
        /// <returns></returns>
        public IArea<float> Area { get; set; }

        public IKeyboardProcess KeyboardProcess { get; set; }
    }
}
