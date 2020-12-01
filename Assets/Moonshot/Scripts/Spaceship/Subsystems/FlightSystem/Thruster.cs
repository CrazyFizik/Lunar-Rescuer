using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class Thruster : MonoBehaviour, IThruster
    {
        [Tooltip("Root transform with attached rigidbody")]
        [SerializeField] private Transform _root;
        public Transform Root
        {
            get
            {
                return _root;
            }
            set
            {
                _root = value;
            }
        }

        [Tooltip("The current thottle amount")]
        [SerializeField] private float _throttle;
        public float Throttle
        {
            get
            {
                return _throttle;
            }
            set
            {
                _throttle = value;
            }
        }

        [Tooltip("Thruster position relative to center of mass")]
        [SerializeField] private Vector3 _centerOfMass = Vector3.zero;
        public Vector3 CentreOfMass
        {
            get
            {
                return _centerOfMass;
            }
        }

        [Tooltip("Thruster direction")]
        [SerializeField] private Vector3 _forwardVector = -Vector3.right;
        public Vector3 ForwardDirection
        {
            get
            {                
                return -transform.right;
            }
        }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
        }

        [Tooltip("Thruster position relative to center of mass")]
        [SerializeField] private Vector3 _relativePosition = Vector3.zero;
        public Vector3 RelativePosition
        {
            get
            {
                return _relativePosition;
            }
        }

        [Tooltip("Thruster position relative to center of mass")]
        [Range(0f, 1f)]
        [SerializeField] private float _torquePriority = 0.5f;
        public float TorquePriority
        {
            get
            {
                return _torquePriority;
            }
            set
            {
                _torquePriority = Mathf.Clamp01(value);

            }
        }
        public float ThrustPriority
        {
            get
            {
                return 1f - _torquePriority;
            }
            set
            {
                _torquePriority = Mathf.Clamp01(1f - value);

            }
        }

        [Tooltip("Linear force effect")]
        [SerializeField] private Vector3 _force;
        public Vector3 Force
        {
            get
            {
                return _force;
            }
        }

        [Tooltip("Torque effect")]
        [SerializeField] private Vector3 _torque;
        public Vector3 Torque
        {
            get
            {
                return _torque;
            }
        }

        [Tooltip("Cached particle system")]
        [SerializeField] private IThrusterFX[] _fx;

        void Reset()
        {
            Init();
        }

        void OnValidate()
        {
            Init();
        }

        public void Init()
        {
            if (_root == null) _root = this.transform.root;

            _fx = GetComponentsInChildren<IThrusterFX>();
            _forwardVector = _root.InverseTransformDirection(ForwardDirection);
            _centerOfMass = _root.position;
            _relativePosition = _root.InverseTransformDirection(transform.position - _centerOfMass);
            _torque = GetTorque();
            _force = GetForce();
        }

        public void Init(Transform root)
        {
            _root = root;
            Init();
        }

        [Obsolete("Need autonomus")]
        public float Activate(Vector3 translation, Vector3 rotation)
        {
            //translation = Vector2.right;

            var F = _root.InverseTransformDirection(ForwardDirection);
            var r = _root.InverseTransformDirection(transform.position - _root.position); // transform.localPosition;// 

            var force = Vector3.Dot(translation, F);
            var cross = Vector3.Cross(r, F);
            var torque = Vector3.Dot(rotation, cross);

            _forwardVector = F;
            _relativePosition = r;
            _throttle = ThrustPriority * force + TorquePriority * torque;

            SetFX(_throttle);
            return _throttle;
        }

        [Obsolete("Incorrect work")]
        public float ActivateRelative(Vector3 translation, Vector3 rotation, Vector3 centerOfMass)
        {
            _forwardVector = ForwardDirection;
            _centerOfMass = centerOfMass;
            _relativePosition = transform.position - centerOfMass; //????

            var force = Vector3.Dot(translation, _forwardVector);
            var cross = Vector3.Cross(_relativePosition, _forwardVector);
            var torque = Vector3.Dot(rotation, cross);

            _throttle = ThrustPriority * force + TorquePriority * torque;
            SetFX(_throttle);

            return _throttle;
        }

        public void SetFX(float throttle)
        {
            if (_fx != null)
            {
                foreach (var fx in _fx)
                {
                    fx.SetFX(throttle);
                }
            }
        }

        #region others

        public Vector3 GetForce()
        {
            var force = ForwardDirection;
            return force * ThrustPriority;
        }

        public Vector3 GetTorque()
        {
            var F = ForwardDirection;
            var r = transform.position - CentreOfMass;
            var torque = Vector3.Cross(r, F);
            return torque * TorquePriority;
        }

        public float GetForceByDirection(Vector3 direction)
        {
            var F = ForwardDirection;
            var force = Vector3.Dot(F, direction);
            if (force < 0) force = 0f;
            return force * ThrustPriority;
        }

        public float GetTorqueByDirection(Vector3 direction)
        {
            var F = ForwardDirection;
            var r = transform.position - CentreOfMass;
            var torque = Vector3.Cross(r, F);
            var force = Vector3.Dot(torque, direction);
            if (force < 0) force = 0f;
            return force * TorquePriority;
        }

        #endregion

    }
}
