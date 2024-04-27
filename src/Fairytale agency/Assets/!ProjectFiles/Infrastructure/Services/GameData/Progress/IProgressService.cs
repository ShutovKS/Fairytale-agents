namespace Infrastructure.Services.GameData.Progress
{
    public interface IProgressService
    {
        public PlayerProgress PlayerProgress { get; }
        public void SetProgress(PlayerProgress playerProgress);
    }
}