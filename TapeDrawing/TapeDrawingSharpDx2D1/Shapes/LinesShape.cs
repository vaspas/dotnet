using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx2D1.Instruments;
using Brush = SharpDX.Direct2D1.Brush;

namespace TapeDrawingSharpDx2D1.Shapes
{
    /// <summary>
    /// Фигура рисует линии
    /// </summary>
    class LinesShape : BaseShape, ILinesShape
    {
        /// <summary>
        /// Карандаш для рисования линий
        /// </summary>
        public Pen Pen;
        
        public void Render(IEnumerable<Point<float>> points)
        {
            Point<float>? last=null;
            foreach(var p in points)
            {
                if(last!=null)
                    Device.RenderTarget2D.DrawLine(new Vector2(last.Value.X, last.Value.Y), new Vector2(p.X, p.Y),
                        new SolidColorBrush(Device.RenderTarget2D, SharpDX.Color.Black));


                last = p;
            }

            // Instantiate Vertex buiffer from vertex data
            /*var vertices = Buffer.Create(Device.DxDevice, BindFlags.VertexBuffer, verts);

            if(Pen.Width==1 && (Pen.Dash1+ Pen.Dash2+ Pen.Dash3+ Pen.Dash4)==0)
                Sprite.Begin();
            else
                LineSprite.Begin(Pen.Width, Pen.Dash1, Pen.Dash2, Pen.Dash3, Pen.Dash4);

            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;
            Device.Context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));

            Device.Context.Draw(count, 0);

            vertices.Dispose();*/
        }
    }
}
