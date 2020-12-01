using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class Missile : MonoBehaviour
    {
        [Header("General Paramaters")]
        [Tooltip(" Missile traveling speed")]
        public float _speed = 20f;

        [Tooltip("Initial force before activate the missile")]
        public float _initialLaunchForce = 5f;

        [Tooltip("Missile acceleration during missile motor is active")]
        public float _acceleration = 20f;

        [Tooltip("Motor life time before it stops accelerating")]
        public float _motorLifeTime = 3f;

        [Tooltip("Time for missile automatically explode")]
        public float _missileLifeTime = 30;

        [Tooltip("Missile turn rate towards target")]
        public float _turnRate = 90;

        [Tooltip("Missile range for guidance towards target")]
        public float _viewRange = 120f;

        [Tooltip("Missile view angle in degree for guidance towards target")]
        [Range(0, 360)]
        public float _viewAngle;

        [Tooltip(" Set explosion active delay")]
        public bool _isExplosionActiveDelay;

        [Tooltip("Set tracking delay")]
        public bool _isTrackingDelay;

        [Tooltip("Missile Flame trail")]
        public GameObject _missileFlameTrail;

        [Tooltip("Missile Explsotion GameObject")]
        public GameObject _explosion;

        [Tooltip("Missile launch Sound effect")]
        public AudioSource _launchSFX;

        private bool _targetTracking = false; // Bool to check whether the missile can track the target;
        private bool _missileActive = false; // Bool  to check if missile is active or not;
        private float _missileLaunchTime; // Get missile launch time;
        private bool _motorActive = false; // Bool  to check if motor is active or not;
        private float _motorActiveTime; // Get missile Motor active time;
        private Quaternion _guideRotation; // Store rotation to guide the missile;
        private Rigidbody2D _rb2d;
        private bool _isLaunch;
        private Transform _target;
        private Vector3 _targetlastPosition; // Target last  position in last frame;
        private bool _explosionActive = false; // Bool to activate the explosive; 
        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            if (!_isLaunch)
                _rb2d.isKinematic = true;
        }

        private void FixedUpdate()
        {
            Run();

            if (_target == null) return;

            GuideMissile();
            //AP();// 
        }

        private void OnCollisionEnter(Collision col)
        {
            if (!_explosionActive) return;
            // Detach rocket flame 
            _missileFlameTrail.transform.parent = null;
            Destroy(_missileFlameTrail, 5f);
            // Destroy this missile
            DestroyMissile();
        }

        // This Launch called from "InterceptMissileControlelr"
        public void Launch(Transform target)
        {
            this._target = target;
            _isLaunch = true;
            _rb2d.isKinematic = false;
            _missileLaunchTime = Time.time;

            if (_isExplosionActiveDelay)
                StartCoroutine(ExplosionDelay());
            else
                _explosionActive = true;

            if (_isTrackingDelay)
                StartCoroutine(TrackingDelay());
            else
                _targetTracking = true;
            // missile activation delay
            StartCoroutine(ActiveDelay(1));
        }

        private void Run()
        {
            if (!_isLaunch) return;

            if (!_missileActive) return;

            // Check if missile motor is still active ?
            if (Since(_motorActiveTime) > _motorLifeTime)
                _motorActive = false; // if motor exceed the "MotorActiveTime" duration : motor will be stopped
            else
                _motorActive = true;  // if not : motor continuing running

            // if missile active move it
            if (!_missileActive) return;

            // Keep missile accelerating when motor is still active
            if (_motorActive)
                _speed += _acceleration * Time.deltaTime;

            _rb2d.velocity = transform.forward * _speed;

            // Rotate missile towards target according to "guideRotation" value
            if (_targetTracking)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _guideRotation, _turnRate * Time.deltaTime);

            if (Since(_missileLaunchTime) > _missileLifeTime) // Destroy Missile if it more than live time
                DestroyMissile();

        }

        // Guide missile towards target
        private void PursuiteMissile()
        {
            Vector2 relativePosition = _target.position - transform.position; // Get current relaPosition towards target;
            float angleToTarget = Mathf.Abs(Vector3.Angle(transform.position.normalized, relativePosition.normalized));
            float distance = Vector3.Distance(_target.position, transform.position);

            // target is out of missile's view angle or target distance out of missile's view range
            if (angleToTarget > _viewAngle || distance > _viewRange)
                _targetTracking = false;

            if (!_targetTracking) return;

            relativePosition -= _rb2d.velocity;
            _guideRotation = Quaternion.LookRotation(relativePosition, transform.up);
        }

        private void GuideMissile()
        {
            Vector2 relativePosition = _target.position - transform.position; // Get current relaPosition towards target;
            float angleToTarget = Mathf.Abs(Vector3.Angle(transform.position.normalized, relativePosition.normalized));
            float distance = Vector3.Distance(_target.position, transform.position);

            // target is out of missile's view angle or target distance out of missile's view range
            if (angleToTarget > _viewAngle || distance > _viewRange)
                _targetTracking = false;

            if (!_targetTracking) return;

            // Get target position in one second ahead 
            Vector3 targetSpeed = (_target.position - _targetlastPosition);
            targetSpeed /= Time.deltaTime; // Target distance in one second. Since "Time.deltaTime" = 1/FPS

            // ---------------------------------------------------------------------------------------------
            // Calculate the the lead target position based on target speed and projectileTravelTime to reach the target

            // Get time to hit based on distance
            float speedPrediction = _speed + _acceleration * Since(_missileLaunchTime);
            float travelTime = distance / speedPrediction;

            // Lead Position based on target position prediction within impact time
            Vector3 targetFuturePosition = _target.position + targetSpeed * travelTime;
            Vector3 aimPosition = targetFuturePosition - transform.position;

            // During Rotation get target 90% in "MissileViewAngle" sinse positionToGo will likely out of "MissileViewAngle"
            relativePosition = Vector3.RotateTowards(relativePosition.normalized, aimPosition.normalized, _viewAngle * Mathf.Deg2Rad * 0.9f, 0f);
            _guideRotation = Quaternion.LookRotation(relativePosition, transform.up);

            _targetlastPosition = _target.position;

        }

        private Vector3 los = Vector3.zero;
        private void NavigatedMissile()
        {
            Vector3 relativePosition = _target.position - transform.position; // Get current relaPosition towards target;
            float angleToTarget = Mathf.Abs(Vector3.Angle(transform.position.normalized, relativePosition.normalized));
            float distance = Vector3.Distance(_target.position, transform.position);

            // target is out of missile's view angle or target distance out of missile's view range
            if (angleToTarget > _viewAngle || distance > _viewRange)
                _targetTracking = false;

            if (!_targetTracking) return;

            // Get target position in one second ahead 
            // Proportional Navigation evaluates the rate of change of the 
            // Line Of Sight (los) to our target. If the rate of change is zero,
            // the missile is on a collision course. If it is not, we apply an
            // acceleration to correct course.
            Vector3 prevLos = los;
            los = _target.position - transform.position;
            Vector3 dLos = los - prevLos;

            // we only want the component perpendicular to the line of sight
            dLos = dLos - Vector3.Project(dLos, los);

            // plain PN would be:
            Vector3 acceleration = Time.fixedDeltaTime * los + dLos * 5;

            _guideRotation = Quaternion.LookRotation(acceleration, transform.up);

            // ANother

        }

        IEnumerator ExplosionDelay()
        {
            yield return new WaitForSeconds(2);
            _explosionActive = true;
        }

        IEnumerator TrackingDelay()
        {
            yield return new WaitForSeconds(2);
            _targetTracking = true;
        }

        IEnumerator ActiveDelay(float time)
        {
            // Put initial speed to missile before activate 
            _rb2d.velocity = transform.forward * _initialLaunchForce;
            yield return new WaitForSeconds(time);
            ActivateMissile();
        }

        // Activate missile
        private void ActivateMissile()
        {
            _missileActive = true;
            _motorActive = true;
            _motorActiveTime = Time.time;
            _missileFlameTrail.SetActive(true);
            _launchSFX.Play();
        }

        // Get the "Since" time from the input/parameter value
        private float Since(float Since)
        {
            return Time.time - Since;
        }

        // Destroy Missile
        private void DestroyMissile()
        {
            Destroy(gameObject);
            Instantiate(_explosion, transform.position, transform.rotation);
        }

    }
}
