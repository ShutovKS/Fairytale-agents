using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Mumu;
using UnityEngine;

namespace Infrastructure.ProjectStateMachine.States
{
    public class MumuState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public MumuState(GameBootstrap initializer, ISaveLoadService saveLoadService, IProgressService progressService)
        {
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progressService;

        private GameManager _gameManager;

        public void OnEnter()
        {
            _gameManager = GameManager.Instance;
            _gameManager.StartGame();

            _gameManager.OnLost += Lost;
            _gameManager.OnWon += Won;

            _progressService.PlayerProgress.gameStageType = GameStageType.Prologue;
            _saveLoadService.SaveProgress();
        }

        public void OnExit()
        {
            _gameManager.OnLost -= Lost;
            _gameManager.OnWon -= Won;
        }

        private void Lost()
        {
            Debug.Log($"Lost");
        }

        private void Won()
        {
            Debug.Log($"Won");
        }
    }
}