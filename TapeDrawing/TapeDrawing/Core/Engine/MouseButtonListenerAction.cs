using System.Linq;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Engine
{
    class MouseButtonListenerAction
    {
        public DrawingEngine Engine { get; set; }

        public MouseMoveListenerAction MoveListener { get; set; }

        private bool _downHandled;
        private bool _upHandled;

        public void OnMouseDown(ILayer layer, MouseButton button)
        {
            _downHandled = false;

            OnMouseDownInternal(layer, button);
        }

        private void OnMouseDownInternal(ILayer layer, MouseButton button)
        {
            if( _downHandled || !MoveListener.MouseHoldLayers.Any(l=>l==layer))
                return;

            if (layer is IMouseListenerLayer
                && (layer as IMouseListenerLayer).MouseListener is IMouseButtonListener)
            {
                if ((layer as IMouseListenerLayer).MouseListener is IMouseButtonHandler)
                    ((layer as IMouseListenerLayer).MouseListener as IMouseButtonHandler).HandleMouseDown =
                        () => _downHandled = true;

                ((layer as IMouseListenerLayer).MouseListener as IMouseButtonListener)
                    .OnMouseDown(button);
            }

            foreach (var l in layer)
                OnMouseDownInternal(l, button);
        }

        public void OnMouseUp(ILayer layer, MouseButton button)
        {
            _upHandled = false;

            OnMouseUpInternal(layer, button);
        }

        private void OnMouseUpInternal(ILayer layer, MouseButton button)
        {
            if (_upHandled)
                return;

            if (layer is IMouseListenerLayer
                && (layer as IMouseListenerLayer).MouseListener is IMouseButtonListener
                && ((layer as IMouseListenerLayer).Settings.MouseUpOutside 
                || MoveListener.MouseHoldLayers.Any(l => l == layer)))
            {
                if ((layer as IMouseListenerLayer).MouseListener is IMouseButtonHandler)
                    ((layer as IMouseListenerLayer).MouseListener as IMouseButtonHandler).HandleMouseUp =
                        () => _upHandled = true;

                ((layer as IMouseListenerLayer).MouseListener as IMouseButtonListener)
                    .OnMouseUp(button);
            }

            foreach (var l in layer)
                OnMouseUpInternal(l, button);
        }

    }
}
