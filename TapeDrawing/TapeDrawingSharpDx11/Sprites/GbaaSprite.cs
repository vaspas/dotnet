using System;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace TapeDrawingSharpDx11.Sprites
{
    public class GbaaSprite:IDisposable
    {
        public GbaaSprite(DeviceDescriptor device)
        {
            _device = device;

            _vertexShaderByteCode = ShaderBytecode.Compile(Properties.Resources.Gbaa, "VS", "vs_4_0");
            _vertexShader = new VertexShader(_device.DxDevice, _vertexShaderByteCode);

            _pixelShaderByteCode = ShaderBytecode.Compile(Properties.Resources.Gbaa, "PS", "ps_4_0");
            _pixelShader = new PixelShader(_device.DxDevice, _pixelShaderByteCode);
            
            _linearsampler = new SamplerState(_device.DxDevice, new SamplerStateDescription
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                BorderColor = Color.Black,
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = 0,
                MaximumLod = 16,
            });
        }

        private readonly DeviceDescriptor _device;

        private readonly ShaderBytecode _vertexShaderByteCode;
        private readonly VertexShader _vertexShader;

        private readonly ShaderBytecode _pixelShaderByteCode;
        private readonly PixelShader _pixelShader;

        private readonly SamplerState _linearsampler;
        
        public void Begin()
        {
            _device.Context.InputAssembler.InputLayout = null;
            _device.Context.VertexShader.Set(_vertexShader);
            _device.Context.GeometryShader.Set(null);
            _device.Context.PixelShader.Set(_pixelShader);
            _device.Context.PixelShader.SetSampler(0, _linearsampler);

            _device.Context.CopyResource(_device.BackBuffer, _device.CopyBuffer);

            
            _device.Context.PixelShader.SetShaderResource(0, _device.CopyBufferShaderView);
            _device.Context.PixelShader.SetShaderResource(1, _device.GeometryBufferShaderView);
        }

        public void Dispose()
        {
            _linearsampler.Dispose();
            _vertexShaderByteCode.Dispose();
            _vertexShader.Dispose();
            _pixelShaderByteCode.Dispose();
            _pixelShader.Dispose();
        }
    }
}
