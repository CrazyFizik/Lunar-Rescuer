using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class FollowLeader : SteeringBehaviour
    {
        public IBoid _leader;
        private Boid _behind;
        private Boid _ahead;
        public List<IBoid> _flockers;

        public Arrive _arrive;
        public Evade _evade;
        public Separation _separation;
        public Aligement _aligement;

        public float _LEADER_BEHIND_DIST = 2f;
        public float _LEADER_SIGHT_RADIUS = 2f;

        public FollowLeader(IBoid host)
        {
            _host = host;
            _flockers = new List<IBoid>();

            _behind = new Boid();
            _behind._position = _host._Position;
            _behind._velocity = _host._Velocity;
            _behind._maxVelocity = _host._MaxVelocity;

            _ahead = new Boid();
            _ahead._position = _host._Position;
            _ahead._velocity = _host._Velocity;
            _ahead._maxVelocity = _host._MaxVelocity;

            _arrive = new Arrive(_host);
            _evade = new Evade(_host);
            _separation = new Separation(_host);
            _aligement = new Aligement(_host);

            _arrive._target = _behind;
            _evade._target = _ahead;

            _separation._boids = _flockers;
            _aligement._boids = _flockers;

            _aligement._RADIUS = 15f;
            _separation._RADIUS = 3f;

            _aligement._WEIGHT = 1f;
            _separation._WEIGHT = 1.5f;
        }

        public override void Update()
        {
            Vector2 lv = Vector2.zero;
            Vector2 follow = Vector2.zero;
            Vector2 evade = Vector2.zero;
            Vector2 separate = Vector2.zero;
            Vector2 aligement = _direction;

            if (_leader != null)
            {
                lv = -_leader._Velocity;
                lv = lv.normalized;
                lv *= _LEADER_BEHIND_DIST;

                _behind._position = _leader._Position + lv;
                _behind._velocity = _leader._Velocity;
                //_arrive._target = _behind;
                _arrive.Update();
                follow = _arrive._steering;

                _ahead._position = _leader._Position - lv;
                _ahead._velocity = _leader._Velocity;
                float ld2 = (_host._Position - _leader._Position).sqrMagnitude;
                float ad2 = (_host._Position - _ahead._position).sqrMagnitude;
                float r2 = _LEADER_SIGHT_RADIUS * _LEADER_SIGHT_RADIUS;
                if (ld2 < r2 || ad2 < r2)
                {
                    //_ahead._position = _leader._Position - lv;
                    //_ahead._velocity = _leader._Velocity;
                    //_evade._target = _ahead;
                    _evade.Update();
                    evade = _evade._steering;
                }
            }

            //_separation._boids = _flockers;
            _separation.Update();
            separate = _separation._steering;
            separate = separate.normalized;
            separate *= _separation._WEIGHT;

            //_aligement._boids = _flockers;
            _aligement.Update();
            aligement = _aligement._direction.normalized;

            _steering = follow + evade + separate;
            _direction = aligement;
        }
    }
}
