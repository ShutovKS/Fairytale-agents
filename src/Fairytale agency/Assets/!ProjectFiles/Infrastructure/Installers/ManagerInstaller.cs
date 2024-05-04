using Infrastructure.Managers.Dialogue;
using Zenject;

namespace Infrastructure.Installers
{
    public class ManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InjectDialogueManager();
        }
        
        private void InjectDialogueManager()
        {
            Container.Inject(DialogueManager.Instance); 
        }
    }
}