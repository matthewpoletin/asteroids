using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Core.Core;
using UnityEngine;

namespace Asteroids.Field
{
    public interface ISaucerBehavior : ITick
    {
        Vector2 MovementDirection { get; }
    }

    public class SaucerFollowShipBehavior : ISaucerBehavior
    {
        private readonly Transform _transform;
        private readonly ShipController _shipController;

        private readonly Func<float> _getDirectionChangeDelay;

        private float _positionChangeTimer;

        private readonly float _fieldWidth;
        private readonly float _fieldHeight;

        private Vector2 SaucerPosition => _transform.transform.position;

        public Vector2 MovementDirection { get; private set; }

        public SaucerFollowShipBehavior(Transform transform, ShipController shipController, SaucerParams saucerParams)
        {
            _transform = transform;
            _shipController = shipController;
            _getDirectionChangeDelay = () => saucerParams.MovementDirectionChangeDelaySeconds;

            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera was not found");
                return;
            }

            _fieldHeight = 2 * mainCamera.orthographicSize;
            _fieldWidth = _fieldHeight / mainCamera.pixelHeight * mainCamera.pixelWidth;

            UpdateMovementDirection();
        }

        public void Tick(float deltaTime)
        {
            if (_positionChangeTimer > 0)
            {
                _positionChangeTimer -= deltaTime;
                if (_positionChangeTimer <= 0)
                {
                    UpdateMovementDirection();
                }
            }
        }

        private void UpdateMovementDirection()
        {
            var shipPosition = (Vector2)_shipController.ShipView.transform.position;

            var allShipPositions = new List<Vector2>
            {
                shipPosition,
                new(shipPosition.x + _fieldWidth, shipPosition.y),
                new(shipPosition.x - _fieldWidth, shipPosition.y),
                new(shipPosition.x, shipPosition.y + _fieldHeight),
                new(shipPosition.x, shipPosition.y - _fieldHeight),
            };

            var targetShipPosition = allShipPositions.OrderBy(position => (position - SaucerPosition).magnitude)
                .FirstOrDefault();
            MovementDirection = (targetShipPosition - SaucerPosition).normalized;

            _positionChangeTimer = _getDirectionChangeDelay.Invoke();
        }
    }
}