using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Evade : SteeringBehaviour
    {
        public IBoid _target;

        private Boid _boid;
        public Flee _flee;

        public Evade(IBoid host)
        {
            _host = host;

            _boid = new Boid();
            _boid._position = _host._Position;
            _boid._velocity = _host._Velocity;
            _boid._maxVelocity = _host._MaxVelocity;

            _flee = new Flee(_host);
            _flee._target = _boid;
        }

        public override void Update()
        {
            float distance = (_target._Position - _host._Position).magnitude;
            float T = distance / _host._MaxVelocity;
            _boid._position = _target._Position + _target._Velocity * T;
            //_flee._target = _boid;
            _flee.Update();

            _steering = _flee._steering;
            _direction = _boid._position;
        }
    }
}
