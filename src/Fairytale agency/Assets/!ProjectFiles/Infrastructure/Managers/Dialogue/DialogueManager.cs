using System;
using System.Collections;
using System.Threading.Tasks;
using Data.Dialogue;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Dialogue;
using Infrastructure.Services.WindowsService;
using UI.Confirmation;
using UI.DialogueScreen;
using UnityEngine;
using Zenject;
using Event = UnityEngine.Event;

namespace Infrastructure.Managers.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager _instance;

        public static DialogueManager Instance => _instance ?? Initialization();

        [Inject]
        public void Construct(IDialogueService dialogueService, IWindowService windowService,
            ICoroutineRunner coroutineRunner)
        {
            _dialogueService = dialogueService;
            _windowService = windowService;
            _coroutineRunner = coroutineRunner;
        }

        public event Action OnExitInMainMenu;
        public event Action OnDialogComplete;

        private Phrase CurrentDialogue => _dialogues.Phrases[_currentDialogueId];

        private IDialogueService _dialogueService;
        private IWindowService _windowService;
        private Data.Dialogue.Dialogue _dialogues;
        private DialogueUI _dialogueUI;
        private static BasicDialogOptions _basicDialogOptions;
        private ICoroutineRunner _coroutineRunner;

        private int _currentDialogueId;
        private string _currentSoundEffect;


        public async void StartDialog(DialogueUI dialogueUI, DialogueID dialogueID)
        {
            _dialogues = _dialogueService.GetDialogues(dialogueID);
            _dialogueUI = dialogueUI;

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

            confirmationUI.Buttons.OnYesButtonClicked = () =>
            {
                _windowService.Close(WindowID.Confirmation);
                OnExitInMainMenu?.Invoke();
            };
            confirmationUI.Buttons.OnNoButtonClicked = () => { _windowService.Close(WindowID.Confirmation); };
        }

        private void SetDialog(int id)
        {
            if (id >= _dialogues.Phrases.Length)
            {
                OnDialogComplete?.Invoke();
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

        private static DialogueManager Initialization()
        {
            _basicDialogOptions = Resources.Load<BasicDialogOptions>("Configs/BasicDialogOptions");

            var instance = new GameObject(nameof(DialogueManager));
            _instance = instance.AddComponent<DialogueManager>();
            DontDestroyOnLoad(instance);

            return _instance;
        }
    }
}