using System;
using System.Runtime.InteropServices;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace TapeDrawingSharpDx11.Sprites
{
    public class LineSprite:IDisposable
    {
        public LineSprite(DeviceDescriptor device)
        {
            _device = device;

            _vertexShaderByteCode = ShaderBytecode.Compile(Properties.Resources.Line, "VS", "vs_4_0");
            _vertexShader = new VertexShader(_device.DxDevice, _vertexShaderByteCode);

            _geometryShaderByteCode = ShaderBytecode.Compile(Properties.Resources.Line, "GS", "gs_4_0");
            _geometryShader = new GeometryShader(_device.DxDevice, _geometryShaderByteCode);

            _pixelShaderByteCode = ShaderBytecode.Compile(Properties.Resources.Line, "PS", "ps_4_0");
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

            _buffer = new Buffer(_device.DxDevice, 32, ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        private readonly DeviceDescriptor _device;

        private readonly ShaderBytecode _vertexShaderByteCode;
        private readonly VertexShader _vertexShader;

        private readonly ShaderBytecode _pixelShaderByteCode;
        private readonly PixelShader _pixelShader;

        private readonly ShaderBytecode _geometryShaderByteCode;
        private readonly GeometryShader _geometryShader;

        private readonly Buffer _buffer;

        private readonly InputLayout _layout;

        private LineParams _lineParams;

        public void Begin(float width, float dash1, float dash2, float dash3, float dash4)
        {
            _lineParams.LineWidth = width;
            _lineParams.dash1 = dash1;
            _lineParams.dash2 = dash2;
            _lineParams.dash3 = dash3;
            _lineParams.dash4 = dash4;

            _device.Context.InputAssembler.InputLayout = _layout;
            _device.Context.VertexShader.Set(_vertexShader);
            _device.Context.GeometryShader.Set(_geometryShader);
            _device.Context.PixelShader.Set(_pixelShader);

            _device.Context.GeometryShader.SetConstantBuffer(1, _buffer);
            _device.Context.PixelShader.SetConstantBuffer(1, _buffer);
            _device.Context.UpdateSubresource(ref _lineParams, _buffer);
        }

        public void Dispose()
        {
            _buffer.Dispose();

            _vertexShaderByteCode.Dispose();
            _vertexShader.Dispose();
            _geometryShaderByteCode.Dispose();
            _geometryShader.Dispose();
            _pixelShaderByteCode.Dispose();
            _pixelShader.Dispose();
            _layout.Dispose();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LineParams
    {
        public float LineWidth;
        public float dash1;
        public float dash2;
        public float dash3;
        public float dash4;
        public float paramr1;
        public float paramr2;
        public float paramr3;
    }
}
