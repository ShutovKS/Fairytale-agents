﻿namespace Infrastructure.Services.GameData.Progress
{
    public class ProgressService : IProgressService
    {
        public PlayerProgress PlayerProgress { get; private set; }

        public void SetProgress(PlayerProgress playerProgress)
        {
            PlayerProgress = playerProgress;
        }
    }
}