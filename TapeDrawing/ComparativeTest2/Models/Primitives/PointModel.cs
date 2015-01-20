using System.Xml.Serialization;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Primitives
{
	/// <summary>
	/// Точка
	/// </summary>
	public class PointModel
	{
	    [XmlIgnore] public Point<float> Target;

	    public float X
	    {
            get { return Target.X; }
            set { Target.X = value; }
	    }

        public float Y
        {
            get { return Target.Y; }
            set { Target.Y = value; }
        }

		public override string ToString()
		{
			return string.Format("({0},{1})", X, Y);
		}
	}
}
