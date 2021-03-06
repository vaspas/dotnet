#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace IppModules.Analiz.FractionalOctaveAnalysis
{
    abstract class DecFir
    {
        public struct DecimationFir
        {
            public DecimationFir(int len,int down_factor,float[] taps)
            {
                Len = len;
                DownFactor = down_factor;
                Taps = taps;
            }

            public int Len;
            public int DownFactor;
            public float[] Taps;
        }

        /* Этот фильтр рассчитан как первая ступень для децимации в 10 раз!!!
******************************************************************************
                      SPECIFICATIONS OF SESSION LP1 (FIR)
******************************************************************************

                             1  digitalfilter type FIR or IIR
                            33  filter length
                             5  filter type: lowpass
                             2  filter bands

                         51.20  sample frequency in [kHz]
                         25.60  maximum signal frequency in [kHz]

     ====================================================================
                                  BAND 1
     ====================================================================

                          0.00  lower band edge in [kHz]
                          2.00  upper band edge in [kHz]
                       1.00000  gain
                       0.00100  ripple
                       0.00868  ripple in [dB]

     ====================================================================
                                  BAND 2
     ====================================================================

                          8.24  lower band edge in [kHz]
                         25.60  upper band edge in [kHz]
                       0.00000  gain
                       0.00010  ripple
                     -80.00000  ripple in [dB]


******************************************************************************
    Этот фильтр рассчитан как первая ступень для децимации в 10 раз!!!
*/
        public static DecimationFir Decimate5 =new DecimationFir(33,5,
 new float[]{
/******************************************************************************
                    IMPULSE RESPONSE OF SESSION LP1 (FIR)
******************************************************************************/

              3.1441115910E-04f  , // h[1]
              1.0574006984E-03f  , // h[2]
              2.2485469780E-03f  , // h[3]
              3.3815508680E-03f  , // h[4]
              3.3786440663E-03f  , // h[5]
              9.0968980875E-04f  , // h[6]
             -4.8108851768E-03f  , // h[7]
             -1.3011411695E-02f  , // h[8]
             -2.0680774192E-02f  , // h[9]
             -2.2814705495E-02f  , // h[10]
             -1.3870092831E-02f  , // h[11]
              9.8965878308E-03f  , // h[12]
              4.8127045868E-02f  , // h[13]
              9.5252448237E-02f  , // h[14]
              1.4138236901E-01f  , // h[15]
              1.7515438354E-01f  , // h[16]
              1.8755391562E-01f  , // h[17]
              1.7515438354E-01f  , // h[18]
              1.4138236901E-01f  , // h[19]
              9.5252448237E-02f  , // h[20]
              4.8127045868E-02f  , // h[21]
              9.8965878308E-03f  , // h[22]
             -1.3870092831E-02f  , // h[23]
             -2.2814705495E-02f  , // h[24]
             -2.0680774192E-02f  , // h[25]
             -1.3011411695E-02f  , // h[26]
             -4.8108851768E-03f  , // h[27]
              9.0968980875E-04f  , // h[28]
              3.3786440663E-03f  , // h[29]
              3.3815508680E-03f  , // h[30]
              2.2485469780E-03f  , // h[31]
              1.0574006984E-03f  , // h[32]
              3.1441115910E-04f   // h[33]
 }
);

        /*
******************************************************************************
                      SPECIFICATIONS OF SESSION LP2 (FIR)
******************************************************************************

                             1  digitalfilter type FIR or IIR
                            37  filter length
                             5  filter type: lowpass
                             2  filter bands

                         10.24  sample frequency in [kHz]
                          5.12  maximum signal frequency in [kHz]

     ====================================================================
                                  BAND 1
     ====================================================================

                          0.00  lower band edge in [kHz]
                          2.00  upper band edge in [kHz]
                       1.00000  gain
                       0.00100  ripple
                       0.00868  ripple in [dB]

     ====================================================================
                                  BAND 2
     ====================================================================

                          3.12  lower band edge in [kHz]
                          5.12  upper band edge in [kHz]
                       0.00000  gain
                       0.00010  ripple
                     -80.00000  ripple in [dB]


******************************************************************************
*/
        public static DecimationFir Decimate2 = new DecimationFir(37, 2,
         new float[]{
/******************************************************************************
                    IMPULSE RESPONSE OF SESSION LP2 (FIR)
******************************************************************************/

              5.1365624092E-04f  , // h[1]
              6.1955929809E-04f  , // h[2]
             -1.2035431595E-03f  , // h[3]
             -2.0961513658E-03f  , // h[4]
              2.0538916807E-03f  , // h[5]
              4.6543639345E-03f  , // h[6]
             -3.4505833989E-03f  , // h[7]
             -9.4187450298E-03f  , // h[8]
              4.9039062506E-03f  , // h[9]
              1.7023029261E-02f  , // h[10]
             -6.6248752271E-03f  , // h[11]
             -2.9600774662E-02f  , // h[12]
              8.1456121255E-03f  , // h[13]
              5.1279112365E-02f  , // h[14]
             -9.5148858160E-03f  , // h[15]
             -9.8280948200E-02f  , // h[16]
              1.0338537708E-02f  , // h[17]
              3.1559565737E-01f  , // h[18]
              4.8930968863E-01f  , // h[19]
              3.1559565737E-01f  , // h[20]
              1.0338537708E-02f  , // h[21]
             -9.8280948200E-02f  , // h[22]
             -9.5148858160E-03f  , // h[23]
              5.1279112365E-02f  , // h[24]
              8.1456121255E-03f  , // h[25]
             -2.9600774662E-02f  , // h[26]
             -6.6248752271E-03f  , // h[27]
              1.7023029261E-02f  , // h[28]
              4.9039062506E-03f  , // h[29]
             -9.4187450298E-03f  , // h[30]
             -3.4505833989E-03f  , // h[31]
              4.6543639345E-03f  , // h[32]
              2.0538916807E-03f  , // h[33]
             -2.0961513658E-03f  , // h[34]
             -1.2035431595E-03f  , // h[35]
              6.1955929809E-04f  , // h[36]
              5.1365624092E-04f    // h[37]
 }
);
//--------------------------Настоящий для децимации 5 ------------
/*****************************************************************************
                      SPECIFICATIONS OF SESSION LP0 (FIR)
******************************************************************************

                             1  digitalfilter type FIR or IIR
                           120  filter length
                             5  filter type: lowpass
                             2  filter bands

                         51.20  sample frequency in [kHz]
                         25.60  maximum signal frequency in [kHz]

     ====================================================================
                                  BAND 1
     ====================================================================

                          0.00  lower band edge in [kHz]
                          4.00  upper band edge in [kHz]
                       1.00000  gain
                       0.00100  ripple
                       0.00868  ripple in [dB]

     ====================================================================
                                  BAND 2
     ====================================================================

                          6.24  lower band edge in [kHz]
                         25.60  upper band edge in [kHz]
                       0.00000  gain
                       0.00001  ripple
                    -100.00000  ripple in [dB]
*/
        public static DecimationFir Decimate5a = new DecimationFir(120, 5,
        new float[]{
/*****************************************************************************
                    IMPULSE RESPONSE OF SESSION LP0 (FIR)
******************************************************************************/

              9.0478464730E-06f  , // h[1]
              2.5403397008E-05f  , // h[2]
              5.1548250816E-05f  , // h[3]
              8.1752597223E-05f  , // h[4]
              1.0373162570E-04f  , // h[5]
              1.0046493807E-04f  , // h[6]
              5.6525094615E-05f  , // h[7]
             -3.2730282072E-05f  , // h[8]
             -1.5267257414E-04f  , // h[9]
             -2.6654821208E-04f  , // h[10]
             -3.2247995808E-04f  , // h[11]
             -2.7121988053E-04f  , // h[12]
             -9.0463969054E-05f  , // h[13]
              1.9359229404E-04f  , // h[14]
              4.9893013906E-04f  , // h[15]
              7.0364124471E-04f  , // h[16]
              6.8600556848E-04f  , // h[17]
              3.7861244177E-04f  , // h[18]
             -1.8371041170E-04f  , // h[19]
             -8.4604622296E-04f  , // h[20]
             -1.3631298205E-03f  , // h[21]
             -1.4779037718E-03f  , // h[22]
             -1.0283435524E-03f  , // h[23]
             -4.3513491022E-05f  , // h[24]
              1.2161808172E-03f  , // h[25]
              2.3070860137E-03f  , // h[26]
              2.7410937579E-03f  , // h[27]
              2.1785667894E-03f  , // h[28]
              6.0646531397E-04f  , // h[29]
             -1.5775487764E-03f  , // h[30]
             -3.6327209167E-03f  , // h[31]
             -4.7010611914E-03f  , // h[32]
             -4.1326956030E-03f  , // h[33]
             -1.7973840396E-03f  , // h[34]
              1.7495919242E-03f  , // h[35]
              5.3462419535E-03f  , // h[36]
              7.5659034201E-03f  , // h[37]
              7.2369185952E-03f  , // h[38]
              3.9651555486E-03f  , // h[39]
             -1.5504637387E-03f  , // h[40]
             -7.5760621384E-03f  , // h[41]
             -1.1823251332E-02f  , // h[42]
             -1.2242364681E-02f  , // h[43]
             -7.8762349570E-03f  , // h[44]
              5.4620358327E-04f  , // h[45]
              1.0553072108E-02f  , // h[46]
              1.8529077125E-02f  , // h[47]
              2.0864363457E-02f  , // h[48]
              1.5324935350E-02f  , // h[49]
              2.1852105318E-03f  , // h[50]
             -1.5319378003E-02f  , // h[51]
             -3.1483624662E-02f  , // h[52]
             -3.9538231762E-02f  , // h[53]
             -3.3624228722E-02f  , // h[54]
             -1.0760117866E-02f  , // h[55]
              2.7830829832E-02f  , // h[56]
              7.6515024881E-02f  , // h[57]
              1.2632510546E-01f  , // h[58]
              1.6708410911E-01f  , // h[59]
              1.9000251749E-01f  , // h[60]
              1.9000251749E-01f  , // h[61]
              1.6708410911E-01f  , // h[62]
              1.2632510546E-01f  , // h[63]
              7.6515024881E-02f  , // h[64]
              2.7830829832E-02f  , // h[65]
             -1.0760117866E-02f  , // h[66]
             -3.3624228722E-02f  , // h[67]
             -3.9538231762E-02f  , // h[68]
             -3.1483624662E-02f  , // h[69]
             -1.5319378003E-02f  , // h[70]
              2.1852105318E-03f  , // h[71]
              1.5324935350E-02f  , // h[72]
              2.0864363457E-02f  , // h[73]
              1.8529077125E-02f  , // h[74]
              1.0553072108E-02f  , // h[75]
              5.4620358327E-04f  , // h[76]
             -7.8762349570E-03f  , // h[77]
             -1.2242364681E-02f  , // h[78]
             -1.1823251332E-02f  , // h[79]
             -7.5760621384E-03f  , // h[80]
             -1.5504637387E-03f  , // h[81]
              3.9651555486E-03f  , // h[82]
              7.2369185952E-03f  , // h[83]
              7.5659034201E-03f  , // h[84]
              5.3462419535E-03f  , // h[85]
              1.7495919242E-03f  , // h[86]
             -1.7973840396E-03f  , // h[87]
             -4.1326956030E-03f  , // h[88]
             -4.7010611914E-03f  , // h[89]
             -3.6327209167E-03f  , // h[90]
             -1.5775487764E-03f  , // h[91]
              6.0646531397E-04f  , // h[92]
              2.1785667894E-03f  , // h[93]
              2.7410937579E-03f  , // h[94]
              2.3070860137E-03f  , // h[95]
              1.2161808172E-03f  , // h[96]
             -4.3513491022E-05f  , // h[97]
             -1.0283435524E-03f  , // h[98]
             -1.4779037718E-03f  , // h[99]
             -1.3631298205E-03f  , // h[100]
             -8.4604622296E-04f  , // h[101]
             -1.8371041170E-04f  , // h[102]
              3.7861244177E-04f  , // h[103]
              6.8600556848E-04f  , // h[104]
              7.0364124471E-04f  , // h[105]
              4.9893013906E-04f  , // h[106]
              1.9359229404E-04f  , // h[107]
             -9.0463969054E-05f  , // h[108]
             -2.7121988053E-04f  , // h[109]
             -3.2247995808E-04f  , // h[110]
             -2.6654821208E-04f  , // h[111]
             -1.5267257414E-04f  , // h[112]
             -3.2730282072E-05f  , // h[113]
              5.6525094615E-05f  , // h[114]
              1.0046493807E-04f  , // h[115]
              1.0373162570E-04f  , // h[116]
              8.1752597223E-05f  , // h[117]
              5.1548250816E-05f  , // h[118]
              2.5403397008E-05f  , // h[119]
              9.0478464730E-06f    // h[120]
 }
);

//-------------------------- 100дБ для децимации 2 ------------
/******************************************************************************
                      SPECIFICATIONS OF SESSION LP2_100DB (FIR)
******************************************************************************

                             1  digitalfilter type FIR or IIR
                            43  filter length
                             5  filter type: lowpass
                             2  filter bands

                         10.24  sample frequency in [kHz]
                          5.12  maximum signal frequency in [kHz]

     ====================================================================
                                  BAND 1
     ====================================================================

                          0.00  lower band edge in [kHz]
                          2.00  upper band edge in [kHz]
                       1.00000  gain
                       0.00100  ripple
                       0.00868  ripple in [dB]

     ====================================================================
                                  BAND 2
     ====================================================================

                          3.12  lower band edge in [kHz]
                          5.12  upper band edge in [kHz]
                       0.00000  gain
                       0.00001  ripple
                    -100.00000  ripple in [dB]

*/
        public static DecimationFir Decimate2a = new DecimationFir(43, 2,
         new float[]{
/*****************************************************************************
                    IMPULSE RESPONSE OF SESSION LP2_100DB (FIR)
*****************************************************************************/
             -1.6462798411E-04f  ,// h[1]
             -3.8489612708E-04f  ,// h[2]
              1.5671587417E-04f  ,// h[3]
              1.3024129516E-03f  ,// h[4]
              5.4541724125E-04f  ,// h[5]
             -2.3844581749E-03f  ,// h[6]
             -1.7416579788E-03f  ,// h[7]
              4.2543911132E-03f  ,// h[8]
              4.4405646238E-03f  ,// h[9]
             -6.4922883837E-03f  ,// h[10]
             -9.0447208475E-03f  ,// h[11]
              9.1917560349E-03f  ,// h[12]
              1.6749066453E-02f  ,// h[13]
             -1.1992489035E-02f  ,// h[14]
             -2.9275471989E-02f  ,// h[15]
              1.4675692998E-02f  ,// h[16]
              5.1076123452E-02f  ,// h[17]
             -1.6875717461E-02f  ,// h[18]
             -9.8103839991E-02f  ,// h[19]
              1.8336150086E-02f  ,// h[20]
              3.1556812660E-01f  ,// h[21]
              4.8115859374E-01f  ,// h[22]
              3.1556812660E-01f  ,// h[23]
              1.8336150086E-02f  ,// h[24]
             -9.8103839991E-02f  ,// h[25]
             -1.6875717461E-02f  ,// h[26]
              5.1076123452E-02f  ,// h[27]
              1.4675692998E-02f  ,// h[28]
             -2.9275471989E-02f  ,// h[29]
             -1.1992489035E-02f  ,// h[30]
              1.6749066453E-02f  ,// h[31]
              9.1917560349E-03f  ,// h[32]
             -9.0447208475E-03f  ,// h[33]
             -6.4922883837E-03f  ,// h[34]
              4.4405646238E-03f  ,// h[35]
              4.2543911132E-03f  ,// h[36]
             -1.7416579788E-03f  ,// h[37]
             -2.3844581749E-03f  ,// h[38]
              5.4541724125E-04f  ,// h[39]
              1.3024129516E-03f  ,// h[40]
              1.5671587417E-04f  ,// h[41]
             -3.8489612708E-04f  ,// h[42]
             -1.6462798411E-04f  //h[43]
 }
);
    }
}
