
using System.Collections.Generic;
using TapeImplement.TapeModels.VagonPrint.Table;

namespace TapeImplement.TapeModels.VagonPrint.Sources
{
    public interface IPrintSource
    {
        string GetCenterInfo();

        string GetRightInfo(IScalePosition<int> position);

        string GetTopInfoLine1(IScalePosition<int> position);

        string GetTopInfoLine2(IScalePosition<int> position);

        string GetTopInfoLineLeft(IScalePosition<int> position);

        string GetBottomInfoLine(IScalePosition<int> position);

        IEnumerable<Row> GetRows(IScalePosition<int> position);
    }
}
