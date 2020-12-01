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

        public float AngularSpeed
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
            var dt = Time.fixedDeltaTime;
            var scale = 2f * MaxAngularSpeed * dt;
            var linearRotation = Mathf.Clamp(rotation / scale, -1f, 1f);            
            var targetAngularSpeed = linearRotation * MaxAngularSpeed;
            var deltaAngularSpeed = targetAngularSpeed - AngularSpeed;  
            
            var acceleration = Mathf.Clamp(deltaAngularSpeed, -MaxAngularSpeed, MaxAngularSpeed)/dt;
            if (Status.GetCondition(eSystemState.OFFLINE) || Status.GetCondition(eSystemState.INVALID))
            {
                acceleration = 0f;
            }
            var maxAngularAcceleration = MaxAngularSpeed / dt;
            var throttle = acceleration / maxAngularAcceleration;
            acceleration = acceleration * FuelSystem.GetFuelFlowRatio(FuelConsuption * Mathf.Abs(throttle) * Time.fixedDeltaTime);

            var torque = acceleration * Mathf.Deg2Rad * Inertia;
            SetTorque(torque);
        }

        public void SetTorque(float torque)
        {
            Body.AddTorque(torque);
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