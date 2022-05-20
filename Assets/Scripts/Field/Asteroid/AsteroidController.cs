using Asteroids.Core.Core;
using UnityEngine;
using Utils;

namespace Asteroids.Field
{
    public class AsteroidController : ITick, IFieldActor
    {
        private readonly GameObjectPool _pool;
        private readonly AsteroidParams _asteroidParams;
        private readonly Vector2 _movementDirection;
        private readonly FieldMover _fieldMover;

        private float MovementSpeed => _asteroidParams.GetAsteroidSpeed(_asteroidSize);

        private readonly AsteroidSize _asteroidSize;

        public AsteroidSize AsteroidSize => _asteroidSize;
        public Transform Transform => AsteroidView.transform;

        public AsteroidView AsteroidView { get; }

        public AsteroidController(GameObjectPool pool, GameObject asteroidPrefab, Transform asteroidContainer,
            AsteroidSize asteroidSize, Vector2 movementDirection,
            Vector3 spawnPosition, AsteroidParams asteroidParams)
        {
            _pool = pool;
            _asteroidParams = asteroidParams;
            _asteroidSize = asteroidSize;

            _movementDirection = movementDirection;

            AsteroidView = _pool.GetObject<AsteroidView>(asteroidPrefab, asteroidContainer);
            AsteroidView.SetAsteroidSpriteBySize(asteroidSize);
            AsteroidView.transform.position = spawnPosition;

            _fieldMover = new FieldMover(AsteroidView.transform);
        }

        public void Tick(float deltaTime)
        {
            _fieldMover.PerformMovement(deltaTime, _movementDirection, MovementSpeed);
        }

        public void Utilize()
        {
            _pool.UtilizeObject(AsteroidView);
        }
    }
}