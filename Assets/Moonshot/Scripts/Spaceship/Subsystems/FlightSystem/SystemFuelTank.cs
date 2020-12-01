using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemFuelTank : Subsystem
    {
        [SerializeField] string _name = "Fuel Tankt";
        [SerializeField] string _type = "FUE";

        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        [SerializeField] SystemState _systemState = new SystemState();
        [Range(0f, 1f)]
        [SerializeField] private float _fuelRatio = 1f;
        [SerializeField] private float _maxFuel = 100f;
        [SerializeField] private float _currentFuel = 100;
        [SerializeField] private float _damagedPumpLeakRatio = 0.1f;
        [SerializeField] private float _offlineLeakRate = 1f;

        private Ship _ship;

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

        public float FuelRatio
        {
            get
            {
                return _fuelRatio;
            }
            private set
            {
                _fuelRatio = value;
            }
        }

        public float Fuel
        {
            get
            {
                return _currentFuel;
            }
            set
            {
                _currentFuel = value;
            }
        }

        public float FuelMax
        {
            get
            {
                return _maxFuel;
            }
            private set
            {
                _maxFuel = value;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            _state = Status.State;

            if (Status.GetCondition(eSystemState.OFFLINE))
            {
                _currentFuel -= _offlineLeakRate * Time.deltaTime;
            }

            FuelRatio = _currentFuel / _maxFuel;
        }

        public float GetFuelFlowRatio(float consumption)
        {
            if (Status.GetCondition(eSystemState.OFFLINE) || Status.GetCondition(eSystemState.INVALID))
            {
                return 0f;
            }
            else if (Status.GetCondition(eSystemState.DAMAGED))
            {
                consumption = consumption * (1f + _damagedPumpLeakRatio);
            }

            float ratio = 1f;
            if (consumption > _currentFuel)
            {
                ratio = _currentFuel / consumption;
                consumption = _currentFuel;
            }
            _currentFuel -= consumption;
            return Mathf.Clamp01(ratio);
        }

        public override void Init()
        {
            Ship = GetComponent<Ship>();
            _currentFuel = _fuelRatio * _maxFuel;
            Status.Initialize(this);
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