using System;
using System.Collections;
using System.Threading.Tasks;
using Beanstalk;
using Data.Dialogue;
using Data.GameData;
using Infrastructure.Managers.Dialogue;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.Input;
using Infrastructure.Services.SoundsService;
using Infrastructure.Services.WindowsService;
using UI.BeanstalkScreen;
using UI.Confirmation;
using UI.DialogueScreen;
using UnityEngine;
using Event = Data.Dialogue.Event;

namespace Infrastructure.ProjectStateMachine.States
{
    public class BeanstalkState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public BeanstalkState(GameBootstrap initializer, ISaveLoadService saveLoadService, ISoundService soundService,
            IProgressService progressService, IWindowService windowService, PlayerInputActionReader inputActionReader)
        {
            _saveLoadService = saveLoadService;
            _soundService = soundService;
            _progressService = progressService;
            _windowService = windowService;
            _inputActionReader = inputActionReader;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISoundService _soundService;
        private readonly IProgressService _progressService;
        private readonly IWindowService _windowService;
        private readonly PlayerInputActionReader _inputActionReader;

        private static DialogueManager DialogueManager => DialogueManager.Instance;
        private static GameManager GameManager => GameManager.Instance;

        public void OnEnter()
        {
            _progressService.PlayerProgress.gameStageType = GameStageType.Beanstalk;
            _saveLoadService.SaveProgress();

            LaunchDialogAtStart();
            
            var audioClip = Resources.Load<AudioClip>("Sounds/beanstalk-sound");
            _soundService.PlaySoundsClip(audioClip);

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
            _windowService.Close(WindowID.Beanstalk);
            Initializer.StateMachine.SwitchState<LoadingGameplayState, GameStageType>(GameStageType.Final);
        }

        private void Won()
        {
            _windowService.Close(WindowID.Beanstalk);
            LaunchDialogAtEnd();
        }

        private async void LaunchDialogAtStart()
        {
            var dialogueUI = await _windowService.OpenAndGetComponent<DialogueUI>(WindowID.Dialogue);
            DialogueManager.StartDialog(dialogueUI, DialogueID.BeanstalkStart);
            DialogueManager.OnExitInMainMenu += ExitInMainMenu;
            DialogueManager.OnDialogComplete += StartGameplay;
            return;

            async void StartGameplay()
            {
                DialogueManager.OnExitInMainMenu -= ExitInMainMenu;
                DialogueManager.OnDialogComplete -= StartGameplay;
                _windowService.Close(WindowID.Dialogue);

                var beanstalkUI = await _windowService.OpenAndGetComponent<BeanstalkUI>(WindowID.Beanstalk);
                GameManager.StartGame(beanstalkUI);
                GameManager.OnLost += Lost;
                GameManager.OnWon += Won;
            }
        }

        private async void LaunchDialogAtEnd()
        {
            var dialogueUI = await _windowService.OpenAndGetComponent<DialogueUI>(WindowID.Dialogue);
            DialogueManager.StartDialog(dialogueUI, DialogueID.BeanstalkStart);
            DialogueManager.OnExitInMainMenu += ExitInMainMenu;
            DialogueManager.OnDialogComplete += NextLevel;
            return;

            void NextLevel()
            {
                DialogueManager.OnExitInMainMenu -= ExitInMainMenu;
                DialogueManager.OnDialogComplete -= NextLevel;
                _windowService.Close(WindowID.Dialogue);

                Initializer.StateMachine.SwitchState<LoadingGameplayState, GameStageType>(GameStageType.Final);
            }
        }

        private void ExitInMainMenu()
        {
        }
    }
}