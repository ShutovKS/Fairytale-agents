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
using UnityEngine.Events;
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
        private const float SECONDS_DELAY_FAST = 0.005f;

        private Coroutine _displayTypingCoroutine;
        private DialogueUI _dialogueUI;
        private Dialogue _dialogues;

        private Phrase CurrentDialogue => _dialogues.Phrases[_currentDialogueId];
        private int _currentDialogueId;

        private UnityAction _onDialogueCompleted;

        private string _currentSoundEffect;
        private float _typingDelay = SECONDS_DELAY_DEFAULT;
        private bool _isDialogCompleted;
        private bool _isSpeedUpMode;
        private bool _isAutoMode;

        public async void OnEnter()
        {
            await LoadSceneAsync();
            _dialogues = _dialogueService.GetDialogues(DialogueID.Prologue);
            await OpenWindow();
            StartDialogue();

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
            ConfigureDialogueUIButtons();
            _windowService.Close(WindowID.Loading);
        }

        private void ConfigureDialogueUIButtons()
        {
            _dialogueUI.Buttons.OnBackButtonClicked = ConfirmExitInMenu;
            _dialogueUI.Buttons.OnHistoryButtonClicked = OpenDialogHistory;
            _dialogueUI.Buttons.OnSpeedUpButtonClicked = ChangeTypingDialogSpeedUp;
            _dialogueUI.Buttons.OnAutoButtonClicked = AutoDialogSwitchMode;
            _dialogueUI.Buttons.OnFurtherButtonClicked = DialogFurther;
        }

        private async void ConfirmExitInMenu()
        {
            StopAutoDialogSwitchMode();
            var confirmationUI = await _windowService.OpenAndGetComponent<ConfirmationUI>(WindowID.Confirmation);
            ConfigureConfirmationUIButtons(confirmationUI);
        }

        private void ConfigureConfirmationUIButtons(ConfirmationUI confirmationUI)
        {
            confirmationUI.Buttons.OnYesButtonClicked = () =>
            {
                _windowService.Close(WindowID.Confirmation);
                OpenMenu();
            };
            confirmationUI.Buttons.OnNoButtonClicked = () => _windowService.Close(WindowID.Confirmation);
        }

        public void OnExit()
        {
            ResetData();
            _windowService.Close(WindowID.Dialogue);
            _windowService.Open(WindowID.Loading);
        }

        private void ResetData()
        {
            _coroutineRunner.StopCoroutine(_displayTypingCoroutine);
            _currentSoundEffect = string.Empty;
            _typingDelay = SECONDS_DELAY_DEFAULT;
            _isDialogCompleted = false;
            _isSpeedUpMode = false;
            _isAutoMode = false;
        }

        private void StartDialogue()
        {
            _dialogueUI.SetActivePanel(true);
            SetDialog(0);
        }

        private void DialogFurther()
        {
            if (_isDialogCompleted)
            {
                SetDialog(_currentDialogueId + 1);
            }
            else
            {
                CompleteDialogue();
            }
        }

        private void CompleteDialogue()
        {
            _isDialogCompleted = true;
            _coroutineRunner.StopCoroutine(_displayTypingCoroutine);
            _dialogueUI.DialogueText.SetText(CurrentDialogue.TextLocalization[0].Text);
            _onDialogueCompleted?.Invoke();
        }

        private void ChangeTypingDialogSpeedUp()
        {
            _isSpeedUpMode = !_isSpeedUpMode;
            _typingDelay = _isSpeedUpMode ? SECONDS_DELAY_FAST : SECONDS_DELAY_DEFAULT;
        }

        private void AutoDialogSwitchMode()
        {
            if (_isAutoMode)
            {
                _isAutoMode = false;
                _onDialogueCompleted -= AutoDialogSwitchIfComplete;
            }
            else
            {
                _isAutoMode = true;
                _onDialogueCompleted += AutoDialogSwitchIfComplete;
                if (_isDialogCompleted)
                {
                    AutoDialogSwitchIfComplete();
                }
            }
        }

        private void StopAutoDialogSwitchMode()
        {
            if (_isAutoMode)
            {
                AutoDialogSwitchMode();
            }
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

        private void AutoDialogSwitchIfComplete()
        {
            if (_isDialogCompleted)
            {
                SetDialog(_currentDialogueId + 1);
            }
        }

        private void SetPhraseTyping(Phrase phrase)
        {
            _dialogueUI.Answers.SetActiveAnswerOptions(false);
            if (phrase.Background != null)
            {
                _dialogueUI.Background.SetImage(phrase.Background);
            }

            _dialogueUI.DialogueText.SetAuthorName(_dialogueService.GetCharacter(phrase.CharacterType).Name);
            _dialogueUI.DialogueText.SetText(string.Empty);
            _dialogueUI.Person.SetAvatar(_dialogueService.GetCharacter(phrase.CharacterType).Avatar);

            AddDialogueInHistory(null, phrase.TextLocalization[0].Text);
            _displayTypingCoroutine = _coroutineRunner.StartCoroutine(DisplayTyping(phrase.TextLocalization[0].Text));
        }

        private IEnumerator DisplayTyping(string text)
        {
            var currentText = string.Empty;
            foreach (var letter in text)
            {
                currentText += letter;
                _dialogueUI.DialogueText.SetText(currentText);
                yield return new WaitForSeconds(_typingDelay);
            }

            yield return new WaitForSeconds(_typingDelay * 5);

            _isDialogCompleted = true;
            _onDialogueCompleted?.Invoke();
        }

        private void OpenDialogHistory()
        {
            _dialogueUI.History.SetActivePanel(true);
            _dialogueUI.History.OnBackButtonClicked = () => _dialogueUI.History.SetActivePanel(false);
        }

        private void HandleActionTrigger(Event @event)
        {
        }

        private void AddDialogueInHistory(string name, string text)
        {
            _dialogueUI.History.CreateHistoryPhrase(name, text);
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