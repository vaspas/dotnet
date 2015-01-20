
namespace TapeDrawing.ShapesDecorators
{
    public interface ITranslatorDecorator<T>
    {
        T Translator { get; set; }
    }
    
    public interface IDecorator<T>
    {
        T Target { get; set; }
    }

}
