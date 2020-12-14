using UnityEngine;
using Utils;
using System.Collections;
using Assets.Moonshot.Scripts.FX;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    public class Projectile : MonoBehaviour, Pool<Projectile>.IPoolOwner
    {       
        private Pool<Projectile> _pool;        
        private Rigidbody2D _rb2d;
        public Collider2D _collider;

        private SpriteRenderer _spriteRenderer;
        private LineRenderer _trail;

        private SystemWeapon _weapon;
        public GameObject _owner;

        [SerializeField] ExplosionFX _hitFx;

        [SerializeField] int _mask = 8;
        [SerializeField] float _damage = 10;
        [SerializeField] float _speed = 10f;        
        [SerializeField] float _lifetime = 10f;
        [SerializeField] float _delay = .1f;
        [SerializeField] bool _pause = true;

        private Vector3 _lastVelocity;
        private Vector3 _lastPosition;
        private float _lastAngularVelocity;

        //public

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _trail = GetComponent<LineRenderer>();
            _collider = GetComponent<Collider2D>();
            _lastPosition = _rb2d.position;
            _lastVelocity = _rb2d.velocity;
            _lastAngularVelocity = _rb2d.angularVelocity;
            gameObject.layer = _mask;
            Physics2D.IgnoreLayerCollision(_mask, _mask, true);
        }

        private void Update()
        {
            _lifetime -= Time.deltaTime;
            _delay -= Time.deltaTime;


            if (_lifetime <= 0)
            {
                Despawn();
                return;
            }

            if (_pause & _delay <= 0 & _owner != null)
            {
                var colliders = _owner.GetComponentsInChildren<Collider2D>();
                foreach (var collider in colliders)
                {
                    Physics2D.IgnoreCollision(_collider, collider, false);
                }
                _pause = false;
            }
        }

        private void FixedUpdate()
        {
            _lastPosition = _rb2d.position;
            _lastVelocity = _rb2d.velocity;
            _lastAngularVelocity = _rb2d.angularVelocity;
        }

        public Pool<Projectile> GetPool()
        {
            if (this._pool == null)
                this._pool = new Pool<Projectile>(this);
            return this._pool;
        }

        public float GetProjectileMass()
        {
            return this._rb2d.mass;
        }

        public float GetProjectileSpeed()
        {
            return this._speed;
        }

        public Projectile Spawn(Vector3 position, Quaternion rotation, Vector3 inheritedVelocity, float speedModifier = 1f, float damageModifier = 1f, GameObject owner = null)
        {
            Projectile projectile = this.Spawn(position, rotation);
            projectile._lifetime = this._lifetime;
            projectile._delay = this._delay;
            if (projectile._delay > 0.0f & owner != null)
            {
                projectile._owner = owner;
                projectile._pause = true;
                projectile._collider.enabled = true;
                if (projectile._collider != null)
                {
                    var colliders = projectile._owner.GetComponentsInChildren<Collider2D>();
                    foreach (var collider in colliders)
                    {
                        Physics2D.IgnoreCollision(projectile._collider, collider, true);
                    }
                }
            }
            else
            {
                projectile._collider.enabled = true;
                projectile._pause = false;
            }

            if (speedModifier <= 0) speedModifier = 1f;
            var speed = speedModifier * this._speed;
            var velocity = inheritedVelocity + rotation * Vector3.right * speed;
            projectile._rb2d.velocity = velocity;
            projectile._rb2d.angularVelocity = 0f;
            projectile._lastVelocity = projectile._rb2d.velocity;
            projectile._lastPosition = projectile._rb2d.position;
            projectile._damage = damageModifier * _damage;
            projectile.transform.localScale = damageModifier * Vector3.one;

            return projectile;
        }

        public Projectile Spawn(Vector3 position, Quaternion rotation)
        {
            Projectile projectile = this.GetPool().Spawn();
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;
            projectile.gameObject.SetActive(true);
            return projectile;
        }

        private void Despawn()
        {
            if (this._hitFx != null)
            {
                this._hitFx.Spawn(transform.position, transform.rotation, _lastVelocity);
            }

            this.gameObject.SetActive(false);
            if (this._collider != null)
            {
                this._collider.enabled = false;
            }

            this.GetPool().Despawn();
        }

        private void OnCollisionEnter2D(Collision2D collisionData)
        {
            this.CollisionDamage(collisionData);
        }

        private void OnCollisionStay2D(Collision2D collisionData)
        {
            this.CollisionDamage(collisionData);
        }

        private void OnCollisionExit2D(Collision2D collisionData)
        {
            this.CollisionDamage(collisionData);
        }

        //private void OnTriggerEnter2D(Collision2D collisionData)
        //{
        //    this.CollisionDamage(collisionData);
        //}

        private void CollisionDamage(Collision2D collisionData)
        {
            if (!this.GetPool().IsSpawned()) return;
            if (collisionData.contacts == null) return;
            if (collisionData.contactCount < 1) return;   

            var point = collisionData.contacts[0].point;
            var normal = collisionData.contacts[0].normal;
            var angle = Vector3.Angle(normal, -this.transform.forward);

            IDamageReciver damagable = null;
            if (collisionData.rigidbody != null)
            {
                damagable = collisionData.rigidbody.gameObject.GetComponent<IDamageReciver>();
                _rb2d.velocity = _lastVelocity = collisionData.rigidbody.velocity;
            }
            if (damagable != null)
            {
                _rb2d.velocity = _lastVelocity = Vector3.zero;
                damagable.Hit(_damage);
            }
                        
            this.Despawn();
        }
    }
}