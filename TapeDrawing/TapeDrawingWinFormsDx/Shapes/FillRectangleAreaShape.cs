using Microsoft.DirectX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingWinFormsDx.Instruments;
using Color = System.Drawing.Color;

namespace TapeDrawingWinFormsDx.Shapes
{
	/// <summary>
	/// Фигура рисует закрашенный прямоугольник
	/// </summary>
	class FillRectangleAreaShape : BaseShape, IFillRectangleAreaShape
	{
		/// <summary>
		/// Кисть заливки прямоугольника
		/// </summary>
		public Brush Brush { get; set; }

        /// <summary>
        /// Выравнивание 
        /// </summary>
        public Alignment Alignment { get; set; }

        public void Render(Point<float> point, Size<float> size)
        {
            Render(point, size, 0, 0);
        }

	    public void Render(Point<float> point, Size<float> size, float marginX, float marginY)
		{
            /* Следующий кусок кода взять отсюда
             * // Select the texture you want to alpha-blendp
             * Dev->SetTexture(0, YourTexture);
             * // Set blending stages for using blend factor
             * pDev->SetRenderState(D3DRS_ALPHABLENDENABLE, TRUE);
             * pDev->SetRenderState(D3DRS_SRCBLEND, D3DBLEND_BLENDFACTOR);
             * pDev->SetRenderState(D3DRS_DESTBLEND, D3DBLEND_INVBLENDFACTOR);
             * // The following is the blending factor, use values from 0 to 255
             * // A value of 0 will make image transparent and a value of 255
             * // will make it opaque.pDev->SetRenderState(D3DRS_BLENDFACTOR, 150);
             */
            if (Brush.A != 255)
            {
                Device.DxDevice.SetRenderState(RenderStates.AlphaBlendEnable, true);
                Device.DxDevice.SetRenderState(RenderStates.SourceBlend, 14);
                Device.DxDevice.SetRenderState(RenderStates.DestinationBlend, 15);
                Device.DxDevice.SetRenderState(RenderStates.BlendFactor,
                                               Color.FromArgb(Brush.A, Brush.A, Brush.A, Brush.A).ToArgb());
            }
            else
                Device.DxDevice.SetRenderState(RenderStates.AlphaBlendEnable, false);
            
            // - Горизонталь -
            float shiftX = 0;

            // Если выравнивание по центру
            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                shiftX += size.Width / 2.0f;
            }
            // Если выравнивание по правому краю
            else if ((Alignment & Alignment.Right) != 0)
            {
                shiftX += size.Width - marginX;
            }
            else
                shiftX += marginX;

            // Вертикаль
            float shiftY = 0;

            // Если выравнивание по центру
            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                shiftY += size.Height / 2.0f;
            }
            //  Если по верхнему краю
            else if ((Alignment & Alignment.Bottom) != 0)
            {
                shiftY += size.Height - marginY;
            }
            else
                shiftY += marginY;

		    var l = point.X - shiftX;
            var r = point.X + size.Width - shiftX;
            var t = point.Y - shiftY;
            var b = point.Y + size.Height - shiftY;

		    var verts = new CustomVertex.TransformedColored[6];
			verts[0] = new CustomVertex.TransformedColored(l, t, 1.0f, 1.0f, Brush.Argb);
            verts[1] = new CustomVertex.TransformedColored(r, t, 1.0f, 1.0f, Brush.Argb);
            verts[2] = new CustomVertex.TransformedColored(r, b, 1.0f, 1.0f, Brush.Argb);
            verts[3] = new CustomVertex.TransformedColored(r, b, 1.0f, 1.0f, Brush.Argb);
            verts[4] = new CustomVertex.TransformedColored(l, b, 1.0f, 1.0f, Brush.Argb);
            verts[5] = new CustomVertex.TransformedColored(l, t, 1.0f, 1.0f, Brush.Argb);

			Device.DxDevice.VertexFormat = CustomVertex.TransformedColored.Format;
			Device.DxDevice.DrawUserPrimitives(PrimitiveType.TriangleList, 2, verts);
		}
	}
}
