using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Wanderer : SteeringBehaviour
    {
        public float _OFFSET = 2f;
        public float _RADIUS = 3f;
        public float _RATE = 60;

        public Wanderer(IBoid host)
        {
            _host = host;
        }

        public override void Update()
        {
            Vector2 circleCenter = _OFFSET * _host._Velocity.normalized;

            float agentOrientation = Vector2Angle(_host._Velocity);
            float wanderOrientation = Random.Range(-1.0f, 1.0f) * _RATE;
            float targetOrientation = wanderOrientation + agentOrientation;

            Vector2 displacement = _RADIUS * Angle2Vector(targetOrientation);
            Vector2 wander = displacement + circleCenter;

            _steering = wander;//?
            _direction = wander;
        }
    }
}
