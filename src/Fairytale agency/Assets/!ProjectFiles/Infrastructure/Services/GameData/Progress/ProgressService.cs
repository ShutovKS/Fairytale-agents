namespace Infrastructure.Services.GameData.Progress
{
    public class ProgressService : IProgressService
    {
        public Data.GameData.PlayerProgress PlayerProgress { get; private set; }

        public void SetProgress(Data.GameData.PlayerProgress playerProgress)
        {
            PlayerProgress = playerProgress;
        }
    }
}