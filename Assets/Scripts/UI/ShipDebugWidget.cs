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

        private ShipView _shipView;

        public void Connect(ShipView shipView)
        {
            _shipView = shipView;
        }

        private void Update()
        {
            if (_shipView == null)
            {
                return;
            }

            var shipTransform = _shipView.transform;
            var transformPosition = shipTransform.position;
            _positionText.text = $"{transformPosition.x:0.00}; {transformPosition.y:0.00}";
            _rotationText.text = $"{shipTransform.eulerAngles.z:0.00}";
            _speedText.text = $"{_shipView.Speed:0.00}";
            _laserAmmoCountText.text = _shipView.ShipModel.LaserAmmoCount.ToString();
            _laserAmmoResetTimerText.text = _shipView.ShipModel.LaserAmmoResetTimer.ToString(".00");
        }
    }
}
