using System;
using UnityEngine;
using System.Collections;


namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    [System.Serializable]
    public class SystemState
    {
        [SerializeField] eSystemState _state = eSystemState.OFFLINE;
        [SerializeField] float _weight = 1f;
        [SerializeField] float _breakChanceOnline = 0.25f;
        [SerializeField] float _breakChanceOffline = 0.5f;
        [SerializeField] float _breakChanceDamaged = 0.5f;        

        [Tooltip("Time to recover from offline state")]
        [SerializeField] float _offlineRecoverTime = 4f;
        [Tooltip("If damaged while offline, recovery time increases by this much")]
        [SerializeField] float _offlineExtraTime = 2f;
        
        [SerializeField] float _timer;
        private Subsystem _subsystem;

        public eSystemState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public float Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }

        public bool GetCondition(eSystemState state)
        {
            return (this._state == state);
        }

        public float GetOfflineTimeRemaining()
        {
            return this._timer;
        }

        public void Initialize(Subsystem sys)
        {
            _subsystem = sys;
            State = eSystemState.ONLINE;
            this._timer = this._offlineRecoverTime;
        }

        public void Update(float dt)
        {
            if (GetCondition(eSystemState.OFFLINE))
            {
                this._timer -= dt;
                if (this._timer > 0.0)
                    return;
                State = eSystemState.DAMAGED;
                this._timer = 0.0f;
            }
            //else
            //{
            //    this._timer = this._offlineRecoverTime;
            //}
        }

        public void TakeDamage()
        {
            float chance = 0f;

            if (GetCondition(eSystemState.DAMAGED))
            {
                chance = this._breakChanceDamaged;
            }
            else if (GetCondition(eSystemState.OFFLINE))
            {
                chance = this._breakChanceOffline;
            }
            else
            {
                chance = this._breakChanceOnline;
            }


            if (GetCondition(eSystemState.INVALID) || UnityEngine.Random.value > chance)
                return;

            if (GetCondition(eSystemState.ONLINE))
            {
                State = eSystemState.OFFLINE;
                this._timer = this._offlineRecoverTime;
            }
            else if (GetCondition(eSystemState.DAMAGED))
            {
                State = eSystemState.OFFLINE;
                this._timer = this._offlineRecoverTime;
            }
            else if (GetCondition(eSystemState.OFFLINE))
            {
                this._timer += this._offlineExtraTime;
            }

            //Debug.Log(this._timer.ToString());
        }

        public void TakeDamage(float damage)
        {
            float chance = 0f;

            if (GetCondition(eSystemState.DAMAGED))
            {
                chance = this._breakChanceDamaged;
            }
            else if (GetCondition(eSystemState.OFFLINE))
            {
                chance = this._breakChanceOffline;
            }
            else
            {
                chance = this._breakChanceOnline;
            }


            if (GetCondition(eSystemState.INVALID) || UnityEngine.Random.value > chance)
                return;

            if (GetCondition(eSystemState.ONLINE))
            {
                State = eSystemState.OFFLINE;
                this._timer = damage;
            }
            else if (GetCondition(eSystemState.DAMAGED))
            {
                State = eSystemState.OFFLINE;
                this._timer = damage;
            }
            else if (GetCondition(eSystemState.OFFLINE))
            {
                this._timer += damage;
            }

            //Debug.Log(this._timer.ToString());
        }
    }
}
