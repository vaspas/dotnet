using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.ObjectRenderers;

namespace TapeImplement.TapeModels.ZonesView.Track
{
    public class TrackModel
    {
        internal TapeModel TapeModel { get; set; }

        internal ILayer DataLayer { get; set; }


        private readonly List<IExtension> _extensions = new List<IExtension>();

        public T GetExtension<T>()
            where T : class,IExtension
        {
            return _extensions.FirstOrDefault(e => e is T) as T;
        }

        public void AddExtension(IExtension extension)
        {
            _extensions.Add(extension);
        }


        public void AddRegionImageRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new RegionObjectRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Image = image,
                    Alignment = Alignment.None,
                    Angle = 0,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }

        public void AddRegionFillRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Func<T, Color> getColor)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new RegionFillRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    GetColor = getColor,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }

        public void AddRegionBorderRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, LineSettings ls)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new RegionBoardsRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    LineColor = ls.Color,
                    LineStyle = ls.Style,
                    LineWidth = ls.Width,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }

        public void AddRegionTextRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, FontSettings font, Func<T, string> getText)
        {
            var translator = TapeModel.Vertical
                       ? PointTranslatorConfigurator.CreateLinear().ChangeAxels().Translator
                       : PointTranslatorConfigurator.CreateLinear().Translator;

            DataLayer.Add(new RendererLayer
            {
                Area = AreasFactory.CreateMarginsArea(0, 0, 0, 0),
                Renderer = new RegionTextRenderer<T>
                {
                    Source = source,
                    GetFrom = getFrom,
                    GetTo = getTo,
                    Angle = 0,
                    FontColor = font.Color,
                    FontName = font.Name,
                    FontSize = font.Size,
                    FontStyle = font.Style,
                    Translator = translator,
                    TapePosition = TapeModel.TapePosition,
                    ObjectPresentation = getText
                }
            });
        }
    }
}
