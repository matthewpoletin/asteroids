using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Field
{
    public class BulletManager
    {
        private readonly List<BulletController> _bullets = new();

        private readonly GameObjectPool _pool;
        private readonly Transform _bulletContainer;
        private readonly FieldView _fieldView;

        public BulletManager(GameObjectPool pool, FieldView fieldView, Transform bulletContainer)
        {
            _pool = pool;
            _fieldView = fieldView;
            _bulletContainer = bulletContainer;
        }

        public void CreateBullet(GameObject bulletPrefab, Vector3 worldPosition, Quaternion rotation)
        {
            var bulletView = _pool.GetObject<BulletView>(bulletPrefab, _bulletContainer);
            bulletView.transform.SetPositionAndRotation(worldPosition, rotation);
            var bulletController = new BulletController(bulletView, _fieldView, OnBulletDeath);
            _bullets.Add(bulletController);
        }

        private void OnBulletDeath(BulletController bulletController)
        {
            _bullets.Remove(bulletController);
            _pool.UtilizeObject(bulletController.BulletView);
        }

        public void Tick(float deltaTime)
        {
            foreach (var bullet in _bullets)
            {
                bullet.Tick(deltaTime);
            }
        }
    }
}