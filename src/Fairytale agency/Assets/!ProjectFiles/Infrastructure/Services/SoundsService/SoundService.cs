using UnityEngine;

namespace Infrastructure.Services.SoundsService
{
    public class SoundService : ISoundService
    {
        public SoundService()
        {
            var audioSource = new GameObject(nameof(_audioSource));
            _audioSource = audioSource.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(audioSource);

            var musicSource = new GameObject(nameof(_musicSource));
            _musicSource = musicSource.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(musicSource);
        }

        private AudioSource _audioSource;
        private AudioSource _musicSource;

        public void PlaySoundsClip(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();

            Debug.Log($"PlayAudioClip: {audioClip.name}");
        }

        public void PlayMusicClip(AudioClip audioClip)
        {
            _musicSource.clip = audioClip;
            _musicSource.Play();

            Debug.Log($"PlayAudioClip: {audioClip.name}");
        }

        public void StopMusic()
        {
            _musicSource.Stop();
            _musicSource.clip = null;

            Debug.Log($"StopMusic");
        }

        public void StopSounds()
        {
            _audioSource.Stop();
            _audioSource.clip = null;

            Debug.Log($"StopSounds");
        }
    }
}