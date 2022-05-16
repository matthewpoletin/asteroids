using Asteroids.Field;

namespace Asteroids.Score
{
    public class ScoreManager
    {
        private readonly Model _model;
        private readonly ScoreParams _scoreParams;

        public ScoreManager(Model model, FieldController fieldController, ScoreParams scoreParams)
        {
            _model = model;
            _scoreParams = scoreParams;

            fieldController.AsteroidDestroyed += OnAsteroidDestroyed;
            fieldController.SaucerDestroyed += OnSaucerDestroyed;
        }

        private void OnAsteroidDestroyed(AsteroidSize size)
        {
            switch (size)
            {
                case AsteroidSize.Small: 
                    _model.UserScore += _scoreParams.AsteroidSmallKill;
                    break;
                case AsteroidSize.Medium:
                    _model.UserScore += _scoreParams.AsteroidMediumKill;
                    break;
                case AsteroidSize.Large:
                    _model.UserScore += _scoreParams.AsteroidLargeKill;
                    break;
            }
        }

        private void OnSaucerDestroyed()
        {
            _model.UserScore += _scoreParams.SaucerKill;
        }

        public void Reset()
        {
            _model.UserScore = 0;
        }
    }
}