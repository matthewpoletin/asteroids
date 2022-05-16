using Asteroids.Core.Core;
using Asteroids.Field;
using Asteroids.Score;
using Asteroids.UI;

namespace Asteroids
{
    public class GameContext : LifecycleItem
    {
        private GameState.GameState _currentState;

        public BattleModuleView UiView { get; }
        public Model Model { get; }
        public FieldController FieldController { get; }
        public ScoreManager ScoreManager { get; }

        public GameContext(BattleModuleView uiView, Model model, FieldController fieldController,
            ScoreManager scoreManager)
        {
            UiView = uiView;
            Model = model;
            FieldController = fieldController;
            ScoreManager = scoreManager;
        }

        public void ChangeState(GameState.GameState state)
        {
            _currentState?.Dispose();

            _currentState = state;
            _currentState.SetContext(this);
            _currentState.Initialize();
        }

        public override void Tick(float deltaTime)
        {
            _currentState.Tick(deltaTime);
        }

        public override void Dispose()
        {
            _currentState.Dispose();
        }
    }
}