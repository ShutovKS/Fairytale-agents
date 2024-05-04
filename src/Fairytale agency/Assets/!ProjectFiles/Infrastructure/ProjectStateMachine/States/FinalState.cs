using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.WindowsService;
using UI.FinalScreen.Scripts;

namespace Infrastructure.ProjectStateMachine.States
{
    public class FinalState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public FinalState(GameBootstrap initializer, IWindowService windowService, IProgressService progressService,
            ISaveLoadService saveLoadService)
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

        private FinalUI _finalUI;

        public async void OnEnter()
        {
            _progressService.PlayerProgress.gameStageType = GameStageType.Final;
            _saveLoadService.SaveProgress();

            _finalUI = await _windowService.OpenAndGetComponent<FinalUI>(WindowID.Final);
            _finalUI.OnExitButtonClicked += ExitInMainMenu;

            _windowService.Close(WindowID.Loading);
        }

        public void OnExit()
        {
            _windowService.Close(WindowID.Final);
            _windowService.Open(WindowID.Loading);

            _finalUI.OnExitButtonClicked -= ExitInMainMenu;
        }

        private void ExitInMainMenu()
        {
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }
    }
}