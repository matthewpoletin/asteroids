using System;
using Asteroids.Core.Core;
using Asteroids.Field;

namespace Asteroids
{
    public class Model : ITick
    {
        private int _userScore = 1;
        public event Action<int> OnUserScoreChanged;

        public int UserScore
        {
            get => _userScore;
            set
            {
                _userScore = value;
                OnUserScoreChanged?.Invoke(_userScore);
            }
        }

        public ShipModel ShipModel { get; set; }

        public void Tick(float deltaTime)
        {
            ShipModel?.Tick(deltaTime);
        }
    }
}