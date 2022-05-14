using Asteroids.Field;
using UnityEngine;

namespace Asteroids.UI
{
    public class BattleModuleView : MonoBehaviour
    {
        [SerializeField] private ScoreWidget _scoreWidget = default;
        [SerializeField] private ShipDebugWidget _shipDebugWidget = default;
        [SerializeField] private ResultsDialog _resultsDialog = default;

        private Model _model;

        public void Connect(Model model, ShipController shipController)
        {
            _model = model;

            _scoreWidget.Connect(_model);
            _shipDebugWidget.Connect(shipController);
        }

        public void OpenResultsDialog()
        {
            _resultsDialog.gameObject.SetActive(true);

            _resultsDialog.Connect(_model);
        }

        public void CloseAllDialogs()
        {
            _resultsDialog.gameObject.SetActive(false);
        }
    }
}