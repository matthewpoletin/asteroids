using UnityEngine.InputSystem;

namespace Asteroids.GameState
{
    public class ShowResultGameState : GameState
    {
        public override void Initialize()
        {
            Context.UiView.OpenResultsDialog(OnRestartButtonClick);
        }

        public override void Tick(float deltaTime)
        {
        }

        private void OnRestartButtonClick()
        {
            Restart();
        }

        private void Restart()
        {
            Context.ChangeState(new InitializeGameState());
        }

        public override void Dispose()
        {
            Context.UiView.CloseAllDialogs();
        }
    }
}