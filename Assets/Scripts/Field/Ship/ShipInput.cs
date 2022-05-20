namespace Asteroids.Field
{
    public class ShipInput
    {
        private readonly ShipControls _shipControls;

        public ShipInput()
        {
            _shipControls = new ShipControls();
            _shipControls.Enable();
        }

        public bool ShootCannonTriggered => _shipControls.Default.ShootCannon.triggered;
        public bool ShootLaserTriggered => _shipControls.Default.ShootLaser.triggered;
        public float SteerValue => _shipControls.Default.Steer.ReadValue<float>();
        public bool SteerInProgress => _shipControls.Default.Steer.inProgress;
        public bool AccelerateInProgress => _shipControls.Default.Accelerate.inProgress;
    }
}