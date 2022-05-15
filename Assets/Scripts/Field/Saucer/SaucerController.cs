using Asteroids.Core.Core;
using UnityEngine;

namespace Asteroids.Field
{
    public class SaucerController : IFieldActor, ITick
    {
        private readonly ISaucerBehavior _saucerFollowShipBehavior;
        private readonly ISaucerMover _saucerMover;

        public Transform Transform => SaucerView.transform;

        public SaucerView SaucerView { get; }

        public SaucerController(SaucerView saucerView, SaucerParams saucerParams, ShipController shipController)
        {
            SaucerView = saucerView;

            var transform = SaucerView.transform;
            _saucerFollowShipBehavior = new SaucerFollowShipBehavior(transform, shipController, saucerParams);
            _saucerMover = new SaucerMover(transform, saucerParams);
        }

        public void Tick(float deltaTime)
        {
            _saucerFollowShipBehavior.Tick(deltaTime);
            _saucerMover.PerformMovement(deltaTime, _saucerFollowShipBehavior.MovementDirection);
        }
    }
}