using System.Xml.Serialization;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Primitives
{
	public class SizeModel
	{
	    [XmlIgnore] public Size<float> Target;

	    public float Width
	    {
            get { return Target.Width; }
            set { Target.Width = value; }
	    }

        public float Height
        {
            get { return Target.Height; }
            set { Target.Height = value; }
        }

		public override string ToString()
		{
			return string.Format("({0},{1})", Width, Height);
		}
	}
}
