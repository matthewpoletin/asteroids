using Asteroids.Application;
using Asteroids.Field;
using Asteroids.UI;

namespace Asteroids
{
    public class GameContext : LifecycleItem
    {
        private GameState _currentState;

        public BattleModuleView UiView { get; }
        public Model Model { get; }
        public FieldController FieldController { get; set; }

        public GameContext(BattleModuleView uiView, Model model, FieldController fieldController)
        {
            UiView = uiView;
            Model = model;
            FieldController = fieldController;
        }

        public void ChangeState(GameState state)
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