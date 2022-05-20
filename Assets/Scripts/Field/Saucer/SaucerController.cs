using Asteroids.Core.Core;
using UnityEngine;

namespace Asteroids.Field
{
    public class SaucerController : IFieldActor, ITick
    {
        private readonly ISaucerBehavior _saucerFollowShipBehavior;
        private readonly FieldMover _fieldMover;
        private readonly SaucerParams _saucerParams;

        public Transform Transform => SaucerView.transform;

        public SaucerView SaucerView { get; }

        public SaucerController(SaucerView saucerView, SaucerParams saucerParams, ShipController shipController)
        {
            SaucerView = saucerView;
            _saucerParams = saucerParams;

            var transform = SaucerView.transform;
            _saucerFollowShipBehavior = new SaucerFollowShipBehavior(transform, shipController, saucerParams);
            _fieldMover = new FieldMover(transform);
        }

        public void Tick(float deltaTime)
        {
            _saucerFollowShipBehavior.Tick(deltaTime);
            _fieldMover.PerformMovement(deltaTime, _saucerFollowShipBehavior.MovementDirection,
                _saucerParams.MovementSpeed);
        }
    }
}