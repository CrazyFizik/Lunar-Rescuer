using UnityEngine;
using Assets.Moonshot.Scripts.FX;
using System.Collections;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Damager : MonoBehaviour
    {
        public static float ratio = 0.1f;
        public static float angleThershold = 0.5f;
        public static float speedThershold = 16f;

        [SerializeField] ExplosionFX _hitFx;
        Rigidbody2D _rb2d;

        // Start is called before the first frame update
        void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        public void OnCollisionEnter2D(Collision2D collisionData)
        {
            CollisionDamage(collisionData);
        }

        public void CollisionDamage(Collision2D collisionData)
        {
            IDamageReciver damaged = null;

            if (collisionData.rigidbody != null)
            {
                //damaged = collisionData.collider.gameObject.GetComponent<IDamageReciver>();
                damaged = collisionData.rigidbody.gameObject.GetComponent<IDamageReciver>();
            }

            if (damaged != null)
            {
                ////float e = 0.5f * collision.relativeVelocity.sqrMagnitude * _rb2d.mass;
                ////Vector2 normal = collision.contacts[0].normal;
                ////Vector2 velocity = collision.relativeVelocity;
                ////float v = Vector2.Dot(normal, velocity); 
                ////float m = _rb2d.mass;
                ////float e = 0.5f * v * v * m;

                //Vector2 n = collision.contacts[0].normal;
                //Vector2 v = collision.relativeVelocity;
                //float dot = Vector2.Dot(n, v.normalized);
                //if (dot < Random.Range(0f, 0.5f)) return;
                //float v2 = dot * dot * v.sqrMagnitude;
                //if (Mathf.Abs(v2) < THR)
                //{
                //    v2 = 0f;
                //    Debug.Log(v2.ToString());
                //}
                //float m = _rb2d.mass;
                //float e = 0.5f * v2 * m;
                ////float e2 = 0.5f * collision.relativeVelocity.sqrMagnitude * _rb2d.mass;

                var e = CalculateCollisionDamage(collisionData);
                if (e > 0)
                {
                    damaged.Hit(e);
                    CreateFX(collisionData);
                }
            }
            else // Self
            {
                //var e = CalculateCollisionDamage(collisionData);
                //gameObject.GetComponent<IDamageReciver>().TakeDamageCollision(e);
            }
        }

        public float CalculateCollisionDamage(Collision2D collisionData)
        {
            Vector2 n = collisionData.contacts[0].normal;
            Vector2 v = collisionData.relativeVelocity;
            
            float dot = Vector2.Dot(n, v.normalized);
            if (dot < Random.Range(0f, angleThershold))
            {
                return 0f;
            }
            
            float v2 = dot * dot * v.sqrMagnitude;
            if (v2 < speedThershold)
            {
                return 0f;
            }
            //Debug.Log(Mathf.Sqrt(v2).ToString());

            float m = this._rb2d.mass + collisionData.rigidbody.mass;
            float e = 0.5f * v2 * m;


            return e;
        }

        void CreateFX(Collision2D collisionData)
        {
            if (_hitFx == null) return;
            int index = 0;
            if (collisionData.contactCount < 1) return;
            else if (collisionData.contactCount == 1) index = Random.Range(0, collisionData.contactCount);            
            var contact = collisionData.contacts[index];
            var rotation = Quaternion.FromToRotation(transform.up, contact.normal) * transform.rotation;
            _hitFx.Spawn(contact.point, rotation);
            //foreach (var contact in collisionData.contacts)
            //{
            //    var rotation = Quaternion.FromToRotation(transform.up, contact.normal) * transform.rotation;
            //    _hitFx.Spawn(contact.point, rotation);
            //}            
        }
    }
}