using System;
using System.Diagnostics;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace TapeDrawingWinFormsDx
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
			_softwareMode = false;

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
				Debug.WriteLine("DirectX: Set params...");

				_deviceLost = false;

				// Запомним родителя, а также будем обрабатывать ситуацию некорректного размера
				_parent = parent;
				_parent.Resize += ParentSizeChanged;

				// Установим параметры
				_presentParams = new PresentParameters { Windowed = true, SwapEffect = SwapEffect.Discard };

				Debug.WriteLine("DirectX: Done");
				Debug.WriteLine("DirectX: Device count = " + Manager.Adapters.Count);
				Debug.WriteLine("DirectX: Device num = " + Manager.Adapters[0].Adapter);

				Caps deviceCaps;

				// Выясним характеристики устройства, которое мы можем создать
				var deviceType = DeviceType.Software;
				var createFlags = CreateFlags.SoftwareVertexProcessing;

				// Будем пытаться создать самое производительное устройство DirectX
				try
				{
					deviceCaps = Manager.GetDeviceCaps(0, DeviceType.Hardware);

                    //убрал проверку для работы со старым железом, т.к. _softwareMode не работал
					//if ((deviceCaps.VertexShaderVersion >= new Version(2, 0)) && (deviceCaps.PixelShaderVersion >= new Version(2, 0)))
					{
						deviceType = DeviceType.Hardware;

						Debug.WriteLine("DirectX: Device: Hardware");

						if (deviceCaps.DeviceCaps.SupportsHardwareTransformAndLight)
						{
							createFlags = CreateFlags.HardwareVertexProcessing;
							Debug.WriteLine("DirectX: Device: HardwareVertexProcessing");
						}
						if (deviceCaps.DeviceCaps.SupportsPureDevice)
						{
							createFlags |= CreateFlags.PureDevice;
							Debug.WriteLine("DirectX: Device: PureDevice");
						}
					}
					/*else
					{
						Debug.WriteLine("DirectX: Device: Software");

						_softwareMode = true;
					}*/
				}
				catch (Exception ex)
				{
					Debug.WriteLine("DirectX: Device: GetDeviceCaps error! " + ex.Message);

					_softwareMode = true;
				}

				Debug.WriteLine("DirectX: Create Device...");

				try
				{
					Device.DxDevice = new Device(0, deviceType, parent, createFlags, _presentParams);
				}
				catch (Exception ex)
				{

					Debug.WriteLine("DirectX: Warn: " + ex.Message);
					Debug.WriteLine("DirectX: Device: Reference");

					deviceType = DeviceType.Reference;
					Device.DxDevice = new Device(0, deviceType, parent, createFlags, _presentParams);
				}

				Debug.WriteLine("DirectX: Done");

				// При сбросе устройства выполним переинициализацию если нужно
				Device.DxDevice.DeviceReset += OnDeviceReset;
			}
			catch (Exception ex)
			{
				Device.DxDevice = null;
				Debug.WriteLine("DirectX: ERR InitGraphics! " + ex.Message);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Работает в режиме программной обработки
		/// </summary>
		public bool SoftwareMode
		{
			get { return _softwareMode; }
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
			catch (DeviceLostException)
			{
				_deviceLost = true;

				Debug.WriteLine("DirectX: Device lost");
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
		/// Режим программной графики DirectX
		/// </summary>
		private bool _softwareMode;
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

			Debug.WriteLine("DirectX: Очистка произведена");
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
		}
		/// <summary>
		/// Вызывается при сбросе устройства (изменение размера?)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnDeviceReset(object sender, EventArgs e)
		{
			// Тут можно перезагрузить текстуры. Но они просто удалятся из кэша если будут уничтожены
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
			catch (DeviceLostException)
			{
				// Тут ничего не будем делать, пропустим
			}
			catch (DeviceNotResetException)
			{
				try
				{
					Device.DxDevice.Reset(_presentParams);
					_deviceLost = false;
					Debug.WriteLine("DirectX: device found");
				}
				catch (DeviceLostException)
				{
					// Если невозможно выполнить, будем ждать дальше
				}
			}
		}

		#endregion
	}
}
