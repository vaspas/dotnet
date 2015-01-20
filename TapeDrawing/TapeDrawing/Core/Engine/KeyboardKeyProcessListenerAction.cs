using System.Linq;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;

namespace TapeDrawing.Core.Engine
{
    class KeyboardKeyProcessListenerAction
    {
        public DrawingEngine Engine { get; set; }
        
        public void OnKeyDown(ILayer layer, KeyboardKey key)
        {
            if (layer is IKeyProcessLayer
                && (layer as IKeyProcessLayer).KeyboardProcess is IKeyProcess)
                ((layer as IKeyProcessLayer).KeyboardProcess as IKeyProcess).OnKeyDown(key);

            foreach (var l in layer)
                OnKeyDown(l, key);
        }

        public void OnKeyUp(ILayer layer, KeyboardKey key)
        {
            if (layer is IKeyProcessLayer
                && (layer as IKeyProcessLayer).KeyboardProcess is IKeyProcess)
                ((layer as IKeyProcessLayer).KeyboardProcess as IKeyProcess).OnKeyUp(key);

            foreach (var l in layer)
                OnKeyUp(l, key);
        }

        public void LostFocus(ILayer layer)
        {
            if (layer is IKeyProcessLayer
                && (layer as IKeyProcessLayer).KeyboardProcess is IFocusProcess)
                ((layer as IKeyProcessLayer).KeyboardProcess as IFocusProcess).LostFocus();

            foreach (var l in layer)
                LostFocus(l);
        }

    }
}
