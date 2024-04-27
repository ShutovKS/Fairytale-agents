using System;
using System.Collections;
using System.Threading.Tasks;
using Data.Dialogue;
using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Dialogue;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.Input;
using Infrastructure.Services.WindowsService;
using Mumu;
using UI.Confirmation;
using UI.DialogueScreen;
using UI.Mumu.Scrips;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Infrastructure.ProjectStateMachine.States
{
    public class MumuState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public MumuState(GameBootstrap initializer, ISaveLoadService saveLoadService, IProgressService progressService,
            IWindowService windowService, PlayerInputActionReader inputActionReader, IDialogueService dialogueService,
            ICoroutineRunner coroutineRunner)
        {
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            _windowService = windowService;
            _inputActionReader = inputActionReader;
            _dialogueService = dialogueService;
            _coroutineRunner = coroutineRunner;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progressService;
        private readonly IWindowService _windowService;
        private readonly PlayerInputActionReader _inputActionReader;
        private readonly IDialogueService _dialogueService;
        private readonly ICoroutineRunner _coroutineRunner;

        private Phrase CurrentDialogue => _dialogues.Phrases[_currentDialogueId];

        private GameManager _gameManager;
        private Dialogue _dialogues;
        private BasicDialogOptions _basicDialogOptions;
        private DialogueUI _dialogueUI;

        private int _currentDialogueId;
        private string _currentSoundEffect;
        private Action _onDialogComplete;

        public void OnEnter()
        {
            _gameManager = GameManager.Instance;
            _gameManager.OnLost += Lost;
            _gameManager.OnWon += Won;

            _progressService.PlayerProgress.gameStageType = GameStageType.Mumu;
            _saveLoadService.SaveProgress();

            _basicDialogOptions = Resources.Load<BasicDialogOptions>("Configs/BasicDialogOptions");

            LaunchDialogAtStart();
            _onDialogComplete = StartGameplay;

            _windowService.Close(WindowID.Loading);
        }

        public void OnExit()
        {
            _windowService.Close(WindowID.Mumu);
            _windowService.Close(WindowID.Dialogue);
            _windowService.Close(WindowID.Confirmation);
            _windowService.Open(WindowID.Loading);

            _gameManager.OnLost -= Lost;
            _gameManager.OnWon -= Won;
        }

        private void Lost()
        {
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }

        private void Won()
        {
            LaunchDialogAtEnd();
            _onDialogComplete += () => Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }

        private async void StartGameplay()
        {
            _onDialogComplete -= StartGameplay;
            _windowService.Close(WindowID.Dialogue);
            var mumuUI = await _windowService.OpenAndGetComponent<MumuUI>(WindowID.Mumu);
            _gameManager.StartGame(mumuUI);
        }

        private void LaunchDialogAtStart()
        {
            StartDialog(DialogueID.MumuStart);
        }

        private void LaunchDialogAtEnd()
        {
            StartDialog(DialogueID.MumuFinal);
            _onDialogComplete = OpenMenu;
        }

        //

        private async void StartDialog(DialogueID dialogueID)
        {
            _dialogues = _dialogueService.GetDialogues(dialogueID);

            await OpenWindow();
            SetDialog(0);
        }

        private async Task OpenWindow()
        {
            _dialogueUI = await _windowService.OpenAndGetComponent<DialogueUI>(WindowID.Dialogue);
            _dialogueUI.OnBackButtonClicked = ConfirmExitInMenu;
        }

        private async void ConfirmExitInMenu()
        {
            var confirmationUI = await _windowService.OpenAndGetComponent<ConfirmationUI>(WindowID.Confirmation);

            confirmationUI.Buttons.OnYesButtonClicked = OpenMenu;
            confirmationUI.Buttons.OnNoButtonClicked = () => _windowService.Close(WindowID.Confirmation);
        }

        private void SetDialog(int id)
        {
            if (id >= _dialogues.Phrases.Length)
            {
                _onDialogComplete?.Invoke();
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
                yield return new WaitForSeconds(_basicDialogOptions.secondsDelayDefault);
            }

            yield return new WaitForSeconds(_basicDialogOptions.delayAfterDialogueEnds);

            SetDialog(_currentDialogueId + 1);
        }

        private void HandleActionTrigger(Event @event)
        {
        }

        private void OpenMenu()
        {
            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }
    }
}