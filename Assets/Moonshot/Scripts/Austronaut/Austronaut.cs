using UnityEngine;
using System.Collections;
using Assets.Moonshot.Scripts.Spaceship;

namespace Assets.Moonshot.Scripts.Austronaut
{
    public class Austronaut : MonoBehaviour, IDamageReciver
    {
        public eTeam _team = eTeam.RED;
        public float _health = 20;
        public float _speed = 1f;

        public Rigidbody2D _rb2d;
        public Animator _animator;
        public Vector2 _direction = Vector2.right;
        public float _aiDistance = 100f;
        public Transform _target;

        public void Destruction()
        {
            Destroy(this.gameObject);
        }

        public void Hit(float energy)
        {
            Hit(Mathf.RoundToInt(0.1f * energy));
        }

        public void Hit(int damage)
        {
            _health -= damage;
            if (_health < 0) _health = 0;
            Destruction();
        }

        public void Move(Vector2 movement)
        {
            _direction = movement;
            Turn(movement.x);
            _animator.Play("Move");
            //_rb2d.MovePosition((Vector2)transform.position + _speed * _direction * Time.deltaTime);

            var targetSpeed = _speed * _direction - _rb2d.velocity;
            _rb2d.AddForce(targetSpeed * _rb2d.mass);
        }

        public void Turn(float direction)
        {
            if (direction * transform.localScale.x >= 0) return;
            direction = Mathf.Sign(direction);
            transform.localScale = new Vector3(direction, 1, 1);
        }

        // Use this for initialization
        void Start()
        {
            _target = null;
            _animator = GetComponent<Animator>();
            _rb2d = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_target != null)
            {
                MoveToTarget();
            }
            if (_target == null)
            {
                SearchTarget();
            }
        }

        void MoveToTarget()
        {
            var r = _target.transform.position - transform.position;
            var d2 = r.sqrMagnitude;
            if (d2 > _aiDistance)
            {
                _target = null;
            }
            else if (_target.GetComponent<Ship>().LifeSupport.Crew <= 0)
            {
                _target = null;
            }
            else
            {
                r.y = 0f;
                r.x = Mathf.Clamp(r.x, -1, 1);
                Move(r);
            }
        }
        void SearchTarget()
        {
            var d2 = _aiDistance + _speed;
            foreach (var ship in Spaceship.Ship._ships)
            {
                //if (ship == null) break;
                var r = transform.position - ship.transform.position;
                var r2 = r.sqrMagnitude;
                if (r2 <= _aiDistance & r2 < d2 & ship.Team == _team & ship.LifeSupport.Crew >0)
                {
                    d2 = r2;
                    _target = ship.transform;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_target != null)
            {
                if (collision.gameObject.transform == _target)
                {
                    var ship = _target.GetComponent<Ship>();
                    if (ship != null)
                    {
                        ship.LifeSupport.Crew += 1;
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}