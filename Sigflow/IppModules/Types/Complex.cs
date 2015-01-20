using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IppModules.Analiz.Types
{
    /// <summary>
    /// ����� ��������� ����������� ������.
    /// </summary>
    [Serializable]
    struct Complex
    {
        #region .ctors

        public Complex(double re)
        {
            re_ = re;
            im_ = 0.0;
        }

        public Complex(double re, double im)
        {
            re_ = re;
            im_ = im;
        }

        public Complex(Complex x)
        {
            re_ = x.Re;
            im_ = x.Im;
        }

        public Complex(Polar x)
        {
            re_ = x.Radius * Math.Cos(x.Angle);
            im_ = x.Radius * Math.Sin(x.Angle);
        }

        #endregion

        #region ///// private fields /////

        /// <summary>
        /// �������� ����� ����������� �����.
        /// </summary>
        private double re_;
        /// <summary>
        /// ������ ����� ������������ �����.
        /// </summary>
        private double im_;

        /// <summary>
        /// ����� 0+j.
        /// </summary>
        private static Complex j_ = new Complex(0.0, 1.0);

        #endregion


        #region ///// public properties /////

        /// <summary>
        /// ���������� � ������������� �������������� ����� ������������ �����.
        /// </summary>
        public double Re
        {
            get { return re_; }
            set { re_ = value; }
        }

        /// <summary>
        /// ���������� � ������������� ������ ����� ������������ �����.
        /// </summary>
        public double Im
        {
            get { return im_; }
            set { im_ = value; }
        }

        /// <summary>
        /// ���������� ������ ������������ �����.
        /// </summary>
        public double Abs
        {
            get { return Math.Sqrt(im_ * im_ + re_ * re_); }
        }


        /// <summary>
        /// �������� ����� � ��������� [0; 2* pi)
        /// </summary>
        public double Arg
        {
            get
            {
                double res = Math.Atan2(im_, re_);
                /*if (res < 0)
            {
                res += Math.PI;
            }*/
                return res;
            }
        }


        /// <summary>
        /// ���������� ��������� 0 + j.
        /// </summary>
        public static Complex J
        {
            get
            {
                return j_;
            }
        }

        /// <summary>
        /// ���������� ����������-����������� �����.
        /// </summary>
        public Complex Conjugate
        {
            get
            {
                return new Complex(re_, -im_);
            }
        }

        /// <summary>
        /// ���������� ����� � �������� ����������� � ������������� ����� �� �������� �����������.
        /// </summary>
        public Polar Polar
        {
            get
            {
                return new Polar(this);
            }
            set
            {
                re_ = value.Radius * Math.Cos(value.Angle);
                im_ = value.Radius * Math.Sin(value.Angle);
            }
        }
        #endregion

        #region ///// public static methods /////

        /// <summary>
        /// ������������ ���������� ������.
        /// </summary>
        /// <param name="c">�����, �� �������� ������� ������</param>
        /// <returns></returns>
        public static Complex Sqrt(Complex c)
        {
            double abs = Math.Sqrt(c.Abs);
            return new Complex(abs * Math.Cos(c.Arg / 2), abs * Math.Sin(c.Arg / 2));
        }

        /// <summary>
        /// ������������ ���������� ������.
        /// </summary>
        /// <param name="x">�����, �� �������� ������� ������</param>
        /// <returns></returns>
        public static Complex Sqrt(double x)
        {
            // �� ����, ���� x < 0, �� ���-� ������ ���� +- a*i, �� ���-���� ���������� ������ � +.
            return x >= 0 ? new Complex(Math.Sqrt(x)) : new Complex(0, Math.Sqrt(-x));
        }

        /// <summary>
        /// ������������ ��� ����� ������� n (n ����).
        /// </summary>
        /// <param name="c">�����, �� �������� ��������� ������.</param>
        /// <param name="n">������� �����.</param>
        /// <returns>������ � ������������.</returns>
        public static Complex[] Radical(Complex c, int n)
        {
            Complex[] res = new Complex[n];

            double abs = Math.Pow(c.Abs, 1.0 / n);

            // ��������� ����
            double Phi0 = c.Arg / n;

            // ��� �� ����
            double dPhi = 2 * Math.PI / n;

            for (int i = 0; i < n; ++i)
            {
                double CurrPhi = Phi0 + i * dPhi;
                res[i] = new Complex(abs * Math.Cos(CurrPhi), abs * Math.Sin(CurrPhi));
            }

            return res;
        }

        /// <summary>
        /// ������������ ���������� ������������ �����.
        /// </summary>
        /// <param name="c">����� �� �������� ���� ����������.</param>
        public static Complex Exp(Complex c)
        {
            return new Complex(new Polar(Math.Exp(c.re_), c.im_));
        }

        /// <summary>
        /// �������� ����������� ����� � �������.
        /// </summary>
        /// <param name="c">�����.</param>
        /// <param name="n">������� � ������� �������� �����.</param>
        /// <returns></returns>
        public static Complex Pow(Complex c, int n)
        {
            double NewArg = c.Arg * n;
            double NewAbs = 1;

            int absn = Math.Abs(n);
            double cabs = c.Abs;

            for (int i = 0; i < absn; ++i)
            {
                NewAbs *= cabs;
            }

            return n > 0 ? new Complex(new Polar(NewAbs, NewArg)) : new Complex(new Polar(1 / NewAbs, NewArg));
        }

        /// <summary>
        /// ������� ���������� ���� Complex �� ������
        /// </summary>
        /// <param name="str">������, �� ������� ������� ����������</param>
        /// <remarks>
        ///	 ��������� �������� �����:
        ///	 1+5i
        ///	 1 + 5i
        ///	 +1 + 5i
        ///	 -1 - 5i
        ///	 -5
        ///	 -6i
        ///	 +6i
        ///	 i
        ///	 1+i
        ///	 -i
        /// </remarks>
        /// <returns>���������� ���������� ��� ������� ���������� � ������ �������</returns>
        public static Complex Parse(string str)
        {
            /*Regex r = new Regex(@"^\s*(?<re>[\+-]?\d+([,.]\d+)?([eE][\+-]?\d+)?)?\s*((?<im>([\+-]\d+([,.]\d+)?([eE]?[\+-]?\d*)?)?)[ij])?\s*$");
        Match match = r.Match(str);


        string re = match.Groups["re"].Value;
        string im = match.Groups["im"].Value;

        if (re.Length == 0)
        {
            re = "0";
        }

        if (im.Length == 0)
        {
            im = "0";
        }*/

            Complex res = 0;

            // ������ ������ ����� ������ � ������ ��������
            Regex regim = new Regex(@"(?<im>[\+-]?\d*(?:[,.]\d+)?(?:[eE][\+-]?\d+)?[ij])");
            Match match = regim.Match(str);

            string im = match.Groups["im"].Value;
            if (im.Length == 0)
            {
                im = "0";
            }
            else if (im[im.Length - 1] == 'i' || im[im.Length - 1] == 'j')
            {
                // ������ ������ �������
                im = im.Substring(0, im.Length - 1);

                if (im == "+" || im.Length == 0)
                {
                    im = "1";
                }
                else if (im == "-")
                {
                    im = "-1";
                }
            }
            else
            {
                throw new FormatException(str);
            }

            string re = regim.Replace(str, "");
            if (re.Length == 0)
            {
                re = "0";
            }

            res = new Complex(Double.Parse(re), Double.Parse(im));

            return res;
        }
        /*public static Complex Parse(string str)
    {
        // � �������� i ����� ���� �������� j � ����� ���� ����� ����� ��������

        // 1. ������� ��� �������
        string TempStr = str.Replace(" ", "");

        // 2. �������� j �� i
        TempStr = TempStr.Replace("j", "i");

        // 3. ������� ������ �����
        double imag = 0;

        // ���� ��������� ������ - i, ������ ������ ����� ����
        int pos = TempStr.Length - 1;
        if (TempStr[TempStr.Length - 1] == 'i')
        {
            string ImagStr = "";

            for (pos--; pos >= 0; --pos)
            {
                char CurrChar = TempStr[pos];

                // ���� ���� �����, �� ������ ��������� ������ � ������
                if (CurrChar >= '0' && CurrChar <= '9' || 
                    CurrChar.ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                {
                    ImagStr = CurrChar + ImagStr;
                }
                else if (CurrChar == '+' || CurrChar == '-')
                {
                    // ���� ����, �� ��������� ��� � �������� ����
                    ImagStr = CurrChar + ImagStr;

                    // ���� ������ ������ ������� �� ������ �����, �� ������� 1
                    if (ImagStr.Length == 1)
                    {
                        ImagStr += "1";
                    }
                    break;
                }
                else
                {
                    // ���� ���-�� ������, ������ ������
                    throw new FormatException (str);
                }
            }

            if (ImagStr.Length == 0)
            {
                imag = 1.0;
            }
            else
            {
                imag = Convert.ToDouble(ImagStr);
            }
            pos--;
        }

        // 4. �������� �������� �����
        double real = 0;
        // ���� ��� ����� ���� �������� �����
        if (pos >= 0)
        {

            // ���� ������ ���� �����
            if (TempStr[pos] >= '0' && TempStr[pos] <= '9')
            {
                string RealStr = TempStr[pos].ToString();

                for (pos--; pos >= 0; --pos)
                {
                    char CurrChar = TempStr[pos];

                    // ���� ���� �����, �� ������ ��������� ������ � ������
                    if (CurrChar >= '0' && CurrChar <= '9' || 
                        CurrChar.ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    {
                        RealStr = CurrChar + RealStr;
                    }
                    else if (CurrChar == '+' || CurrChar == '-')
                    {
                        // ���� ����, �� ��������� ��� � �������� ����
                        RealStr = CurrChar + RealStr;
                        break;
                    }
                    else
                    {
                        // ���� ���-�� ������, ������ ������
                        throw new FormatException (str);
                    }
                }

                real = Convert.ToDouble(RealStr);
            }
            else
            {
                throw new FormatException (str);
            }
        }

        return new Complex(real, imag);
    }*/

        #endregion


        #region ������������� ��������� ��������

        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.re_ + c2.re_, c1.im_ + c2.im_);
        }

        public static Complex operator +(Complex c1, double c2)
        {
            return new Complex(c1.re_ + c2, c1.im_);
        }

        public static Complex operator +(double c1, Complex c2)
        {
            return new Complex(c1 + c2.re_, c2.im_);
        }

        #endregion

        #region ������������� ��������� ���������
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.re_ - c2.re_, c1.im_ - c2.im_);
        }

        public static Complex operator -(Complex c1, double c2)
        {
            return new Complex(c1.re_ - c2, c1.im_);
        }

        public static Complex operator -(double c1, Complex c2)
        {
            return new Complex(c1 - c2.re_, -c2.im_);
        }
        #endregion

        #region ������������� ��������� ���������
        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.re_ * c2.re_ - c1.im_ * c2.im_,
                c1.re_ * c2.im_ + c1.im_ * c2.re_);
        }

        public static Complex operator *(Complex c1, double c2)
        {
            return new Complex(c1.re_ * c2, c1.im_ * c2);
        }

        public static Complex operator *(double c1, Complex c2)
        {
            return new Complex(c1 * c2.re_, c1 * c2.im_);
        }
        #endregion

        #region ������������� ��������� �������
        public static Complex operator /(Complex c1, Complex c2)
        {
            double Denominator = c2.re_ * c2.re_ + c2.im_ * c2.im_;
            return new Complex((c1.re_ * c2.re_ + c1.im_ * c2.im_) / Denominator,
                (c2.re_ * c1.im_ - c2.im_ * c1.re_) / Denominator);
        }

        public static Complex operator /(Complex c1, double c2)
        {
            return new Complex(c1.re_ / c2, c1.im_ / c2);
        }

        public static Complex operator /(double c1, Complex c2)
        {
            double Denominator = c2.re_ * c2.re_ + c2.im_ * c2.im_;
            return new Complex((c1 * c2.re_) / Denominator, (-c2.im_ * c1) / Denominator);
        }
        #endregion

        #region ��������� ���������
        public static bool operator ==(Complex c1, Complex c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator ==(Complex c, double x)
        {
            return c.Im == 0 && c.Re == x;
        }

        public static bool operator ==(double d, Complex c)
        {
            return c == d;
        }

        public static bool operator !=(Complex c1, Complex c2)
        {
            return !c1.Equals(c2);
        }

        public static bool operator !=(Complex c, double x)
        {
            return !(c == x);
        }

        public static bool operator !=(double x, Complex c)
        {
            return !(c == x);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Complex))
                throw new ArgumentException("obj must be Complex type.");

            if (obj == null)
                return false;

            Complex cn = (Complex)obj;

            return (this.Re == cn.Re && this.Im == cn.Im);
        }

        #endregion


        public override int GetHashCode()
        {
            return re_.GetHashCode() + im_.GetHashCode();
        }

        public override string ToString()
        {
            string res = re_.ToString();

            if (im_ != 0.0)
            {
                if (im_ > 0)
                    res += "+";

                res += im_.ToString() + "i";
            }

            return res;
        }

        public static implicit operator Complex(double x)
        {
            return new Complex(x);
        }

    }
}
