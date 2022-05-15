namespace Asteroids.GameState
{
    public class RunningGameState : GameState
    {
        public override void Initialize()
        {
            Context.Model.ShipModel.OnDeath += OnShipDeath;
        }

        public override void Tick(float deltaTime)
        {
        }

        private void OnShipDeath()
        {
            Context.ChangeState(new ShowResultGameState());
        }

        public override void Dispose()
        {
        }
    }
}