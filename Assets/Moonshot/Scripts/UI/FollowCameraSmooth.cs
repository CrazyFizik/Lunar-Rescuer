using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Moonshot.Scripts.UI
{
    public class FollowCameraSmooth : MonoBehaviour
    {
        [Header("Camera")]
        private Camera camera;
        [Tooltip("Reference to the target GameObject")]
        public Transform target;
        [Tooltip("Current relative offset to the target")]
        public Vector3 offset;
        [Tooltip("Multiplier for the movement speed")]
        [Range(0f, 5f)]
        public float smoothSpeed = 0.125f;

        void Start()
        {
            camera = Camera.main;
        }

        private void FixedUpdate()
        {
            Follow();
        }

        private void Update()
        {

        }

        private void LateUpdate()
        {
            //Follow();
        }


        void Follow()
        {
            if (target == null) return;
            offset.z = transform.position.z;
            Vector3 position = target.transform.position;
            Vector2 smoothedPosition = Vector2.Lerp(transform.position, position + offset, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, position.z + offset.z);
        }

        float _interp = 5f;
        float _interpVelocity = 0f;
        float _minDistance = 0f;
        float _followDistance = 0.125f;
        Vector3 _offset = Vector3.zero;
        Vector3 _targetPos = Vector3.zero;
        Vector2 _position = Vector2.zero;
        Vector2 _velocity = Vector2.zero;
        void FollowLerp()
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;
            Vector3 targetDirection = target.transform.position - posNoZ;
            _interpVelocity = targetDirection.magnitude * _interp;
            var velocity = targetDirection.normalized * _interpVelocity * Time.deltaTime;
            _targetPos = transform.position + velocity;
            transform.position = Vector3.Lerp(transform.position, _targetPos + _offset, 1f);
            _velocity = (Vector2)transform.position - _position;
            _position = transform.position;
        }
    }
}