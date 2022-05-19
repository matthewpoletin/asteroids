using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.UI
{
    public class ResultsDialog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText = default;
        [SerializeField] private Button _restartButton = default;

        private Action _onRestartButtonClick;

        public void Connect(Model model, Action onRestartButtonClick)
        {
            _scoreText.text = model.UserScore.ToString();

            _onRestartButtonClick = onRestartButtonClick;
        }

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        private void OnRestartButtonClick()
        {
            _onRestartButtonClick?.Invoke();
        }

        public void Utilize()
        {
            _onRestartButtonClick = null;
        }
    }
}