namespace Asteroids.GameState
{
    public class InitializeGameState : GameState
    {
        public override void Initialize()
        {
            Context.FieldController.Reset();

            Context.ScoreManager.Reset();
            var shipController = Context.FieldController.SpawnShip();
            Context.FieldController.SpawnNewWave();

            Context.UiView.Connect(Context.Model, shipController);
        }

        public override void Tick(float deltaTime)
        {
            Context.ChangeState(new RunningGameState());
        }

        public override void Dispose()
        {
        }
    }
}