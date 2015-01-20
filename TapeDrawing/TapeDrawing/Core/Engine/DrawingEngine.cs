using System;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeDrawing.Core.Engine
{
    public class DrawingEngine
    {
        public DrawingEngine()
        {
            _drawingAction = new DrawingAction
                                 {
                                     Engine = this
                               };
            _mouseMoveListenerAction = new MouseMoveListenerAction
                                       {
                                           Engine = this
                                       };
            _mouseButtonListenerAction = new MouseButtonListenerAction
                                             {
                                                 Engine = this,
                                                 MoveListener = _mouseMoveListenerAction
                                             };
            _keyboardListenerAction = new KeyboardKeyProcessListenerAction
            {
                Engine = this
            };
            _mouseWheelListenerAction = new MouseWheelListenerAction
            {
                Engine = this,
                MoveListener = _mouseMoveListenerAction
            };
        }

        public ILayer MainLayer { get; set; }

        public IGraphicContext GraphicContext { get; set; }

        public event EventHandler BeforeDraw;

        public event EventHandler AfterDraw;


        private readonly DrawingAction _drawingAction;
        private readonly MouseMoveListenerAction _mouseMoveListenerAction;
        private readonly MouseButtonListenerAction _mouseButtonListenerAction;
        private readonly MouseWheelListenerAction _mouseWheelListenerAction;
        private readonly KeyboardKeyProcessListenerAction _keyboardListenerAction;


        public void Draw()
        {
            OnBeforeDraw();
            
            _drawingAction.Draw();

            OnAfterDraw();
        }

        public void OnMouseMove(Point<float> position)
        {
            var translator = PointTranslatorConfigurator.CreateLinear().Translator;
            translator.Dst = ToPositiveSystem(Area);
            translator.Src = Area;

            _mouseMoveListenerAction.OnMouseMove(
                MainLayer, 
                translator.Dst,
                translator.Translate(position));
        }
        public void OnMouseButtonDown(MouseButton button)
        {
            _mouseButtonListenerAction.OnMouseDown(MainLayer, button);
        }
        public void OnMouseButtonUp(MouseButton button)
        {
            _mouseButtonListenerAction.OnMouseUp(MainLayer, button);
        }
        public void OnMouseWheel(int delta)
        {
            _mouseWheelListenerAction.OnMouseWheel(MainLayer, delta);
        }
        public void OnMouseLeave()
        {
            _mouseMoveListenerAction.OnMouseLeave();
        }

        public void OnKeyboardKeyDown(KeyboardKey key)
        {
            _keyboardListenerAction.OnKeyDown(MainLayer, key);
        }
        public void OnKeyboardKeyUp(KeyboardKey key)
        {
            _keyboardListenerAction.OnKeyUp(MainLayer, key);
        }
        public void LostFocus()
        {
            _keyboardListenerAction.LostFocus(MainLayer);
        }



        public Rectangle<float> Area { get; set; }


        internal Rectangle<float> GetLayerArea(ILayer layer, Rectangle<float> parentArea)
        {
            var layerRect = layer.Area.GetRectangle(
                new Size<float>
                {
                    Width = Math.Abs(parentArea.Right - parentArea.Left),
                    Height = Math.Abs(parentArea.Top - parentArea.Bottom)
                });

            layerRect.Left += parentArea.Left;
            layerRect.Right += parentArea.Left;
            layerRect.Top += parentArea.Bottom;
            layerRect.Bottom += parentArea.Bottom;

            return layerRect;
        }

        internal Rectangle<float> ToDrawingSystem(Rectangle<float> rect)
        {
            return new Rectangle<float>
            {
                Left = Area.Left <= Area.Right
                ? rect.Left
                : Area.Left - rect.Left + Area.Right,
                Right = Area.Left <= Area.Right
                ? rect.Right
                : Area.Left - rect.Right + Area.Right,
                Bottom = Area.Bottom <= Area.Top
                ? rect.Bottom
                : Area.Bottom - rect.Bottom + Area.Top,
                Top = Area.Bottom <= Area.Top
                ? rect.Top
                : Area.Bottom - rect.Top + Area.Top

            };
        }

        internal Rectangle<float> ToPositiveSystem(Rectangle<float> rect)
        {
            return new Rectangle<float>
                       {
                           Left = Math.Min(rect.Left, rect.Right),
                           Right = Math.Max(rect.Left, rect.Right),
                           Bottom = Math.Min(rect.Top, rect.Bottom),
                           Top = Math.Max(rect.Top, rect.Bottom)
                       };
        }


        private void OnBeforeDraw()
        {
            if (BeforeDraw != null)
                BeforeDraw(this, EventArgs.Empty);
        }

        private void OnAfterDraw()
        {
            if (AfterDraw != null)
                AfterDraw(this, EventArgs.Empty);
        }
    }
}
