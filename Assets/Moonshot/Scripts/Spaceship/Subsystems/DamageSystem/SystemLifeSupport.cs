using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemLifeSupport :  Subsystem
    {
        [SerializeField] string _name = "Life Support";
        [SerializeField] string _type = "CRW";

        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        [SerializeField] SystemState _systemState = new SystemState();

        [SerializeField] int _crewNumberMax = 2;
        [SerializeField] int _crewNumberCurrent = 2;

        private Ship _ship;
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

        public int Crew
        {
            get
            {
                return _crewNumberCurrent;
            }
            set
            {
                _crewNumberCurrent = value;
            }
        }

        public int CrewMax
        {
            get
            {
                return _crewNumberMax;
            }
            set
            {
                _crewNumberMax = value;
            }
        }

        public override string Name { get => _name; set => _name = value; }
        public override string Type { get => _type; set => _type = value; }

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
            _crewNumberCurrent = _crewNumberMax;
            Status.Initialize(this);
        }

        public override void TakeDamage()
        {
            Status.TakeDamage();
            if (Status.GetCondition(eSystemState.OFFLINE))
            {
                _crewNumberCurrent -= 1;
                if (_crewNumberCurrent <=0)
                {
                    _crewNumberCurrent = 0;
                    Status.State = eSystemState.INVALID;
                }
            }
        }

        public override void TakeDamage(float damage)
        {
            Status.TakeDamage(damage);
            if (Status.GetCondition(eSystemState.OFFLINE))
            {
                _crewNumberCurrent -= 1;
                if (_crewNumberCurrent <= 0)
                {
                    _crewNumberCurrent = 0;
                    Status.State = eSystemState.INVALID;
                }
            }
        }

        public override void Repair(float dt)
        {
            Status.Update(dt);
        }

    }
}