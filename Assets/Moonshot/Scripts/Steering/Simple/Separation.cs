using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Separation : SteeringBehaviour
    {
        public float _RADIUS = 3f;
        public List<IBoid> _boids = new List<IBoid>();

        public Separation(IBoid host)
        {
            _host = host;
            _boids = new List<IBoid>();
        }

        public override void Update()
        {
            int n = _boids.Count;

            float r2 = _RADIUS * _RADIUS;
            float d2 = 0;
            float force = 0f;
            float maxForce = _host._MaxVelocity;
            Vector2 direction = Vector2.zero;

            Vector2 moveDirection = Vector2.zero;
            Vector2 separation = Vector2.zero;

            for (int i = 0; i < n; i++)
            {
                direction = _boids[i]._Position - _host._Position;
                d2 = direction.sqrMagnitude;
                if (d2 <= r2)
                {
                    if (d2 > 0)
                    {
                        force = Mathf.Min( 1f / d2, maxForce);
                    }
                    else
                    {
                        force = maxForce;
                    }
                    moveDirection += force * direction.normalized;
                }
            }
            if (n > 0) moveDirection /= n;

            _steering = -moveDirection;
            _direction = moveDirection;
        }        
    }
}
