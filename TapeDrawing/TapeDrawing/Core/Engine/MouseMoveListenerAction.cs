using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Engine
{
    class MouseMoveListenerAction
    {
        public MouseMoveListenerAction()
        {
            MouseHoldLayers = new List<ILayer>();
        }

        public DrawingEngine Engine { get; set; }

        public List<ILayer> MouseHoldLayers { get; private set; }

        public void OnMouseMove(ILayer layer, Rectangle<float> parentArea, Point<float> position)
        {
            var layerRect = Engine.GetLayerArea(layer, parentArea);

            bool contains = layerRect.Left <= position.X && layerRect.Right >= position.X &&
                                layerRect.Bottom <= position.Y && layerRect.Top >= position.Y;

            IMouseMoveListener listener = null;
            if (layer is IMouseListenerLayer
                        && (layer as IMouseListenerLayer).MouseListener is IMouseMoveListener)
                listener= (layer as IMouseListenerLayer).MouseListener as IMouseMoveListener;

            if (contains != MouseHoldLayers.Contains(layer))
            {
                if (contains)
                {
                    if (listener!=null)
                        listener.OnMouseEnter();
                    MouseHoldLayers.Add(layer);
                }
                else
                {
                    if (listener != null)
                        listener.OnMouseLeave();
                    MouseHoldLayers.Remove(layer);
                }
            }

            if (listener != null && ((layer as IMouseListenerLayer).Settings.MouseMoveOutside || contains))
                listener.OnMouseMove(position, layerRect);

            foreach (var l in layer)
                OnMouseMove(l, layerRect, position);
        }

        public void OnMouseLeave()
        {
            foreach (var mml in MouseHoldLayers.OfType<IMouseListenerLayer>()
                .Where(l => l.Settings.ControlMouseLeave)
                .Select(l => l.MouseListener)
                .OfType<IMouseMoveListener>())
                mml.OnMouseLeave();

            MouseHoldLayers.Clear();
        }
    }
}
