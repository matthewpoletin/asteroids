using UnityEngine;

namespace Asteroids.Field
{
    public class SaucerMover : ISaucerMover
    {
        private readonly Transform _transform;
        private readonly SaucerParams _saucerParams;

        public SaucerMover(Transform transform, SaucerParams saucerParams)
        {
            _transform = transform;
            _saucerParams = saucerParams;
        }

        public void PerformMovement(float deltaTime, Vector2 movementDirection)
        {
            _transform.position += (Vector3)movementDirection * (_saucerParams.MovementSpeed * deltaTime);
        }
    }
}