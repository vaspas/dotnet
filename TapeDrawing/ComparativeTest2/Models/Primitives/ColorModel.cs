using System.Xml.Serialization;
using TapeDrawing.Core.Primitives;

namespace ComparativeTest2.Models.Primitives
{
	/// <summary>
	/// Цвет
	/// </summary>
	public class ColorModel
	{
	    [XmlIgnore] public Color Target;

		public ColorModel()
		{
            Target.A = 255;
            Target.R = 0;
            Target.G = 0;
            Target.B = 0;
		}

		[XmlIgnore]
		public System.Drawing.Color _Color
		{
            get { return System.Drawing.Color.FromArgb(Target.A, Target.R, Target.G, Target.B); }
			set
			{
                Target.A = value.A;
                Target.R = value.R;
                Target.G = value.G;
                Target.B = value.B;
			}
		}

	    public byte A
	    {
            get { return Target.A; }
            set { Target.A = value; }
	    }

        public byte R
        {
            get { return Target.R; }
            set { Target.R = value; }
        }

        public byte G
        {
            get { return Target.G; }
            set { Target.G = value; }
        }

        public byte B
        {
            get { return Target.B; }
            set { Target.B = value; }
        }

		public override string ToString()
		{
            return string.Format("ARGB({0},{1},{2},{3})", Target.A, Target.R, Target.G, Target.B);
		}
	}
}
