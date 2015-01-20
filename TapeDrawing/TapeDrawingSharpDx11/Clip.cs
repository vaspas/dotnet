using System;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx11
{
    class Clip : IClip
    {
        public Clip(DirectxGraphics gr)
        {
            _gr = gr;
            
            _savedXfrom = _gr.Device.VertexConstant.XFrom;
            _savedXto = _gr.Device.VertexConstant.XTo;
            _savedYfrom = _gr.Device.VertexConstant.YFrom;
            _savedYto = _gr.Device.VertexConstant.YTo;
        }

        private readonly DirectxGraphics _gr;

        private readonly float _savedXfrom;
        private readonly float _savedXto;
        private readonly float _savedYfrom;
        private readonly float _savedYto;

        public void Set(Rectangle<float> rectangle)
        {
            //нельзя чтобы Viewport выходил за пределы экрана
            //иначе изображение внутри региона пустое

            _gr.Device.VertexConstant.XFrom = Math.Max(Math.Max(0, (int)rectangle.Left), _savedXfrom);
            _gr.Device.VertexConstant.XTo = Math.Min(Math.Min(_gr.Width - 1, (int)rectangle.Right), _savedXto);
            _gr.Device.VertexConstant.YFrom = Math.Max(Math.Max(0, (int)rectangle.Top), _savedYfrom);
            _gr.Device.VertexConstant.YTo = Math.Min(Math.Min(_gr.Heigth - 1, (int)rectangle.Bottom), _savedYto);

            _gr.Device.Context.UpdateSubresource(ref _gr.Device.VertexConstant, _gr.Device.VertexConstantBuffer);
        }

        public void Undo()
        {
            _gr.Device.VertexConstant.XFrom = _savedXfrom;
            _gr.Device.VertexConstant.XTo = _savedXto;
            _gr.Device.VertexConstant.YFrom = _savedYfrom;
            _gr.Device.VertexConstant.YTo = _savedYto;

            _gr.Device.Context.UpdateSubresource(ref _gr.Device.VertexConstant, _gr.Device.VertexConstantBuffer);
        }
    }
}
