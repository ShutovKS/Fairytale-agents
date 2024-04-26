namespace Infrastructure.Services.DynamicData.Progress
{
    public interface IProgressService
    {
        public PlayerProgress PlayerProgress { get; }
        public void SetProgress(PlayerProgress playerProgress);
    }
}