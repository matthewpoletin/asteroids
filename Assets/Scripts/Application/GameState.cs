namespace Asteroids.Application
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

