
namespace TapeDrawing.Core.Layer
{
    /// <summary>
    /// Слой с отображением на экране.
    /// </summary>
    public interface IRenderingLayer
    {
        IRenderer Renderer { get; }

        RendererLayerSettings Settings { get; set; }
    }
}
