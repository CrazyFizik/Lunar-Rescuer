using UnityEngine;
using Utils;
using Utils.Extensions;
using System.Collections;

namespace Assets.Moonshot.Scripts.FX
{
    public class ExplosionFX : MonoBehaviour, Pool<ExplosionFX>.IPoolOwner
    {
        private Pool<ExplosionFX> _pool;
        private Vector3 _velocity = Vector2.zero;

        public void Update()
        {
            transform.position = transform.position + _velocity * Time.deltaTime;
        }

        public IEnumerator DestroyObject()
        {
            yield return null;

            if (gameObject.activeInHierarchy == true)
            {
                Despawn();
            }

        }

        public Pool<ExplosionFX> GetPool()
        {
            if (this._pool == null)
            {
                this._pool = new Pool<ExplosionFX>(this);
            }
            return this._pool;
        }

        public ExplosionFX Spawn(Vector3 position, Quaternion rotation,  Vector3 inheritedVelocity)
        {
            var fx = Spawn(position, rotation);
            fx._velocity = inheritedVelocity;
            return fx;
        }

        public ExplosionFX Spawn(Vector3 position, Quaternion rotation)
        {
            var fx = this.GetPool().Spawn();
            fx.transform.position = position;
            fx.transform.rotation = rotation;
            fx.gameObject.SetActive(true);
            return fx;
        }

        private void Despawn()
        {
            this.gameObject.SetActive(false);
            this.GetPool().Despawn();
        }
    }
}
