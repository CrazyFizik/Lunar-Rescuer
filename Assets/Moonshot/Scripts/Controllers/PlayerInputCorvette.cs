using System;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Controllers
{
    public class PlayerInputCorvette : PlayerInput
    {
        [SerializeField] private Vector2 _forward = Vector2.right;        
        [SerializeField] private float _catchTime = 0.25f;
        [SerializeField] private float _lastClickTime;
        [SerializeField] private Controls _controls = new Controls();
        [SerializeField] private GameObject _ship;
        [SerializeField] private IControls _shipControls;

        public override GameObject Ship
        {
            get
            {
                return _ship;
            }
            set
            {
                _ship = value;
                if (_ship != null)
                {
                    _shipControls = _ship.GetComponent<IControls>();
                    _controls = _shipControls.Controls = new Controls();
                }
            }
        }

        public override IControls ShipControls => _shipControls;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateControls();
            SetControls();
        }

        private void FixedUpdate()
        {

        }

        private void LateUpdate()
        {
            
        }

        void UpdateControls()
        {
            if (_ship == null) return;

            UpdateButtons();
            UpdateSteering();            
        }

        void UpdateSteering()
        {
            // Вычисляем движение
            float strafe = -Input.GetAxisRaw("Horizontal");
            float forward = Input.GetAxisRaw("Vertical");
            if (Input.GetButton("Fire2")) forward = 1f; 
            Vector2 movement = new Vector2(forward, strafe);
            movement.Normalize();

            // Вычисляем направление
            Vector2 targeting = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = targeting;
            direction.x -= _ship.transform.position.x;
            direction.y -= _ship.transform.position.y;

            //Вычисляем поворот в соответсвии с положением указателя мыши
            var turn = Vector2.SignedAngle(_ship.transform.TransformDirection(_forward), direction);

            // Присваиваем направления
            _controls.turn = turn;
            //_controls.spot = targeting;
            if (Input.GetButton("Fire2")) _controls.spot = direction;
            _controls.movement = movement;
            _controls.direction = direction;
            _controls.target = targeting;
        }

        void UpdateButtons()
        {
            // Проверяем кнопки
            bool fire = Input.GetButton("Fire1");
            bool special = Input.GetButtonUp("Fire2");
            bool run = Input.GetButton("Fire3");
            bool ready = Input.GetButtonDown("Jump");

            // Doble click
            bool dclick = false;
            if (Input.GetButtonDown("Fire1"))
            {
                if (Time.time - _lastClickTime < _catchTime)
                {
                    //double click
                    dclick = true;
                }
                else
                {
                    //normal click
                }
                _lastClickTime = Time.time;
            }

            // Присваеваем кнопки
            _controls.attack = fire;
            _controls.special = special;
            _controls.run = run;
            if (ready)
            {
                _controls.ready = !_controls.ready;
            }
            else if (dclick && !_controls.ready)
            {
                //_controls.ready = dclick;
            }
        }

        void SetControls()
        {
            _shipControls.Controls = _controls;
        }
    }
}
