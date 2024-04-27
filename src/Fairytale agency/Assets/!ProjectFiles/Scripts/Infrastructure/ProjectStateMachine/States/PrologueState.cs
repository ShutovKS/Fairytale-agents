using System.Collections;
using System.Threading.Tasks;
using Data.Dialogue;
using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Dialogue;
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
            ICoroutineRunner coroutineRunner, IDialogueService dialogueService, ISaveLoadService saveLoadService,
            IProgressService progressService)
        {
            _windowService = windowService;
            _soundService = soundService;
            _coroutineRunner = coroutineRunner;
            _dialogueService = dialogueService;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly IWindowService _windowService;
        private readonly ISoundService _soundService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IDialogueService _dialogueService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progressService;

        private const float SECONDS_DELAY_DEFAULT = 0.05f;

        private DialogueUI _dialogueUI;
        private Dialogue _dialogues;

        private Phrase CurrentDialogue => _dialogues.Phrases[_currentDialogueId];
        private int _currentDialogueId;
        private string _currentSoundEffect;

        public async void OnEnter()
        {
            await LoadSceneAsync();

            _dialogues = _dialogueService.GetDialogues(DialogueID.Prologue);

            await OpenWindow();
            SetDialog(0);

            _progressService.PlayerProgress.gameStageType = GameStageType.Prologue;
            _saveLoadService.SaveProgress();
        }

        private async Task LoadSceneAsync()
        {
            var asyncOperation = Addressables.LoadSceneAsync(AssetsAddressableConstants.EMPTY_2D_SCENE);
            await asyncOperation.Task;
        }

        private async Task OpenWindow()
        {
            _dialogueUI = await _windowService.OpenAndGetComponent<DialogueUI>(WindowID.Dialogue);
            _dialogueUI.OnBackButtonClicked = ConfirmExitInMenu;

            _windowService.Close(WindowID.Loading);
        }

        private async void ConfirmExitInMenu()
        {
            var confirmationUI = await _windowService.OpenAndGetComponent<ConfirmationUI>(WindowID.Confirmation);

            confirmationUI.Buttons.OnYesButtonClicked = OpenMenu;
            confirmationUI.Buttons.OnNoButtonClicked = () => _windowService.Close(WindowID.Confirmation);
        }


        public void OnExit()
        {
            _windowService.Close(WindowID.Confirmation);
            _windowService.Close(WindowID.Dialogue);
            _windowService.Open(WindowID.Loading);
        }

        private void SetDialog(int id)
        {
            if (id >= _dialogues.Phrases.Length)
            {
                DialogueEnded();
                return;
            }

            _currentDialogueId = id;
            SetPhraseTyping(CurrentDialogue);
        }

        private void SetPhraseTyping(Phrase phrase)
        {
            if (phrase.Background != null)
            {
                _dialogueUI.SetImage(phrase.Background);
            }

            _dialogueUI.SetAuthorName(_dialogueService.GetCharacter(phrase.CharacterType).Name);
            _dialogueUI.SetAvatar(_dialogueService.GetCharacter(phrase.CharacterType).Avatar);
            _dialogueUI.SetText(string.Empty);

            _coroutineRunner.StartCoroutine(DisplayTyping(phrase.TextLocalization[0].Text));
        }

        private IEnumerator DisplayTyping(string text)
        {
            var currentText = string.Empty;
            foreach (var letter in text)
            {
                currentText += letter;
                _dialogueUI.SetText(currentText);
                yield return new WaitForSeconds(SECONDS_DELAY_DEFAULT);
            }

            yield return new WaitForSeconds(SECONDS_DELAY_DEFAULT * 5);

            SetDialog(_currentDialogueId + 1);
        }

        private void HandleActionTrigger(Event @event)
        {
        }

        private void OpenMenu()
        {
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }

        private void DialogueEnded()
        {
            Initializer.StateMachine.SwitchState<LoadingGameplayState, GameStageType>(GameStageType.Mumu);
        }
    }
}