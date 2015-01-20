using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeImplement.MouseListenerLayers.LinearScale
{
    public class ShiftDiapazoneMouseWheelListener<T> : IMouseWheelListener,IMouseWheelHandler
        where T : struct,IComparable<T>, IEquatable<T>
    {
        public MouseButton Button { get; set; }

        public IPointTranslator Translator { get; set; }

        public IScalePosition<T> Diapazone { get; set; }

        public float Factor { get; set; }

        public Action OnChanged { get; set; }

        Action IMouseWheelHandler.HandleMouseWheel { get; set; }

        public void OnMouseWheel(int val)
        {
            var change = (T)(((dynamic)Diapazone.To - Diapazone.From) * Factor * val);

            Diapazone.Set((dynamic)Diapazone.From + change, (dynamic)Diapazone.To + change);

            OnChanged();

            (this as IMouseWheelHandler).HandleMouseWheel();
        }
    }
}

