using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Asteroids.Field
{
    public class ShipView : MonoBehaviour
    {
        [SerializeField] private Animator _animator = default;

        [FormerlySerializedAs("_moveSpeed")]
        [SerializeField]
        private float _moveAcceleration = 10f;

        [SerializeField] private float _inertia = 1f;
        [SerializeField] private float _maxSpeed = 6;
        [SerializeField] private float _steerSpeed = 10f;
        [SerializeField] private GameObject _bulletPrefab = default;
        [SerializeField] private GameObject _laserPrefab = default;
        [SerializeField] private Transform _muzzlePlaceholder = default;

        [FormerlySerializedAs("_ammoResetTimeInSeconds")]
        [SerializeField]
        private float _laserAmmoResetTimeInSeconds = 5f;

        [SerializeField] private int _startLifeCount = 3;
        [SerializeField] private float _immuneOnDamageTimeInSeconds = 5f;
        [SerializeField] private int _maxLaserAmmoCount = 3;
        [SerializeField] private float _laserLifeTimeSeconds = 0.3f;

        [SerializeField] private GameObject _stateIdle = default;
        [SerializeField] private GameObject _stateAccelerating = default;

        private GameObjectPool _pool;
        private BulletManager _bulletManager;

        private ShipControls _shipControls;

        private Vector3 _currentSpeed = Vector2.zero;
        private LaserController _laserController;
        public ShipModel ShipModel { get; private set; }

        public float Speed => _currentSpeed.magnitude;

        public void Awake()
        {
            _shipControls = new ShipControls();
        }

        private void OnEnable()
        {
            _shipControls.Enable();
        }

        private void OnDisable()
        {
            _shipControls.Disable();
        }

        public void Connect(GameObjectPool pool, BulletManager bulletManager)
        {
            _pool = pool;
            _bulletManager = bulletManager;

            ShipModel = new ShipModel(_maxLaserAmmoCount, _laserAmmoResetTimeInSeconds, _startLifeCount,
                _immuneOnDamageTimeInSeconds);

            ShipModel.OnImmuneStatusChanged += ImmuneStatusChanged;
        }

        private void ImmuneStatusChanged()
        {
            _animator.Play(ShipModel.IsImmune() ? "immune" : "idle");
        }

        public void Tick(float deltaTime)
        {
            var accelerating = _shipControls.Default.Accelerate.inProgress;
            if (accelerating)
            {
                _currentSpeed += transform.up * _moveAcceleration;
            }

            _currentSpeed += _currentSpeed.normalized * (-1 * _inertia);
            _currentSpeed = Math.Min(_currentSpeed.magnitude, _maxSpeed) * _currentSpeed.normalized;

            transform.position += _currentSpeed * deltaTime;

            _stateIdle.SetActive(!accelerating);
            _stateAccelerating.SetActive(accelerating);

            if (_shipControls.Default.Steer.inProgress)
            {
                var direction = _shipControls.Default.Steer.ReadValue<float>();
                transform.RotateAround(transform.position, transform.forward,
                    -1 * _steerSpeed * deltaTime * direction);
            }

            if (_shipControls.Default.ShootLaser.triggered)
            {
                ShootLaser();
            }

            if (_shipControls.Default.ShootCannon.triggered)
            {
                ShootCannon();
            }

            ShipModel.Tick(deltaTime);
            _laserController?.Tick(deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<AsteroidView>(out _))
            {
                ShipModel.ReceiveDamage();
            }
        }

        private void ShootCannon()
        {
            _bulletManager.CreateBullet(_bulletPrefab, _muzzlePlaceholder.position, transform.rotation);
        }

        private void ShootLaser()
        {
            if (ShipModel.CanShootLaser())
            {
                ShipModel.LaserAmmoCount--;

                _laserController = new LaserController(_pool, _laserPrefab, _muzzlePlaceholder, OnLaserComplete,
                    _laserLifeTimeSeconds);
            }
        }

        private void OnLaserComplete(LaserController obj)
        {
            _laserController.Utilize();
            _laserController = null;
        }
    }
}