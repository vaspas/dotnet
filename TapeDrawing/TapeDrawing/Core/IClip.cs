using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core
{
    public interface IClip
    {
        void Set(Rectangle<float> rectangle);

        void Undo();
    }
}
