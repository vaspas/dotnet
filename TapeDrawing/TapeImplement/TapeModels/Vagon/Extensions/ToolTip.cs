
using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Vagon.ToolTip;

namespace TapeImplement.TapeModels.Vagon.Extensions
{
    public class ToolTip : IExtension<TapeModel>
    {
        private abstract class Pair
        {
            public abstract object Object { get; }
            public object Sender;

            public abstract string GetText();
        }

        private class Pair<T> : Pair where T:class
        {
            public Pair(T obj, Func<T, string> toolTip, object sender)
            {
                _obj = obj;
                Sender = sender;
                _toolTip = toolTip;
            }

            private T _obj;

            private Func<T, string> _toolTip;

            public override object Object 
            {
                get { return _obj; }
            }

            public override string GetText()
            {
                return _toolTip(_obj);
            }
        }

        private readonly List<Pair> _mouseOverObjects = new List<Pair>();

        public void AddMouseOver<T>(T obj, Func<T, string> toolTip, object sender)
            where T:class
        {
            if (_mouseOverObjects.Any(p => p.Object == obj && p.Sender == sender))
                return;

            var pair = new Pair<T>(obj,toolTip, sender);

            _toolTipRenderer.Objects.Add(pair.GetText);

            _mouseOverObjects.Add(pair);
        }

        public void RemoveMouseOverFor(object sender)
        {
            _mouseOverObjects.FindAll(p=>p.Sender==sender)
                .ForEach(p=>_toolTipRenderer.Objects.Remove(p.GetText));

            _mouseOverObjects.RemoveAll(p => p.Sender == sender);
        }



        private ToolTipRenderer _toolTipRenderer;

        public void Build(TapeModel tapeModel)
        {
            var width = 20;
            _toolTipRenderer = new ToolTipRenderer
            {
                Redraw = tapeModel.Redraw,
                Translator = tapeModel.Vertical
                                 ? PointTranslatorConfigurator.CreateLinear().ShiftX(width).
                                       Translator
                                 : PointTranslatorConfigurator.CreateLinear().ShiftY(width).
                                       Translator,
                TextAlignment = tapeModel.Vertical
                                    ? Alignment.Left
                                    : Alignment.Bottom
            };

            tapeModel.MainLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = _toolTipRenderer
            });
            tapeModel.MainLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Settings = new MouseListenerLayerSettings { ControlMouseLeave = false, MouseMoveOutside = true },
                MouseListener = _toolTipRenderer
            });
        }
    }
}
