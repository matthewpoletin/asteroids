using UnityEngine;

namespace Asteroids.Field
{
    public enum AsteroidSize
    {
        Small = 1,
        Medium = 2,
        Large = 3,
    }

    public class FieldView : MonoBehaviour
    {
        [SerializeField] private GameObject _asteroidPrefab = default;
        [SerializeField] private Transform _asteroidContainer = default;
        [SerializeField] private GameObject _shipPrefab = default;
        [SerializeField] private Transform _shipSpawnPoint = default;
        [SerializeField] private GameObject _saucerPrefab = default;
        [SerializeField] private ShipParams _shipParams = default;
        [SerializeField] private AsteroidParams _asteroidParams;
        [SerializeField] private SaucerParams _saucerParams;

        public GameObject AsteroidPrefab => _asteroidPrefab;
        public Transform AsteroidContainer => _asteroidContainer;
        public GameObject ShipPrefab => _shipPrefab;
        public Transform ShipSpawnPoint => _shipSpawnPoint;
        public GameObject SaucerPrefab => _saucerPrefab;
        public ShipParams ShipParams => _shipParams;
        public AsteroidParams AsteroidParams => _asteroidParams;
        public SaucerParams SaucerParams => _saucerParams;
    }
}