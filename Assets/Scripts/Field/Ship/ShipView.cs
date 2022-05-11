using System;
using UnityEngine;

namespace Asteroids.Field
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private Animator _animator = default;
        [SerializeField] private GameObject _bulletPrefab = default;
        [SerializeField] private GameObject _laserPrefab = default;
        [SerializeField] private Transform _muzzlePlaceholder = default;
        [SerializeField] private GameObject _stateIdle = default;
        [SerializeField] private GameObject _stateAccelerating = default;

        private Action<Collider2D> _onTriggerEnter;

        public Transform MuzzlePlaceholder => _muzzlePlaceholder;

        // TODO: Move to specialized assets class
        public GameObject BulletPrefab => _bulletPrefab;
        public GameObject LaserPrefab => _laserPrefab;

        public void Connect(Action<Collider2D> onTriggerEnter)
        {
            _onTriggerEnter = onTriggerEnter;
        }

        public void PlayImmuneAnimation()
        {
            _animator.Play("immune");
        }

        public void PlayIdleAnimation()
        {
            _animator.Play("idle");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onTriggerEnter.Invoke(other);
        }

        public void UpdateIdleState(bool isAccelerating)
        {
            _stateIdle.SetActive(!isAccelerating);
            _stateAccelerating.SetActive(isAccelerating);
        }
    }
}