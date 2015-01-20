using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.MouseListenerLayers.TapeCursor
{
    /// <summary>
    /// Обработчик клавиатуры для работы с курсором.
    /// </summary>
    public class TapeRefPositionCursorKeyboardKeyListener : IKeyProcess
    {
        public TapeRefPositionCursorRenderer Renderer { get; set; }
        
        public IScalePosition<int> TapePosition { get; set; }

        public KeyboardKey DownKey { get; set; }

        public KeyboardKey UpKey { get; set; }

        public Action PositionChanged { get; set; }


        private void ChangePosition(float shift)
        {
            var newPosition = TapePosition.From+ Renderer.Position * (TapePosition.To - TapePosition.From) + shift;

            var newRelPosition = (newPosition - TapePosition.From) / (TapePosition.To - TapePosition.From);

            if (newRelPosition > 1)
                newRelPosition = newRelPosition % 1;
            else if (newRelPosition < 0)
                newRelPosition = (int)Math.Abs(newRelPosition - 1) - newRelPosition;

            Renderer.Position = newRelPosition;

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
