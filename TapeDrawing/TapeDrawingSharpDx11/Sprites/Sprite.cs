using System;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace TapeDrawingSharpDx11.Sprites
{
    public class Sprite:IDisposable
    {
        public Sprite(DeviceDescriptor device)
        {
            _device = device;

            _vertexShaderByteCode = ShaderBytecode.Compile(Properties.Resources.MiniTri, "VS", "vs_4_0");
            _vertexShader = new VertexShader(_device.DxDevice, _vertexShaderByteCode);

            _pixelShaderByteCode = ShaderBytecode.Compile(Properties.Resources.MiniTri, "PS", "ps_4_0");
            _pixelShader = new PixelShader(_device.DxDevice, _pixelShaderByteCode);

            // Layout from VertexShader input signature
            _layout = new InputLayout(
                _device.DxDevice,
                ShaderSignature.GetInputSignature(_vertexShaderByteCode),
                new[]
                    {
                        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0)
                    });
        }

        private readonly DeviceDescriptor _device;

        private readonly ShaderBytecode _vertexShaderByteCode;
        private readonly VertexShader _vertexShader;

        private readonly ShaderBytecode _pixelShaderByteCode;
        private readonly PixelShader _pixelShader;

        private readonly InputLayout _layout;

        public void Begin()
        {
            _device.Context.InputAssembler.InputLayout = _layout;
            _device.Context.VertexShader.Set(_vertexShader);
            _device.Context.GeometryShader.Set(null);
            _device.Context.PixelShader.Set(_pixelShader);
        }

        public void Dispose()
        {
            _vertexShaderByteCode.Dispose();
            _vertexShader.Dispose();
            _pixelShaderByteCode.Dispose();
            _pixelShader.Dispose();
            _layout.Dispose();
        }
    }
}
