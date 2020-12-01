namespace Utils.Math
{
    using System;

    public struct Complex
    {
        public double real;
        public double img;
        public Complex(double aReal, double aImg)
        {
            real = aReal;
            img = aImg;
        }
        public static Complex FromAngle(double aAngle, double aMagnitude)
        {
            return new Complex(Math.Cos(aAngle) * aMagnitude, Math.Sin(aAngle) * aMagnitude);
        }

        public Complex conjugate { get { return new Complex(real, -img); } }
        public double magnitude { get { return Math.Sqrt(real * real + img * img); } }
        public double sqrMagnitude { get { return real * real + img * img; } }
        public double angle { get { return Math.Atan2(img, real); } }

        public float fReal { get { return (float)real; } set { real = value; } }
        public float fImg { get { return (float)img; } set { img = value; } }
        public float fMagnitude { get { return (float)Math.Sqrt(real * real + img * img); } }
        public float fSqrMagnitude { get { return (float)(real * real + img * img); } }
        public float fAngle { get { return (float)Math.Atan2(img, real); } }

        #region Basic operations + - * /
        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.real + b.real, a.img + b.img);
        }
        public static Complex operator +(Complex a, double b)
        {
            return new Complex(a.real + b, a.img);
        }
        public static Complex operator +(double b, Complex a)
        {
            return new Complex(a.real + b, a.img);
        }

        public static Complex operator -(Complex a)
        {
            return new Complex(-a.real, -a.img);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.real - b.real, a.img - b.img);
        }
        public static Complex operator -(Complex a, double b)
        {
            return new Complex(a.real - b, a.img);
        }
        public static Complex operator -(double b, Complex a)
        {
            return new Complex(b - a.real, -a.img);
        }

        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.real * b.real - a.img * b.img, a.real * b.img + a.img * b.real);
        }
        public static Complex operator *(Complex a, double b)
        {
            return new Complex(a.real * b, a.img * b);
        }
        public static Complex operator *(double b, Complex a)
        {
            return new Complex(a.real * b, a.img * b);
        }

        public static Complex operator /(Complex a, Complex b)
        {
            double d = 1d / (b.real * b.real + b.img * b.img);
            return new Complex((a.real * b.real + a.img * b.img) * d, (-a.real * b.img + a.img * b.real) * d);
        }
        public static Complex operator /(Complex a, double b)
        {
            return new Complex(a.real / b, a.img / b);
        }
        public static Complex operator /(double a, Complex b)
        {
            double d = 1d / (b.real * b.real + b.img * b.img);
            return new Complex(a * b.real * d, -a * b.img);
        }

        #endregion Basic operations + - * /

        #region Trigonometic operations

        public static Complex Sin(Complex a)
        {
            return new Complex(Math.Sin(a.real) * Math.Cosh(a.img), Math.Cos(a.real) * Math.Sinh(a.img));
        }
        public static Complex Cos(Complex a)
        {
            return new Complex(Math.Cos(a.real) * Math.Cosh(a.img), -Math.Sin(a.real) * Math.Sinh(a.img));
        }

        private static double arcosh(double a)
        {
            return Math.Log(a + Math.Sqrt(a * a - 1));
        }
        private static double artanh(double a)
        {
            return 0.5 * Math.Log((1 + a) / (1 - a));

        }

        public static Complex ASin(Complex a)
        {
            double r2 = a.real * a.real;
            double i2 = a.img * a.img;
            double sMag = r2 + i2;
            double c = Math.Sqrt((sMag - 1) * (sMag - 1) + 4 * i2);
            double sr = a.real > 0 ? 0.5 : a.real < 0 ? -0.5 : 0;
            double si = a.img > 0 ? 0.5 : a.img < 0 ? -0.5 : 0;

            return new Complex(sr * Math.Acos(c - sMag), si * arcosh(c + sMag));
        }
        public static Complex ACos(Complex a)
        {
            return Math.PI * 0.5 - ASin(a);
        }

        public static Complex Sinh(Complex a)
        {
            return new Complex(Math.Sinh(a.real) * Math.Cos(a.img), Math.Cosh(a.real) * Math.Sin(a.img));
        }
        public static Complex Cosh(Complex a)
        {
            return new Complex(Math.Cosh(a.real) * Math.Cos(a.img), Math.Sinh(a.real) * Math.Sin(a.img));
        }
        public static Complex Tan(Complex a)
        {
            return new Complex(Math.Sin(2 * a.real) / (Math.Cos(2 * a.real) + Math.Cosh(2 * a.img)), Math.Sinh(2 * a.img) / (Math.Cos(2 * a.real) + Math.Cosh(2 * a.img)));
        }
        public static Complex ATan(Complex a)
        {
            double sMag = a.real * a.real + a.img * a.img;
            double i = 0.5 * artanh(2 * a.img / (sMag + 1));
            if (a.real == 0)
                return new Complex(a.img > 1 ? Math.PI * 0.5 : a.img < -1 ? -Math.PI * 0.5 : 0, i);
            double sr = a.real > 0 ? 0.5 : a.real < 0 ? -0.5 : 0;
            return new Complex(0.5 * (Math.Atan((sMag - 1) / (2 * a.real)) + Math.PI * sr), i);
        }

        #endregion Trigonometic operations

        public static Complex Exp(Complex a)
        {
            double e = Math.Exp(a.real);
            return new Complex(e * Math.Cos(a.img), e * Math.Sin(a.img));
        }
        public static Complex Log(Complex a)
        {
            return new Complex(Math.Log(Math.Sqrt(a.real * a.real + a.img * a.img)), Math.Atan2(a.img, a.real));
        }
        public Complex sqrt(Complex a)
        {
            double r = Math.Sqrt(Math.Sqrt(a.real * a.real + a.img * a.img));
            double halfAngle = 0.5 * Math.Atan2(a.img, a.real);
            return new Complex(r * Math.Cos(halfAngle), r * Math.Sin(halfAngle));
        }

#if UNITY
        public static explicit operator UnityEngine.Vector2(Complex a)
        {
            return new UnityEngine.Vector2((float)a.real, (float)a.img);
        }
        public static explicit operator UnityEngine.Vector3(Complex a)
        {
            return new UnityEngine.Vector3((float)a.real, (float)a.img);
        }
        public static explicit operator UnityEngine.Vector4(Complex a)
        {
            return new UnityEngine.Vector4((float)a.real, (float)a.img);
        }
        public static implicit operator Complex(UnityEngine.Vector2 a)
        {
            return new Complex(a.x, a.y);
        }
        public static implicit operator Complex(UnityEngine.Vector3 a)
        {
            return new Complex(a.x, a.y);
        }
        public static implicit operator Complex(UnityEngine.Vector4 a)
        {
            return new Complex(a.x, a.y);
        }
#endif
    }
}
