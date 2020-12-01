using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemFlightComputer : Subsystem
    {
        [SerializeField] string _name = "Flight Computer";
        [SerializeField] string _type = "FCS";

        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        [SerializeField] SystemState _systemState = new SystemState();
        [Header("Flight Data")]
        [SerializeField] FlightData _flightData = new FlightData();

        private Ship _ship;
        private Rigidbody2D _rb2d;
        private SystemFuelTank _fuel;
        private SystemMainEngine _mainEngine;
        private SystemRCS _rcs;

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

        public SystemMainEngine MainEngine
        {
            get
            {
                return _mainEngine;
            }
            set
            {
                _mainEngine = value;
            }
        }
        public SystemRCS RCS
        {
            get
            {
                return _rcs;
            }
            set
            {
                _rcs = value;
            }
        }

        public FlightData Data
        {
            get
            {
                return _flightData;
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

        private void FixedUpdate()
        {
            if (Status.GetCondition(eSystemState.OFFLINE) || Status.GetCondition(eSystemState.INVALID)) return;
            _flightData.Update(Ship, Time.fixedDeltaTime);
        }

        public override void Init()
        {
            Ship = GetComponent<Ship>();
            Body = GetComponent<Rigidbody2D>();
            FuelSystem = GetComponent<SystemFuelTank>();
            MainEngine = GetComponent<SystemMainEngine>();
            RCS = GetComponent<SystemRCS>();

            MainEngine.Init();
            RCS.Init();

            Status.Initialize(this);
        }

        public void SetInput(float movement, float rotation)
        {
            if (Status.GetCondition(eSystemState.OFFLINE) || Status.GetCondition(eSystemState.INVALID))
            {
                movement = 0f;
                rotation = 0f;
            }

            RCS.SetRotation(rotation);
            MainEngine.SetThrottle(movement);
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