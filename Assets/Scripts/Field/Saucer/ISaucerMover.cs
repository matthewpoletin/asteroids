using UnityEngine;

namespace Asteroids.Field
{
    public interface ISaucerMover
    {
        void PerformMovement(float deltaTime, Vector2 movementDirection);
    }
}