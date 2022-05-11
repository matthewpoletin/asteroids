using Asteroids.Field;
using UnityEngine;

namespace Asteroids.UI
{
    public class BattleModuleView : MonoBehaviour
    {
        [SerializeField] private ScoreWidget _scoreWidget = default;
        [SerializeField] private ShipDebugWidget _shipDebugWidget = default;

        public void Connect(Model model, ShipController shipController)
        {
            _scoreWidget.Connect(model);
            _shipDebugWidget.Connect(shipController);
        }
    }
}