//using System;


namespace IppModules.Analiz.Types
{
    /// <summary>
    /// �������� ������������� ������������ �����
    /// </summary>
    struct Polar
    {
        #region .ctor

        public Polar(double r)
        {
            rad_ = r;
            ang_ = 0.0;
        }

        public Polar(double r, double ang)
        {
            rad_ = r;
            ang_ = ang;
        }

        public Polar(Complex x)
        {
            rad_ = x.Abs;
            ang_ = x.Arg;
        }
        #endregion


        /// <summary>
        /// ������, �� ���� ������
        /// </summary>
        private double rad_;

        /// <summary>
        /// ����
        /// </summary>
        private double ang_;


        /// <summary>
        /// ���������� ����������� ����� �� ��������� ����� ������, 
        /// � ������������� �������� �� ������������ �����.
        /// </summary>
        public Complex Complex
        {
            get
            {
                return new Complex(this);
            }
            set
            {
                rad_ = value.Abs;
                ang_ = value.Arg;
            }
        }

        /// <summary>
        /// ���������� � ������������� ������.
        /// </summary>
        public double Radius
        {
            get { return rad_; }
            set { rad_ = value; }
        }

        /// <summary>
        /// ���������� � ������������� ����.
        /// </summary>
        public double Angle
        {
            get { return ang_; }
            set { ang_ = value; }
        }

        public override string ToString()
        {
            return "r=" + rad_.ToString() + " phi=" + ang_.ToString();
        }

        /// <summary>
        /// �������� �������������� �����.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator Complex(Polar p)
        {
            return new Complex(p);
        }

        /// <summary>
        /// �������� �������������� �����.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static explicit operator Polar(Complex c)
        {
            return new Polar(c);
        }

    }
}
