
using SharpDX.Direct3D11;

namespace TapeDrawingSharpDx11
{
	/// <summary>
	/// Дескриптор устройства с доп. информацией
	/// </summary>
	public class DeviceDescriptor
	{
	    /// <summary>
	    /// Устройство DirectX
	    /// </summary>
	    public Device DxDevice;

        public DeviceContext Context;
        public RenderTargetView RenderView;

	    public VertexConstant VertexConstant;
	    public Buffer VertexConstantBuffer;

	    public Texture2D BackBuffer;
        public Texture2D CopyBuffer;
        public ShaderResourceView CopyBufferShaderView;

        public Texture2D GeometryBuffer;
        public RenderTargetView GeometryBufferView;
        public ShaderResourceView GeometryBufferShaderView;
	}
}
