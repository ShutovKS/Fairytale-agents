namespace Infrastructure.Services.GameData.Progress
{
    public interface IProgressService
    {
        public Data.GameData.PlayerProgress PlayerProgress { get; }
        public void SetProgress(Data.GameData.PlayerProgress playerProgress);
    }
}