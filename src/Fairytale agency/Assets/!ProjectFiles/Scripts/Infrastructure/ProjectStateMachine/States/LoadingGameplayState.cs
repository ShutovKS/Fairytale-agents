using System;
using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.AssetsAddressables;
using UnityEngine.AddressableAssets;

namespace Infrastructure.ProjectStateMachine.States
{
    public class LoadingGameplayState : IState<GameBootstrap>, IEnterableWithOneArg<GameStageType>
    {
        public LoadingGameplayState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }

        public void OnEnter(GameStageType gameStageType)
        {
            switch (gameStageType)
            {
                case GameStageType.None:
                case GameStageType.Prologue:
                    var asyncOperation = Addressables.LoadSceneAsync(AssetsAddressableConstants.EMPTY_2D_SCENE);
                    asyncOperation.Completed += _ => Initializer.StateMachine.SwitchState<PrologueState>();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}