using UnityEngine;

namespace Asteroids.Field
{
    [CreateAssetMenu(fileName = "saucer_params", menuName = "Params/SaucerParams", order = 3)]
    public class SaucerParams : ScriptableObject
    {
        [SerializeField] private float _movementSpeed = default;
        [SerializeField] private float _movementDirectionChangeDelaySeconds = 1f;

        public float MovementSpeed => _movementSpeed;
        public float MovementDirectionChangeDelaySeconds => _movementDirectionChangeDelaySeconds;
    }
}