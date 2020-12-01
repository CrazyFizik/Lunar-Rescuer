using UnityEngine;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class SystemTacticalComputer : Subsystem
    {
        [SerializeField] string _name = "Tactical Computer";
        [SerializeField] string _type = "TCS";

        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        [SerializeField] SystemState _systemState = new SystemState();
        [SerializeField] Transform _gunMountPoint;
        [SerializeField] public SystemWeapon _gunPrefab;

        Ship _ship;
        IWeapon _gun;

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

            if (_gunPrefab != null)
            {
                _gun = Instantiate<SystemWeapon>(_gunPrefab.GetComponent<SystemWeapon>(), _gunMountPoint.position, _gunMountPoint.transform.rotation, _gunMountPoint);
                (_gun as Subsystem).Init();
            }

            Status.Initialize(this);
        }

        public void SetInput(bool primary, bool secondary, Transform target = null)
        {
            if (Status.GetCondition(eSystemState.OFFLINE)) return;

            if (_gun != null) WeaponOperation(_gun, primary, target);
            //if (_launcher != null) WeaponOperation(_launcher, secondary, target);
        }

        private void WeaponOperation(IWeapon weapon, bool fire, Transform target = null)
        {
            if (fire)
            {
                weapon.Fire(target);
            }
            else
            {
                weapon.Release();
            }
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

        public static bool ComputeFiringSolution(
              Vector3 shooterPosition,
              Vector3 shooterVelocity,
              Vector3 targetPosition,
              Vector3 targetVelocity,
              float projectileSpeed,
              out Vector3 solution,
              out float timeToTarget)
        {
            return ComputeFiringSolution(targetPosition - shooterPosition, targetVelocity - shooterVelocity, projectileSpeed, out solution, out timeToTarget);
        }

        public static bool ComputeFiringSolution(
          Vector3 deltaPosition,
          Vector3 deltaVelocity,
          float projectileSpeed,
          out Vector3 solution,
          out float timeToTarget)
        {
            float a = deltaVelocity.sqrMagnitude - projectileSpeed * projectileSpeed;
            float b = 2f * Vector3.Dot(deltaPosition, deltaVelocity);
            float c = deltaPosition.sqrMagnitude;
            float D = b * b - 4.0f * c * a;

            if (D < 0.0f)
            {
                solution = Vector3.zero;
                timeToTarget = 0.0f;
                return false;
            }

            float t1 = -b - 0.5f * Mathf.Sqrt(D) / a;
            float t2 = -b + 0.5f * Mathf.Sqrt(D) / a;
            if (t1 > t2)
            {
                float temp = t2;
                t2 = t1;
                t1 = temp;
            }
            if (t1 >= 0.0f)
            {
                timeToTarget = t1;
            }
            else if (t2 >= 0.0f)
            {
                timeToTarget = t2;
            }
            else
            {
                solution = Vector3.zero;
                timeToTarget = 0.0f;
                return false;
            }
            if (timeToTarget <= Time.fixedDeltaTime)// 0.00999999977648258f)
            {
                solution = deltaPosition.normalized;
                timeToTarget = 0.0f;
            }
            else
            {
                solution = deltaPosition / timeToTarget + deltaVelocity;
                solution.Normalize();
            }
            return true;
        }
    }
}