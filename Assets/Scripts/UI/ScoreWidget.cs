using TMPro;
using UnityEngine;

namespace Asteroids.UI
{
    public class ScoreWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText = default;
        [SerializeField] private TextMeshProUGUI _lifeCountText = default;
        [SerializeField] private TextMeshProUGUI _laserAmmoCountText = default;

        private Model _model;

        public void Connect(Model model)
        {
            _model = model;
            _model.OnUserScoreChanged += OnUserScoreChanged;
            OnUserScoreChanged(_model.UserScore);

            _model.ShipModel.OnLifeCountChanged += OnLifeCountChanged;
            OnLifeCountChanged(_model.ShipModel.LifeCount);

            _model.ShipModel.OnLaserAmmoCountChanged += OnLaserAmmoCountChanged;
            OnLaserAmmoCountChanged(_model.ShipModel.LaserAmmoCount);
        }

        private void OnUserScoreChanged(int newValue)
        {
            _scoreText.text = newValue.ToString("0000");
        }

        private void OnLifeCountChanged(int newValue)
        {
            var result = "";
            for (var i = 0; i < newValue; i++)
            {
                result += "V";
            }

            _lifeCountText.text = result;
        }

        private void OnLaserAmmoCountChanged(int newValue)
        {
            var result = "";
            for (var i = 0; i < newValue; i++)
            {
                result += "I";
            }

            _laserAmmoCountText.text = result;
        }

        private void OnDispose()
        {
            _model.OnUserScoreChanged -= OnUserScoreChanged;
            _model.ShipModel.OnLifeCountChanged -= OnLifeCountChanged;
            _model.ShipModel.OnLaserAmmoCountChanged -= OnLaserAmmoCountChanged;
        }
    }
}