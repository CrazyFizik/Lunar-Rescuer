using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Flee : SteeringBehaviour
    {
        public IBoid _target;

        public Flee(IBoid host)
        {
            _host = host;
        }

        public override void Update()
        {
            Vector2 desired = _host._Position - _target._Position;
            desired = desired.normalized * _host._MaxVelocity;

            _steering = desired - _host._Velocity;
            _direction = desired;
        }
    }
}
