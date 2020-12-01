using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Aligement : SteeringBehaviour
    {
        public float _RADIUS = 5f;
        public List<IBoid> _boids;

        public Aligement(IBoid host)
        {
            _host = host;
            _boids = new List<IBoid>();
        }

        public override void Update()
        {
            int n = _boids.Count;

            float r2 = _RADIUS * _RADIUS;
            float d2 = 0;
            Vector2 dp = Vector2.zero;

            Vector2 averageDirection = new Vector2();
            for (int i = 0; i < n; i++)
            {
                dp = _boids[i]._Position - _host._Position;
                d2 = dp.sqrMagnitude;
                if (d2 <= r2)
                {
                    averageDirection += _boids[i]._Velocity;
                }
            }
            if (n > 0) averageDirection /= n;

            _steering = averageDirection - _host._Velocity;
            _direction = averageDirection;
        }
    }
}
