using System;
using UnityEngine;

namespace Asteroids.Field
{
    public class LaserView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem = default;

        private Action<Collider2D> _onTriggerEnter;

        public void Connect(float lifetimeDurationSeconds, Action<Collider2D> onTriggerEnter)
        {
            _particleSystem.time = lifetimeDurationSeconds;
            _onTriggerEnter = onTriggerEnter;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onTriggerEnter.Invoke(other);
            
        }
    }
}