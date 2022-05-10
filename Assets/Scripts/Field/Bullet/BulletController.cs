using System;
using UnityEngine;

namespace Asteroids.Field
{
    public class BulletController : ITick
    {
        private readonly Action<BulletController> _onBulletDeath;
        public BulletView BulletView { get; }

        public BulletController(BulletView bulletView, Action<BulletController> onBulletDeath)
        {
            BulletView = bulletView;
            _onBulletDeath = onBulletDeath;

            BulletView.Connect(OnTriggerEnter, OnTriggerExit);
        }

        public void Tick(float deltaTime)
        {
            BulletView.transform.position += BulletView.transform.up * (BulletView.Speed * deltaTime);
        }

        private void OnTriggerExit(Collider2D other)
        {
            if (other.TryGetComponent<BoundsController>(out _))
            {
                _onBulletDeath.Invoke(this);
            }
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.TryGetComponent<IReceivingDamage>(out var target))
            {
                target.ReceiveDamage();

                _onBulletDeath.Invoke(this);
            }
        }
    }
}