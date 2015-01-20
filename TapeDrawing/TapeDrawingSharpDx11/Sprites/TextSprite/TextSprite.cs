using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace TapeDrawingSharpDx11.Sprites.TextSprite
{

    /// <summary>
    /// This class is responsible for rendering 2D sprites. Typically, only one instance of this class is necessary.
    /// </summary>
    public class TextSprite : IDisposable
    {
        private readonly int _bufferSize=128;

        public Device Device { get; private set; }



        public TextSprite(Device device)
        {
            Device = device;

            CreateInputLayout(Properties.Resources.SpriteShader);
        }

        InputLayout _inputLayout;
        




        /// <summary>
        /// A list of all sprites to draw. Sprites are drawn in the order in this list.
        /// </summary>
        private readonly List<SpriteSegment> _sprites = new List<SpriteSegment>();
        /// <summary>
        /// Allows direct access to the according SpriteSegments based on the texture
        /// </summary>
        private readonly Dictionary<object, List<SpriteSegment>> _textureSprites = new Dictionary<object,List<SpriteSegment>>();

        /// <summary>
        /// The number of currently buffered sprites
        /// </summary>
        private int _spriteCount = 0;

        private ShaderBytecode _vertexShaderByteCode;
        private VertexShader _vertexShader;

        private ShaderBytecode _geometryShaderByteCode;
        private GeometryShader _geometryShader;

        private ShaderBytecode _pixelShaderByteCode;
        private PixelShader _pixelShader;

        private SamplerState _sampler;
        
        /// <summary>
        /// Compiles the effect and saves it as the renderer's FX. Furthermore, one effect resource variable is saved.
        /// </summary>
        /// <param name="hlslSource">The source code to compile</param>
        /// <param name="variableName">The variable's name</param>
        private void CreateInputLayout(string hlslSource)
        {
            _vertexShaderByteCode = ShaderBytecode.Compile(hlslSource, "VS", "vs_4_0");
            _vertexShader = new VertexShader(Device, _vertexShaderByteCode);

            _geometryShaderByteCode = ShaderBytecode.Compile(hlslSource, "GS", "gs_4_0");
            _geometryShader = new GeometryShader(Device, _geometryShaderByteCode);

            _pixelShaderByteCode = ShaderBytecode.Compile(hlslSource, "PS", "ps_4_0");
            _pixelShader = new PixelShader(Device, _pixelShaderByteCode);

            // Layout from VertexShader input signature
            _inputLayout = new InputLayout(
                Device,
                ShaderSignature.GetInputSignature(_vertexShaderByteCode),
                new[]
                    {
                        new InputElement("TEXCOORD", 0, Format.R32G32_Float, 0, 0),
                                                   new InputElement("TEXCOORDSIZE", 0, Format.R32G32_Float, 8, 0),
                                                   new InputElement("COLOR", 0, Format.B8G8R8A8_UNorm, 16, 0),
                                                   new InputElement("TOPLEFT", 0, Format.R32G32_Float, 20, 0),
                                                   new InputElement("TOPRIGHT", 0, Format.R32G32_Float, 28, 0),
                                                   new InputElement("BOTTOMLEFT", 0, Format.R32G32_Float, 36, 0),
                                                   new InputElement("BOTTOMRIGHT", 0, Format.R32G32_Float, 44, 0)
                    });

            _sampler = new SamplerState(Device, new SamplerStateDescription
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                BorderColor = Color.Black,
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = 0,
                MaximumLod = 16,
            });
        }
        







        /// <summary>
        /// Draws a region of a texture on the screen.
        /// </summary>
        /// <param name="texture">The shader resource view of the texture to draw</param>
        /// <param name="position">Position of the center of the texture in the chosen coordinate system</param>
        /// <param name="size">Size of the texture in the chosen coordinate system. The size is specified in the screen's coordinate system.</param>
        /// <param name="center">Specify the texture's center in the chosen coordinate system. The center is specified in the texture's local coordinate system. E.g. for <paramref name="coordinateType"/>=CoordinateType.SNorm, the texture's center is defined by (0, 0).</param>
        /// <param name="rotationAngle">The angle in radians to rotate the texture. Positive values mean counter-clockwise rotation. Rotations can only be applied for relative or absolute coordinates. Consider using the Degrees or Radians helper structs.</param>
        /// <param name="coordinateType">A custom coordinate system in which to draw the texture</param>
        /// <param name="color">The color with which to multiply the texture</param>
        /// <param name="texCoords">Texture coordinates for the top left corner</param>
        /// <param name="texCoordsSize">Size of the region in texture coordinates</param>
        public void Draw(ShaderResourceView texture, Vector2 position, Vector2 size, Vector2 center,  Vector2 texCoords, Vector2 texCoordsSize, Color4 color)
        {
            if (texture == null)
                return;

            size.X = Math.Abs(size.X);
            size.Y = Math.Abs(size.Y);

            //Difference vectors from the center to the texture edges (in screen coordinates).
            Vector2 left, up, right, down;
            left = new Vector2(-center.X, 0);
                up = new Vector2(0, -center.Y);
                right = new Vector2(size.X - center.X, 0);
                down = new Vector2(0, size.Y - center.Y);

            var data = new SpriteVertexLayout.Struct();            
            data.TexCoord = texCoords;
            data.TexCoordSize = texCoordsSize;
            data.Color = color.ToBgra();
            data.TopLeft = position + up + left;
            data.TopRight = position + up + right;
            data.BottomLeft = position + down + left;
            data.BottomRight = position + down + right;
            
                //Is there already a sprite for this texture?
                if (_textureSprites.ContainsKey(texture))
                {
                    //Add the sprite to the last segment for this texture
                    var segment = _textureSprites[texture].Last();
                    segment.Sprites.Add(data);
                    
                }
                else
                    //Add a new segment for this texture
                    AddNew(texture, data);

            _spriteCount++;
            if (_spriteCount >= _bufferSize)
                Flush();
        }

        private void AddNew(ShaderResourceView texture, SpriteVertexLayout.Struct data)
        {
            //Create new segment with initial values
            var newSegment = new SpriteSegment();
            newSegment.Texture = texture;
            newSegment.Sprites.Add(data);
            _sprites.Add(newSegment);

            //Create reference for segment in dictionary
            if (!_textureSprites.ContainsKey(texture))
                _textureSprites.Add(texture, new List<SpriteSegment>());

            _textureSprites[texture].Add(newSegment);
        }





        /// <summary>
        /// This method causes the SpriteRenderer to immediately draw all buffered sprites.
        /// </summary>
        /// <remarks>
        /// This method should be called at the end of a frame in order to draw the last sprites that are in the buffer.
        /// </remarks>
        public void Flush()
        {
            if (_spriteCount == 0)
                return;
            

            var vertices = Buffer.Create(Device, BindFlags.VertexBuffer,
                                         _sprites.SelectMany(s => s.Sprites).ToArray());

            Device.ImmediateContext.InputAssembler.InputLayout = _inputLayout;
            Device.ImmediateContext.VertexShader.Set(_vertexShader);
            Device.ImmediateContext.GeometryShader.Set(_geometryShader);
            Device.ImmediateContext.PixelShader.Set(_pixelShader);
            Device.ImmediateContext.PixelShader.SetSampler(0, _sampler);

            Device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            Device.ImmediateContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, SpriteVertexLayout.Struct.SizeInBytes, 0));
            

            //Draw
            var offset = 0;
            foreach (var segment in _sprites)
            {
                var count = segment.Sprites.Count;
                
                Device.ImmediateContext.PixelShader.SetShaderResource(0, segment.Texture);
                Device.ImmediateContext.Draw(count, offset);

                offset += count;
            }


            //Reset buffers
            _spriteCount = 0;
            _sprites.Clear();
            _textureSprites.Clear();
        }

        #region IDisposable Support

        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _sampler.Dispose();
                _vertexShaderByteCode.Dispose();
                _vertexShader.Dispose();
                _geometryShaderByteCode.Dispose();
                _geometryShader.Dispose();
                _pixelShaderByteCode.Dispose();
                _pixelShader.Dispose();
                _inputLayout.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Disposes of the SpriteRenderer.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
