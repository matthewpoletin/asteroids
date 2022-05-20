using System;
using Asteroids.Core.Core;
using UnityEngine;
using Utils;

namespace Asteroids.Field
{
    public class ShipController : IFieldActor, ITick
    {
        private readonly GameObjectPool _pool;
        private readonly BulletManager _bulletManager;
        private readonly FieldController _fieldController;

        private LaserController _laserController;

        private Vector3 _currentSpeed = Vector2.zero;
        private readonly ShipInput _shipInput;
        public float Speed => _currentSpeed.magnitude;

        public ShipView ShipView { get; }
        public ShipModel ShipModel { get; }

        public Transform Transform => ShipView.transform;

        private ShipParams Params => ShipModel.Params;

        public ShipController(ShipView shipView, ShipModel model, GameObjectPool pool, FieldController fieldController, BulletManager bulletManager)
        {
            ShipView = shipView;
            ShipModel = model;

            _pool = pool;
            _fieldController = fieldController;
            _bulletManager = bulletManager;

            _shipInput = new ShipInput();

            ShipView.Connect(OnTriggerEnter);

            ShipModel.OnImmuneStatusChanged += ImmuneStatusChanged;
        }

        public void Tick(float deltaTime)
        {
            ShipModel.Tick(deltaTime);
            _laserController?.Tick(deltaTime);

            ProcessInput(deltaTime);
        }

        private void ProcessInput(float deltaTime)
        {
            var transform = ShipView.transform;

            var accelerating = _shipInput.AccelerateInProgress;
            if (accelerating)
            {
                _currentSpeed += transform.up * Params.MoveAcceleration;
            }

            _currentSpeed += _currentSpeed.normalized * (-1 * Params.Inertia);
            _currentSpeed = Math.Min(_currentSpeed.magnitude, Params.MaxSpeed) * _currentSpeed.normalized;

            transform.position += _currentSpeed * deltaTime;

            ShipView.UpdateIdleState(accelerating);

            if (_shipInput.SteerInProgress)
            {
                var direction = _shipInput.SteerValue;
                transform.RotateAround(transform.position, transform.forward,
                    -1 * Params.SteerSpeed * deltaTime * direction);
            }

            if (_shipInput.ShootLaserTriggered)
            {
                ShootLaser();
            }

            if (_shipInput.ShootCannonTriggered)
            {
                ShootCannon();
            }
        }

        private void ImmuneStatusChanged()
        {
            if (ShipModel.IsImmune())
            {
                ShipView.PlayImmuneAnimation();
            }
            else
            {
                ShipView.PlayIdleAnimation();
            }
        }

        private void OnTriggerEnter(Collider2D other)
        {
            if (other.TryGetComponent<IShipDamagingView>(out _))
            {
                ShipModel.ReceiveCollisionDamage();
            }
        }

        private void ShootCannon()
        {
            _bulletManager.CreateBullet(ShipView.BulletPrefab, ShipView.MuzzlePlaceholder.position,
                ShipView.transform.rotation);
        }

        private void ShootLaser()
        {
            if (ShipModel.CanShootLaser())
            {
                ShipModel.LaserAmmoCount--;

                _laserController = new LaserController(_pool, ShipView.LaserPrefab, ShipView.MuzzlePlaceholder,
                    _fieldController, OnLaserComplete, ShipModel.Params.LaserLifeTimeSeconds);
            }
        }

        private void OnLaserComplete(LaserController obj)
        {
            _laserController.Utilize();
            _laserController = null;
        }
    }
}