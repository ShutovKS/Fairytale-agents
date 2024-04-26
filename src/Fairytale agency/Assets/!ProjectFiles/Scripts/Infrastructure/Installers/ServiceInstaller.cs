using Services.AssetsAddressables;
using Services.CoroutineRunner;
using Services.DynamicData.Progress;
using Services.DynamicData.SaveLoad;
using Services.Factory.UIFactory;
using Services.Input;
using Services.SoundsService;
using Services.WindowsService;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ServiceInstaller : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private PlayerInputActionReader _playerInputActionReader;
        
        public override void InstallBindings()
        {
            BindPlayerProgressService();
            BindUIFactory();
            BindAssetsAddressableService();
            BindPlayerInputActionReader();
            BindWindowService();
            BindCoroutineRunnerService();
            BindSaveLoadService();
            BindSoundsService();
        }

        private void BindSoundsService()
        {
            Container.BindInterfacesTo<SoundService>().AsSingle(); 
        }

        private void BindSaveLoadService()
        {
            Container.BindInterfacesTo<SaveLoadService>().AsSingle(); 
        }

        private void BindPlayerProgressService()
        {
            Container.BindInterfacesTo<ProgressService>().AsSingle();
        }

        private void BindWindowService()
        {
            Container.BindInterfacesTo<WindowService>().AsSingle();
        }

        private void BindAssetsAddressableService()
        {
            Container.BindInterfacesTo<AssetsAddressablesProvider>().AsSingle();
        }

        private void BindPlayerInputActionReader()
        {
            Container.Bind<PlayerInputActionReader>().FromInstance(_playerInputActionReader).AsSingle();
        }

        private void BindUIFactory()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle();
        }

        private void BindCoroutineRunnerService()
        {
            Container.Bind<ICoroutineRunner>().FromInstance(this);
        }
    }
}
