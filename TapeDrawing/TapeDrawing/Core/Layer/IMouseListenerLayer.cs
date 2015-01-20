
namespace TapeDrawing.Core.Layer
{
    /// <summary>
    /// Слой получающий события мыши.
    /// </summary>
    public interface IMouseListenerLayer
    {
        IMouseListener MouseListener { get; }

        MouseListenerLayerSettings Settings { get; set; }
    }
}
