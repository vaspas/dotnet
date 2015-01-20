using System.Linq;
using TapeDrawing.Core.Layer;

namespace TapeDrawing.Core.Engine
{
    class MouseWheelListenerAction
    {
        public DrawingEngine Engine { get; set; }

        public MouseMoveListenerAction MoveListener { get; set; }

        private bool _wheelHandled;

        public void OnMouseWheel(ILayer layer, int delta)
        {
            _wheelHandled = false;

            OnMouseWheelInternal(layer, delta);
        }

        private void OnMouseWheelInternal(ILayer layer, int delta)
        {
            if (_wheelHandled || !MoveListener.MouseHoldLayers.Any(l => l == layer))
                return;

            if (layer is IMouseListenerLayer
                && (layer as IMouseListenerLayer).MouseListener is IMouseWheelListener)
            {
                if ((layer as IMouseListenerLayer).MouseListener is IMouseWheelHandler)
                    ((layer as IMouseListenerLayer).MouseListener as IMouseWheelHandler).HandleMouseWheel =
                        () => 
                            _wheelHandled = true;

                ((layer as IMouseListenerLayer).MouseListener as IMouseWheelListener).OnMouseWheel(delta);
            }

            foreach (var l in layer)
                OnMouseWheelInternal(l, delta);
        }
    }
}
