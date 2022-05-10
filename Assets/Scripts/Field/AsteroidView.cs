using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Field
{
    public class AsteroidView : MonoBehaviour, IReceivingDamage
    {
        [SerializeField] private float _movementSpeed = 1f;
        [SerializeField] private SpriteRenderer _spriteRenderer = default;
        [SerializeField] private List<Sprite> _smallSprites = default;
        [SerializeField] private List<Sprite> _mediumSprites = default;
        [SerializeField] private List<Sprite> _largeSprites = default;

        private Action<AsteroidView> _onDeathCallback;

        private Vector2 _movementDirection;
        private AsteroidSize _asteroidSize;

        public AsteroidSize AsteroidSize => _asteroidSize;

        // TODO: View shouldn't utilize itself
        private GameObjectPool _pool;

        public void Connect(GameObjectPool pool, AsteroidSize asteroidSize, Vector2 movementDirection, Action<AsteroidView> onDeathCallback)
        {
            _pool = pool;
            _asteroidSize = asteroidSize;
            _movementDirection = movementDirection;

            var sprites = _smallSprites;
            switch (asteroidSize)
            {
                case AsteroidSize.Small:
                    sprites = _smallSprites;
                    break;
                case AsteroidSize.Medium:
                    sprites = _mediumSprites;
                    break;
                case AsteroidSize.Large:
                    sprites = _largeSprites;
                    break;
                case AsteroidSize.Dead:
                default:
                    break;
            }

            _spriteRenderer.sprite = sprites[Random.Range(0, _largeSprites.Count)];

            _onDeathCallback = onDeathCallback;
        }

        public void Tick(float deltaTime)
        {
            transform.position += (Vector3)_movementDirection * (_movementSpeed * deltaTime);
        }

        public void ReceiveDamage()
        {
            _onDeathCallback?.Invoke(this);
        }

        public void Kill()
        {
            _pool.UtilizeObject(this);
        }
    }
}