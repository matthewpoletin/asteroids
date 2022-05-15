using UnityEngine;

namespace Asteroids.Field
{
    [CreateAssetMenu(fileName = "ship_params", menuName = "Params/ShipParams", order = 1)]
    public class ShipParams : ScriptableObject
    {
        [SerializeField] private int _startLifeCount = 3;
        [SerializeField] private float _immuneOnDamageTimeInSeconds = 5f;

        [SerializeField] private float _moveAcceleration = 0.1f;
        [SerializeField] private float _inertia = 0.025f;
        [SerializeField] private float _maxSpeed = 6;
        [SerializeField] private float _steerSpeed = 250f;

        [SerializeField] private float _laserAmmoResetTimeInSeconds = 5f;
        [SerializeField] private int _maxLaserAmmoCount = 3;
        [SerializeField] private float _laserLifeTimeSeconds = 0.3f;

        public int StartLifeCount => _startLifeCount;
        public float ImmuneOnDamageTimeInSeconds => _immuneOnDamageTimeInSeconds;

        public float MoveAcceleration => _moveAcceleration;
        public float Inertia => _inertia;
        public float MaxSpeed => _maxSpeed;
        public float SteerSpeed => _steerSpeed;

        public float LaserAmmoResetTimeInSeconds => _laserAmmoResetTimeInSeconds;
        public int MaxLaserAmmoCount => _maxLaserAmmoCount;
        public float LaserLifeTimeSeconds => _laserLifeTimeSeconds;
    }
}