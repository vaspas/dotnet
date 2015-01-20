﻿using System;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace TapeDrawingSharpDx11.Sprites
{
    public class GpaaSprite:IDisposable
    {
        public GpaaSprite(DeviceDescriptor device)
        {
            _device = device;

            _vertexShaderByteCode = ShaderBytecode.Compile(Properties.Resources.Gpaa, "VS", "vs_4_0");
            _vertexShader = new VertexShader(_device.DxDevice, _vertexShaderByteCode);

            _pixelShaderByteCode = ShaderBytecode.Compile(Properties.Resources.Gpaa, "PS", "ps_4_0");
            _pixelShader = new PixelShader(_device.DxDevice, _pixelShaderByteCode);

            // Layout from VertexShader input signature
            _layout = new InputLayout(
                _device.DxDevice,
                ShaderSignature.GetInputSignature(_vertexShaderByteCode),
                new[]
                    {
                        new InputElement("POSITIONA", 0, Format.R32G32B32A32_Float, 0,0),
                        new InputElement("POSITIONB", 0, Format.R32G32B32A32_Float, 16, 0)
                    });

            _sampler = new SamplerState(_device.DxDevice, new SamplerStateDescription
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

        private readonly SamplerState _sampler;

        private readonly InputLayout _layout;

        public void Begin()
        {
            _device.Context.InputAssembler.InputLayout = _layout;
            _device.Context.VertexShader.Set(_vertexShader);
            _device.Context.GeometryShader.Set(null);
            _device.Context.PixelShader.Set(_pixelShader);
            _device.Context.PixelShader.SetSampler(0, _sampler);

            _device.Context.CopyResource(_device.BackBuffer, _device.CopyBuffer);
            
            _device.Context.PixelShader.SetShaderResource(0, _device.CopyBufferShaderView);
        }

        public void Dispose()
        {
            _sampler.Dispose();
            _vertexShaderByteCode.Dispose();
            _vertexShader.Dispose();
            _pixelShaderByteCode.Dispose();
            _pixelShader.Dispose();
            _layout.Dispose();
        }
    }
}
