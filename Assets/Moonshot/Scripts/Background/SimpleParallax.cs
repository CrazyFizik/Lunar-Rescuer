using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Moonshot.Scripts.Background
{
    public class SimpleParallax : MonoBehaviour
    {
        public int _distance = 1; // greater the distance, the slower the scroll
        public Transform _target;

        private float _paralaxSpeed = 1f;
        private Transform _tx;
        private Vector2 _lastPosition = new Vector2();
        [SerializeField] private Vector2 _scale = new Vector2(1,1);
        [SerializeField] private Vector2 _velocity = new Vector2();

        void Start()
        {
            _tx = transform;
            if (_distance > 1)
            {
                _paralaxSpeed = 1f / _distance;
            }
        }

        void LateUpdate()
        {
            try
            {
                var position = new Vector2();
                position = _target.position;
                _velocity = position - _lastPosition;
                _velocity.x *= _scale.x;
                _velocity.y *= _scale.y;
                _lastPosition = position;
                GetComponent<MeshRenderer>().material.mainTextureOffset += _velocity * _paralaxSpeed * Time.deltaTime;
            }
            catch
            {
                _target = Camera.main.transform;
            }
        }
    }
}
