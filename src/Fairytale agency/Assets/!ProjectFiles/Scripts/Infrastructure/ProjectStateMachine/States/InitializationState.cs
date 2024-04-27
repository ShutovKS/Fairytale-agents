using Infrastructure.ProjectStateMachine.Base;
using Infrastructure.Services.AssetsAddressables;
using Infrastructure.Services.GameData;
using Infrastructure.Services.GameData.Progress;
using Infrastructure.Services.GameData.SaveLoad;

namespace Infrastructure.ProjectStateMachine.States
{
    public class InitializationState : IState<GameBootstrap>, IEnterable
    {
        public InitializationState(GameBootstrap initializer, IProgressService progressService,
            ISaveLoadService saveLoadService, IAssetsAddressablesProvider assetsAddressablesProvider)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _assetsAddressablesProvider = assetsAddressablesProvider;
            Initializer = initializer;
        }

        public GameBootstrap Initializer { get; }
        private readonly IProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAssetsAddressablesProvider _assetsAddressablesProvider;

        public void OnEnter()
        {
            LoadLocalGame();
            InitializeGame();
            ChangeStateToLoading();
        }

        private void LoadLocalGame()
        {
            _progressService.SetProgress(_saveLoadService.LoadProgress() ?? InitializeProgress());
        }

        private void InitializeGame()
        {
            _assetsAddressablesProvider.Initialize();
        }

        private void ChangeStateToLoading()
        {
            Initializer.StateMachine.SwitchState<ResourcesLoadingState>();
        }

        private PlayerProgress InitializeProgress()
        {
            return new PlayerProgress();
        }
    }
}