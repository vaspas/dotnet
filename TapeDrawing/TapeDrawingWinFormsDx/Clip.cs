using System;
using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingWinFormsDx
{
    class Clip:IClip
    {
        public Clip(DirectxGraphics gr)
        {
            _gr = gr;
            _saved = _gr.Device.DxDevice.Viewport;
        }

        private DirectxGraphics _gr;

        private Viewport _saved;

        public void Set(Rectangle<float> rectangle)
        {
            //нельзя чтобы Viewport выходил за пределы экрана
            //иначе изображение внутри региона пустое

            var x = Math.Max(Math.Max(0, (int) rectangle.Left), _saved.X);
            var y = Math.Max(Math.Max(0, (int) rectangle.Top), _saved.Y);
            var w = Math.Min(Math.Min(_gr.Width - 1, (int)rectangle.Right),_saved.X+_saved.Width)-x  ;
            var h = Math.Min(Math.Min(_gr.Heigth - 1, (int)rectangle.Bottom), _saved.Y+ _saved.Height)-y  ;

            _gr.Device.DxDevice.Viewport = new Viewport
                                               {
                                                   X =x,
                                                   Y = y,
                                                   Width = w,
                                                   Height = h,
                                                   MinZ = 0,
                                                   MaxZ = 1
                                               };
        }

        public void Undo()
        {
            _gr.Device.DxDevice.Viewport = _saved;
        }
    }
}
