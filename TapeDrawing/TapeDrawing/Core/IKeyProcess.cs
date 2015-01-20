
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core
{
    public interface IKeyboardProcess
    {
    }

    public interface IKeyProcess : IKeyboardProcess
    {
        void OnKeyDown(KeyboardKey key);

        void OnKeyUp(KeyboardKey key);
    }

    public interface IFocusProcess : IKeyboardProcess
    {
        void LostFocus();
    }
}
