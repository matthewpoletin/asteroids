using TMPro;
using UnityEngine;

namespace Asteroids.UI
{
    public class ResultsDialog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText = default;

        public void Connect(Model model)
        {
            _scoreText.text = model.UserScore.ToString();
        }
    }
}