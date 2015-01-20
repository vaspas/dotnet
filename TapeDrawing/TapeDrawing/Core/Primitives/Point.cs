
namespace TapeDrawing.Core.Primitives
{
	public struct Point<T> where T : struct
	{
		public Point(T x, T y)
		{
			X = x;
			Y = y;
		}

		public T X;
		public T Y;
	}
}
