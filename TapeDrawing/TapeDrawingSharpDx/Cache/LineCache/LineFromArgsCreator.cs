
using SharpDX.Direct3D9;

namespace TapeDrawingSharpDx.Cache.LineCache
{
    class LineFromArgsCreator : ICacher<Line, LineCreatorArgs>
    {
        public DeviceDescriptor Device { get; set; }

        public Line Get(ref LineCreatorArgs args)
        {
            return new Line(Device.DxDevice)
                       {
                           Antialias = false,
                           Width = args.Width,
                           Pattern = Converter.Convert(args.Style, args.Width),
                           PatternScale = 1.0f
                       };
        }

        public void Dispose()
        {}
    }
}
