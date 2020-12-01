using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Engagement : SteeringBehaviour
    {
        public IBoid _target;
        public IBoid _friend;
        private Boid _ahead;
        public List<IBoid> _flockers;

        public Arrive _arrive;
        public Evade _evade;
        public Separation _separation;

        public float _BEHIND_DIST = 2f;
        public float _SIGHT_RADIUS = 3f;

        public Engagement(IBoid host)
        {
            _host = host;
            _flockers = new List<IBoid>();

            _ahead = new Boid();
            _ahead._position = _host._Position;
            _ahead._velocity = _host._Velocity;
            _ahead._maxVelocity = _host._MaxVelocity;

            _arrive = new Arrive(_host);
            _evade = new Evade(_host);
            _separation = new Separation(_host);

            _arrive._target = _target;
            _evade._target = _ahead;

            _separation._boids = _flockers;
            _separation._RADIUS = 1.0f;
            _separation._WEIGHT = 0.75f;
        }

        public override void Update()
        {
            Vector2 lv = Vector2.zero;
            Vector2 follow = Vector2.zero;
            Vector2 evade = Vector2.zero;
            Vector2 separate = Vector2.zero;
            Vector2 aligement = _direction;

            if (_target != null)
            {
                _arrive._target = _target;
                _arrive.Update();
                follow = _arrive._steering;
                aligement = _arrive._direction;
            }

            if (_flockers != null && _flockers.Count > 0)
            {
                // Evade fire line
                _friend = _flockers[0];
                float fd2 = (_host._Position - _friend._Position).sqrMagnitude;

                foreach (IBoid boid in _flockers)
                {
                    float l = (_host._Position - boid._Position).sqrMagnitude;
                    if (l < fd2)
                    {
                        fd2 = l;
                        _friend = boid;
                    }
                }

                lv = -_friend._Velocity;
                lv = lv.normalized;
                lv *= _BEHIND_DIST;
                _ahead._position = _friend._Position + lv;
                _ahead._velocity = _friend._Velocity;
                float ad2 = (_host._Position - _ahead._position).sqrMagnitude;
                float r2 = _SIGHT_RADIUS * _SIGHT_RADIUS;
                if (fd2 < r2 || ad2 < r2)
                {
                    _evade._target = _ahead;
                    _evade.Update();
                    evade = _evade._steering;

                    _ahead._position *= -1;
                    _evade._target = _ahead;
                    _evade.Update();
                    evade += _evade._steering;
                }

                // Separate from flockers
                //_separation._boids = _flockers;
                _separation.Update();
                separate = _separation._steering;
                separate = separate.normalized;
                separate *= _separation._WEIGHT;
            }
            
            _steering = follow + evade + separate;
            _direction = aligement;
        }
    }
}
