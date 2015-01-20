using System;
using System.IO;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using TapeImplement.ObjectRenderers;

namespace TapeImplement.TapeModels.Kuges.Track
{
    /// <summary>
    /// Модель дорожки с объектами.
    /// </summary>
    public class ObjectTrackModel:BaseTrackModel
    {
        public void AddPointObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getIndex, Stream image)
        {
            DataLayer.Add(new RendererLayer
                             {
                                 Area = AreasFactory.CreateMarginsArea(0,0,0,0),
                                 Renderer = new PointObjectRenderer<T>
                                                {
                                                    Source = source,
                                                    GetIndex = getIndex,
                                                    Image = image,
                                                    Alignment = Alignment.None,
                                                    Angle = 0,
                                                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                                                    TapePosition = TapeModel.TapePosition
                                                }
                             });
        }

        public void AddRegionObjectRenderer<T>(IObjectSource<T> source, Func<T, int> getFrom, Func<T, int> getTo, Stream image)
        {
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
                    Translator = PointTranslatorConfigurator.CreateLinear().Translator,
                    TapePosition = TapeModel.TapePosition
                }
            });
        }

    }
}
