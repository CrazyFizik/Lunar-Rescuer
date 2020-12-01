using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Moonshot.Scripts.FX;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemDamageControl : Subsystem, IDamageReciver
    {
        [SerializeField] string _name = "Damage Control";
        [SerializeField] string _type = "DCS";
        [SerializeField] ExplosionFX _exploisonFx;

        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        [SerializeField] SystemState _systemState = new SystemState();

        [SerializeField] float _health = 1000f;
        [SerializeField] float _maxHealth = 1000f;

        [SerializeField] float _repearRate = 1f;
        [SerializeField] float _damagedRepairRate = 0.5f;

        private Ship _ship;
        private List<Subsystem> _systems;

        public override string Name { get => _name; set => _name = value; }
        public override string Type { get => _type; set => _type = value; }

        public float RepairRate
        {
            get
            {
                if (Status.GetCondition(eSystemState.DAMAGED) || Status.GetCondition(eSystemState.OFFLINE))
                {
                    return _repearRate;
                }
                return _damagedRepairRate;
            }
        }

        public List<Subsystem> Systems
        {
            get
            {
                return _systems;
            }
            private set
            {
                _systems = value;
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

        public float CurrentHealth
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        public float MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            private set
            {
                _maxHealth = value;
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
            _systems = new List<Subsystem>();
            var systems = GetComponentsInChildren<Subsystem>();
            var sum = 0f;
            foreach(var sys in systems)
            {
                sum += sys.Status.Weight;
                _systems.Add(sys);
            }
            _systems.Sort((x, y) => x.Status.Weight.CompareTo(y.Status.Weight));
            //_systems.Reverse();
            //_systems.Sort((a, b) => 1 - 2 * UnityEngine.Random.Range(0, 2));
            foreach (var sys in _systems)
            {
                sys.Status.Weight = sys.Status.Weight / sum;
            }

            this._health = this._maxHealth;
            Status.Initialize(this);
        }

        public void Update(float dt)
        {
            if (Status.GetCondition(eSystemState.INVALID))
                return;
            
            if (Status.GetCondition(eSystemState.OFFLINE))
            {
                this.Repair(dt * RepairRate);
                return;
            }

            foreach (var sys in _systems)
            {
                sys.Repair(dt * RepairRate);
            }
        }

        public void Damage(int damage)
        {            
            if (damage <= 0f) return;
            //Debug.Log("Damage = " + damage.ToString());

            this._health -= damage;
            if (_health <= 0)
            {
                _health = 0f;
                Destruction();
                return;
            }

            var N = this._systems.Count;
            var healthRatio = 1.0f - this._health / this._maxHealth;
            var damageRatio = damage / this._maxHealth;
            var numberOfHits = Mathf.RoundToInt(2 * N * healthRatio);
            //var numberOfHits = Mathf.RoundToInt(20 * damageRatio);
            if (numberOfHits < 1) return;
            //Debug.Log("Number of hits = " + numberOfHits.ToString());
            var p = 0f;
            for (int i = 0; i < numberOfHits; i++)
            {                
                for (int j = 0; j < N; j++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, N);
                    p += this._systems[randomIndex].Status.Weight;
                    if (UnityEngine.Random.value <= p)
                    {
                        this._systems[randomIndex].TakeDamage();
                        break;
                    }
                }
            }

            // roulette wheel
            //var N = this._systems.Count;
            //var probability = 1.0f - this._health / this._maxHealth;
            //var i = UnityEngine.Random.Range(0, N);
            //for (int j = 0; j < damage; ++j)
            //{
            //    for (float chance = 0.0f; chance < 1.0f; chance += 1f / N)
            //    {
            //        if (UnityEngine.Random.value < this._systems[i].Status.Weight + probability - chance)
            //        {
            //            this._systems[i].TakeDamage();
            //            break;
            //        }
            //        i = UnityEngine.Random.Range(0, N);
            //    }
            //}

            //var N = this._systems.Count;
            //var damageRatio = 1.0f - this._health / this._maxHealth;
            //var chance = 0f;
            //for (int i = 1; i < N; i++)
            //{
            //    chance += this._systems[i].Weight;
            //    if (UnityEngine.Random.value <= chance)
            //    {
            //        if (UnityEngine.Random.value < damageRatio)
            //        {
            //            this._systems[i].TakeDamage(damage);
            //        }
            //    }
            //}
        }

        public void Hit(int damage)
        {
            Damage(damage);
        }

        public void Hit(float energy)
        {
            var damage = Mathf.RoundToInt(energy);
            Damage(damage);
        }        

        public void Destruction()
        {            
            foreach (var sys in this._systems)
            {
                sys.Status.State = eSystemState.INVALID;
            }
            if (_exploisonFx != null) _exploisonFx.Spawn(transform.position, transform.rotation);
            Ship.Deregister();
            Destroy(gameObject, 0.05f);
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

        private float _repairTime = 10f;
        private float _repairTimer = 0f;
        public LunarBase _base;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (Ship == null) return;
            if (!Ship.IsInit) return;

            var lunarBase = collision.rigidbody.gameObject.GetComponent<LunarBase>();
            if (lunarBase != null)
            {
                if (lunarBase != _base)
                {
                    _base = lunarBase;
                    _repairTimer = 0f;
                }

                var fuelSystem = Ship.FlightComputer.FuelSystem;
                if (fuelSystem != null)
                {
                    fuelSystem.Fuel = Mathf.Lerp(fuelSystem.Fuel, fuelSystem.FuelMax, RepairRate * Time.fixedDeltaTime);
                }

                var damageSystem = Ship.DamageControl;
                if (damageSystem != null)
                {
                    damageSystem.CurrentHealth = Mathf.Lerp(damageSystem.CurrentHealth, damageSystem.MaxHealth, RepairRate * Time.fixedDeltaTime);
                }

                var repairTime = 0f;
                foreach (var sys in Ship.Subsustems)
                {
                    if (!sys.Status.GetCondition(eSystemState.ONLINE)) repairTime += 1f;
                }
                _repairTime = repairTime + 1f;
                _repairTimer += RepairRate * Time.fixedDeltaTime;
                if (_repairTimer > _repairTime & repairTime > 0)
                {
                    _repairTime = 0f;
                    Ship.Init();
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.rigidbody.gameObject.GetComponent<LunarBase>() == _base)
            {
                _repairTimer = 0f;
                _base = null;
            }
        }
    }
}