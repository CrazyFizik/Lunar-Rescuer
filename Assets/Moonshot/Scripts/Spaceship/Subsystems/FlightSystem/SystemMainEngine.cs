using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemMainEngine : Subsystem
    {
        [SerializeField] string _name = "Main Engine";
        [SerializeField] string _type = "ENG";

        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        [SerializeField] SystemState _systemState = new SystemState();
        [SerializeField] private float _twr = 5;
        [SerializeField] private float _fuelConsuption = 1f;
        [SerializeField] private float _damagedTwr = 4f;
        [SerializeField] private float _damagedFuelConsuption = 2f;

        private Ship _ship;
        private Rigidbody2D _rb2d;
        private SystemFuelTank _fuel;
        private IThruster[] _thrusters;

        public override string Name { get => _name; set => _name = value; }
        public override string Type { get => _type; set => _type = value; }

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

        public float TWR
        {
            get
            {
                if (Status.GetCondition(eSystemState.DAMAGED))
                {
                    return _damagedTwr;
                }
                return _twr;
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

        public void SetThrottle(float throttle)
        {
            if (Status.GetCondition(eSystemState.OFFLINE) || Status.GetCondition(eSystemState.INVALID))
            {
                throttle = 0f;
            }

            throttle = throttle * FuelSystem.GetFuelFlowRatio(FuelConsuption * throttle * Time.fixedDeltaTime);
            var f = TWR * throttle * Body.mass;
            SetForce(f);

            foreach (var thruster in _thrusters)
            {
                thruster.Activate(Vector2.right * throttle, Vector3.zero);
            }
        }

        public void SetForce(float force)
        {
            var f = Vector2.right * force;
            Body.AddRelativeForce(f);
        }

        public override void TakeDamage()
        {
            Status.TakeDamage();
        }

        public override void TakeDamage(float damage)
        {
            Status.TakeDamage(damage);
        }

        public override void Repair(float dt)
        {
            Status.Update(dt);
        }
    }
}