using System.IO;

namespace ComparativeTest2.Configuration
{
	/// <summary>
	/// Парсер умеет сохранять и восстанавливать объекты из XML файлов
	/// Использование 
	/// Сохранение:
	/// ObjectParser<Type> parser = new ObjectParser<Type>("out.xml");
	/// parser.SetNewObject(destination);
	///	parser.Save();
	/// Загрузка: 
	/// Type destination = parser.Object;
	/// Чтобы ингорировать сериализацию некоторых атрибутов нужно указать [XmlIgnore]
	/// [XmlRoot("Name")] - изменить имя корневого элемента
	/// [XmlAttribute("Name")] - атрибут XML. Хранится как <Data Name="Value"></Data>Data>
	/// [XmlElement("Name")] - элемент объекта. Хранится как <Data><Name>Value</Name></Data>
	/// </summary>
	/// <typeparam name="T">Тип объекта, который загружается из XML файла</typeparam>
	public class ObjectParser<T> where T : class, new()
	{
		/// <summary>
		/// Создает объект парсера
		/// </summary>
		/// <param name="name">Название XML файла</param>
		public ObjectParser(string name)
		{
			_name = name;
		}
		/// <summary>
		/// Создает объект парсера
		/// </summary>
		/// <param name="stream">Поток с данными</param>
		public ObjectParser(Stream stream)
		{
			_stream = stream;
		}

		#region - Открытые методы и свойства -
		/// <summary>
		/// Возвращает объект, загруженный из конфиг. файла.
		/// </summary>
		public T Object
		{
			get
			{
				// Если объект загружен, вернуть объект
				if (_instance != null) return _instance;

				lock (_lockFlag)
				{
					//Пытаемся загрузить данные
					if (_stream != null)
					{
						// Из потока
						var xs = new System.Xml.Serialization.XmlSerializer(typeof (T));
						_instance = (T)xs.Deserialize(_stream);
					}
					else
					{
						// Из файла
						using (var fs = new FileStream(_name, FileMode.Open))
						{
							var xs = new System.Xml.Serialization.XmlSerializer(typeof (T));
							_instance = (T) xs.Deserialize(fs);
						}
					}
				}
				return _instance;
			}
		}
		/// <summary>
		/// Перезагрузка
		/// </summary>
		public void Reset()
		{
			_instance = null;
		}
		/// <summary>
		/// Сохранение текущих настроек.
		/// </summary>
		public void Save()
		{
			if (_instance == null)
				return;

			if (_stream != null)
			{
				// В поток
				var xs = new System.Xml.Serialization.XmlSerializer(typeof (T));
				xs.Serialize(_stream, _instance);
			}
			else
			{
				// В файл
				using (var fs = new FileStream(_name, FileMode.Create))
				{
					var xs = new System.Xml.Serialization.XmlSerializer(typeof (T));
					xs.Serialize(fs, _instance);
				}
			}
		}
		/// <summary>
		/// Устанавливает новый объект с параметрами.
		/// </summary>
		/// <param name="obj"></param>
		public void SetNewObject(T obj)
		{
			_instance = obj;
		}
		/// <summary>
		/// Копирует объект
		/// </summary>
		/// <param name="obj">Объект, копию которого нужно получить</param>
		/// <returns>Копию объекта</returns>
		public T Copy(T obj)
		{
			var ms = new MemoryStream();

			// Сериализуем в поток
			var xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
			xs.Serialize(ms, obj);

			// Десериализуем из потока в копию
			ms.Position = 0;
			var copy = (T)xs.Deserialize(ms);

			return copy;
		}
		#endregion

		#region - Закрытые методы и свойства -
		/// <summary>
		/// Имя XML файла
		/// </summary>
		private readonly string _name;
		/// <summary>
		/// Поток, из котороге читать или в который сохранять
		/// </summary>
		private readonly Stream _stream;
		/// <summary>
		/// Объект для синхронизации.
		/// </summary>
		private readonly object _lockFlag = new object();
		/// <summary>
		/// Ссылка на объект класса.
		/// </summary>
		private T _instance;
		#endregion
	}
}
