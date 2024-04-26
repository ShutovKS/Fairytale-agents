using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.ProjectStateMachine.States;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.WindowsService;

namespace Infrastructure.ProjectStateMachine
{
    public class GameBootstrap
    {
        public GameBootstrap(IWindowService windowService,
            IAssetsAddressablesProvider assetsAddressablesProvider)
        {
            StateMachine = new StateMachine<GameBootstrap>(new BootstrapState(this),
                new InitializationState(this),
                new ResourcesLoadingState(this, windowService),
                new GameMainMenuState(this, windowService),
                new GameplayState(this, windowService, assetsAddressablesProvider)
            );

            StateMachine.SwitchState<BootstrapState>();
        }

        public readonly StateMachine<GameBootstrap> StateMachine;
    }
}