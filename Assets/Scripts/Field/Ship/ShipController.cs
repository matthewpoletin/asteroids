using System;
using UnityEngine;

namespace Asteroids.Field
{
    public class ShipController : ITick
    {
        private readonly GameObjectPool _pool;
        private readonly BulletManager _bulletManager;
        private readonly ShipControls _shipControls;

        private LaserController _laserController;

        private Vector3 _currentSpeed = Vector2.zero;
        public float Speed => _currentSpeed.magnitude;

        public ShipView ShipView { get; }
        public ShipModel ShipModel { get; }

        private ShipParams Params => ShipModel.Params;

        public ShipController(ShipView shipView, ShipModel model, GameObjectPool pool, BulletManager bulletManager)
        {
            ShipView = shipView;
            ShipModel = model;

            _pool = pool;
            _bulletManager = bulletManager;

            _shipControls = new ShipControls();
            _shipControls.Enable();

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

            var accelerating = _shipControls.Default.Accelerate.inProgress;
            if (accelerating)
            {
                _currentSpeed += transform.up * Params.MoveAcceleration;
            }

            _currentSpeed += _currentSpeed.normalized * (-1 * Params.Inertia);
            _currentSpeed = Math.Min(_currentSpeed.magnitude, Params.MaxSpeed) * _currentSpeed.normalized;

            transform.position += _currentSpeed * deltaTime;

            ShipView.UpdateIdleState(accelerating);

            if (_shipControls.Default.Steer.inProgress)
            {
                var direction = _shipControls.Default.Steer.ReadValue<float>();
                transform.RotateAround(transform.position, transform.forward,
                    -1 * Params.SteerSpeed * deltaTime * direction);
            }

            if (_shipControls.Default.ShootLaser.triggered)
            {
                ShootLaser();
            }

            if (_shipControls.Default.ShootCannon.triggered)
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
            if (other.TryGetComponent<AsteroidView>(out _))
            {
                ShipModel.ReceiveDamage();
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
                    OnLaserComplete,
                    ShipModel.Params.LaserLifeTimeSeconds);
            }
        }

        private void OnLaserComplete(LaserController obj)
        {
            _laserController.Utilize();
            _laserController = null;
        }
    }
}