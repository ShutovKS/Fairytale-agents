using System;
using System.Collections;
using System.Threading.Tasks;
using Data.Dialogue;
using Infrastructure.Services.Dialogue;
using Infrastructure.Services.WindowsService;
using UI.Confirmation;
using UI.DialogueScreen;
using UI.Mumu.Scrips;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Mumu
{
    public class GameManager : MonoBehaviour
    {
        public event Action OnLost;
        public event Action OnWon;
        public static GameManager Instance { get; private set; }

        [SerializeField] private EnemiesSpawner enemiesSpawner;
        [SerializeField] private Boat boat;
        [SerializeField] private BoatMovement boatMovement;

        private Phrase CurrentDialogue => _dialogues.Phrases[_currentDialogueId];

        private MumuUI _mumuUI;
        private DialogueUI _dialogueUI;
        private Dialogue _dialogues;
        private IWindowService _windowService;
        private IDialogueService _dialogueService;
        private BasicDialogOptions _basicDialogOptions;

        private int _currentDialogueId;
        private string _currentSoundEffect;

        public void Awake()
        {
            Instance = this;
            _basicDialogOptions = Resources.Load<BasicDialogOptions>("Configs/BasicDialogOptions");
        }

        public async Task StartGame(IWindowService windowService, IDialogueService dialogueService)
        {
            _windowService = windowService;
            _dialogueService = dialogueService;
            
            _mumuUI = await _windowService.OpenAndGetComponent<MumuUI>(WindowID.Mumu);
            _mumuUI.SetHealthPoints((int)boat.Health);
            _mumuUI.SetDestroyed(enemiesSpawner.NumberDeadEnemies);
            _mumuUI.SetLeft(enemiesSpawner.NumberRemainingEnemies);

            boat.OnHealthChange = _mumuUI.SetHealthPoints;
            boat.OnDead += enemiesSpawner.StopSpawn;
            boat.OnDead += () => OnLost?.Invoke();

            enemiesSpawner.OnAllDeadEnemies = MovementTowardShore;
            enemiesSpawner.OnNumberDeadEnemies = _mumuUI.SetDestroyed;
            enemiesSpawner.OnNumberRemainingEnemies = _mumuUI.SetLeft;

            enemiesSpawner.StartSpawn();
        }

        private void MovementTowardShore()
        {
            boatMovement.enabled = true;
            boatMovement.OnCompleted += StartDialog;
        }

        private async void StartDialog()
        {
            _dialogues = _dialogueService.GetDialogues(DialogueID.Mumu);

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

            StartCoroutine(DisplayTyping(phrase.TextLocalization[0].Text));
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
            // Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }

        private void DialogueEnded()
        {
            OnWon?.Invoke();
        }
    }
}