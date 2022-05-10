using System;
using UnityEngine;

namespace Asteroids.Field
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private float _speed = default;

        private Action<Collider2D> _onTriggerEnter;
        private Action<Collider2D> _onTriggerExit;

        public float Speed => _speed;

        public void Connect(Action<Collider2D> onTriggerEnter, Action<Collider2D> onTriggerExit)
        {
            _onTriggerEnter = onTriggerEnter;
            _onTriggerExit = onTriggerExit;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onTriggerEnter.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _onTriggerExit.Invoke(other);
        }
    }
}