using System;
using UnityEngine;

namespace Asteroids.Field
{
    public class BulletController : IDestroyedFieldActor, ITick
    {
        private readonly Action<BulletController> _onBulletDeath;
        private readonly FieldView _fieldView;

        public BulletView BulletView { get; }

        public Transform Transform => BulletView.transform;

        public BulletController(BulletView bulletView, FieldView fieldView, Action<BulletController> onBulletDeath)
        {
            BulletView = bulletView;

            _fieldView = fieldView;

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
                var asteroidController = _fieldView.GetAsteroidController(asteroidView);
                _fieldView.SplitOrDestroyAsteroid(asteroidController);

                DestroyBullet();
            }

            if (other.TryGetComponent<SaucerView>(out var saucerView))
            {
                var saucerController = _fieldView.GetSaucerController(saucerView);
                _fieldView.DestroySaucer(saucerController);

                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            _onBulletDeath.Invoke(this);
        }
    }
}