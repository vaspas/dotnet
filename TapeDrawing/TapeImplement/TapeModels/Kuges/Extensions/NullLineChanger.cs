
using System;
using System.IO;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Layers;
using TapeImplement.MouseListenerLayers;
using TapeImplement.ObjectRenderers;
using TapeImplement.ObjectRenderers.Signals;
using TapeImplement.SimpleRenderers;
using TapeImplement.TapeModels.Kuges.Track;
using TapeDrawing.Core.Translators;


namespace TapeImplement.TapeModels.Kuges.Extensions
{
    public class NullLineChanger : IExtension
    {
        /// <summary>
        /// Изображение для обозначения точек.
        /// </summary>
        public Stream MarkImage { get; set; }
        /// <summary>
        /// Изображение для обозначения захваченой точки.
        /// </summary>
        public Stream SelectedMarkImage { get; set; }

        /// <summary>
        /// Размер изображения.
        /// </summary>
        public Size<int> ImageSize { get; set; }

        /// <summary>
        /// Выравнивание изображения относительно точки.
        /// </summary>
        public Alignment ImageAlignment { get; set; }

        /// <summary>
        /// Источник данных.
        /// </summary>
        public ISignalPointSource Source { get; set; }

        /// <summary>
        /// Настройки выделения нулевой линии.
        /// </summary>
        public LineSettings LineSettings { get; set; }

        private DataTrackModel _trackModel;

        /// <summary>
        /// Вызывается при изменении точки.
        /// </summary>
        public Action<Point<float>, Point<float>> PointChanged { get; set; }

        public void Build(DataTrackModel trackModel)
        {
            _trackModel = trackModel;
            _trackModel.AddExtension(this);

            //настройка смещения зоны захвата по выравниванию изображения относительно точки
            var shiftX = 0f;
            if ((ImageAlignment & Alignment.Left) != Alignment.None)
                shiftX += ImageSize.Width/2f;
            if ((ImageAlignment & Alignment.Right) != Alignment.None)
                shiftX -= ImageSize.Width / 2f;
            var shiftY = 0f;
            if ((ImageAlignment & Alignment.Bottom) != Alignment.None)
                shiftY += ImageSize.Height / 2f;
            if ((ImageAlignment & Alignment.Top) != Alignment.None)
                shiftY -= ImageSize.Height / 2f;

            //обработчик перемещения курора мыши
            var mouseholder = new SignalPointHolderMouseListener
                                  {
                                      Diapazone = _trackModel.Position,
                                      ZonePixels = (ImageSize.Height+ImageSize.Width)/4f,
                                      Source = Source,
                                      TapePosition = _trackModel.TapeModel.TapePosition,
                                      Translator = PointTranslatorConfigurator.CreateLinear().ShiftY(shiftY).ShiftX(shiftX).Translator
                                  };
            //отображение активной точки
            var selectedRenderer = new PointImageRenderer
                                       {
                                           Diapazone = _trackModel.Position,
                                           ImageAlignment = Alignment.Bottom,
                                           ImageStream = SelectedMarkImage,
                                           TapePosition = _trackModel.TapeModel.TapePosition,
                                           Translator = PointTranslatorConfigurator.CreateLinear().Translator
                                       };
            //источник данных с измененными точками
            var changedSignalPointSource = new SignalPointSourceChangeDecorator
                                               { Internal = Source };
            //отображение данных  с измененными точками
            var changedSignalPointRenderer = new SignalPointRenderer
                                                 {
                                                     Source = changedSignalPointSource,
                                                     Diapazone = _trackModel.Position,
                                                     Translator =
                                                         PointTranslatorConfigurator.CreateLinear().Translator,
                                                     TapePosition = _trackModel.TapeModel.TapePosition,
                                                     LineColor = LineSettings.Color,
                                                     LineStyle = LineSettings.Style,
                                                     LineWidth = LineSettings.Width
                                                 };
            //слой отображения данных  с измененными точками
            var changedSignalPointLayer = new RendererLayer
                                              {
                                                  Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                                                  Renderer = new FakeRenderer()
                                              };
            _trackModel.DataLayer.Add(changedSignalPointLayer);


            //перемещение точки
            Point<float>? selectedPoint=null;
            Point<float>? dragDropPoint=null;
            var dragDrop = new PressedListener
                               {
                                   Diapazone = _trackModel.Position,
                                   TapePosition = _trackModel.TapeModel.TapePosition,
                                   Button = MouseButton.Left,
                                   CanStart = p => mouseholder.SelectedPoint != null,
                                   Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                   PositionChanged = 
                                   (p1, p2) =>
                                       {
                                           if (dragDropPoint == null)
                                           {
                                               selectedPoint= mouseholder.SelectedPoint;
                                               dragDropPoint = mouseholder.SelectedPoint;

                                               changedSignalPointLayer.Renderer = changedSignalPointRenderer;
                                               changedSignalPointSource.FromPoint = selectedPoint;
                                           }

                                           dragDropPoint = new Point<float> { X = selectedPoint.Value.X+p2.X-p1.X, Y=selectedPoint.Value.Y };
                                           selectedRenderer.Point = dragDropPoint;
                                           changedSignalPointSource.ToPoint = dragDropPoint;
                                           _trackModel.TapeModel.Redraw();
                                       },
                                   Completed = 
                                   (p1, p2) =>
                                       {
                                           if (dragDropPoint == null)
                                               return false;

                                           if (dragDropPoint.Value.X!=selectedPoint.Value.X)
                                               PointChanged(selectedPoint.Value, dragDropPoint.Value);

                                           dragDropPoint = null;
                                           selectedPoint=null;
                                           selectedRenderer.Point = mouseholder.SelectedPoint; 
                                           changedSignalPointLayer.Renderer = new FakeRenderer();
                                           _trackModel.TapeModel.Redraw();

                                           return true;
                                       }
                               };
            //при изменении активной точки меняем отображение
            mouseholder.OnSelectedPointChanged +=
                () =>
                    {
                        if (dragDropPoint != null)
                            return;

                        selectedRenderer.Point = mouseholder.SelectedPoint;
                        _trackModel.TapeModel.Redraw();
                    };
            
            //отображение всех точек сигнала
            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new SignalPointMarksRenderer
                {
                    Diapazone = _trackModel.Position,
                    ImageAlignment = Alignment.Bottom,
                    ImageStream = MarkImage,
                    //фильруем активные точки
                    Source = new SignalPointSourceFilterDecorator
                                 {
                                     Internal = Source,
                                     Filter = p =>
                                                  {
                                                      if(selectedPoint != null)
                                                          return p.X == selectedPoint.Value.X && p.Y == selectedPoint.Value.Y;

                                                      return mouseholder.SelectedPoint != null
                                                             && p.X == mouseholder.SelectedPoint.Value.X
                                                             && p.Y == mouseholder.SelectedPoint.Value.Y;
                                                  },
                                     StopSearch = p => _trackModel.TapeModel.TapePosition.To < p.X
                                 },
                    TapePosition =  _trackModel.TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator
                }
            });


            //слой отображения активных точек
            _trackModel.DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = selectedRenderer
            });

            //слой обработчика перемещения курора мыши
            _trackModel.DataLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener = mouseholder
            });


            _trackModel.DataLayer.Add(new MouseListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                MouseListener = dragDrop
            });
            _trackModel.DataLayer.Add(new KeyboardListenerLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                KeyboardProcess = dragDrop
            });
        }
    }
}
