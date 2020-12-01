using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Pursue : SteeringBehaviour
    {
        public  IBoid   _target;

        private Boid    _spot;
        public  Seek    _seek;

        public Pursue(IBoid host)
        {
            _host = host;

            _spot = new Boid();
            _spot._position = _host._Position;
            _spot._velocity = _host._Velocity;
            _spot._maxVelocity = _host._MaxVelocity;

            _seek = new Seek(_host);
            _seek._target = _spot;
        }

        public override void Update()
        {
            float distance = (_target._Position - _host._Position).magnitude;
            float T = distance / _host._MaxVelocity;
            _spot._position = _target._Position + _target._Velocity * T;
            //_seek._target = _boid;
            _seek.Update();

            _steering = _seek._steering;
            _direction = _spot._position;
        }
    }
}
