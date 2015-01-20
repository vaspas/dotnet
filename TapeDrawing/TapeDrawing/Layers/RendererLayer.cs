using System.Collections.Generic;
using TapeDrawing.Core;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;

namespace TapeDrawing.Layers
{
    public class RendererLayer : List<ILayer>, ILayer, IRenderingLayer
    {
        public RendererLayer()
        {
            Settings=new RendererLayerSettings();
        }

        /// <summary>
        /// Возвращает область слоя.
        /// </summary>
        /// <returns></returns>
        public IArea<float> Area { get; set; }

        public RendererLayerSettings Settings { get; set; }

        public IRenderer Renderer { get; set; }
    }
}
