using System;
using System.Collections.Generic;
using System.Text;

namespace IppModules.Analiz.NarrowBandSpectrum.FastFourierTransform
{
    /// <summary>
    /// ��������� ��� ������� �������������� �����.
    /// </summary>
    interface IFastFourierTransform
    {
        /// <summary>
        /// �������������� ����� ��� ������.
        /// ������� ������� �������� ��� ������� ����� ������� ������������� ������.
        /// </summary>
        /// <param name="block_size_power2">������ ����� ��� ������� ������.</param>
        /// <param name="winType">��� ����.</param>
        /// <returns>true ���� ��������� ����������, false ���� � ��� ������ �����</returns>
        bool Prepare(int block_size_power2, WindowType winType);

        /// <summary>
        /// ���������� ������ �� ������ �������� ����.
        /// </summary>
        float[] WinArr
        {
            get;
        }

        /// <summary>
        /// ���������� ������ ����� �������.
        /// </summary>
        int BlockSize
        {
            get;
        }

        /// <summary>
        /// ���������� ������ ����� ����������.
        /// </summary>
        int OutBlockSize
        {
            get;
        }

        /// <summary>
        /// ���������� ������ �� ������ ��������� ������. ��������.
        /// </summary>
        float[] ResultRe
        {
            get;
        }

        /// <summary>
        /// ���������� ������ �� ������ ��������� ������. ��������.
        /// </summary>
        float[] ResultIm
        {
            get;
        }
    }
}
