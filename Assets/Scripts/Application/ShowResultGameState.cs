using UnityEngine.InputSystem;

namespace Asteroids.Application
{
    public class ShowResultGameState : GameState
    {
        public override void Initialize()
        {
            Context.UiView.OpenResultsDialog();
        }

        public override void Tick(float deltaTime)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                Context.ChangeState(new InitializeGameState());
            }
        }

        public override void Dispose()
        {
            Context.UiView.CloseAllDialogs();
        }
    }
}