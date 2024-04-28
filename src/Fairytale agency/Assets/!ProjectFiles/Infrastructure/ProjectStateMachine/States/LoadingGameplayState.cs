using System;
using Data.GameData;
using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.AssetsAddressables;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

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
            AsyncOperationHandle<SceneInstance> asyncOperation;
            switch (gameStageType)
            {
                case GameStageType.None:
                case GameStageType.Prologue:
                    asyncOperation = Addressables.LoadSceneAsync(AssetsAddressableConstants.EMPTY_2D_SCENE);
                    asyncOperation.Completed += _ => Initializer.StateMachine.SwitchState<PrologueState>();
                    break;
                case GameStageType.Mumu:
                    asyncOperation = Addressables.LoadSceneAsync(AssetsAddressableConstants.MUMU_SCENE);
                    asyncOperation.Completed += _ => Initializer.StateMachine.SwitchState<MumuState>();
                    break;
                case GameStageType.Beanstalk:
                    asyncOperation = Addressables.LoadSceneAsync(AssetsAddressableConstants.BEANSTALK_SCENE);
                    asyncOperation.Completed += _ => Initializer.StateMachine.SwitchState<BeanstalkState>();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}