
namespace TapeImplement.TapeModels.ZonesView
{
    public interface IExtension
    {
    }

    public interface IExtension<in T> : IExtension
    {
        void Build(T model);
    }
}
