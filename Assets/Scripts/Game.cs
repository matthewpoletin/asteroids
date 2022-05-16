using Asteroids.Core;
using Asteroids.Core.Core;
using Asteroids.Field;
using Asteroids.GameState;
using Asteroids.Score;
using Asteroids.UI;
using UnityEngine;
using Utils;

namespace Asteroids
{
    public class Game
    {
        private GameContext _gameContext;
        private FieldController _fieldController;
        private ScoreManager _scoreManager;

        private Model _model;

        public void Connect(GameObjectPool pool, ApplicationTicker applicationTicker)
        {
            applicationTicker.OnTick += Tick;

            _model = new Model();

            var fieldView = Object.FindObjectOfType<FieldView>();
            _fieldController = new FieldController(fieldView, pool, _model);

            var battleModuleView = Object.FindObjectOfType<BattleModuleView>();

            _scoreManager = new ScoreManager(_model, _fieldController, fieldView.ScoreParams);

            _gameContext = new GameContext(battleModuleView, _model, _fieldController, _scoreManager);

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