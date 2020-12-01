using System.Collections.Generic;
using UnityEngine;

namespace Steering
{
    [System.Serializable]
    public class Flock : SteeringBehaviour
    {
        public List<IBoid> _flockers;

        public Cohesion _cohesion;
        public Aligement _aligement;
        public Separation _separation;

        public Flock(IBoid host)
        {
            _host = host;

            _flockers   = new List<IBoid>();

            _cohesion   = new Cohesion(_host);
            _aligement  = new Aligement(_host);
            _separation = new Separation(_host);

            _cohesion._boids    = _flockers;
            _aligement._boids   = _flockers;
            _separation._boids  = _flockers;

            _cohesion._RADIUS   = 15f;
            _aligement._RADIUS  = 15f;
            _separation._RADIUS = 3f;

            _cohesion._WEIGHT   = 1f;
            _aligement._WEIGHT  = 1f;
            _separation._WEIGHT = 1.5f;
        }

        public override void Update()
        {
            _aligement._boids   = _flockers;
            _cohesion._boids    = _flockers;
            _separation._boids  = _flockers;

            _aligement.Update();
            _cohesion.Update();
            _separation.Update();

            Vector2 aligement   = _aligement._WEIGHT    * _aligement._steering.normalized;
            Vector2 cohesion    = _cohesion._WEIGHT     * _cohesion._steering.normalized;
            Vector2 separation  = _separation._WEIGHT   * _separation._steering.normalized;

            _steering   =  aligement + cohesion + separation;
            _direction  = _aligement._direction;
        }
    }
}
