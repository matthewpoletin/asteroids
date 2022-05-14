using UnityEngine;

namespace Asteroids.Field
{
    public interface IFieldActor
    {
        public Transform Transform { get; }
    }

    public interface IDestroyedFieldActor : IFieldActor
    {
        public void OnLeavingBounds();
    }
}