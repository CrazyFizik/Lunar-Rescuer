using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Moonshot.Scripts.FX;

namespace Assets.Moonshot.Scripts.Environment
{
    public class Asteroid : MonoBehaviour, IDamageReciver
    {
        [SerializeField] ExplosionFX _exploisonPrefab;
        [SerializeField] float _health = 10;
        [SerializeField] float _maxHealth = 10;

        Rigidbody2D _rb2d;

        // Start is called before the first frame update
        void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _maxHealth = _rb2d.mass * _rb2d.mass;
            _health = _maxHealth;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Repair(float dt)
        {
            throw new System.NotImplementedException();
        }

        public void Damage(int damage)
        {
            //Debug.Log(damage.ToString());
            this._health -= damage;
            if (_health < 0) Destruction();
        }

        public void Hit(int damage)
        {
            Damage(damage);
        }

        public void Hit(float energy)
        {
            var damage = Mathf.RoundToInt(energy);
            Damage(damage);
        }

        public void Destruction()
        {
            _rb2d.velocity = Vector2.zero;
            if (this._exploisonPrefab != null)
            {
                this._exploisonPrefab.Spawn(transform.position, transform.rotation, _rb2d.velocity);
            }
            this.gameObject.SetActive(false);
            Destroy(this.gameObject, 0.05f);
        }

    }
}
