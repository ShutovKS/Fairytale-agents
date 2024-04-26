namespace Infrastructure.Services.GameData.SaveLoad
{
    public interface ISaveLoadService
    {
        public void SaveProgress();
        public Data.GameData.PlayerProgress LoadProgress();
    }
}