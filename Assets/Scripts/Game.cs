using Asteroids.Application;
using Asteroids.Field;
using Asteroids.UI;
using UnityEngine;

namespace Asteroids
{
    public class Game
    {
        private GameContext _gameContext;
        private FieldController _fieldController;

        private Model _model;

        public void Connect(GameObjectPool pool, ApplicationTicker applicationTicker)
        {
            applicationTicker.OnTick += Tick;

            _model = new Model();

            var fieldView = Object.FindObjectOfType<FieldView>();
            _fieldController = new FieldController(fieldView, pool, _model);

            var battleModuleView = Object.FindObjectOfType<BattleModuleView>();

            _gameContext = new GameContext(battleModuleView, _model, _fieldController);

            var initialGameState = new InitializeGameState();
            _gameContext.ChangeState(initialGameState);
        }

        private void Tick(float deltaTime)
        {
            _gameContext.Tick(deltaTime);
            _model.Tick(deltaTime);
            _fieldController.Tick(deltaTime);
        }
    }
}