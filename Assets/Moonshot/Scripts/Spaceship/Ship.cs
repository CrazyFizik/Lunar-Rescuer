using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Moonshot.Scripts.Controllers;
using Assets.Moonshot.Scripts.Spaceship.Subsystems;

namespace Assets.Moonshot.Scripts.Spaceship
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Damager))]
    [RequireComponent(typeof(SystemDamageControl))]
    [RequireComponent(typeof(SystemLifeSupport))]
    [RequireComponent(typeof(SystemFlightComputer))]
    [RequireComponent(typeof(SystemMainEngine))]
    [RequireComponent(typeof(SystemRCS))]
    [RequireComponent(typeof(SystemFuelTank))]
    [RequireComponent(typeof(SystemTacticalComputer))]
    [RequireComponent(typeof(SystemRadar))]
    public class Ship : MonoBehaviour, IControls
    {
        public enum eClass {Corvette, Frigate, Cruiser }

        [SerializeField] public eClass _class = eClass.Corvette;
        [SerializeField] public eTeam  _team = eTeam.RED;
        [SerializeField] public bool   _init = false;
        [SerializeField] public Controls _controls = new Controls();

        public bool IsInit
        {
            get
            {
                return _init;
            }
        }

        public eTeam Team
        {
            get
            {
                return _team;
            }
        }

        public Rigidbody2D Body
        {
            get
            {
                return GetComponent<Rigidbody2D>();
            }
        }

        public Controls Controls 
        { 
            get
            {
                return _controls;
            }
            set
            {
                _controls = value;
            }
        }

        public SystemLifeSupport LifeSupport
        {
            get
            {
                return GetComponent<SystemLifeSupport>();
            }
        }

        public SystemDamageControl DamageControl
        {
            get
            {
                return GetComponent<SystemDamageControl>();
            }
        }

        public SystemFlightComputer FlightComputer
        {
            get
            {
                return GetComponent<SystemFlightComputer>();
            }
        }

        public SystemTacticalComputer TacticalComputer
        {
            get
            {
                return GetComponent<SystemTacticalComputer>();
            }
        }

        public SystemRadar Radar
        {
            get
            {
                return GetComponent<SystemRadar>();
            }
        }

        public AudioSource AudioSource
        {
            get
            {
                return GetComponent<AudioSource>();
            }
        }

        public static List<Ship> _ships = new List<Ship>();

        private List<Subsystems.Subsystem> _subsystems = new List<Subsystems.Subsystem>();
        public List<Subsystems.Subsystem> Subsustems
        {
            get
            {
                return _subsystems;
            }
            set
            {
                _subsystems = value;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!IsInit) return;

            DamageControl.Update(Time.deltaTime);

            //if (LifeSupport.Status.GetCondition(eSystemState.OFFLINE) || LifeSupport.Status.GetCondition(eSystemState.INVALID))
            //{
            //    Controls.attack = false;
            //    Controls.special = false;
            //}
            
            //TacticalComputer.SetInput(Controls.attack, Controls.special, null);
        }


        void FixedUpdate()
        {            
            if (!IsInit) return;

            if (LifeSupport.Status.GetCondition(eSystemState.OFFLINE) || LifeSupport.Status.GetCondition(eSystemState.INVALID))
            {
                Controls.movement =  Vector2.zero;
                Controls.turn = 0f;
            }

            FlightComputer.SetInput(Controls.movement.x, Controls.turn);

            if (LifeSupport.Status.GetCondition(eSystemState.OFFLINE) || LifeSupport.Status.GetCondition(eSystemState.INVALID))
            {
                Controls.attack = false;
                Controls.special = false;
            }

            TacticalComputer.SetInput(Controls.attack, Controls.special, null);
        }

        private void Floating(float size)
        {
            var pos = transform.position;
            if (pos.x >= size/2f)
            {
                pos.x = pos.x - size;
                transform.position = pos;
            }
            else if (pos.x <= -size/2f)
            {
                pos.x = pos.x + size;
                transform.position = pos;
            }
        }

        public Ship Spawn(Ship shipPrefab, Vector2 position)
        {
            var ship = Instantiate<Ship>(shipPrefab, position, Quaternion.Euler(0,0,90));
            ship.Register();
            //ship.GetComponent<SystemTacticalComputer>()._gunPrefab = weaponPrefab;
            return ship;
        }

        public void Despawn()
        {

        }

        public void Init()
        {            
            StartCoroutine(FadeInit());
            
            //var subsystems = GetComponentsInChildren<Subsystems.Subsystem>();
            //foreach (var subsystem in subsystems)
            //{
            //    Subsustems.Add(subsystem);
            //    subsystem.Init();
            //}
            //DamageControl.Init();

            //Controls = new Controls();

            //_init = true;
        }

        public void Register()
        {
            _ships.Add(this);
        }

        public void Deregister()
        {
            Ship._ships.Remove(this);
        }

        [System.Obsolete("Проблема с пушкой, которая создается TCS")]
        IEnumerator FadeInit()
        {
            Debug.Log("Start system init");
            _init = false;
            Subsustems = new List<Subsystems.Subsystem>();
            var subsystems = GetComponentsInChildren<Subsystems.Subsystem>();
            foreach (var subsystem in subsystems)
            {                
                Subsustems.Add(subsystem);
                subsystem.Init();
                subsystem.Status.State = eSystemState.OFFLINE;
            }
            DamageControl.Init();
            foreach (var subsystem in subsystems)
            {
                subsystem.Status.State = eSystemState.ONLINE;
                yield return new WaitForSeconds(0.25f);
            }
            Controls = new Controls();
            _init = true;
            Debug.Log("End system init");
        }
    }
}