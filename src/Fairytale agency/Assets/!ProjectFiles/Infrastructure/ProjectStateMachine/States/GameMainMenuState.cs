using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.GameData;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.SoundsService;
using Infrastructure.Services.WindowsService;
using UI.MainMenuScreen;
using UnityEngine;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using UnityEditor;

#else
using UnityEngine;
#endif

namespace Infrastructure.ProjectStateMachine.States
{
    public class GameMainMenuState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public GameMainMenuState(GameBootstrap initializer, IWindowService windowService,
            IProgressService progressService, ISaveLoadService saveLoadService, ISoundService soundService)
        {
            _windowService = windowService;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _soundService = soundService;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly IWindowService _windowService;
        private readonly IProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISoundService _soundService;

        public void OnEnter()
        {
            var asyncOperation = Addressables.LoadSceneAsync(AssetsAddressableConstants.EMPTY_2D_SCENE);
            asyncOperation.Completed += _ => OpenMainMenuWindow();
        }

        public void OnExit()
        {
            CloseMainMenuWindow();
        }

        private async void OpenMainMenuWindow()
        {
            var mainMenuScreen = await _windowService.OpenAndGetComponent<MainMenuScreen>(WindowID.MainMenu);

            mainMenuScreen.SetLoadGameButtonIsInteractable(_progressService.PlayerProgress.gameStageType !=
                                                           GameStageType.None);

            mainMenuScreen.OnStartNewGameButtonClicked += StartNewGame;
            mainMenuScreen.OnLoadGameButtonClicked += LoadGame;
            mainMenuScreen.OnExitButtonClicked += Quit;

            var audioClip = Resources.Load<AudioClip>("Sounds/menu-sound");
            _soundService.PlaySoundsClip(audioClip);

            CloseLoadingWindow();
        }

        private void StartNewGame()
        {
            _progressService.SetProgress(new PlayerProgress());
            _saveLoadService.SaveProgress();

            LoadGame();
        }

        private void LoadGame()
        {
            var stageType = _progressService.PlayerProgress.gameStageType;
            Initializer.StateMachine.SwitchState<LoadingGameplayState, GameStageType>(stageType);
        }

        private static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            const string WEBPLAYER_QUIT_URL = "https://github.com/ShutovKS";
            Application.OpenURL(WEBPLAYER_QUIT_URL);
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
    }
}