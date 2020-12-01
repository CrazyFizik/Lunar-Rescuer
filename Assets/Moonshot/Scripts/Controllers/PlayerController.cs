using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Moonshot.Scripts.Spaceship;
using Assets.Moonshot.Scripts.Spaceship.Subsystems;
using Assets.Moonshot.Scripts.UI;
using Assets.Moonshot.Scripts;
using UnityEngine.Rendering.PostProcessing;


namespace Assets.Moonshot.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public bool _isActive = false;
        public bool _isFirstGame = true;

        // Start is called before the first frame update
        public Ship[] _shipsPrefab;
        public SystemWeapon[] _weaponPrefab;
        public Projectile[] _projectilePrefab;

        public Ship _ship;
        public Camera _mainCamera;
        public GUIManager _ingameGUI;
        public FollowCameraSmooth _cameraFollowing;
        public PlayerInputCorvette _corvetInput;
        public PlayerInputFirgate _frigateInput;
        public MissionManager _mission;
        public Transform _launchpad;

        PostProcessVolume _volume;
        Vignette _vignette;
        Bloom _bloom;

        private void Awake()
        {
            StartCoroutine(StartGame());
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_isActive == false)
            {
                return;
            }
            if (_ship == null)
            {
                _isActive = false;
                StartCoroutine(StartGame(5));
                return;
            }
            if (_ship.LifeSupport.Crew == 0)
            {
                _isActive = false;
                StartCoroutine(StartGame(5));
                return;
            }

            VingetteController(_ship);
        }

        IEnumerator StartGame(int i = 0)
        {
            Init();
            if (i > 0)
            {
                for (; i > 0; i--)
                {
                    yield return new WaitForSeconds(1);
                }
            }
            _isActive = true;
            SpawnShip();
        }

        void Init()
        {
            _mainCamera = GetComponentInChildren<Camera>();
            _volume = GetComponentInChildren<PostProcessVolume>();
            _volume.profile.TryGetSettings(out _vignette);
            _vignette.intensity.value = 1f;
            _volume.profile.TryGetSettings(out _bloom);

            _ingameGUI = GetComponentInChildren<GUIManager>();
            //_ingameGUI.enabled = false;

            _cameraFollowing = GetComponentInChildren<FollowCameraSmooth>();
            _cameraFollowing.enabled = false;

            _corvetInput = GetComponentInChildren<PlayerInputCorvette>();
            _corvetInput.enabled = false;

            _frigateInput = GetComponentInChildren<PlayerInputFirgate>();
            _frigateInput.enabled = false;

            _mission = GetComponent<MissionManager>();
            _mission.enabled = false;
        }

        void SpawnShip()
        {
            int shipIndex, projectileIndex, weaponIndex;
            shipIndex = projectileIndex = weaponIndex = 0;
            if (!this._isFirstGame)
            {
                shipIndex = Random.Range(0, _shipsPrefab.Length);
                weaponIndex = Random.Range(0, _weaponPrefab.Length);
                projectileIndex = Random.Range(0, _projectilePrefab.Length);
            }
            else
            {
                this._isFirstGame = false;
            }

            var shipPrefab = _shipsPrefab[shipIndex];
            if (shipPrefab._class == Ship.eClass.Corvette)
            {
                _corvetInput.enabled = true;
                _frigateInput.enabled = false;
            }
            else
            {
                _corvetInput.enabled = false;
                _frigateInput.enabled = true;
            }

            var weaponPrefab = _weaponPrefab[weaponIndex];
            weaponPrefab._projectile = _projectilePrefab[projectileIndex];
            shipPrefab.GetComponent<SystemTacticalComputer>()._gunPrefab = weaponPrefab;
            _ship = shipPrefab.Spawn(shipPrefab, _launchpad.position);

            SetShip();
        }

        void SetShip()
        {
            _ship.Init();

            _ingameGUI.enabled = true;
            _cameraFollowing.enabled = true;
            _mission.enabled = true;
            _ingameGUI.Init(this);
            _cameraFollowing.target = _ship.transform;

            if (_ship._class == Ship.eClass.Corvette)
            {
                _frigateInput.enabled = false;
                _corvetInput.enabled = true;                
                _corvetInput.Ship = _ship.gameObject;
            }
            else
            {
                _corvetInput.enabled = false;
                _frigateInput.enabled = true;
                _frigateInput.Ship = _ship.gameObject;
            }
            
            _isActive = true;
        }

        void VingetteController(Ship ship)
        {
            if (ship.Radar.Status.GetCondition(Spaceship.Subsystems.eSystemState.OFFLINE))
            {
                _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, 1f, 2f * Time.deltaTime);
            }
            else if (ship.Radar.Status.GetCondition(Spaceship.Subsystems.eSystemState.DAMAGED))
            {
                _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, 0.5f, 2f * Time.deltaTime);
            }
            else
            {
                _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, 0.4f, 2f * Time.deltaTime);
            }
        }
    }
}
