using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeImplement.SimpleRenderers
{
    /// <summary>
    /// Переключает используемые рендереры в зависимости от масштаба изображения.
    /// </summary>
    public class SwitchRenderer<T>:IRenderer
    {
        public IScalePosition<T> ScalePosition;

        /// <summary>
        /// Выполнение некоторой операции перед рисованием с параметром ширины пикселя в значениях щкалы.
        /// </summary>
        public Action<float> BeforeDraw;

        public float OriginalStep;

        public float SwitchValue;

        public bool IsHorizontal;

        public IRenderer LowDensityRenderer;
        public IRenderer HighDensityRenderer;

        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            var pixels = IsHorizontal ? rect.Right - rect.Left : rect.Top - rect.Bottom;

            if (pixels <= 0)
                return;
            var ws = ((dynamic)ScalePosition.To - ScalePosition.From) / pixels;

            BeforeDraw(ws);

            if (ws / OriginalStep >= SwitchValue)
                HighDensityRenderer.Draw(gr, rect);
            else
                LowDensityRenderer.Draw(gr, rect);
        }

        
    }
}
