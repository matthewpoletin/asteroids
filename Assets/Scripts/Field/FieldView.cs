using Asteroids.Score;
using UnityEngine;

namespace Asteroids.Field
{
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
        [SerializeField] private ScoreParams _scoreParams;

        public GameObject AsteroidPrefab => _asteroidPrefab;
        public Transform AsteroidContainer => _asteroidContainer;
        public GameObject ShipPrefab => _shipPrefab;
        public Transform ShipSpawnPoint => _shipSpawnPoint;
        public GameObject SaucerPrefab => _saucerPrefab;
        public ShipParams ShipParams => _shipParams;
        public AsteroidParams AsteroidParams => _asteroidParams;
        public SaucerParams SaucerParams => _saucerParams;
        public ScoreParams ScoreParams => _scoreParams;
    }
}