
namespace TapeImplement
{
    public interface IScaleDiapazone<T>
    {
        T Min { get; set; }
        T Max { get; set; }

        T MinWidth { get; set; }
        T MaxWidth { get; set; }
    }
}
