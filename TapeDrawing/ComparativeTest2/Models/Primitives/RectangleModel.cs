using System.Xml.Serialization;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Primitives
{
	/// <summary>
	/// Прямоугольник
	/// </summary>
	public class RectangleModel
	{
	    [XmlIgnore] public Rectangle<float> Target;

	    public float Left
	    {
            get { return Target.Left; }
            set { Target.Left = value; }
	    }

        public float Right
        {
            get { return Target.Right; }
            set { Target.Right = value; }
        }

        public float Bottom
        {
            get { return Target.Bottom; }
            set { Target.Bottom = value; }
        }

        public float Top
        {
            get { return Target.Top; }
            set { Target.Top = value; }
        }

		public override string ToString()
		{
			return string.Format("LTRB({0},{1},{2},{3})", Left, Top, Right, Bottom);
		}
	}
}
