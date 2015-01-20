
namespace TapeDrawing.Core.Layer
{
    /// <summary>
    /// ���� � ������������ �� ������.
    /// </summary>
    public interface IRenderingLayer
    {
        IRenderer Renderer { get; }

        RendererLayerSettings Settings { get; set; }
    }
}
