using Microsoft.DirectX.Direct3D;

namespace TapeDrawingWinFormsDx.Cache.LineCache
{
    class LineFromArgsCreator : ICacher<Line, LineCreatorArgs>
    {
        public DeviceDescriptor Device { get; set; }

        public bool Antialias { get; set; }

        public Line Get(ref LineCreatorArgs args)
        {
            return new Line(Device.DxDevice)
                       {
                           Antialias = Antialias,
                           Width = args.Width*(Antialias?0.7f:1),
                           Pattern = Converter.Convert(args.Style, args.Width),
                           PatternScale = 1.0f
                       };
        }

        public void Dispose()
        {}
    }
}
