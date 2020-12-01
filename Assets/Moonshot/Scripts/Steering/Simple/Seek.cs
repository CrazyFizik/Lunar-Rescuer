using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Seek : SteeringBehaviour
    {
        public IBoid _target;

        public Seek(IBoid host)
        {
            _host = host;
        }

        public override void Update()
        {
            Vector2 desired = _target._Position - _host._Position;
            desired = desired.normalized * _host._MaxVelocity;

            _steering = desired - _host._Velocity;
            _direction = desired;
        }
    }
}
