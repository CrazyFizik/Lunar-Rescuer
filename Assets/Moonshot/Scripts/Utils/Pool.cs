using UnityEngine;
using System;

namespace Utils
{
    public class Pool<T> where T : MonoBehaviour, Pool<T>.IPoolOwner
    {
        private T _owner;
        private ulong _lastSpawnID;
        private T[] _pool;
        private int _poolSize;
        private GameObject _poolParent;
        private ulong _spawnID;
        private T _prefab;

        private Pool()
        {
        }

        public Pool(T owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("Pools must have owners");
            }
            this._owner = owner;
            this._pool = new T[8];
        }

        public T Spawn()
        {
            if (this._prefab != null)
            {
                throw new InvalidOperationException("Spawn must be called only on prefabs");
            }

            int num = 0;
            T obj = (T)null;
            for (int index = 0; index < this._poolSize; ++index)
            {
                if (this._pool[index] == null)
                {
                    ++num;
                }
                else if (obj == null)
                {
                    obj = this._pool[index];
                    this._pool[index] = (T)null;
                    ++num;
                }
                else if (num > 0)
                {
                    this._pool[index - num] = this._pool[index];
                    this._pool[index] = (T)null;
                }
            }
            this._poolSize -= num;

            if (obj == null)
            {
                obj = UnityEngine.Object.Instantiate<T>(this._owner);
            }
            else
            {
                obj.transform.SetParent(null);
            }

            Pool<T> pool = obj.GetPool();
            pool._prefab = this._owner;

            if (pool._spawnID != 0UL)
            {
                Debug.LogError("Found object in pool with non-zero spawnID. Object=" + obj.name + " spawnID = " + pool._spawnID);
            }

            ++this._lastSpawnID;
            pool._spawnID = this._lastSpawnID;
            obj.gameObject.name = this._owner.gameObject.name + " " + this._lastSpawnID;
            return obj;
        }

        public void Despawn()
        {
            if (this._prefab == null)
                throw new InvalidOperationException("Trying to call Despawn() but _prefab is null");
            if (this._spawnID == 0UL)
            {
                Debug.LogError((object)"Trying to call Despawn() but spawnID is already 0");
            }
            else
            {
                this._spawnID = 0UL;
                this._prefab.GetPool().AddToPool(this);
            }
        }

        public bool IsSpawned()
        {
            return !(this._prefab == null) && this._spawnID != 0UL;
        }

        public ulong GetSpawnID()
        {
            return this._spawnID;
        }

        private void AddToPool(Pool<T> despawned)
        {
            if (this._poolParent == null)
            {
                this._poolParent = new GameObject("Pool for " + this._owner.gameObject.name);
            }

            despawned._owner.transform.SetParent(this._poolParent.transform);
            if (this._poolSize >= this._pool.Length)
            {
                this._poolSize = this._pool.Length;
                this._pool = this._pool.GetResizedCopy<T>(this._pool.Length * 2 + 1);
            }

            this._pool[this._poolSize] = despawned._owner;
            ++this._poolSize;
        }

        public interface IPoolOwner
        {
            Pool<T> GetPool();
        }

        public struct Instance
        {
            public static readonly Pool<T>.Instance EMPTY = new Pool<T>.Instance((T)null);
            public readonly T _object;
            public readonly ulong _id;

            public Instance(T objectInstance)
            {
                this._object = objectInstance;
                this._id = !(objectInstance != null) ? 0UL : objectInstance.GetPool()._spawnID;
            }

            public bool IsValid()
            {
                return !(this._object == null) && this._id != 0UL && (long)this._object.GetPool()._spawnID == (long)this._id;
            }
        }
    }
}