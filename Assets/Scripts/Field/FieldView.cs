using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Field
{
    public enum AsteroidSize
    {
        Dead = 0,
        Small = 1,
        Medium = 2,
        Large = 3,
    }

    public class FieldView : MonoBehaviour, ITick
    {
        private const int ASTEROID_SPAWN_COUNT = 1;

        [SerializeField] private GameObject _asteroidPrefab = default;
        [SerializeField] private Transform _asteroidContainer = default;
        [SerializeField] private GameObject _shipPrefab = default;
        [SerializeField] private Transform _shipSpawnPoint = default;
        [SerializeField] private ShipParams _shipParams = default;

        private readonly List<AsteroidView> _asteroids = new();

        private ShipController _shipController;

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        private BulletManager _bulletManager;
        private GameObjectPool _pool;
        private Model _model;

        private void Awake()
        {
            var mainCamera = Camera.main;
            var orthographicHeight = mainCamera.orthographicSize;
            var orthographicWidth = orthographicHeight / mainCamera.pixelHeight * mainCamera.pixelWidth;

            var position = mainCamera.transform.position;
            _minX = position.x - orthographicWidth;
            _maxX = position.x + orthographicWidth;
            _minY = position.y - orthographicHeight;
            _maxY = position.y + orthographicHeight;
        }

        public void Connect(GameObjectPool pool, Model model)
        {
            _pool = pool;
            _model = model;

            _bulletManager = new BulletManager(pool, transform);
        }

        public void Tick(float deltaTime)
        {
            _shipController.Tick(deltaTime);
            foreach (var asteroidView in _asteroids)
            {
                asteroidView.Tick(deltaTime);
            }
            _bulletManager.Tick(deltaTime);
        }

        public ShipController SpawnShip()
        {
            var shipView = _pool.GetObject<ShipView>(_shipPrefab, transform);
            shipView.transform.position = _shipSpawnPoint.transform.position;

            var shipModel = new ShipModel(_shipParams);
            _shipController = new ShipController(shipView, shipModel, _pool, _bulletManager);
            
            _model.ShipModel = shipModel;

            shipModel.OnDeath += OnShipDeath;

            return _shipController;
        }

        private void OnShipDeath()
        {
            _pool.UtilizeObject(_shipController.ShipView);
        }

        public void SpawnNewWave()
        {
            for (var i = 0; i < ASTEROID_SPAWN_COUNT; i++)
            {
                var spawnPosition = new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));
                var dirVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                SpawnAsteroid(spawnPosition, dirVector, AsteroidSize.Large);
            }
        }

        private void SpawnAsteroid(Vector2 spawnPosition, Vector2 movementDirection, AsteroidSize asteroidSize)
        {
            var asteroidView = _pool.GetObject<AsteroidView>(_asteroidPrefab, _asteroidContainer);
            asteroidView.transform.position = spawnPosition;
            asteroidView.Connect(_pool , asteroidSize, movementDirection, OnAsteroidDeath);
            _asteroids.Add(asteroidView);
        }

        private void OnAsteroidDeath(AsteroidView asteroidView)
        {
            var oldSize = asteroidView.AsteroidSize;
            var position = asteroidView.transform.position;
            var newSize = (AsteroidSize)((int)oldSize - 1);
            if (newSize != AsteroidSize.Dead)
            {
                const float offset = 0.5f;
                var dirVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                SpawnAsteroid((Vector2)position + dirVector * offset, dirVector, newSize);
                SpawnAsteroid((Vector2)position + dirVector * -offset, -dirVector, newSize);
            }

            _pool.UtilizeObject(asteroidView.gameObject);
        }
    }
}