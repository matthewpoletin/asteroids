using System;

namespace Asteroids.Core.Core
{
    public abstract class LifecycleItem : ITick, IDisposable
    {
        public abstract void Tick(float deltaTime);
        public abstract void Dispose();
    }
}