﻿using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.ProjectStateMachine.States;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Dialogue;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;
using Infrastructure.Services.SoundsService;
using Infrastructure.Services.WindowsService;

namespace Infrastructure.ProjectStateMachine
{
    public class GameBootstrap
    {
        public GameBootstrap(IWindowService windowService,
            IAssetsAddressablesProvider assetsAddressablesProvider,
            IProgressService progressService,
            ISaveLoadService saveLoadService,
            ISoundService soundService,
            ICoroutineRunner coroutineRunner,
            IDialogueService dialogueService)
        {
            StateMachine = new StateMachine<GameBootstrap>(new BootstrapState(this),
                new InitializationState(this, progressService, saveLoadService, assetsAddressablesProvider),
                new ResourcesLoadingState(this, windowService),
                new GameMainMenuState(this, windowService, progressService, saveLoadService),
                new LoadingGameplayState(this),
                new PrologueState(this, windowService, progressService, soundService, coroutineRunner, dialogueService)
            );

            StateMachine.SwitchState<BootstrapState>();
        }

        public readonly StateMachine<GameBootstrap> StateMachine;
    }
}