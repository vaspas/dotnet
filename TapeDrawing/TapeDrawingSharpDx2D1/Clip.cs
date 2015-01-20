using System;
using SharpDX;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx2D1
{
    class Clip:IClip
    {
        public Clip(DirectxGraphics gr)
        {
            _gr = gr;
        }

        private DirectxGraphics _gr;

        private Viewport _saved;

        public void Set(Rectangle<float> rectangle)
        {
        }

        public void Undo()
        {
        }
    }
}
