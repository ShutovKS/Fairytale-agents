using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.WindowsService;

namespace Infrastructure.ProjectStateMachine.States
{
    public class ResourcesLoadingState : IState<GameBootstrap>, IEnterable
    {
        public ResourcesLoadingState(GameBootstrap initializer, IWindowService windowService)
        {
            _windowService = windowService;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly IWindowService _windowService;

        public async void OnEnter()
        {
            await _windowService.Open(WindowID.Loading);

            Initializer.StateMachine.SwitchState<GameMainMenuState>();
        }

        private void LoadResources()
        {
        }
    }
}