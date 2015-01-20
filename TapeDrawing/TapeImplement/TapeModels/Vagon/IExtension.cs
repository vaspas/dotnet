
namespace TapeImplement.TapeModels.Vagon
{
    public interface IExtension
    {
    }

    public interface IExtension<in T> : IExtension
    {
        void Build(T model);
    }
}
