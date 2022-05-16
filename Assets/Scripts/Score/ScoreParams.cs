using UnityEngine;

namespace Asteroids.Score
{
    [CreateAssetMenu(fileName = "score_params", menuName = "Params/ScoreParams", order = 0)]
    public class ScoreParams : ScriptableObject
    {
        [SerializeField] private int _asteroidSmallKill = default;
        [SerializeField] private int _asteroidMediumKill = default;
        [SerializeField] private int _asteroidLargeKill = default;
        [SerializeField] private int _saucerKill = default;

        public int AsteroidSmallKill => _asteroidSmallKill;
        public int AsteroidMediumKill => _asteroidMediumKill;
        public int AsteroidLargeKill => _asteroidLargeKill;
        public int SaucerKill => _saucerKill;
    }
}