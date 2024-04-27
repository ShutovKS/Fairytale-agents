using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.Input;
using Infrastructure.Services.WindowsService;
using Mumu;
using UI.Mumu.Scrips;
using UnityEngine;

namespace Infrastructure.ProjectStateMachine.States
{
    public class MumuState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public MumuState(GameBootstrap initializer, ISaveLoadService saveLoadService, IProgressService progressService,
            IWindowService windowService, PlayerInputActionReader inputActionReader)
        {
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            _windowService = windowService;
            _inputActionReader = inputActionReader;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progressService;
        private readonly IWindowService _windowService;
        private readonly PlayerInputActionReader _inputActionReader;

        private GameManager _gameManager;

        public async void OnEnter()
        {
            var mumuUI = await _windowService.OpenAndGetComponent<MumuUI>(WindowID.Mumu);

            _gameManager = GameManager.Instance;
            _gameManager.StartGame(mumuUI);

            _gameManager.OnLost += Lost;
            _gameManager.OnWon += Won;

            _progressService.PlayerProgress.gameStageType = GameStageType.Mumu;
            _saveLoadService.SaveProgress();

            _windowService.Close(WindowID.Loading);
        }

        public void OnExit()
        {
            _windowService.Close(WindowID.Mumu);
            _windowService.Open(WindowID.Loading);

            _gameManager.OnLost -= Lost;
            _gameManager.OnWon -= Won;
        }

        private void Lost()
        {
            Debug.Log($"Lost");
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }

        private void Won()
        {
            Debug.Log($"Won");
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }
    }
}