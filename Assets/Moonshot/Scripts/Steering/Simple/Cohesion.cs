using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Cohesion : SteeringBehaviour
    {
        public float _RADIUS = 5f;
        public List<IBoid> _boids;

        public Cohesion(IBoid host)
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

            Vector2 averagePosition = Vector2.zero;
            for (int i = 0; i < n; i++)
            {
                dp = _boids[i]._Position - _host._Position;
                d2 = dp.sqrMagnitude;
                if (d2 <= r2)
                {
                    averagePosition += _boids[i]._Position;
                }
            }

            if (n > 0)
            {
                averagePosition /= n;
            }

            _steering   = (averagePosition - _host._Position);
            _direction  = averagePosition;
        }
        
    }
}
