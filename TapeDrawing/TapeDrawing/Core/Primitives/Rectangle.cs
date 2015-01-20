
namespace TapeDrawing.Core.Primitives
{
	public struct Rectangle<T> where T:struct
	{
	    public T Left;

	    public T Right;

	    public T Bottom;

	    public T Top;

        public bool IsEmpty()
        {
            return (dynamic)Left == Right && (dynamic)Right == Bottom && (dynamic)Bottom == Top;
        }
	}
}
