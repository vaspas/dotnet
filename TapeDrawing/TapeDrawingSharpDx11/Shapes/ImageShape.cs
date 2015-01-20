using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx11.Instruments;
using TapeDrawingSharpDx11.Sprites;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace TapeDrawingSharpDx11.Shapes
{
	/// <summary>
	/// Фигура для рисования изображений
	/// </summary>
	class ImageShape : BaseShape, IImageShape
	{
	    public TextureSprite Sprite;

	    public Image Image;

        /// <summary>
        /// Выравнивание изображения
        /// </summary>
        public Alignment Alignment;

        /// <summary>
        /// Угол повотора изображения
        /// </summary>
        public float Angle;

		/*public void Render(Point<float> point)
		{
            var x = point.X + (int)CalculateShiftX();
            var y = point.Y + (int)CalculateShiftY();

            var w = Image.Roi.IsEmpty() ? Image.Width : Math.Abs(Image.Roi.Right - Image.Roi.Left);
            var h = Image.Roi.IsEmpty() ? Image.Height : Math.Abs(Image.Roi.Top - Image.Roi.Bottom);

		    var roiLeft = 0f;
            var roiRight = 1f;
            var roiTop = 0f;
            var roiBottom = 1f;
            if (!Image.Roi.IsEmpty())
            {
                roiLeft = Image.Roi.Left / Image.Width;
                roiRight = Image.Roi.Right / Image.Width;
                roiTop = Image.Roi.Top / Image.Height;
                roiBottom = Image.Roi.Bottom / Image.Height;
            }

            // Instantiate Vertex buiffer from vertex data
            var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, new[]
                                  {
                                      // 3D coordinates              UV Texture coordinates
                                      x, y+h, 0.5f, 1.0f,     roiLeft, roiBottom, // Front
                                      x, y, 0.5f, 1.0f,     roiLeft, roiTop,
                                      x+w, y, 0.5f, 1.0f,     roiRight, roiTop,
                                      x, y+h, 0.5f, 1.0f,     roiLeft, roiBottom,
                                      x+w, y, 0.5f, 1.0f,     roiRight, roiTop,
                                      x+w, y+h, 0.5f, 1.0f,     roiRight, roiBottom
                            });
            
            Sprite.Begin();

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
            Device.Context.PixelShader.SetShaderResource(0, Image.TextureShaderResourceView);

            Device.Context.Draw(6, 0);

            vertices.Dispose();
		}*/

        public void Render(Point<float> point)
        {
            var w = Image.Roi.IsEmpty() ? Image.Width : Math.Abs(Image.Roi.Right - Image.Roi.Left);
            var h = Image.Roi.IsEmpty() ? Image.Height : Math.Abs(Image.Roi.Top - Image.Roi.Bottom);

            var shiftX =-CalculateShiftX();
            var shiftY = -CalculateShiftY();

            var roiLeft = 0f;
            var roiRight = 1f;
            var roiTop = 0f;
            var roiBottom = 1f;
            if (!Image.Roi.IsEmpty())
            {
                roiLeft = Image.Roi.Left / Image.Width;
                roiRight = Image.Roi.Right / Image.Width;
                roiTop = Image.Roi.Top / Image.Height;
                roiBottom = Image.Roi.Bottom / Image.Height;
            }

            // Instantiate Vertex buiffer from vertex data
            var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer,
                                         new[]
                                             {
                                                 0, h, 0.5f, 1.0f, roiLeft, roiBottom,
                                                 0, 0, 0.5f, 1.0f, roiLeft, roiTop,
                                                  w, 0, 0.5f, 1.0f, roiRight , roiTop,
                                                 0, h, 0.5f, 1.0f, roiLeft, roiBottom,
                                                 w, 0, 0.5f, 1.0f, roiRight , roiTop,
                                                 w, h, 0.5f, 1.0f, roiRight, roiBottom
                                             });

            

            Sprite.Begin();

            

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, Utilities.SizeOf<Vector4>() + Utilities.SizeOf<Vector2>(), 0));
            Device.Context.PixelShader.SetShaderResource(0, Image.TextureShaderResourceView);

            Device.VertexConstant.Translate = Matrix.AffineTransformation2D(1,new Vector2(shiftX,shiftY),  (float)(Math.PI* Angle/180),
                new Vector2(point.X-shiftX,point.Y-shiftY));
            Device.VertexConstant.Translate.Transpose();
            Device.Context.UpdateSubresource(ref Device.VertexConstant, Device.VertexConstantBuffer);

            Device.Context.Draw(6, 0);

            Device.VertexConstant.Translate = Matrix.Identity;
            Device.VertexConstant.Translate.Transpose();
            Device.Context.UpdateSubresource(ref Device.VertexConstant, Device.VertexConstantBuffer);

            vertices.Dispose();
        }

        private float CalculateShiftX()
        {
            var w = Image.Roi.IsEmpty() ? Image.Width : Math.Abs(Image.Roi.Right - Image.Roi.Left);

            if (((Alignment & Alignment.Left) != 0 && (Alignment & Alignment.Right) != 0)
                || ((Alignment & Alignment.Left) == 0 && (Alignment & Alignment.Right) == 0))
            {
                return -w / 2;
            }

            if ((Alignment & Alignment.Right) != 0)
            {
                return -w;
            }

            return 0;
        }

        private float CalculateShiftY()
        {
            var h = Image.Roi.IsEmpty() ? Image.Height : Math.Abs(Image.Roi.Top - Image.Roi.Bottom);

            if (((Alignment & Alignment.Bottom) != 0 && (Alignment & Alignment.Top) != 0)
                || ((Alignment & Alignment.Bottom) == 0 && (Alignment & Alignment.Top) == 0))
            {
                return -h / 2;
            }
            if ((Alignment & Alignment.Bottom) != 0)
            {
                return -h;
            }

            return 0;
        }
	}
}
