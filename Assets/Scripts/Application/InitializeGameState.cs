namespace Asteroids.Application
{
    public class InitializeGameState : GameState
    {
        public override void Initialize()
        {
            Context.FieldView.Reset();

            var shipController = Context.FieldView.SpawnShip();
            Context.FieldView.SpawnNewWave();

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