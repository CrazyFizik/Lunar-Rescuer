using UnityEngine;
using System.Collections;
using UnityEngineInternal;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemRCS : Subsystem
    {
        [SerializeField] string _name = "RCS";
        [SerializeField] string _type = "RCS";

        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        [SerializeField] SystemState _systemState = new SystemState();
        [SerializeField] private float _angularSpeed = 90f;
        [SerializeField] private float _fuelConsuption = 1f;
        [SerializeField] private float _damagedAngularSpeed = 60f;
        [SerializeField] private float _damagedFuelConsuption = 2f;

        private Ship _ship;
        private Rigidbody2D _rb2d;
        private SystemFuelTank _fuel;
        private IThruster[] _thrusters;

        public override string Name { get => _name; set => _name = value; }
        public override string Type { get => _type; set => _type = value; }

        public float MaxAngularSpeed
        {
            get
            {
                if (Status.GetCondition(eSystemState.DAMAGED))
                {
                    return _damagedAngularSpeed;
                }
                return _angularSpeed;
            }
        }

        public float FuelConsuption
        {
            get
            {
                if (Status.GetCondition(eSystemState.DAMAGED))
                {
                    return _damagedFuelConsuption;
                }
                return _fuelConsuption;
            }
        }

        public float AngularVelocity
        {
            get
            {
                return _rb2d.angularVelocity;
            }
        }

        public float Inertia
        {
            get
            {
                return _rb2d.inertia;
            }
        }

        public override SystemState Status
        {
            get
            {
                return _systemState;
            }
            set
            {
                _systemState = value;
            }
        }

        public override Ship Ship
        {
            get
            {
                return _ship;
            }
            set
            {
                _ship = value;
            }
        }

        public Rigidbody2D Body
        {
            get
            {
                return _rb2d;
            }
            set
            {
                _rb2d = value;
            }
        }

        public SystemFuelTank FuelSystem
        {
            get
            {
                return _fuel;
            }
            set
            {
                _fuel = value;
            }
        }

        public IThruster[] Thrusters
        {
            get
            {
                return _thrusters;
            }
            set
            {
                _thrusters = value;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _state = Status.State;
        }

        public override void Init()
        {
            Ship = GetComponent<Ship>();
            Body = GetComponent<Rigidbody2D>();
            FuelSystem = GetComponent<SystemFuelTank>();
            Thrusters = GetComponentsInChildren<Thruster>();
            foreach (var thruster in Thrusters)
            {
                thruster.Init(Ship.transform);
            }
            Status.Initialize(this);
        }

        public void SetRotation(float rotation)
        {
            var torque = 0f;
            var isOnline = !(Status.GetCondition(eSystemState.OFFLINE) || Status.GetCondition(eSystemState.INVALID));
            if (isOnline)
            {
                var maxAcceleration = 0f;
                var steer = 0f;

                maxAcceleration = Mathf.Exp(2) * MaxAngularSpeed;
                //maxAcceleration = Mathf.Log(120f) * MaxAngularSpeed; // 120 Hz
                //maxAcceleration = Mathf.Sqrt(1f / Time.fixedDeltaTime) * MaxAngularSpeed;
                //maxAcceleration = 20f * Mathf.Log10(1f / Time.fixedDeltaTime / 20f) * MaxAngularSpeed;

                steer = rotation / (maxAcceleration * Time.fixedDeltaTime); // Good: simple divide by minimal angle
                steer = Mathf.Clamp(steer, -1f, 1f);

                //steer = rotation / 90f; //Good: simple divide by quarter    
                //steer = Mathf.Clamp(steer, -1f, 1f);

                //steer = Mathf.Cos((90f - Mathf.Clamp(rotation, -90f, 90f)) * Mathf.Deg2Rad); //Good: lateral component of directional vector
                //steer = Mathf.Clamp(steer, -1f, 1f);

                //steer = rotation / Time.fixedDeltaTime; steer = steer / MaxAngularSpeed; //Bad: overshooting
                //steer = Mathf.Clamp(steer, -1f, 1f);

                //steer = 1f - Mathf.Cos(rotation * Mathf.Deg2Rad); // Bad: slow zero
                //steer = Mathf.Sign(rotation) * Mathf.Clamp01(steer);

                //steer = rotation / MaxAngularSpeed;
                //steer = Mathf.Sign(steer) * Utils.MathHelper.SmootherStep(0f, 90f / MaxAngularSpeed, Mathf.Abs(steer)); // Bad: slow zero

                var targetVelocity = steer * MaxAngularSpeed;
                torque = GetTorqueByTargetSpeed(targetVelocity, maxAcceleration);
                //torque = GetTorque(steer, maxAcceleration * Mathf.Deg2Rad);
            }
            SetTorque(torque);
        }

        float GetTorqueByTargetSpeed(float targetVelocity, float maxAcceleration)
        {
            var deltaVelocity = targetVelocity - AngularVelocity;
            var acceleration = deltaVelocity / Time.fixedDeltaTime;
            acceleration = Mathf.Clamp(acceleration, -maxAcceleration, maxAcceleration);

            var throttle = acceleration / maxAcceleration;
            acceleration = acceleration * FuelSystem.GetFuelFlowRatio(FuelConsuption * Mathf.Abs(throttle) * Time.fixedDeltaTime);
            var torque = acceleration * Mathf.Deg2Rad * Inertia;
            return torque;
        }

        float GetTorque(float x, float maxAcceleration)
        {
            var damp = maxAcceleration / (Mathf.Deg2Rad * MaxAngularSpeed);
            var acceleration = maxAcceleration * x - (Mathf.Deg2Rad * AngularVelocity) * damp;
            acceleration = Mathf.Clamp(acceleration, -maxAcceleration, maxAcceleration);

            var throttle = acceleration / maxAcceleration;
            acceleration = acceleration * FuelSystem.GetFuelFlowRatio(FuelConsuption * Mathf.Abs(throttle) * Time.fixedDeltaTime);
            var torque = acceleration * Inertia;
            return torque;
        }

        public void SetTorque(float torque)
        {            
            if (Mathf.Abs(Body.angularVelocity) < 0.001f)
            {
                Body.angularVelocity = 0f;
            }
            else
            {
                Body.AddTorque(torque);
            }
        }

        public override void TakeDamage()
        {
            Status.TakeDamage();
            if (Status.GetCondition(eSystemState.OFFLINE))
            {
                var torque = 2f * Inertia * Random.Range(-MaxAngularSpeed, MaxAngularSpeed) * Mathf.Deg2Rad;
                Body.AddTorque(torque, ForceMode2D.Impulse);
            }
        }

        public override void TakeDamage(float damage)
        {
            Status.TakeDamage(damage);
            if (Status.GetCondition(eSystemState.OFFLINE))
            {
                var torque = 2f * Inertia * Random.Range(-MaxAngularSpeed, MaxAngularSpeed) * Mathf.Deg2Rad;
                Body.AddTorque(torque, ForceMode2D.Impulse);
            }
        }

        public override void Repair(float dt)
        {
            Status.Update(dt);
        }
    }
}