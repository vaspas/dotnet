using System;
using System.Diagnostics;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D9;

namespace TapeDrawingSharpDx
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
                Debug.WriteLine("SharpDx: Set params...");

				_deviceLost = false;

				// Запомним родителя, а также будем обрабатывать ситуацию некорректного размера
				_parent = parent;
				_parent.Resize += ParentSizeChanged;

			    _presentParams = new PresentParameters {Windowed = true, SwapEffect = SwapEffect.Discard};
                Device.DxDevice = new Device(new Direct3D(), 0, DeviceType.Hardware, _parent.Handle, 
                    CreateFlags.HardwareVertexProcessing|CreateFlags.PureDevice,
                    _presentParams
                    );
                
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

		/// <summary>
		/// Начинает рисование сцены
		/// </summary>
		public void StartDraw()
		{

			Device.DxDevice.BeginScene();
		}
		/// <summary>
		/// Заканчивает рисование сцены
		/// </summary>
		public void EndDraw()
		{
			Device.DxDevice.EndScene();
		}
		/// <summary>
		/// Выводит содержимое буфера устройства на экран
		/// </summary>
		public void Show()
		{
			if (_deviceLost)
			{
				// Попробуем восстановить графическое устройство
				AttemptRecovery();
			}

			if (_deviceLost)
			{
				// Восстановить не получилось, выход
				return;
			}

			try
			{
				Device.DxDevice.Present();
			}
			catch (SharpDXException)
			{
				_deviceLost = true;

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
		/// <summary>
		/// Параметры отображения
		/// </summary>
		private PresentParameters _presentParams;
		/// <summary>
		/// Флаг "устройство потеряно" (например если нажать WINDOWS + L)
		/// </summary>
		private bool _deviceLost;
		#endregion

		#region - Закрытые методы -
		/// <summary>
		/// Выполняет очистку контекста устройства
		/// </summary>
		public void Dispose()
		{
			// Освободить устройство
			if (Device.DxDevice != null) Device.DxDevice.Dispose();

			_parent.Resize -= ParentSizeChanged;

			GC.Collect();

			Debug.WriteLine("SharpDx: Очистка произведена");
		}
		/// <summary>
		/// Вызываетя при изменении размеров родительского контрола или формы.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ParentSizeChanged(object sender, EventArgs e)
		{
			if ((_parent.Height <= 0) || (_parent.Width <= 0))
			{
				_parent.Width = 1;
				_parent.Height = 1;
				Device.Pause = true;
			}
			else
			{
				Device.Pause = false;
			}

            //_presentParams.BackBufferWidth = _parent.ClientRectangle.Width;
            //_presentParams.BackBufferHeight = _parent.ClientRectangle.Height;
            //Device.DxDevice.Reset(_presentParams);
		}

        /// <summary>
        /// Метод делает попытку восстановить графическое устройство 
        /// в случае его потери
        /// </summary>
        private void AttemptRecovery()
        {
            try
            {
                Device.DxDevice.TestCooperativeLevel();
            }
            catch (SharpDXException)
            {
                try
                {
                    Device.DxDevice.Reset(_presentParams);
                    _deviceLost = false;
                    Debug.WriteLine("DirectX: device found");
                }
                catch (SharpDXException)
                {
                    // Если невозможно выполнить, будем ждать дальше
                }
            }
        }

		#endregion
	}
}
