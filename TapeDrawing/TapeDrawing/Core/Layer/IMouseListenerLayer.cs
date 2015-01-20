
namespace TapeDrawing.Core.Layer
{
    /// <summary>
    /// ���� ���������� ������� ����.
    /// </summary>
    public interface IMouseListenerLayer
    {
        IMouseListener MouseListener { get; }

        MouseListenerLayerSettings Settings { get; set; }
    }
}
