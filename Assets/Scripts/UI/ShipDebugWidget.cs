using Asteroids.Field;
using TMPro;
using UnityEngine;

namespace Asteroids.UI
{
    public class ShipDebugWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _positionText = default;
        [SerializeField] private TextMeshProUGUI _rotationText = default;
        [SerializeField] private TextMeshProUGUI _speedText = default;
        [SerializeField] private TextMeshProUGUI _laserAmmoCountText = default;
        [SerializeField] private TextMeshProUGUI _laserAmmoResetTimerText = default;

        private ShipController _shipController;

        public void Connect(ShipController shipController)
        {
            _shipController = shipController;
        }

        private void Update()
        {
            if (_shipController == null)
            {
                return;
            }

            var shipTransform = _shipController.ShipView.transform;
            var transformPosition = shipTransform.position;
            _positionText.text = $"{transformPosition.x:0.00}; {transformPosition.y:0.00}";
            _rotationText.text = $"{shipTransform.eulerAngles.z:0.00}";
            _speedText.text = $"{_shipController.Speed:0.00}";
            _laserAmmoCountText.text = _shipController.ShipModel.LaserAmmoCount.ToString();
            _laserAmmoResetTimerText.text = _shipController.ShipModel.LaserAmmoResetTimer.ToString(".00");
        }
    }
}