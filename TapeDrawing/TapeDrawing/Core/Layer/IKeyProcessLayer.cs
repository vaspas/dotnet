
namespace TapeDrawing.Core.Layer
{
    /// <summary>
    /// Слой получающий события клавиатуры.
    /// </summary>
    public interface IKeyProcessLayer
    {
        IKeyboardProcess KeyboardProcess { get; }
    }
}
