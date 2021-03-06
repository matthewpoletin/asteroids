using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Core.Core;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Asteroids.Field
{
    public class FieldController : ITick
    {
        private const int ASTEROID_SPAWN_COUNT = 5;
        private const int SAUCER_SPAWN_COUNT = 1;

        public event Action<AsteroidSize> AsteroidDestroyed;
        public event Action SaucerDestroyed;

        private readonly List<AsteroidController> _asteroids = new();
        private readonly List<SaucerController> _saucers = new();
        private ShipController _shipController;

        private readonly GameObjectPool _pool;
        private readonly Model _model;

        private readonly BoundsController _boundsController;
        private readonly BulletManager _bulletManager;

        public BoundsController BoundsController => _boundsController;

        public bool AllTargetsDestroyed => _asteroids.Count == 0 && _saucers.Count == 0;

        private FieldView FieldView { get; }

        public FieldController(FieldView fieldView, GameObjectPool pool, Model model)
        {
            FieldView = fieldView;
            _pool = pool;
            _model = model;

            _boundsController = new BoundsController();
            _bulletManager = new BulletManager(pool, this, FieldView.transform);
        }

        public void Tick(float deltaTime)
        {
            _shipController?.Tick(deltaTime);
            foreach (var asteroidController in _asteroids)
            {
                asteroidController.Tick(deltaTime);
            }

            foreach (var saucerController in _saucers)
            {
                saucerController.Tick(deltaTime);
            }

            _bulletManager.Tick(deltaTime);

            BoundsController.ValidateEntitiesPositions();
        }

        public ShipController SpawnShip()
        {
            var shipView = _pool.GetObject<ShipView>(FieldView.ShipPrefab, FieldView.transform);
            shipView.transform.position = FieldView.ShipSpawnPoint.transform.position;

            var shipModel = new ShipModel(FieldView.ShipParams);
            _shipController = new ShipController(shipView, shipModel, _pool, this, _bulletManager);

            _model.ShipModel = shipModel;

            shipModel.OnDeath += OnShipDeath;

            BoundsController.AddFieldEntity(_shipController);

            return _shipController;
        }

        private void OnShipDeath()
        {
            _model.ShipModel.OnDeath -= OnShipDeath;
            _model.ShipModel = null;

            _pool.UtilizeObject(_shipController.ShipView);

            _shipController = null;

            BoundsController.RemoveFieldEntity(_shipController);
        }

        public void SpawnNewWave()
        {
            for (var i = 0; i < ASTEROID_SPAWN_COUNT; i++)
            {
                var spawnPosition = BoundsController.GetSpawnPosition();
                var dirVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                SpawnAsteroid(spawnPosition, dirVector, AsteroidSize.Large);
            }

            for (var i = 0; i < SAUCER_SPAWN_COUNT; i++)
            {
                var spawnPosition = BoundsController.GetSpawnPosition();
                SpawnSaucer(spawnPosition);
            }
        }

        private void SpawnAsteroid(Vector2 spawnPosition, Vector2 movementDirection, AsteroidSize asteroidSize)
        {
            var asteroidController = new AsteroidController(_pool, FieldView.AsteroidPrefab,
                FieldView.AsteroidContainer, asteroidSize,
                movementDirection, spawnPosition, FieldView.AsteroidParams);
            _asteroids.Add(asteroidController);
            BoundsController.AddFieldEntity(asteroidController);
        }

        public AsteroidController GetAsteroidController(AsteroidView asteroidView)
        {
            return _asteroids.FirstOrDefault(controller => controller.AsteroidView == asteroidView);
        }

        public void DestroyAsteroid(AsteroidController asteroidController)
        {
            AsteroidDestroyed?.Invoke(asteroidController.AsteroidSize);
            UtilizeAsteroid(asteroidController);
        }

        private void UtilizeAsteroid(AsteroidController asteroidController)
        {
            asteroidController.Utilize();
            _asteroids.Remove(asteroidController);
            BoundsController.RemoveFieldEntity(asteroidController);
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

            DestroyAsteroid(asteroidController);
        }

        private void SpawnSaucer(Vector2 position)
        {
            var saucerView = _pool.GetObject<SaucerView>(FieldView.SaucerPrefab, FieldView.transform);
            saucerView.transform.position = position;
            var saucerController = new SaucerController(saucerView, FieldView.SaucerParams, _shipController);
            _saucers.Add(saucerController);
            BoundsController.AddFieldEntity(saucerController);
        }

        public SaucerController GetSaucerController(SaucerView saucerView)
        {
            return _saucers.FirstOrDefault(controller => controller.SaucerView == saucerView);
        }

        public void DestroySaucer(SaucerController saucerController)
        {
            SaucerDestroyed?.Invoke();
            UtilizeSaucer(saucerController);
        }

        private void UtilizeSaucer(SaucerController saucerController)
        {
            _saucers.Remove(saucerController);
            BoundsController.RemoveFieldEntity(saucerController);
            _pool.UtilizeObject(saucerController.SaucerView);
        }

        public void Reset()
        {
            for (var index = _asteroids.Count - 1; index >= 0; index--)
            {
                var asteroidController = _asteroids[index];
                UtilizeAsteroid(asteroidController);
            }

            _asteroids.Clear();

            for (var index = _saucers.Count - 1; index >= 0; index--)
            {
                var saucerController = _saucers[index];
                UtilizeSaucer(saucerController);
            }

            _saucers.Clear();

            BoundsController.Reset();
            _bulletManager.Reset();
        }
    }
}