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
	class FillRectangleShape : BaseShape, IFillRectangleShape
	{
		/// <summary>
		/// Кисть заливки прямоугольника
		/// </summary>
		public Brush Brush { get; set; }

		public void Render(Rectangle<float> rectangle)
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
			bool needRestoreAlpha = false;
            if (Brush.A != 255)
            {
                Device.DxDevice.SetRenderState(RenderStates.AlphaBlendEnable, true);
                Device.DxDevice.SetRenderState(RenderStates.SourceBlend, 14);
                Device.DxDevice.SetRenderState(RenderStates.DestinationBlend, 15);
                Device.DxDevice.SetRenderState(RenderStates.BlendFactor,
                                               Color.FromArgb(Brush.A, Brush.A, Brush.A, Brush.A).ToArgb());
            	needRestoreAlpha = true;
            }
            else
                Device.DxDevice.SetRenderState(RenderStates.AlphaBlendEnable, false);


		    var verts = new CustomVertex.TransformedColored[6];
			verts[0] = new CustomVertex.TransformedColored(rectangle.Left, rectangle.Top, 1.0f, 1.0f, Brush.Argb);
            verts[1] = new CustomVertex.TransformedColored(rectangle.Right, rectangle.Top, 1.0f, 1.0f, Brush.Argb);
            verts[2] = new CustomVertex.TransformedColored(rectangle.Right, rectangle.Bottom, 1.0f, 1.0f, Brush.Argb);
            verts[3] = new CustomVertex.TransformedColored(rectangle.Right, rectangle.Bottom, 1.0f, 1.0f, Brush.Argb);
            verts[4] = new CustomVertex.TransformedColored(rectangle.Left, rectangle.Bottom, 1.0f, 1.0f, Brush.Argb);
            verts[5] = new CustomVertex.TransformedColored(rectangle.Left, rectangle.Top, 1.0f, 1.0f, Brush.Argb);

			Device.DxDevice.VertexFormat = CustomVertex.TransformedColored.Format;
			Device.DxDevice.DrawUserPrimitives(PrimitiveType.TriangleList, 2, verts);

			// Восстановить статус прозрачности
			if(needRestoreAlpha) Device.DxDevice.SetRenderState(RenderStates.AlphaBlendEnable, false);
		}
	}
}
