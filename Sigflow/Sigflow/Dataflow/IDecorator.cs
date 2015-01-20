
namespace Sigflow.Dataflow
{
    public interface IDecorator
    {}
    public interface IDecorator<T>:IDecorator
    {
        T Internal { get; set; }
    }
}
