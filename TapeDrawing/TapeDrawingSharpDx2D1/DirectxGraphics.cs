using System;
using System.Diagnostics;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace TapeDrawingSharpDx2D1
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
                Debug.WriteLine("SharpDx2D1: Set params...");
                
				// Запомним родителя, а также будем обрабатывать ситуацию некорректного размера
				_parent = parent;
                _parent.Resize += ParentSizeChanged;

                _factory2D1 = new SharpDX.Direct2D1.Factory();
                _factoryDirectWrite = new SharpDX.DirectWrite.Factory();

                var properties = new HwndRenderTargetProperties();
                properties.Hwnd = _parent.Handle;
                properties.PixelSize = new Size2(_parent.ClientSize.Width, _parent.ClientSize.Height);
                properties.PresentOptions = PresentOptions.None;

                Device.RenderTarget2D = new WindowRenderTarget(_factory2D1, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)), properties);
                Device.RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
                Device.RenderTarget2D.TextAntialiasMode = TextAntialiasMode.Cleartype;

                //SceneColorBrush = new SolidColorBrush(RenderTarget2D, Color.Black);
			}
			catch (Exception ex)
			{
				Device.RenderTarget2D = null;
                Debug.WriteLine("SharpDx2D1: Warn: " + ex.Message);
				return false;
			}

			return true;
		}

	    private SharpDX.Direct2D1.Factory _factory2D1;
        private SharpDX.DirectWrite.Factory _factoryDirectWrite;
        
	    
        bool _userResized = true;
		/// <summary>
		/// Начинает рисование сцены
		/// </summary>
		public void StartDraw()
		{
            if (_userResized)
            {
                
                // We are done resizing
                _userResized = false;
            }

            Device.RenderTarget2D.BeginDraw();
		}
		/// <summary>
		/// Заканчивает рисование сцены
		/// </summary>
		public void EndDraw()
		{
            Device.RenderTarget2D.EndDraw();
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
            Device.RenderTarget2D.Dispose();

            _factory2D1.Dispose();
            _factoryDirectWrite.Dispose();

			_parent.Resize -= ParentSizeChanged;

			GC.Collect();

			Debug.WriteLine("SharpDx2D1: Очистка произведена");
		}

        private void ParentSizeChanged(object sender, EventArgs e)
        {
            Device.RenderTarget2D.Resize(new Size2(_parent.ClientSize.Width, _parent.ClientSize.Height));

            _userResized = true;
        }

	    #endregion
	}
}
