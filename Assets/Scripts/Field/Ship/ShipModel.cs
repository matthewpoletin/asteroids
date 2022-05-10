using System;

namespace Asteroids.Field
{
    public class ShipModel
    {
        private readonly float _laserAmmoResetTimeSeconds;
        private readonly int _maxLaserAmmoCount;

        private int _laserAmmoCount;

        public event Action<int> OnLaserAmmoCountChanged;

        public int LaserAmmoCount
        {
            get => _laserAmmoCount;
            set
            {
                _laserAmmoCount = Math.Clamp(value, 0, _maxLaserAmmoCount);
                UpdateLaserAmmoResetTimer();
                OnLaserAmmoCountChanged?.Invoke(_laserAmmoCount);
            }
        }

        private float _laserAmmoResetTimer;

        public float LaserAmmoResetTimer => _laserAmmoResetTimer;

        private int _lifeCount;
        public event Action<int> OnLifeCountChanged;
        public event Action OnDeath;

        public int LifeCount
        {
            get => _lifeCount;
            set
            {
                var oldValue = _lifeCount;
                _lifeCount = Math.Max(value, 0);
                if (_lifeCount == 0)
                {
                    OnDeath?.Invoke();
                    return;
                }

                OnLifeCountChanged?.Invoke(_lifeCount);
                if (_lifeCount < oldValue)
                {
                    UpdateImmuneOnDamageTimer();
                }
            }
        }

        private readonly float _immuneOnDamageTimeInSeconds;
        private float _immuneTimer;
        public event Action OnImmuneStatusChanged;

        public bool IsImmune()
        {
            return _immuneTimer > 0;
        }

        public ShipModel(int maxLaserAmmoCount, float laserAmmoResetTimeInSeconds, int startLifeCount,
            float immuneOnDamageTimeInSeconds)
        {
            _laserAmmoResetTimeSeconds = laserAmmoResetTimeInSeconds;
            _maxLaserAmmoCount = maxLaserAmmoCount;
            _laserAmmoCount = _maxLaserAmmoCount;

            _lifeCount = startLifeCount;
            _immuneOnDamageTimeInSeconds = immuneOnDamageTimeInSeconds;
        }

        public void Tick(float deltaTime)
        {
            if (_laserAmmoResetTimer > 0)
            {
                _laserAmmoResetTimer -= deltaTime;
                if (_laserAmmoResetTimer <= 0)
                {
                    LaserAmmoCount += 1;
                }
            }

            if (_immuneTimer > 0)
            {
                _immuneTimer -= deltaTime;
                if (_immuneTimer <= 0)
                {
                    OnImmuneStatusChanged?.Invoke();
                }
            }
        }

        private void UpdateLaserAmmoResetTimer()
        {
            if (_laserAmmoResetTimer > 0f)
            {
                return;
            }

            if (_laserAmmoCount < _maxLaserAmmoCount)
            {
                _laserAmmoResetTimer = _laserAmmoResetTimeSeconds;
            }
        }

        public bool CanShootLaser()
        {
            return _laserAmmoCount > 0;
        }

        public void ReceiveDamage()
        {
            if (_immuneTimer > 0)
            {
                return;
            }

            LifeCount--;
        }

        private void UpdateImmuneOnDamageTimer()
        {
            _immuneTimer = Math.Max(_immuneTimer, _immuneOnDamageTimeInSeconds);
            OnImmuneStatusChanged?.Invoke();
        }
    }
}