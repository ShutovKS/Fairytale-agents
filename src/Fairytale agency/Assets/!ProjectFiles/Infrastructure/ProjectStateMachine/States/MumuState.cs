using Data.Dialogue;
using Data.GameData;
using Infrastructure.Managers.Dialogue;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.WindowsService;
using Mumu;
using UI.DialogueScreen;
using UI.Mumu.Scrips;

namespace Infrastructure.ProjectStateMachine.States
{
    public class MumuState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public MumuState(GameBootstrap initializer, ISaveLoadService saveLoadService, IProgressService progressService,
            IWindowService windowService)
        {
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            _windowService = windowService;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progressService;
        private readonly IWindowService _windowService;

        private static DialogueManager DialogueManager => DialogueManager.Instance;
        private static GameManager GameManager => GameManager.Instance;

        public void OnEnter()
        {
            _progressService.PlayerProgress.gameStageType = GameStageType.Mumu;
            _saveLoadService.SaveProgress();

            LaunchDialogAtStart();

            _windowService.Close(WindowID.Loading);
        }

        public void OnExit()
        {
            _windowService.Open(WindowID.Loading);

            GameManager.OnLost -= Lost;
            GameManager.OnWon -= Won;
        }

        private void Lost()
        {
            _windowService.Close(WindowID.Mumu);
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }

        private void Won()
        {
            _windowService.Close(WindowID.Mumu);
            LaunchDialogAtEnd();
        }

        private async void LaunchDialogAtStart()
        {
            var dialogueUI = await _windowService.OpenAndGetComponent<DialogueUI>(WindowID.Dialogue);
            DialogueManager.StartDialog(dialogueUI, DialogueID.MumuStart);
            DialogueManager.OnExitInMainMenu += ExitInMainMenu;
            DialogueManager.OnDialogComplete += StartGameplay;
            return;

            async void StartGameplay()
            {
                DialogueManager.OnExitInMainMenu -= ExitInMainMenu;
                DialogueManager.OnDialogComplete -= StartGameplay;
                _windowService.Close(WindowID.Dialogue);

                var mumuUI = await _windowService.OpenAndGetComponent<MumuUI>(WindowID.Mumu);
                GameManager.StartGame(mumuUI);
                GameManager.OnLost += Lost;
                GameManager.OnWon += Won;
            }
        }

        private async void LaunchDialogAtEnd()
        {
            var dialogueUI = await _windowService.OpenAndGetComponent<DialogueUI>(WindowID.Dialogue);
            DialogueManager.StartDialog(dialogueUI, DialogueID.MumuFinal);
            DialogueManager.OnExitInMainMenu += ExitInMainMenu;
            DialogueManager.OnDialogComplete += NextLevel;
            return;

            void NextLevel()
            {
                DialogueManager.OnExitInMainMenu -= ExitInMainMenu;
                DialogueManager.OnDialogComplete -= NextLevel;
                _windowService.Close(WindowID.Dialogue);

                Initializer.StateMachine.SwitchState<LoadingGameplayState, GameStageType>(GameStageType.Beanstalk);
            }
        }

        private void ExitInMainMenu()
        {
        }
    }
}