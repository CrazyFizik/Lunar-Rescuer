using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
            screenPoint.z = 0;

            Vector2 screenPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
            {
                return screenPos;
            }

            return screenPoint;
        }

        /// <summary>
        /// Creates dampened motion between a and b that is framerate independent.
        /// Thanks to Rory Driscoll
        /// http://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
        /// </summary>
        /// <param name="a">Initial parameter</param>
        /// <param name="b">Target parameter</param>
        /// <param name="lambda">Smoothing factor</param>
        /// <param name="dt">Time since last damp call</param>
        /// <returns></returns>
        public static Quaternion SmoothDamp(Quaternion a, Quaternion b, float lambda, float dt)
        {
            return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edge0"></param>
        /// <param name="edge1"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float SmootherStep(float edge0, float edge1, float x)
        {
            // Scale, and clamp x to 0..1 range
            x = Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            // Evaluate polynomial
            return x * x * x * (x * (x * 6 - 15) + 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="lowerlimit"></param>
        /// <param name="upperlimit"></param>
        /// <returns></returns>
        public static float Clamp(float x, float lowerlimit, float upperlimit)
        {
            if (x < lowerlimit)
                x = lowerlimit;
            if (x > upperlimit)
                x = upperlimit;
            return x;
        }
    }
}
