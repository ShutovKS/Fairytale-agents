using Infrastructure.ProjectStateMachine.Base;
using Zenject;

namespace Infrastructure.ProjectStateMachine.States
{
    public class BootstrapState : IState<GameBootstrap>, IInitializable
    {
        public GameBootstrap Initializer { get; }

        public BootstrapState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public void Initialize()
        {
            Initializer.StateMachine.SwitchState<InitializationState>();
        }
    }
}