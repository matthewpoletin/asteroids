using Asteroids.Application;
using Asteroids.Field;
using Asteroids.UI;
using UnityEngine;

namespace Asteroids
{
    // TODO: Rename to Game/Application/GameController
    public class BattleModule
    {
        private GameContext _gameContext;
        private FieldView _fieldView;

        public void Connect(GameObjectPool pool)
        {
            // TODO: Move ApplicationTicker to bootstrap
            new GameObject(nameof(ApplicationTicker))
                .AddComponent<ApplicationTicker>()
                .Connect(Tick);

            var model = new Model();

            _fieldView = Object.FindObjectOfType<FieldView>();
            _fieldView.Connect(pool, model);

            var battleModuleView = Object.FindObjectOfType<BattleModuleView>();

            _gameContext = new GameContext(battleModuleView, model, _fieldView);

            var initialGameState = new InitializeGameState();
            _gameContext.ChangeState(initialGameState);
        }

        private void Tick(float deltaTime)
        {
            _gameContext.Tick(deltaTime);

            _fieldView.Tick(deltaTime);
        }
    }
}