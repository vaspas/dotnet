using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.MouseListenerLayers.TapeCursor
{
    /// <summary>
    /// Обработчик клавиатуры для работы с курсором.
    /// </summary>
    public class TapePositionCursorKeyboardKeyListener : IKeyProcess
    {
        public TapePositionCursorRenderer Renderer { get; set; }
        
        public IScalePosition<int> TapePosition { get; set; }

        public KeyboardKey DownKey { get; set; }

        public KeyboardKey UpKey { get; set; }

        public Action PositionChanged { get; set; }


        private void ChangePosition(int shift)
        {
            Renderer.Position += shift;

            PositionChanged();
        }

        public void OnKeyDown(KeyboardKey key)
        {
            if (key == DownKey)
                ChangePosition(-1);
            if (key == UpKey)
                ChangePosition(1);
        }

        public void OnKeyUp(KeyboardKey key)
        {
        }
    }
}
