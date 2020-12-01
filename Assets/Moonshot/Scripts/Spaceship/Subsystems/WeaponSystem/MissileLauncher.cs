using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class MissileLauncher : Subsystem, IWeapon
    {
        [SerializeField] string _name = "Missile Launcher";
        [SerializeField] string _type = "MSL";

        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        public override string Name { get => _name; set => _name = value; }
        public override string Type { get => _type; set => _type = value; }

        public override Ship Ship { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public AudioSource AudiouOutput { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public AudioClip ShootSound => throw new System.NotImplementedException();

        public override SystemState Status { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


        [Tooltip("Missile Fire Rate / how frequently to launch the missile")]
        public float _fireRate = 1f;

        [Tooltip("Target Transform")]
        public Transform _target;

        [Tooltip("Missile to instantiate")]
        public Missile _missile;

        [Header("Missile settings")]
        [Tooltip("How many missile in turret")]
        public float MissileCount;

        [Tooltip("Position for missile to launch")]
        public Transform[] _launchSpot;

        private bool _fire; // Get value from user whether to fire or not
        private float _nextlaunch; // Store next "MissileFireRate" 

        [HideInInspector]
        public float loadedMissileCount; // Count of loaded missile on launcher

        private List<Missile> loadedMissile = new List<Missile>(); // loaded missile list on launcher

        private void Update()
        {
            // Only fire the missile when user give some input and time more than next launch time;
            if (_fire && Time.time >= _nextlaunch)
            {
                _nextlaunch = Time.time + _fireRate;
            }
        }

        public override void Init()
        {
            _state = eSystemState.ONLINE;
        }

        IEnumerator RespawnMissile()
        {
            yield return new WaitForSeconds(1f / _fireRate);
            if (MissileCount <= 0) yield return 0;
            SpawnMissile();
        }

        private void SpawnMissile()
        {
            if (_launchSpot.Length == 0) // check for missile launchSpot
            {
                Debug.Log("No LaunchSpot found, Please drag it into this script");
            }

            foreach (Transform spot in _launchSpot)
            {
                if (MissileCount <= 0) return;
                Missile newMissile = Instantiate(_missile, spot.position, spot.rotation);
                newMissile.transform.parent = spot;

                Vector3 offset = new Vector3(0, 8, -0.3f);
                newMissile.transform.localPosition = offset; // Note: optional position

                loadedMissile.Add(newMissile);
                //CameraManager.CameraTargets.Add(newMissile.transform); // just for missile camera
                loadedMissileCount++;
                MissileCount--;
            }
        }

        public void SetTargetMissile(Transform targetPosition)
        {
            this._target = targetPosition;
            Launch(targetPosition);
        }

        private void Launch(Transform targetPosition)
        {
            loadedMissile[(int)loadedMissileCount - 1].Launch(targetPosition); // Launch missile according to its sequence in list
            loadedMissile[((int)loadedMissileCount - 1)].transform.parent = null;
            loadedMissile.Remove(loadedMissile[((int)loadedMissileCount - 1)]); // Remove missile from loaded missile list
            loadedMissileCount--;

            if (loadedMissileCount <= 0)
                StartCoroutine(RespawnMissile()); //if loaded missile on launcher is null Respawn
        }

        public void Fire(Transform target)
        {
            SetTargetMissile(target);
        }

        public void Release()
        {
            //throw new NotImplementedException();
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