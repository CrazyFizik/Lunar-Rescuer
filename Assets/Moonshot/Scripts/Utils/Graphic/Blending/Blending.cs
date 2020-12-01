using UnityEngine;

namespace Utils.Graphics
{
    public static class Blending
    {
        public static Color Blend(Color one, Color two, float transition)
        {
            return Color.Lerp(one, two, transition);
        }

        public static Color GammaBlend(Color background, Color overlay, float transition)
        {
            float gamma = 2.2f;
            overlay.r = Mathf.Pow(overlay.r, gamma);
            overlay.g = Mathf.Pow(overlay.g, gamma);
            overlay.b = Mathf.Pow(overlay.b, gamma);
            overlay *= transition;

            background.r = Mathf.Pow(background.r, gamma);
            background.g = Mathf.Pow(background.g, gamma);
            background.b = Mathf.Pow(background.b, gamma);
            background = background * (1f - transition);

            Color result = overlay + background;

            result.r = Mathf.Pow(result.r, 1f / gamma);
            result.g = Mathf.Pow(result.g, 1f / gamma);
            result.b = Mathf.Pow(result.b, 1f / gamma);
            result.a = 1f;

            return result;
        }

        public static Color InverseBlend(Color color, Color backColor, float amount)
        {
            float r = ((color.r * amount) + backColor.r * (1f - amount));
            float g = ((color.g * amount) + backColor.g * (1f - amount));
            float b = ((color.b * amount) + backColor.b * (1f - amount));
            float a = backColor.a;

            return new Color(r, g, b, a);
        }

        public static Color G(Color c)
        {
            return new Color(.299f * c.r, .587f * c.g, .114f * c.b);
        }

        public static Color Multiply(Color a, Color b)
        {
            Color r = a * b;
            r.a = b.a;
            return r;
        }

        public static Color ColorBurn(Color a, Color b)
        {
            Color r = new Color();
            r.r = 1f - (1f - a.r) / b.r;
            r.g = 1f - (1f - a.g) / b.g;
            r.b = 1f - (1f - a.b) / b.b;
            r.a = b.a;
            return r;
        }

        public static Color LinearBurn(Color a, Color b)
        {
            Color r = a + b - Color.white;
            r.a = b.a;
            return r;
        }

        public static Color Screen(Color a, Color b)
        {
            Color r = Color.white - (Color.white - a) * (Color.white - b);
            r.r = 1f - (1f - a.r) * (1f - b.r);
            r.g = 1f - (1f - a.g) * (1f - b.g);
            r.b = 1f - (1f - a.b) * (1f - b.b);
            r.a = b.a;
            return r;
        }

        public static Color ColorDodge(Color a, Color b)
        {
            Color r = new Color();
            r.r = a.r / (1f - b.r);
            r.g = a.g / (1f - b.g);
            r.b = a.b / (1f - b.b);
            r.a = b.a;
            return r;
        }

        public static Color LinearDodge(Color a, Color b)
        {
            Color r = a + b;
            r.a = b.a;
            return r;
        }

        public static Color SoftLight(Color a, Color b)
        {
            Color r = (Color.white - a) * a * b + a * (Color.white - (Color.white - a) * (Color.white - b));
            r.a = b.a;
            return r;
        }

        public static Color Difference(Color a, Color b)
        {
            Color r = new Color();
            r.r = Mathf.Abs(a.r - b.r);
            r.g = Mathf.Abs(a.g - b.g);
            r.b = Mathf.Abs(a.b - b.b);
            r.a = b.a;
            return r;
        }

        public static Color Exclusion(Color a, Color b)
        {
            Color r = a + b - 2.0f * a * b;
            r.a = b.a;
            return r;
        }

        public static Color Subtract(Color a, Color b)
        {
            Color r = a - b;
            r.a = b.a;
            return r;
        }

        public static Color Divide(Color a, Color b)
        {
            Color r = new Color();
            r.r = a.r / b.r;
            r.g = a.g / b.g;
            r.b = a.b / b.b;
            r.a = b.a;
            return r;
        }
    }
}