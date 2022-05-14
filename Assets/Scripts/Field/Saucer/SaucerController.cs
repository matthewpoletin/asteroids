using UnityEngine;

namespace Asteroids.Field
{
    public class SaucerController : ITick
    {
        private readonly SaucerBrain _saucerBrain;
        private readonly SaucerMover _saucerMover;

        public SaucerView SaucerView { get; }

        public SaucerController(SaucerView saucerView, SaucerParams saucerParams, ShipController shipController)
        {
            SaucerView = saucerView;

            // TODO: Set position change delay from outside
            _saucerBrain = new SaucerBrain(shipController, SaucerView, saucerParams);
            _saucerMover = new SaucerMover(SaucerView.transform, saucerParams);
        }

        public void Tick(float deltaTime)
        {
            _saucerBrain.Tick(deltaTime);
            _saucerMover.PerformMovement(deltaTime, _saucerBrain.MovementDirection);
        }
    }
}