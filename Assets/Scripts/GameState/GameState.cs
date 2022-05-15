using Asteroids.Core.Core;

namespace Asteroids.GameState
{
    public abstract class GameState : LifecycleItem
    {
        protected GameContext Context;

        public void SetContext(GameContext context)
        {
            Context = context;
        }

        public abstract void Initialize();
    }
}

