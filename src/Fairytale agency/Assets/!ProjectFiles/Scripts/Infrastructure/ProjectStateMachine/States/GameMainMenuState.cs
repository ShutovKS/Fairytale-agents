using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.WindowsService;
using UI.MainMenuScreen;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.ProjectStateMachine.States
{
    public class GameMainMenuState : IState<GameBootstrap>, IEnterable, IExitable
    {
        private readonly IWindowService _windowService;
        public GameBootstrap Initializer { get; }

        public GameMainMenuState(GameBootstrap initializer,
            IWindowService windowService)
        {
            _windowService = windowService;
            Initializer = initializer;
        }

        public void OnEnter()
        {
            var asyncOperation = Addressables.LoadSceneAsync(AssetsAddressableConstants.MAIN_MENU_SCENE);

            asyncOperation.Completed += _ => { OpenMainMenuWindow(); };
        }

        private async void OpenMainMenuWindow()
        {
            var mainMenuScreen = await _windowService.OpenAndGetComponent<MainMenuScreen>(WindowID.MainMenu);

            mainMenuScreen.OnStartNewGameButtonClicked += OnStartNewGameButtonClicked;
            mainMenuScreen.OnExitButtonClicked += OnExitButtonClicked;

            CloseLoadingWindow();
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }

        private void OnStartNewGameButtonClicked()
        {
            Initializer.StateMachine.SwitchState<GameplayState>();
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