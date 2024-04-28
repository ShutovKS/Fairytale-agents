using System.Collections;
using System.Threading.Tasks;
using Data.Dialogue;
using Data.GameData;
using Infrastructure.Managers.Dialogue;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.SoundsService;
using Infrastructure.Services.WindowsService;
using UI.Confirmation;
using UI.DialogueScreen;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Event = Data.Dialogue.Event;

namespace Infrastructure.ProjectStateMachine.States
{
    public class PrologueState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public PrologueState(GameBootstrap initializer, IWindowService windowService, ISoundService soundService,
            ISaveLoadService saveLoadService, IProgressService progressService)
        {
            _windowService = windowService;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly IWindowService _windowService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progressService;

        private DialogueUI _dialogueUI;
        private Dialogue _dialogues;
        private BasicDialogOptions _basicDialogOptions;

        private static DialogueManager DialogueManager => DialogueManager.Instance;

        public async void OnEnter()
        {
            _progressService.PlayerProgress.gameStageType = GameStageType.Prologue;
            _saveLoadService.SaveProgress();

            var dialogueUI = await _windowService.OpenAndGetComponent<DialogueUI>(WindowID.Dialogue);
            DialogueManager.StartDialog(dialogueUI, DialogueID.Prologue);
            DialogueManager.OnExitInMainMenu += ExitInMainMenu;
            DialogueManager.OnDialogComplete += NextLevel;

            _windowService.Close(WindowID.Loading);
        }

        public void OnExit()
        {
            _windowService.Close(WindowID.Dialogue);
            _windowService.Open(WindowID.Loading);
        }

        private void ExitInMainMenu()
        {
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }

        private void NextLevel()
        {
            Initializer.StateMachine.SwitchState<LoadingGameplayState, GameStageType>(GameStageType.Mumu);
        }
    }
}