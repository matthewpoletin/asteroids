using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Field
{
    public class AsteroidView : MonoBehaviour, IShipDamagingView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer = default;
        [SerializeField] private List<Sprite> _smallSprites = default;
        [SerializeField] private List<Sprite> _mediumSprites = default;
        [SerializeField] private List<Sprite> _largeSprites = default;

        public void SetAsteroidSpriteBySize(AsteroidSize asteroidSize)
        {
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
            }

            _spriteRenderer.sprite = sprites[Random.Range(0, _largeSprites.Count)];
        }
    }
}