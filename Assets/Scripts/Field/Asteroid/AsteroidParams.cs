using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteroids.Field
{
    public enum AsteroidSize
    {
        Small = 1,
        Medium = 2,
        Large = 3,
    }

    [CreateAssetMenu(fileName = "asteroid_params", menuName = "Params/AsteroidParams", order = 2)]
    public class AsteroidParams : ScriptableObject
    {
        [Serializable]
        public class AsteroidData
        {
            [SerializeField] private AsteroidSize _size;
            [SerializeField] private float _speed;

            public AsteroidSize Size => _size;
            public float Speed => _speed;
        }

        [SerializeField] private List<AsteroidData> _data;

        public float GetAsteroidSpeed(AsteroidSize size)
        {
            return _data.FirstOrDefault(data => data.Size == size)?.Speed ?? 0f;
        }
    }
}