using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Assets.Moonshot.Scripts.Spaceship.Subsystems
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ThrusterParticle : MonoBehaviour, IThrusterFX
    {
        [Tooltip("Cached particle system")]
        public ParticleSystem _particleSystem;

        [Tooltip("The scale of this thruster when throttle is 1")]
        public float _edge0 = 0.05f;

        [Tooltip("The scale of this thruster when throttle is 1")]
        public float _edge1 = 0.5f;

        [Tooltip("The scale of this thruster when throttle is 1")]
        public Vector3 _maxScale = Vector3.one;

        [Tooltip("The amount the thruster effect can flicker")]
        public float _flicker = 0.1f;

        //[Tooltip("How quickly the throttle scales to the desired value")]
        //public float _dampening = 10.0f;

        [Tooltip("Current interpolated throttle value")]
        [SerializeField]
        private float _currentThrottle;
        private float _rate = 1f;
        private float _rateOverTime = 1f;
        private float _rateOverSpeed = 1f;

        public void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _rate = _particleSystem.emissionRate;
            _rateOverTime = _particleSystem.emission.rateOverTimeMultiplier;
            _rateOverSpeed = _particleSystem.main.startSpeedMultiplier;
        }

        public void SetFX(float throttle)
        {

            if (throttle > 0f)
            {
                _currentThrottle = Utils.MathHelper.SmootherStep(_edge0, _edge1, throttle);
                var scale = _currentThrottle * _maxScale * Random.Range(1.0f - _flicker, 1.0f + _flicker);

                //_particleSystem.main = _rateOverSpeed * Mathf.Clamp01(scale.z);
                //_particleSystem.emission.rateOverTimeMultiplier = _rateOverTime * Mathf.Clamp01(scale.z);
                _particleSystem.emissionRate = _rate * Mathf.Clamp01(scale.z);

                _particleSystem.Play();
            }
            else
            {
                _particleSystem.Stop();
            }
        }

        public static float DampenFactor(float dampening, float deltaTime)
        {
            if (dampening < 0.0f)
            {
                return 1.0f;
            }
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                return 1.0f;
            }
#endif
            return 1.0f - Mathf.Exp(-dampening * deltaTime);
        }

    }
}
