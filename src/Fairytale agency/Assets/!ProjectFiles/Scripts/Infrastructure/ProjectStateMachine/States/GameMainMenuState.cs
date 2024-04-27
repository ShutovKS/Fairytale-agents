using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.GameData;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.WindowsService;
using UI.MainMenuScreen;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.ProjectStateMachine.States
{
    public class GameMainMenuState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public GameMainMenuState(GameBootstrap initializer, IWindowService windowService,
            IProgressService progressService, ISaveLoadService saveLoadService)
        {
            _windowService = windowService;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly IWindowService _windowService;
        private readonly IProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public void OnEnter()
        {
            var asyncOperation = Addressables.LoadSceneAsync(AssetsAddressableConstants.EMPTY_2D_SCENE);
            asyncOperation.Completed += _ => OpenMainMenuWindow();
        }

        private async void OpenMainMenuWindow()
        {
            var mainMenuScreen = await _windowService.OpenAndGetComponent<MainMenuScreen>(WindowID.MainMenu);

            mainMenuScreen.SetLoadGameButtonIsInteractable(_progressService.PlayerProgress.gameStageType !=
                                                           GameStageType.None);

            mainMenuScreen.OnStartNewGameButtonClicked += OnStartNewGameButtonClicked;
            mainMenuScreen.OnLoadGameButtonClicked += OnLoadGameButtonClicked;
            mainMenuScreen.OnExitButtonClicked += OnExitButtonClicked;

            CloseLoadingWindow();
        }

        private void OnStartNewGameButtonClicked()
        {
            _progressService.SetProgress(new PlayerProgress());
            _saveLoadService.SaveProgress();

            OnLoadGameButtonClicked();
        }

        private void OnLoadGameButtonClicked()
        {
            Initializer.StateMachine.SwitchState<LoadingGameplayState, GameStageType>(_progressService.PlayerProgress
                .gameStageType);
        }

        private static void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void CloseLoadingWindow()
        {
            _windowService.Close(WindowID.Loading);
        }

        private void CloseMainMenuWindow()
        {
            _windowService.Close(WindowID.MainMenu);
            _windowService.Open(WindowID.Loading);
        }

        public void OnExit()
        {
            CloseMainMenuWindow();
        }
    }
}