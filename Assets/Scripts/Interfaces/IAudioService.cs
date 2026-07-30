using UnityEngine;

namespace EndlessRunner.Interfaces
{
    /// <summary>
    /// Contract for background music, audio channels, and sound effect triggers.
    /// </summary>
    public interface IAudioService : IGameService
    {
        void PlayMusic(AudioClip clip, bool loop = true);
        void PlaySFX(AudioClip clip, Vector3 position = default);
        void SetMasterVolume(float value);
        void SetSFXVolume(float value);
    }
}