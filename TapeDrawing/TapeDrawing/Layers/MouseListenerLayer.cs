using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;

namespace TapeDrawing.Layers
{
    public class MouseListenerLayer : List<ILayer>, ILayer, IMouseListenerLayer
    {
        public MouseListenerLayer()
        {
            Settings=new MouseListenerLayerSettings();
        }

        /// <summary>
        /// Возвращает область слоя.
        /// </summary>
        /// <returns></returns>
        public IArea<float> Area { get; set; }

        public MouseListenerLayerSettings Settings { get; set; }

        public IMouseListener MouseListener { get; set; }
    }
}
