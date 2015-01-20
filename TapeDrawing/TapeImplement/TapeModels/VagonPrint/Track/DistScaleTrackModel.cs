using System;
using System.IO;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.CoordGridRenderers;
using TapeImplement.ObjectRenderers;
using TapeImplement.SimpleRenderers;

namespace TapeImplement.TapeModels.VagonPrint.Track
{
    /// <summary>
    /// Модель дорожки шкалы дистанции.
    /// </summary>
    public class DistScaleTrackModel : BaseTrackModel
    {
        /// <summary>
        /// Добавляет простой источник коодинат, interrupts и units.
        /// В качестве unit можно выдавать метры или пикеты (метр/100)
        /// </summary>
        /// <param name="source"></param>
        public void AddSource(ICoordinateSource source)
        {
            AddInterrupt(source, ci=>true, ci=>false);
            AddUnit(source, 8);

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new BorderRenderer
                {
                    Left = true,
                    Color = TapeModel.Settings.DefaultColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = TapeModel.Settings.DefaultLineWidth
                }
            });
        }

        /// <summary>
        /// Добавляет источник коодинат, где interrupts делятся на пикеты, километры и отметки координат.
        /// В качестве unit используются метры.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="isCoordSet">Смена координаты.</param>
        /// <param name="isPk">Пикет.</param>
        public void AddSourceWithPk(ICoordinateSource source, Predicate<ICoordInterrupt> isCoordSet, Predicate<ICoordInterrupt> isPk)
        {
            AddInterrupt(source, ci=>!isPk(ci), isCoordSet);
            AddPkInterrupt(source, isPk);
            AddUnit(source, 6);

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new BorderRenderer
                {
                    Left = true,
                    Color = TapeModel.Settings.DefaultColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = TapeModel.Settings.DefaultLineWidth
                }
            });
        }
        
        private void AddInterrupt(ICoordinateSource source, Predicate<ICoordInterrupt> filter, Predicate<ICoordInterrupt> priorityFilter)
        {
            var interruptLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 0.2f, 0, 1),
                Renderer = new CoordInterruptGridRenderer
                {
                    Source =source,
                    LineColor = TapeModel.Settings.DefaultColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = TapeModel.Settings.DefaultLineWidth,
                    TapePosition = TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    Filter = filter
                }
            };
            DataLayer.Add(interruptLayer);

            var interruptATextLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(-1, 1, 0, 1),
                Renderer = new CoordInterruptTextRenderer
                {
                    Source = source,
                    Alignment = Alignment.Right|Alignment.Bottom,
                    TapePosition = TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    Filter = filter,
                    Angle = 90,
                    Color = new Color(0,0,0),
                    FontName = TapeModel.Settings.FontName,
                    FontSize = 10,
                    FontStyle = FontStyle.None,
                    MinPixelsDistance = 30,
                    PriorityFilter = priorityFilter,
                    TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                    TextFormatString = string.Empty
                }
            };
            DataLayer.Add(interruptATextLayer);

            
        }
        /// <summary>
        /// Отображает прерывания по пикетам.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="isPk"></param>
        private void AddPkInterrupt(ICoordinateSource source, Predicate<ICoordInterrupt> isPk)
        {
            var interruptLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 0.2f, 0, 1),
                Renderer = new CoordInterruptGridRenderer
                {
                    Source = source,
                    LineColor = TapeModel.Settings.DefaultColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = TapeModel.Settings.DefaultLineWidth,
                    TapePosition = TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    Filter = isPk
                }
            };
            DataLayer.Add(interruptLayer);

            var interruptATextLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(-1, 1, 0, 1),
                Renderer = new CoordInterruptTextRenderer
                {
                    Source = source,
                    Alignment = Alignment.Right | Alignment.Bottom,
                    TapePosition = TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    Filter = isPk,
                    Angle = 90,
                    Color = new Color(0, 0, 0),
                    FontName = TapeModel.Settings.FontName,
                    FontSize = 7,
                    FontStyle = FontStyle.None,
                    MinPixelsDistance = 30,
                    PriorityFilter = ci=>!isPk(ci),
                    TextAlignmentTranslator = AlignmentTranslatorConfigurator.Create().Translator,
                    TextFormatString = string.Empty
                }
            };
            DataLayer.Add(interruptATextLayer);


        }

        private void AddUnit(ICoordinateSource source, int fontSize)
        {
            var unitLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 0.2f, 0, 1),
                Settings = new RendererLayerSettings { Clip = true },
                Renderer = new CoordUnitGridRenderer
                {
                    Source = source,
                    LineColor = TapeModel.Settings.DefaultColor,
                    LineStyle = LineStyle.Solid,
                    LineWidth = TapeModel.Settings.DefaultLineWidth,
                    Mask = new[] { 1f},
                    MinPixelsDistance = 30,
                    TapePosition = TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                }
            };
            DataLayer.Add(unitLayer);

            var unitTextLayer = new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(-1, 1, 0, 1),
                Renderer = new CoordUnitTextRenderer
                {
                    Source = source,
                    IncreaseAlignment = Alignment.Right | Alignment.Bottom,
                    DecreaseAlignment = Alignment.Left | Alignment.Bottom,
                    TapePosition = TapeModel.TapePosition,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    Angle = 90,
                    Color = TapeModel.Settings.DefaultColor,
                    FontName = TapeModel.Settings.FontName,
                    FontSize = fontSize,
                    Mask = new[] { 1f },
                    FontStyle = FontStyle.None,
                    MinPixelsDistance = 30,
                    TextFormatString = string.Empty
                }
            };
            DataLayer.Add(unitTextLayer);
        }




        public void AddRecordObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Stream image)
        {
            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 2, 0, 1),
                Renderer = new PointObjectRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Image = image,
                    Alignment = Alignment.Top,
                    Angle = 90,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }

        public void AddRecordTextRenderer<T>(IObjectSource<T> source, Func<T,int> getIndex, Func<T, string> presentation)
        {
            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 2, 0, 1),
                Renderer = new RecordTextRenderer<T>
                {
                    Source = source,
                    GetIndex = getIndex,
                    Alignment = Alignment.Top,
                    Angle = 90,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    TapePosition = TapeModel.TapePosition,
                    FontColor = TapeModel.Settings.DefaultColor,
                    FontSize = 8,
                    FontType = TapeModel.Settings.FontName,
                    FontStyle = FontStyle.None,
                    ObjectPresentation = presentation
                }
            });
        }

        public void AddRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 2, 0, 1),
                Renderer = new RegionObjectRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Image = image,
                    Alignment = Alignment.Right | Alignment.Top,
                    Angle = 90,
                    Translator = PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }
    }
}
