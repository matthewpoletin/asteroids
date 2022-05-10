using Asteroids.Field;
using UnityEngine;

namespace Asteroids.Application
{
    public class InitializeGameState : GameState
    {
        public override void Initialize()
        {
            Context.FieldView.SpawnShip();
            Context.FieldView.SpawnNewWave();

            var shipView = Object.FindObjectOfType<ShipView>();
            Context.UiView.Connect(Context.Model, shipView);
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