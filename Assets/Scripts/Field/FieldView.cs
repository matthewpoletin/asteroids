using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Field
{
    public enum AsteroidSize
    {
        Small = 1,
        Medium = 2,
        Large = 3,
    }

    public class FieldView : MonoBehaviour, ITick
    {
        private const int ASTEROID_SPAWN_COUNT = 5;
        private const int SAUCER_SPAWN_COUNT = 1;

        [SerializeField] private GameObject _asteroidPrefab = default;
        [SerializeField] private Transform _asteroidContainer = default;
        [SerializeField] private GameObject _shipPrefab = default;
        [SerializeField] private Transform _shipSpawnPoint = default;
        [SerializeField] private GameObject _saucerPrefab = default;
        [SerializeField] private ShipParams _shipParams = default;
        [SerializeField] private AsteroidParams _asteroidParams;
        [SerializeField] private SaucerParams _saucerParams;

        private readonly List<AsteroidController> _asteroids = new();
        private readonly List<SaucerController> _saucers = new();

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

            _bulletManager = new BulletManager(pool, this, transform);
        }

        public void Tick(float deltaTime)
        {
            _shipController.Tick(deltaTime);
            foreach (var asteroidController in _asteroids)
            {
                asteroidController.Tick(deltaTime);
            }

            foreach (var saucerController in _saucers)
            {
                saucerController.Tick(deltaTime);
            }

            _bulletManager.Tick(deltaTime);
        }

        public ShipController SpawnShip()
        {
            var shipView = _pool.GetObject<ShipView>(_shipPrefab, transform);
            shipView.transform.position = _shipSpawnPoint.transform.position;

            var shipModel = new ShipModel(_shipParams);
            _shipController = new ShipController(shipView, shipModel, _pool, this, _bulletManager);

            _model.ShipModel = shipModel;

            shipModel.OnDeath += OnShipDeath;

            return _shipController;
        }

        private void OnShipDeath()
        {
            _model.ShipModel.OnDeath -= OnShipDeath;
            _model.ShipModel = null;

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

            for (var i = 0; i < SAUCER_SPAWN_COUNT; i++)
            {
                var spawnPosition = new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));
                SpawnSaucer(spawnPosition);
            }
        }

        private void SpawnAsteroid(Vector2 spawnPosition, Vector2 movementDirection, AsteroidSize asteroidSize)
        {
            var asteroidController = new AsteroidController(_pool, _asteroidPrefab, _asteroidContainer, asteroidSize,
                movementDirection, spawnPosition, _asteroidParams);
            _asteroids.Add(asteroidController);
        }

        public AsteroidController GetAsteroidController(AsteroidView asteroidView)
        {
            return _asteroids.FirstOrDefault(controller => controller.AsteroidView == asteroidView);
        }

        private void OnAsteroidDeath(AsteroidController asteroidController)
        {
            asteroidController.Utilize();
            _asteroids.Remove(asteroidController);
            _pool.UtilizeObject(asteroidController.AsteroidView);
        }

        public void SplitOrDestroyAsteroid(AsteroidController asteroidController)
        {
            var oldSize = asteroidController.AsteroidSize;
            var position = asteroidController.AsteroidView.transform.position;
            if (oldSize != AsteroidSize.Small)
            {
                var newSize = (AsteroidSize)((int)oldSize - 1);
                const float offset = 0.5f;
                var dirVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                SpawnAsteroid((Vector2)position + dirVector * offset, dirVector, newSize);
                SpawnAsteroid((Vector2)position + dirVector * -offset, -dirVector, newSize);
            }

            OnAsteroidDeath(asteroidController);
        }

        public void DestroyAsteroid(AsteroidController asteroidController)
        {
            asteroidController.Utilize();
            _asteroids.Remove(asteroidController);
        }

        private void SpawnSaucer(Vector2 position)
        {
            var saucerView = _pool.GetObject<SaucerView>(_saucerPrefab, transform);
            saucerView.transform.position = position;
            var saucerController = new SaucerController(saucerView, _saucerParams, _shipController);
            _saucers.Add(saucerController);
        }

        public SaucerController GetSaucerController(SaucerView saucerView)
        {
            return _saucers.FirstOrDefault(controller => controller.SaucerView == saucerView);
        }

        public void DestroySaucer(SaucerController saucerController)
        {
            _saucers.Remove(saucerController);
            _pool.UtilizeObject(saucerController.SaucerView);
        }
    }
}