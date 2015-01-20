using System;
using System.Diagnostics;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using TapeDrawingSharpDx11.Sprites;
using Device = SharpDX.Direct3D11.Device;

namespace TapeDrawingSharpDx11
{
	/// <summary>
	/// Класс обеспечивает примитивную графику с использованием DirectX
	/// 
	/// Применение:
	/// - создать объект класса
	/// - вызвать для объекта InitGraphics(Control parent) с указанием контрола или формы
	///		на который будет производиться вывод
	/// - В методе рисования выполнить посдедовательность вызовов:
	///		а) StartDraw() - для начала рисования
	///		б) Произвести необходимую отрисовку
	///		в) EndDraw() - для окончания отрисовки
	///		г) Show() - отобразить на экране то, что было нарисовано в буфер DirectX
	/// 
	/// Изменить шрифт по умолчанию для вывода текста SetFont(...)
	/// </summary>
	public class DirectxGraphics : IDisposable
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		public DirectxGraphics()
		{
			_parent = null;

			Device = new DeviceDescriptor();
		}

        public bool Antialias { get; set; }

        public GbaaSprite GbaaSprite { get; set; }

		#region - Основные методы -
		/// <summary>
		/// Выполняет инициализацию гафического объекта с параметрами по умолчанию
		/// (видокарта по умолчанию, аппаратное ускорение, программная обработка вершин, оконный режим)
		/// а также инициализацию шрифта по умолчанию - Arial, 12 обычный
		/// </summary>
		/// <param name="parent">Это контрол, на поверхность которого будет производиться вывод</param>
		/// <returns>true если инициализация прошла успешно, false если инициализация неудачна</returns>
		public bool InitGraphics(Control parent)
		{
			try
			{
                Debug.WriteLine("SharpDx: Set params...");


				// Запомним родителя, а также будем обрабатывать ситуацию некорректного размера
				_parent = parent;
                _parent.Resize += ParentSizeChanged;

                // SwapChain description
                _desc = new SwapChainDescription
                {
                    BufferCount = 1,
                    ModeDescription =
                        new ModeDescription(_parent.ClientSize.Width, _parent.ClientSize.Height,
                                            new Rational(60, 1), Format.R8G8B8A8_UNorm),
                    IsWindowed = true,
                    OutputHandle = _parent.Handle,
                    SampleDescription = new SampleDescription(1, 0),
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput|Usage.ShaderInput
                };

                // Create Device and SwapChain
                Device device;
                SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, _desc, out device, out _swapChain);
			    Device.DxDevice = device;
                Device.Context = device.ImmediateContext;
                
                // Ignore all windows events
                var factory = _swapChain.GetParent<Factory>();
                factory.MakeWindowAssociation(_parent.Handle, WindowAssociationFlags.IgnoreAll);
                

                var blendDescription = new BlendStateDescription();
                blendDescription.RenderTarget[0].IsBlendEnabled = true;
                blendDescription.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
                blendDescription.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
                blendDescription.RenderTarget[0].BlendOperation = BlendOperation.Add;
                blendDescription.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
                blendDescription.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
                blendDescription.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
                blendDescription.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All | ColorWriteMaskFlags.Alpha;


                var blendState = new BlendState(device, blendDescription);
                Device.Context.OutputMerger.SetBlendState(blendState);
                
                Device.VertexConstantBuffer = new SharpDX.Direct3D11.Buffer(device, Utilities.SizeOf<Matrix>()*2+8*sizeof(float), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
                Device.Context.VertexShader.SetConstantBuffer(0, Device.VertexConstantBuffer);
                Device.Context.GeometryShader.SetConstantBuffer(0, Device.VertexConstantBuffer);
                Device.Context.PixelShader.SetConstantBuffer(0, Device.VertexConstantBuffer);
                
                Debug.WriteLine("SharpDx: Done");
			}
			catch (Exception ex)
			{
				Device.DxDevice = null;
                Debug.WriteLine("SharpDx: Warn: " + ex.Message);
				return false;
			}

			return true;
		}
        

	    private SwapChainDescription _desc;
        
        SwapChain _swapChain;
        bool _userResized = true;
		/// <summary>
		/// Начинает рисование сцены
		/// </summary>
		public void StartDraw()
		{
            if (_userResized)
            {
                // Dispose all previous allocated resources
                Utilities.Dispose(ref Device.BackBuffer);
                Utilities.Dispose(ref Device.RenderView);
                Utilities.Dispose(ref Device.GeometryBuffer);
                //Utilities.Dispose(ref _depthBuffer);
                //Utilities.Dispose(ref Device.DepthView);
                Utilities.Dispose(ref Device.CopyBuffer);
                if(Device.CopyBufferShaderView!=null)
                    Device.CopyBufferShaderView.Dispose();
                if (Device.GeometryBufferView != null)
                    Device.GeometryBufferView.Dispose();

                // Resize the backbuffer
                _swapChain.ResizeBuffers(_desc.BufferCount, _parent.ClientSize.Width, _parent.ClientSize.Height, Format.Unknown, SwapChainFlags.None);

                // Get the backbuffer from the swapchain
                Device.BackBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);

                if (Antialias || GbaaSprite!=null)
                {
                    Device.CopyBuffer = new Texture2D(Device.DxDevice, Device.BackBuffer.Description);
                    Device.CopyBufferShaderView = new ShaderResourceView(Device.DxDevice, Device.CopyBuffer);    
                }
                
                Device.GeometryBuffer = new Texture2D(Device.DxDevice,
                                                   new Texture2DDescription
                                                       {
                                                           Format = Format.R8G8B8A8_UNorm,
                                                           ArraySize = 1,
                                                           MipLevels = 1,
                                                           Width = _parent.ClientSize.Width,
                                                           Height = _parent.ClientSize.Height,
                                                           SampleDescription =
                                                               new SampleDescription(1, 0),
                                                           Usage = ResourceUsage.Default,
                                                           BindFlags =
                                                               BindFlags.ShaderResource |
                                                               BindFlags.RenderTarget,
                                                           CpuAccessFlags = CpuAccessFlags.None,
                                                           OptionFlags = ResourceOptionFlags.Shared
                                                       });
                

                // Renderview on the backbuffer
                Device.RenderView = new RenderTargetView(Device.DxDevice, Device.BackBuffer);
                Device.GeometryBufferView = new RenderTargetView(Device.DxDevice, Device.GeometryBuffer);
                Device.GeometryBufferShaderView = new ShaderResourceView(Device.DxDevice, Device.GeometryBuffer);


                // Setup targets and viewport for rendering
                Device.Context.Rasterizer.SetViewport(new Viewport(0, 0, _parent.ClientSize.Width, _parent.ClientSize.Height, 0.0f, 1.0f));
                Device.Context.OutputMerger.SetTargets(Device.RenderView, Device.GeometryBufferView);
                
                Device.VertexConstant.Width = _parent.ClientSize.Width;
                Device.VertexConstant.Height = _parent.ClientSize.Height;
                Device.VertexConstant.XFrom = 0;
                Device.VertexConstant.XTo = _parent.ClientSize.Width;
                Device.VertexConstant.YFrom = 0;
                Device.VertexConstant.YTo = _parent.ClientSize.Height;

                Device.VertexConstant.World = Matrix.Scaling(2f / _parent.ClientSize.Width, -2f / _parent.ClientSize.Height, 1) * Matrix.Translation(-1f, 1f, 0);
                Device.VertexConstant.World.Transpose();

                Device.VertexConstant.Translate = Matrix.Identity;

                // We are done resizing
                _userResized = false;
            }


            Device.Context.UpdateSubresource(ref Device.VertexConstant, Device.VertexConstantBuffer);
		}
		/// <summary>
		/// Заканчивает рисование сцены
		/// </summary>
		public void EndDraw()
		{
            GbaaSprite.Begin();
            
            Device.Context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            Device.Context.Draw(3, 0);
		}
		/// <summary>
		/// Выводит содержимое буфера устройства на экран
		/// </summary>
		public void Show()
		{
			try
			{
                _swapChain.Present(0, PresentFlags.None);
			}
			catch (SharpDXException)
			{
                Debug.WriteLine("SharpDx: Device lost");
			}

		}
		#endregion

		public DeviceDescriptor Device { get; private set; }

        public int Width
        {
            get { return _parent.Width; }
        }

        public int Heigth
        {
            get { return _parent.Height; }
        }

		#region - Закрытые атрибуты -
		/// <summary>
		/// Контрол, на поверхность которого производится вывод
		/// </summary>
		private Control _parent;
		
		#endregion

		#region - Закрытые методы -
		/// <summary>
		/// Выполняет очистку контекста устройства
		/// </summary>
		public void Dispose()
		{
            if (Device.VertexConstantBuffer!= null)
                Device.VertexConstantBuffer.Dispose();

            if (Device.RenderView != null)
                Device.RenderView.Dispose();

            if (Device.BackBuffer != null)
                Device.BackBuffer.Dispose();

            if (Device.CopyBuffer != null)
                Device.CopyBuffer.Dispose();

            if (Device.CopyBufferShaderView != null)
                Device.CopyBufferShaderView.Dispose();
            
            if (Device.GeometryBuffer != null)
                Device.GeometryBuffer.Dispose();

            if (Device.GeometryBufferView != null)
                Device.GeometryBufferView.Dispose();

            if (Device.GeometryBufferShaderView != null)
                Device.GeometryBufferShaderView.Dispose();

            if (Device.Context != null)
            {
                Device.Context.ClearState();
                Device.Context.Flush();
            }

			// Освободить устройство
			if (Device.DxDevice != null) 
                Device.DxDevice.Dispose();

            if (Device.Context != null)
                Device.Context.Dispose();

            if(_swapChain!=null)
                _swapChain.Dispose();

			_parent.Resize -= ParentSizeChanged;

			GC.Collect();

			Debug.WriteLine("SharpDx: Очистка произведена");
		}

        private void ParentSizeChanged(object sender, EventArgs e)
        {
            _userResized = true;
        }

	    #endregion
	}
}
