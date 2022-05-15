using System;
using Asteroids.Core.Core;
using UnityEngine;

namespace Asteroids.Field
{
    public class BulletController : IDestroyedFieldActor, ITick
    {
        private readonly Action<BulletController> _onBulletDeath;
        private readonly FieldController _fieldController;

        public BulletView BulletView { get; }

        public Transform Transform => BulletView.transform;

        public BulletController(BulletView bulletView, FieldController fieldController, Action<BulletController> onBulletDeath)
        {
            BulletView = bulletView;

            _fieldController = fieldController;

            _onBulletDeath = onBulletDeath;

            BulletView.Connect(OnTriggerEnter);
        }

        public void Tick(float deltaTime)
        {
            Transform.position += BulletView.transform.up * (BulletView.Speed * deltaTime);
        }

        public void OnLeavingBounds()
        {
            DestroyBullet();
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.TryGetComponent<AsteroidView>(out var asteroidView))
            {
                var asteroidController = _fieldController.GetAsteroidController(asteroidView);
                _fieldController.SplitOrDestroyAsteroid(asteroidController);

                DestroyBullet();
            }

            if (other.TryGetComponent<SaucerView>(out var saucerView))
            {
                var saucerController = _fieldController.GetSaucerController(saucerView);
                _fieldController.DestroySaucer(saucerController);

                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            _onBulletDeath.Invoke(this);
        }
    }
}