using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public abstract class SteeringBehaviour
    {
        public int _PRIORITY = 1;
        public float _WEIGHT = 1f;

        public IBoid _host;
        public Vector2 _steering;
        public Vector2 _direction;

        public abstract void Update();

        /// <summary>
        /// Convert from polar to cortesian of unit vector
        /// </summary>
        /// <param name="alpha">Angle of Unit vector</param>
        /// <returns></returns>
        public Vector2 Angle2Vector(float alpha)
        {
            Vector2 vector = Vector2.zero;
            float radius = 1.0f;
            if (alpha < 0) alpha += 2 * Mathf.PI;

            vector.y = Mathf.Sin(alpha * Mathf.Deg2Rad) * radius;
            vector.x = Mathf.Cos(alpha * Mathf.Deg2Rad) * radius;
            return vector.normalized;
        }

        /// <summary>
        /// Convert from cortesian to polar of unit vector
        /// </summary>
        /// <param name="direction">Direction vector</param>
        /// <returns></returns>
        public float Vector2Angle(Vector2 direction)
        {
            float alpha = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (alpha < 0) alpha += 2 * Mathf.PI;
            return alpha;
        }
    }
}
