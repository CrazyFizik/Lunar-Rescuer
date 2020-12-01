using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Arrive : SteeringBehaviour
    {
        public IBoid _target;

        public float _STOP_RADIUS = 2f;
        public float _SLOW_RADIUS = 3f;

        public Arrive(IBoid host)
        {
            _host = host;
        }

        public override void Update()
        {
            float stop2 = _STOP_RADIUS * _STOP_RADIUS;
            float slow2 = _SLOW_RADIUS * _SLOW_RADIUS;

            // Calculate the desired velocity
            Vector2 desired     = _target._Position - _host._Position;
            float   distance    = desired.sqrMagnitude;

            _direction = desired;
            
            // Check the distance to detect whether the character
            //is inside in stop radius
            if (distance < stop2)
            {
                desired = Vector2.zero;
            }
            // is inside the slowing area
            else if (distance < slow2)
            {
                // Inside the slowing area
                desired = desired.normalized * _host._MaxVelocity;
                desired *= Mathf.Sqrt(distance / slow2);
            }
            else
            {
                // Outside the slowing area.
                desired = desired.normalized * _host._MaxVelocity;
            }

            // Set the steering based on this
            _steering = desired - _host._Velocity;
            
        }
    }
}
