using UnityEngine;

namespace Infrastructure.Services.SoundsService
{
    public interface ISoundService
    {
        public void PlaySoundsClip(AudioClip audioClip);
        public void PlayMusicClip(AudioClip audioClip);

        public void StopMusic();
        public void StopSounds();
    }
}