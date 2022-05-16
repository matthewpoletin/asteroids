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
            if (Context.FieldController.AllTargetsDestroyed)
            {
                Context.FieldController.SpawnNewWave();
            }
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