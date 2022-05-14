using System;
using UnityEngine;

namespace Asteroids.Field
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private float _speed = default;

        private Action<Collider2D> _onTriggerEnter;

        public float Speed => _speed;

        public void Connect(Action<Collider2D> onTriggerEnter)
        {
            _onTriggerEnter = onTriggerEnter;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onTriggerEnter.Invoke(other);
        }
    }
}