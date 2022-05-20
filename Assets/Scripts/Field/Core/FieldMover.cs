using UnityEngine;

namespace Asteroids.Field
{
    public class FieldMover
    {
        private readonly Transform _transform;

        public FieldMover(Transform transform)
        {
            _transform = transform;
        }

        public void PerformMovement(float deltaTime, Vector2 direction, float velocity)
        {
            _transform.position += (Vector3)direction * (deltaTime * velocity);
        }
    }
}