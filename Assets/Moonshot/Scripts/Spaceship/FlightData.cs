using System;
using UnityEngine;


namespace Assets.Moonshot.Scripts.Spaceship
{
    [System.Serializable]
    public class FlightData
    {
        [Header("Altitude State")]
        [SerializeField] float _altitude;
        [SerializeField] float _orientation;
        [SerializeField] Vector2 _heading;
        [SerializeField] Vector2 _position;
        [SerializeField] Vector2 _velocity;

        [Header("Speed State")]
        [SerializeField] float _linearSpeed = 0f;
        [SerializeField] float _linearAcceleration = 0f;
        [SerializeField] float _angularSpeed = 0f;
        [SerializeField] float _angularAcceleration = 0f;

        public float Altitude
        {
            get
            {
                return _altitude;
            }
        }

        public float Orientation
        {
            get
            {
                return _orientation;
            }
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
        }

        public float LinearAcceleration
        {
            get
            {
                return _linearAcceleration;
            }
        }

        public float LinearSpeed
        {
            get
            {
                return _linearSpeed;
            }
        }

        public float AngularAcceleration
        {
            get
            {
                return _linearAcceleration;
            }
        }

        public float AngularSpeed
        {
            get
            {
                return _linearSpeed;
            }
        }

        public void Update(Ship ship, float dt)
        {
            var angularSpeed0 = _angularSpeed;
            var linearSpeed0 = _linearSpeed;

            // Angular
            _angularSpeed = ship.Body.angularVelocity;
            _angularAcceleration = Mathf.Deg2Rad * (_angularSpeed - angularSpeed0);
            _angularAcceleration /= dt;

            // Linear
            _linearSpeed = _velocity.magnitude;
            _linearAcceleration = (_linearSpeed - linearSpeed0);
            _linearAcceleration /= dt;

            // Alttitude
            _orientation = ship.Body.rotation;
            _heading = ship.Body.transform.right;
            _velocity = ship.Body.velocity;
            _position = ship.Body.position;
            _altitude = ship.Body.position.y;
        }

    }
}
