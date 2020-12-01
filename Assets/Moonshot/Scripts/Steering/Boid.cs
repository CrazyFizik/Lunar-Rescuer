using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Boid : IBoid
    {
        public Vector2 _position;
        public Vector2 _velocity;
        public float _maxVelocity;

        public float _MaxVelocity
        {
            get
            {
                return _maxVelocity;
            }
        }

        public Vector2 _Position
        {
            get
            {
                return _position;
            }
        }

        public Vector2 _Velocity
        {
            get
            {
                return _velocity;
            }
        }
    }
}
