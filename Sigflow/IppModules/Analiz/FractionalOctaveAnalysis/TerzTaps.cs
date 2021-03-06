#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using IppModules.Analiz.Types;

#endregion

namespace IppModules.Analiz.FractionalOctaveAnalysis
{
    public static class TerzTaps
    {
        static double arsh(double z)
        {
            return Math.Log(z + Math.Sqrt(z * z + 1));
        }

        unsafe static double zf(float* f, Complex w)
        {
            Complex c1, c2;
            c1 = (w * w * *f + w * *(f + 1) + *(f + 2));
            c2 = (w * w * *(f + 3) + w * *(f + 4) + *(f + 5));
            double a1 = c1.Abs;
            double a2 = c2.Abs;
            double r = a1 / a2;
            return r;
        }

        unsafe static double tf(float* f, int nzv, Complex w)
        {
            double y = 1;
            for (int i = 0; i < nzv; i++)
                y *= zf(f + 6 * i, w);
            return y < (double)float.Epsilon ? 0 : y;
        }

        unsafe static double tfx(float* f, int nzv, float x)
        {
            var w = Complex.Exp(new Complex(0, x*Math.PI*2));
            return tf(f, nzv, w);
        }

        unsafe static float tfdb(float* f, int nzv, Complex w)
        {
            double x = tf(f, nzv, w);
            if (x < (double)float.Epsilon) x = float.Epsilon;
            return (float)(20 * Math.Log10(x));
        }

        unsafe static float ener_band(float* f, int nzv)
        {
            Complex w;
            double q, band = 0;

            for (float x = 0; x < 0.5f; x += 0.001f)
            {
                w = Complex.Exp(new Complex(0, x * Math.PI * 2));
                q = tf(f, nzv, w);
                band += q * q * 0.001;
            }
            return (float)band;
        }

        unsafe static void find_3dB_bands(float* f, int nzv, float* f1, float* f2)
        {// исходные *f1 *f2 - границы плоской части
            // при выходе *f1 *f2 - границы по уровню -3дБ
            float df = 0.0001f, lev = 0.63f; // -4дБ
            double y;
            do { *f1 -= df; y = tfx(f, nzv, *f1); } while ((y > lev) && (*f1 > 0));
            *f1 += df;
            do { *f2 += df; y = tfx(f, nzv, *f2); } while ((y > lev) && (*f2 < 0.5));
            *f2 -= df;
        }

        /// <summary>
        /// Расчет коэффициентов b0,b1,b2,1,a1,a2 ячеек 2-го порядка
        /// полосового БИХ-фильтра Чебышева.
        /// </summary>
        /// <param name="taps">Результаты. Кол-во значений 6*2*(m-(int)m/2)</param>
        /// <param name="m">Число звеньев.</param>
        /// <param name="ripple">Пульсации в полосе пропускания (дБ).</param>
        /// <param name="band1">Нижняя граничная частота, значение от 0 до 0.5, частота квантования как 1.</param>
        /// <param name="band2">Верхняя граничная частота, значение от 0 до 0.5, частота квантования как 1.</param>
        public unsafe static void bp_cheb(float* taps, int m, float ripple, float band1, float band2)
        {
            float a1, a2, b0, b1, b2, k;

            int m1;

            double al, fact, ang, sector, wedge, am, bm, g;
            Complex qm, sqm, z;
            Complex[] zm = new Complex[2];//  complex<double> qm,zm[2],sqm,z;

            m1 = m - m / 2;
            g = (double)1 / Math.Tan(Math.PI * (band2 - band1));
            al = Math.Cos(Math.PI * (band2 + band1)) / Math.Cos(Math.PI * (band2 - band1));
            z = Complex.Exp(new Complex(0, Math.PI * (band2 + band1)));
            sector = Math.PI / m;
            wedge = sector / 2;
            for (int i = 0; i < m1; i++)
            {
                fact = (double)1 / m * arsh(1 / Math.Sqrt(Math.Pow(10, ripple / 10) - 1));
                ang = sector * i + wedge;
                am = -Math.Sinh(fact) * Math.Sin(ang);
                bm = Math.Cosh(fact) * Math.Cos(ang);
                qm = new Complex(am, bm) / g;
                sqm = Complex.Sqrt(al * al - 1 + qm * qm);
                zm[0] = (al + sqm) / (1 - qm);
                zm[1] = (al - sqm) / (1 - qm);

                for (int j = 0; j < 2; j++)
                {
                    a1 = -(float)((Complex)(zm[j] + zm[j].Conjugate)).Re;
                    a2 = (float)((Complex)(zm[j] * zm[j].Conjugate)).Re;
                    b0 = 1; b1 = 0; b2 = -1;

                    k = (float)((Complex)((1 + a1 * z + a2 * z * z) / (b0 + b1 * z + b2 * z * z))).Abs;
                    *taps++ = k * b0;
                    *taps++ = k * b1;
                    *taps++ = k * b2;
                    *taps++ = 1;
                    *taps++ = a1;
                    *taps++ = a2;
                    /*taps[tapsOffset + i * m1 * 6 + j * 6 + 0] = k * b0;
                        taps[tapsOffset + i * m1 * 6 + j * 6 + 1] = k * b1;
                        taps[tapsOffset + i * m1 * 6 + j * 6 + 2] = k * b2;
                        taps[tapsOffset + i * m1 * 6 + j * 6 + 3] = 1;
                        taps[tapsOffset + i * m1 * 6 + j * 6 + 4] = a1;
                        taps[tapsOffset + i * m1 * 6 + j * 6 + 5] = a2;*/
                    if (i >= m / 2) j++;
                    //*taps++=k*b0; *taps++=k*b1; *taps++=k*b2;
                    //*taps++=1; *taps++=a1; *taps++=a2;
                    //   printf("%g\t%g\t%g\t%g\t%g\t%g\n",k,-a1,-a2,b0,b1,b2);
                }

            }
        }

        

        /// <summary>
        /// Расчет коэффициентов b0,b1,b2,1,a1,a2 ячеек 2-го порядка
        /// для 1/3-октавных (или октавного) фильтров
        /// Выдает коэффициент уменьшения полосы, выбираемый
        /// автоматически для коррекции шумовой полосы пропускания
        /// </summary>
        /// <param name="taps">Результаты</param>
        /// <param name="n_Oct">число октав</param>
        /// <param name="nf_per_oct">число фильтров 12, 3 или 1</param>
        /// <param name="nzv">число звеньев фильтра</param>
        /// <param name="ripple">пульсации в полосе пропускания (дБ)</param>
        /// <param name="f">средняя частота первого фильтра</param>
        /// <param name="qx">относительный сдвиг частоты</param>
        /// <returns></returns>
        public unsafe static float CalcTerzOctTaps(ref float[] taps, int n_Oct, int nf_per_oct, int nzv,
            float ripple, double f, double qx)
        {
            var df1 = new float[100];
            var df2 = new float[100];

            bool fini = false;
            // Задать ширину полосы и шаг
            double kf = 1,
                   dkf = -0.001,
                   bami,
                   bama,
                   bam,
                   f1n = 1/Math.Sqrt(qx),
                   f2n = Math.Sqrt(qx),
                   bnom = f2n - f1n;
            fixed (float* ptaps = taps)
            {
                do
                {
// Рассчитать коэффициенты и шумовые полосы
                    bam = 0;
                    bami = 10;
                    bama = 0;
                    for (int j = 0; j < nf_per_oct; j++)
                    {
                        float f0 = (float) (f*Math.Pow(qx, j));
                        float f1 = (float) (f0*f1n/kf);
                        float f2 = (float) (f0*f2n*kf);
                        int tapsOffset = 6*nzv*(j + nf_per_oct*(n_Oct - 1));
                        double ba;
                        // рассчитать фильтр с границами по уровню -ripple
                        bp_cheb(ptaps + tapsOffset, nzv, ripple, f1, f2);
                        // найти границы по уровню -3дБ
                        // сдвинуть границы и пересчитать фильтр
                        float f1_3dB = f1, f2_3dB = f2;
                        find_3dB_bands(ptaps + tapsOffset, nzv, &f1_3dB, &f2_3dB);
                        df1[j] = f1_3dB - f1;
                        df2[j] = f2_3dB - f2;
                        // еще раз
                        bp_cheb(ptaps + tapsOffset, nzv, ripple, f1 - df1[j], f2 - df2[j]);
                        f1_3dB = f1 - df1[j];
                        f2_3dB = f2 - df2[j];
                        find_3dB_bands(ptaps + tapsOffset, nzv, &f1_3dB, &f2_3dB);
                        df1[j] += f1_3dB - f1;
                        df2[j] += f2_3dB - f2;

                        bp_cheb(ptaps + tapsOffset, nzv, ripple, f1 - df1[j], f2 - df2[j]);
                        bam += ba = ener_band(ptaps + tapsOffset, nzv)/f0/bnom;
                        bami = Math.Min(bami, ba);
                        bama = Math.Max(bama, ba);
                    }
                    bam /= nf_per_oct; // средняя по фильтрам октавы полоса
                    // Проверить шумовую полосу cкорректировать ширину полосы и шаг
                    //if (bam > 1) kf += dkf;
                    //else 
                    fini = true;
                } while (!fini);
                // FILE* fp=fopen("ТА_сетка.txt","w");
                // fprintf(fp,"qx=%lg bami=%lg bam=%lg bama=%lg\n",qx,bami,bam,bama);
                for (int i = 0; i < n_Oct - 1; i++)
                    for (int j = 0; j < nf_per_oct; j++)
                    {
                        float f0 =
                            (float) (f*Math.Pow(qx, j + nf_per_oct*(i - n_Oct + 1))*(float) Math.Pow(2, n_Oct - 1 - i));
                        float f1 = (float) (f0*f1n/kf) - df1[j];
                        float f2 = (float) (f0*f2n*kf) - df2[j];
                        int tapsOffset = 6*nzv*(j + nf_per_oct*i);
                        bp_cheb(ptaps + tapsOffset, nzv, ripple, f1, f2);
                        //   fprintf(fp,"%d %d %g\n",i,j,f0);
                    }
            }
            // fclose(fp);

/*            System.Diagnostics.Trace.WriteLine(string.Format("qx={0} bami={1} bam={2} bama={3}", qx, bami, bam, bama));
            System.Diagnostics.Trace.WriteLine("Окт фильтр частота коэффициенты");
            for (int i = 0; i < n_Oct; i++)
                for (int j = 0; j < nf_per_oct; j++)
                {
                    float f0 =
                            (float)(f * Math.Pow(qx, j + nf_per_oct * (i - n_Oct + 1)) * (float)Math.Pow(2, n_Oct - 1 - i));
                    System.Diagnostics.Trace.WriteLine(string.Format("{0} {1} {2}", i, j, f0));
                    for (int k = 0; k < nzv; k++)
                    {
                        
                        fixed (float* ptaps = taps)
                        {
                            var f_taps = ptaps + 6 * (k + nzv * (j + nf_per_oct * i));
                            System.Diagnostics.Trace.WriteLine(string.Format("{0} {1} {2} {3} {4} {5}", f_taps[0], f_taps[1],
                                                                         f_taps[2],
                                                                         f_taps[3], f_taps[4], f_taps[5]));
                        }
                    }
                }*/

            return (float) (bama - bami);
        }

    }
}
