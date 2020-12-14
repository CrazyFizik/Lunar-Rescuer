using System;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Controllers
{
    public class PlayerInputFirgate : PlayerInput
    {
        public Vector2 _forward = Vector2.right;
        public GameObject _ship;
        public IControls _shipControls;

        private float _lastClickTime;
        public float _catchTime = 0.25f;
        public Controls _controls = new Controls();
        
        public float _turnRate = 90f;
        public float _rotationSensetivity = 0.25f;
        float _rotationVelocity;

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
                    _shipControls.Controls = _controls = new Controls();
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

        }

        private void FixedUpdate()
        {

        }

        private void LateUpdate()
        {
            UpdateControls();
            SetControls();
        }

        void UpdateControls()
        {            
            UpdateButtons();
            UpdateSteering();
        }

        void UpdateSteering()
        {
            // Вычисляем движение
            float strafe = 0f;
            if (Input.GetKey(KeyCode.E))
            {
                strafe -= 1f;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                strafe += 1f;
            }
            float forward = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(forward, strafe);
            movement.Normalize();

            // Вычисляем направление
            Vector2 targeting = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = targeting;
            direction.x -= _ship.transform.position.x;
            direction.y -= _ship.transform.position.y;

            //Вычисляем поворот в соответсвии с положением указателя мыши
            var turn = -Input.GetAxis("Horizontal") * _turnRate;
            turn = Mathf.SmoothDamp(turn, turn, ref _rotationVelocity, _rotationSensetivity);

            // Присваиваем направления
            _controls.turn = turn; 
            _controls.spot = targeting;
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
