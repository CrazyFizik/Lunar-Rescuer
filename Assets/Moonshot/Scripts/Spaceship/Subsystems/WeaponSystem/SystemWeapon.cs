using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemWeapon : Subsystem, IWeapon
    {
        [SerializeField] string _name = "Gun";
        [SerializeField] string _type = "GUN";

        [SerializeField] SystemState _state = new SystemState();
        [SerializeField] string _weaponName = "Weapon";
        [SerializeField] Transform _muzzlePoint;

        [Header("Ammo")]
        [SerializeField] public Projectile _projectile;
        [SerializeField] int _capicity = -1;

        [Header("Specifications")]
        [SerializeField] eROF _rof = eROF.Auto;
        [SerializeField] int _rpm = 60;
        [SerializeField] float _range = 20f;
        [SerializeField] float _accuracy = 0.5f;
        [SerializeField] float _speedModifier = 1f;
        [SerializeField] float _damageModifier = 1f;

        [Header("Audio")]
        [SerializeField] AudioClip _shootSound;

        Ship _owner;
        Transform _transform;
        Vector2 _inheritedPosition = Vector2.zero;
        [SerializeField] Vector2 _inheritedVelocity = Vector2.zero;

        float _dt = 0.1f;
        float _timer = 0f;
        bool _trigger = false;
        bool _fire = false;

        public override string Name { get => _name; set => _name = value; }
        public override string Type { get => _type; set => _type = value; }

        public override Ship Ship 
        { 
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }

        public AudioSource AudiouOutput 
        { 
            get
            {
                return Ship.AudioSource;
            }
        }

        public AudioClip ShootSound
        {
            get
            {
                return _shootSound;
            }
        }

        public override SystemState Status
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
        }

        void FixedUpdate()
        {
            var position = (Vector2)_muzzlePoint.position;
            _inheritedVelocity = (position - _inheritedPosition) / Time.fixedDeltaTime;
            _inheritedPosition = position;
        }

        public override void Init()
        {
            _transform = transform;
            _dt = 60f / _rpm;
            _inheritedPosition = _muzzlePoint.position;
            Ship = GetComponentInParent<Ship>();
            Status.Initialize(this);
        }

        private void ShootFX()
        {
            if (AudiouOutput == null) return;
            AudiouOutput.PlayOneShot(ShootSound);
        }

        public void Fire(Transform target)
        {
            if (Status.GetCondition(eSystemState.OFFLINE)) return;

            bool fire = (_timer <= 0) & (!_fire) & (!_trigger);
            if (!fire) return;
            
            int rounds = (int)_rof;
            if (rounds > 0)
            {
                var shoot = ShootRoutine(rounds);
                _timer = rounds * _dt;
                StartCoroutine(shoot);
            }
            else
            {
                _timer = _dt;
                Shoot();
            }
        }

        public void Release()
        {
            _trigger = false;
        }

        IEnumerator ShootRoutine(int rounds)
        {
            _trigger = true;
            _fire = true;
            Shoot();
            if (rounds > 1)
            {
                float dt = _dt;
                for (int i = 0; i < rounds - 1; i++)
                {
                    yield return new WaitForSeconds(dt);
                    Shoot();
                }
            }
            _fire = false;
        }

        private void Shoot()
        {
            if (_capicity != 0)
            {
                var direction = GetDirection();
                CreateBullet(direction);
                ShootFX();
                if (_capicity >0) _capicity--;
            }
        }

        private Vector2 GetDirection()
        {
            var direction = _transform.right * _range;
            direction.x += Random.Range(-_accuracy, _accuracy);
            direction.y += Random.Range(-_accuracy, _accuracy);

            return direction;
        }

        private void CreateBullet(Vector2 direction)
        {
            var r = _owner.FlightComputer.FuelSystem.GetFuelFlowRatio(_projectile.GetComponent<Rigidbody2D>().mass);
            if (r < 1f) return;

            var rotation = Quaternion.FromToRotation(Vector2.right, direction);
            var position = _muzzlePoint.transform.position;
            _projectile.Spawn(position, rotation, _inheritedVelocity, _speedModifier, _damageModifier);
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