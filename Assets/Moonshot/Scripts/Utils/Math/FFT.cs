#region License and Information
/*****
*
* This is an implementation of a complex number class which supports most
* common operations. Though, not all of them has been tested. Some are staight
* forward implementation as you can find them on Wikipedia and other sources.
*
* In addition the FFT class contains a fast implementation of the Fast Fourier
* Transformation. It's basically a port of the implementation of Paul Bourke
* http://paulbourke.net/miscellaneous/dft/
*
* CalculateFFT is designed to perform both the FFT as well as the inverse FFT.
* If you pass "true" to the "reverse" parameter it will calculate the inverse
* FFT. The FFT is calculated in-place on an array of Complex values.
*
* For convenience i added a few helper functions to convert a float array as
* well as a double array into a Complex array. The reverse also exists. The
* Complex2Float and Complex2Double have also a "reverse" parameter which will
* preserve the sign of the real part
*
* Keep in mind when using this as FFT filter you have to preserve the Complex
* samples as the phase information might be required for the inverse FFT.
*
* Final note: I've written this mainly to better understand the FFT algorithm.
* The original version of Paul Bourke is probably faster, but i wanted to use
* actual Complex numbers so it's easier to follow the code.
*
*
* Copyright (c) 2015 Bunny83
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to
* deal in the Software without restriction, including without limitation the
* rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
* sell copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
* IN THE SOFTWARE.
*
*****/
#endregion License and Information
#define UNITY


namespace Utils.Math
{
    using System;

    public class FFT
    {
        // aSamples.Length need to be a power of two
        public static Complex[] CalculateFFT(Complex[] aSamples, bool aReverse)
        {
            int power = (int)Math.Log(aSamples.Length, 2);
            int count = 1;
            for (int i = 0; i < power; i++)
            {
                count <<= 1;
            }

            int mid = count >> 1; // mid = count / 2;
            int j   = 0;
            for (int i = 0; i < count - 1; i++)
            {
                if (i < j)
                {
                    var tmp     = aSamples[i];
                    aSamples[i] = aSamples[j];
                    aSamples[j] = tmp;
                }
                int k = mid;
                while (k <= j)
                {
                    j -= k;
                    k >>= 1;
                }
                j += k;
            }
            Complex r   = new Complex(-1, 0);
            int     l2  = 1;
            for (int l = 0; l < power; l++)
            {
                int l1  =   l2;
                l2      <<= 1;

                Complex r2 = new Complex(1, 0);
                for (int n = 0; n < l1; n++)
                {
                    for (int i = n; i < count; i += l2)
                    {
                        int i1          =   i + l1;
                        Complex tmp     =   r2 * aSamples[i1];
                        aSamples[i1]    =   aSamples[i] - tmp;
                        aSamples[i]     +=  tmp;
                    }
                    r2 = r2 * r;
                }
                r.img   = Math.Sqrt((1d - r.real) / 2d);
                r.real  = Math.Sqrt((1d + r.real) / 2d);
                if (!aReverse) r.img = -r.img;
            }
            if (!aReverse)
            {
                double scale = 1d / count;
                for (int i = 0; i < count; i++)
                    aSamples[i] *= scale;
            }
            return aSamples;
        }


        #region float / double array conversion helpers
        public static Complex[] Double2Complex(double[] aData)
        {
            Complex[] data = new Complex[aData.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Complex(aData[i], 0);
            }
            return data;
        }
        public static double[] Complex2Double(Complex[] aData, bool aReverse)
        {
            double[] result = new double[aData.Length];
            if (!aReverse)
            {
                for (int i = 0; i < aData.Length; i++)
                {
                    result[i] = aData[i].magnitude;
                }
                return result;
            }
            for (int i = 0; i < aData.Length; i++)
            {
                result[i] = Math.Sign(aData[i].real) * aData[i].magnitude;
            }
            return result;
        }

        public static Complex[] Float2Complex(float[] aData)
        {
            Complex[] data = new Complex[aData.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Complex(aData[i], 0);
            }
            return data;
        }
        public static float[] Complex2Float(Complex[] aData, bool aReverse)
        {
            float[] result = new float[aData.Length];
            if (!aReverse)
            {
                for (int i = 0; i < aData.Length; i++)
                {
                    result[i] = (float)aData[i].magnitude;
                }
                return result;
            }
            for (int i = 0; i < aData.Length; i++)
            {
                result[i] = (float)(Math.Sign(aData[i].real) * aData[i].magnitude);
            }
            return result;
        }
        #endregion float / double array conversion helpers
    }
}
