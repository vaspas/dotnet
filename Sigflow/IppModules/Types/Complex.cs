using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IppModules.Analiz.Types
{
    /// <summary>
    /// Класс реализует комплексное чисело.
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
        /// Реальная часть комплекного числа.
        /// </summary>
        private double re_;
        /// <summary>
        /// Мнимая часть комплексного числа.
        /// </summary>
        private double im_;

        /// <summary>
        /// Число 0+j.
        /// </summary>
        private static Complex j_ = new Complex(0.0, 1.0);

        #endregion


        #region ///// public properties /////

        /// <summary>
        /// Возвращает и устанавливает действительную часть комплексного числа.
        /// </summary>
        public double Re
        {
            get { return re_; }
            set { re_ = value; }
        }

        /// <summary>
        /// Возвращает и устанавливает мнимую часть комплексного числа.
        /// </summary>
        public double Im
        {
            get { return im_; }
            set { im_ = value; }
        }

        /// <summary>
        /// Возвращает модуль комплексного числа.
        /// </summary>
        public double Abs
        {
            get { return Math.Sqrt(im_ * im_ + re_ * re_); }
        }


        /// <summary>
        /// Аргумент числа в интервале [0; 2* pi)
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
        /// Возвращает константу 0 + j.
        /// </summary>
        public static Complex J
        {
            get
            {
                return j_;
            }
        }

        /// <summary>
        /// Возвращает комплексно-сопряженное число.
        /// </summary>
        public Complex Conjugate
        {
            get
            {
                return new Complex(re_, -im_);
            }
        }

        /// <summary>
        /// Возвращает число в полярных координатах и устанавливает число по полярным координатам.
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
        /// Рассчитывает квадратный корень.
        /// </summary>
        /// <param name="c">Число, из которого считают корень</param>
        /// <returns></returns>
        public static Complex Sqrt(Complex c)
        {
            double abs = Math.Sqrt(c.Abs);
            return new Complex(abs * Math.Cos(c.Arg / 2), abs * Math.Sin(c.Arg / 2));
        }

        /// <summary>
        /// Рассчитывает квадратный корень.
        /// </summary>
        /// <param name="x">Число, из которого считают корень</param>
        /// <returns></returns>
        public static Complex Sqrt(double x)
        {
            // По идее, если x < 0, то рез-т должен быть +- a*i, но все-таки возвращаем только с +.
            return x >= 0 ? new Complex(Math.Sqrt(x)) : new Complex(0, Math.Sqrt(-x));
        }

        /// <summary>
        /// Рассчитывает все корни степени n (n штук).
        /// </summary>
        /// <param name="c">Число, из которого извлекаем корень.</param>
        /// <param name="n">Степень корня.</param>
        /// <returns>Массив с результатами.</returns>
        public static Complex[] Radical(Complex c, int n)
        {
            Complex[] res = new Complex[n];

            double abs = Math.Pow(c.Abs, 1.0 / n);

            // Начальный угол
            double Phi0 = c.Arg / n;

            // Шаг по углу
            double dPhi = 2 * Math.PI / n;

            for (int i = 0; i < n; ++i)
            {
                double CurrPhi = Phi0 + i * dPhi;
                res[i] = new Complex(abs * Math.Cos(CurrPhi), abs * Math.Sin(CurrPhi));
            }

            return res;
        }

        /// <summary>
        /// Рассчитывает экспоненту комплексного числа.
        /// </summary>
        /// <param name="c">Число от которого ищут экспоненту.</param>
        public static Complex Exp(Complex c)
        {
            return new Complex(new Polar(Math.Exp(c.re_), c.im_));
        }

        /// <summary>
        /// Возводит комплексное число в степень.
        /// </summary>
        /// <param name="c">Число.</param>
        /// <param name="n">Степень в которую возводим число.</param>
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
        /// Создает переменную типа Complex из строки
        /// </summary>
        /// <param name="str">Строка, по которой создаем переменную</param>
        /// <remarks>
        ///	 Возможные варианты строк:
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
        /// <returns>Полученную переменную или бросает исключение в случае неудачи</returns>
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

            // Найдем мнимую часть вместе с мниной единицей
            Regex regim = new Regex(@"(?<im>[\+-]?\d*(?:[,.]\d+)?(?:[eE][\+-]?\d+)?[ij])");
            Match match = regim.Match(str);

            string im = match.Groups["im"].Value;
            if (im.Length == 0)
            {
                im = "0";
            }
            else if (im[im.Length - 1] == 'i' || im[im.Length - 1] == 'j')
            {
                // Уберем мнимую единицу
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
        // В качестве i может быть написано j и может быть любое число пробелов

        // 1. Удаляем все пробелы
        string TempStr = str.Replace(" ", "");

        // 2. Заменяем j на i
        TempStr = TempStr.Replace("j", "i");

        // 3. Находим мнимую часть
        double imag = 0;

        // Если последний символ - i, значит мнимая часть есть
        int pos = TempStr.Length - 1;
        if (TempStr[TempStr.Length - 1] == 'i')
        {
            string ImagStr = "";

            for (pos--; pos >= 0; --pos)
            {
                char CurrChar = TempStr[pos];

                // Если идет число, то просто добавляем символ в начало
                if (CurrChar >= '0' && CurrChar <= '9' || 
                    CurrChar.ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                {
                    ImagStr = CurrChar + ImagStr;
                }
                else if (CurrChar == '+' || CurrChar == '-')
                {
                    // Если знак, то добавляем его и обрываем цикл
                    ImagStr = CurrChar + ImagStr;

                    // Если строка теперь состоит из одного знака, то добавим 1
                    if (ImagStr.Length == 1)
                    {
                        ImagStr += "1";
                    }
                    break;
                }
                else
                {
                    // Если что-то другое, значит ошибка
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

        // 4. Получаем реальную часть
        double real = 0;
        // Если еще может быть реальная часть
        if (pos >= 0)
        {

            // Если дальше идет число
            if (TempStr[pos] >= '0' && TempStr[pos] <= '9')
            {
                string RealStr = TempStr[pos].ToString();

                for (pos--; pos >= 0; --pos)
                {
                    char CurrChar = TempStr[pos];

                    // Если идет число, то просто добавляем символ в начало
                    if (CurrChar >= '0' && CurrChar <= '9' || 
                        CurrChar.ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    {
                        RealStr = CurrChar + RealStr;
                    }
                    else if (CurrChar == '+' || CurrChar == '-')
                    {
                        // Если знак, то добавляем его и обрываем цикл
                        RealStr = CurrChar + RealStr;
                        break;
                    }
                    else
                    {
                        // Если что-то другое, значит ошибка
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


        #region Перегруженные операторы сложения

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

        #region Перегруженные операторы вычитания
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

        #region Перегруженные операторы умножения
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

        #region Перегруженные операторы деления
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

        #region Операторы сравнения
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
