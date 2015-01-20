
using System;
using System.Diagnostics;
using System.Linq;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;

namespace TapeDrawing.Core.Engine
{
    class DrawingAction
    {
        public DrawingEngine Engine { get; set; }

        public void Draw()
        {
            DrawLayer(
                new TranslatedGraphicContext { Target = Engine.GraphicContext, Translator = PointTranslatorConfigurator.CreateLinear().Translator },
                Engine.MainLayer,
                Engine.ToPositiveSystem(Engine.Area));
        }

        private void DrawLayer(TranslatedGraphicContext context, ILayer layer, Rectangle<float> parentArea)
        {
            var layerRect = Engine.GetLayerArea(layer, parentArea);

            context.Translator.Dst = Engine.ToDrawingSystem(layerRect);
            context.Translator.Src = layerRect;

            IClip clip=null;
            if (layer is IRenderingLayer && (layer as IRenderingLayer).Settings.Clip)
            {
                clip=context.CreateClip();
                clip.Set(layerRect);
            }
            

            if (layer is IRenderingLayer)
            {
                try
                {
                    (layer as IRenderingLayer).Renderer.Draw(context, layerRect);
                }
                catch (Exception ex)
                {
                    Trace.Write("Draw rendering layer exception: ");
                    Trace.Write(ex.ToString());
                }
                
            }

            layer.ToList().ForEach(l => DrawLayer(context, l, layerRect));

            if(clip!=null)
                clip.Undo();
        }
    }
}
