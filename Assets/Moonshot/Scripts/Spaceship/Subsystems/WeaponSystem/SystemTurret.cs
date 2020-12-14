using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemTurret : Subsystem
    {
        [SerializeField] string _name = "Turret";
        [SerializeField] string _type = "TRT";
        [SerializeField] SystemState _state = new SystemState();
        [SerializeField] float _turnRate = 360f;
        [SerializeField] float _turnRateDamaged = 270f;

        private Ship _ship;

        public override string Name { get => _name; set => _name = value; }
        public override string Type { get => _type; set => _type = value; }

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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Init()
        {
            Ship = GetComponentInParent<Ship>();
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

        public void RotateTo(Vector2 target, float turnRate, float dt)
        {
            var direction = target - (Vector2)transform.position;
            var absoluteAngle = Vector2.SignedAngle(Vector2.right, direction);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, absoluteAngle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnRate * dt);
        }

        private void FixedUpdate()
        {
            if (Status.GetCondition(eSystemState.ONLINE))
            {
                RotateTo(Ship.Controls.target, _turnRate, Time.fixedDeltaTime);
            }
            else if (Status.GetCondition(eSystemState.DAMAGED))
            {
                RotateTo(Ship.Controls.target, _turnRateDamaged, Time.fixedDeltaTime);
            }
        }
    }
}