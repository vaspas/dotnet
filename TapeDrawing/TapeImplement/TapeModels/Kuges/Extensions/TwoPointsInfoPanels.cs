using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.TapeModels.Kuges.LayerArea;
using TapeImplement.TapeModels.Kuges.Track;

namespace TapeImplement.TapeModels.Kuges.Extensions
{
    /// <summary>
    /// Расширение для расположения панелей на треке.
    /// Расположение панели рассчитывается по двум точкам, размерам панели и выравниванию.
    /// </summary>
    public class TwoPointsInfoPanels : IExtension
    {
        private DataTrackModel _trackModel;

        private ILayer _hostLayer;

        public void Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);

            _hostLayer = new EmptyLayer
                             {
                                 Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
                             };
            _trackModel.DataLayer.Add(_hostLayer);
        }

        class Item
        {
            public string Id;
            public object Data;
            public ILayer Layer;
        }

        private readonly List<Item> _items=new List<Item>();

        /// <summary>
        /// Регистрирует новую панель, но не отображает ее на экране.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">Идетификатор панели.</param>
        /// <param name="data">Ссылка на данные, связанные с этой панелью.</param>
        /// <param name="layer">Слой для добавления на панель.</param>
        public void Register<T>(string id, T data, ILayer layer)
        {
            var panel = new EmptyLayer
            {
                Area = new TwoPointsArea
                {
                    TapePosition = _trackModel.TapeModel.TapePosition,
                    Diapazone = _trackModel.Position,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    FirstPoint = default(Point<float>),
                    SecondPoint = default(Point<float>),
                    Size = default(Size<float>)
                }
            };
            panel.Add(layer);

            _items.Add(new Item
                           {
                               Id = id,
                               Data = data,
                               Layer = panel
                           });
            
        }

        /// <summary>
        /// Регистрирует новую панель, но не отображает ее на экране.
        /// </summary>
        /// <param name="id">Идетификатор панели.</param>
        /// <param name="layer">Слой для добавления на панель.</param>
        public void Register(string id, ILayer layer)
        {
            Register<object>(id, null, layer);
        }

        /// <summary>
        /// Точка для корректировки расположения панели на треке.
        /// x - номер отсчета дистанции
        /// y - значение
        /// </summary>
        /// <param name="id">Идетификатор панели.</param>
        /// <returns></returns>
        public Point<float> FirstPointOf(string id)
        {
            return (_items.First(i => i.Id == id).Layer.Area as TwoPointsArea).FirstPoint;
        }

        /// <summary>
        /// Точка для корректировки расположения панели на треке.
        /// </summary>
        /// x - номер отсчета дистанции
        /// y - значение
        /// <param name="id">Идетификатор панели.</param>
        /// <returns></returns>
        public Point<float> SecondPointOf(string id)
        {
            return (_items.First(i => i.Id == id).Layer.Area as TwoPointsArea).SecondPoint;
        }

        
        
        /// <summary>
        /// Отображает панель на экране. После вызова метода нужно выполнить перерисовку.
        /// </summary>
        /// <param name="id">Идетификатор панели.</param>
        /// <param name="first">Точка1</param>
        /// <param name="second">Точка2</param>
        /// <param name="size">Размер панели.</param>
        /// <param name="alignment">Выравнивание относительно точек.</param>
        public void Show(string id, Point<float> first, Point<float> second, Size<float> size, Alignment alignment)
        {
            var panel = _items.First(i => i.Id == id).Layer;

            var area = panel.Area as TwoPointsArea;
            area.FirstPoint = first;
            area.SecondPoint = second;
            area.Size = size;
            area.Alignment = alignment;

            if (_hostLayer.Contains(panel))
                return;

            _hostLayer.Add(panel);
        }

        /// <summary>
        /// Скрывает панель c экрана. После вызова метода нужно выполнить перерисовку.
        /// </summary>
        /// <param name="id"></param>
        public void Hide(string id)
        {
            var panel = _items.First(i => i.Id == id).Layer;

            if (!_hostLayer.Contains(panel))
                return;

            _hostLayer.Remove(panel);
        }


        public bool Contains(string id)
        {
            return _items.Any(i => i.Id == id);
        }

        /// <summary>
        /// Возвращает ссылку на данные, связанные с этой панелью.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetData<T>(string id)
            where T:class
        {
            return _items.FirstOrDefault(i => i.Id == id).Data as T;
        }


        public void Remove(string id)
        {
            Hide(id);

            _items.RemoveAll(i => i.Id == id);
        }
    }
}
