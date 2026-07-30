using System.Threading.Tasks;
using UnityEngine;
using EndlessRunner.Events;
using EndlessRunner.Interfaces;
using EndlessRunner.ScriptableObjects;

namespace EndlessRunner.Audio
{
    /// <summary>
    /// Audio management service handling background music loops and SFX triggers.
    /// Operates completely decoupled by reacting to global struct events.
    /// </summary>
    public class AudioManager : MonoBehaviour, IGameService, IAudioService
    {
        [Header("Configurations")]
        [SerializeField] private AudioConfigSO audioConfig;

        [Header("Audio Channels")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        public Task InitializeAsync()
        {
            EventBus<CoinCollectedEvent>.Subscribe(OnCoinCollected);
            EventBus<PlayerJumpedEvent>.Subscribe(OnPlayerJumped);
            EventBus<PlayerDiedEvent>.Subscribe(OnPlayerDied);

            PlayMusic(audioConfig.BackgroundMusic, true);
            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            EventBus<CoinCollectedEvent>.Unsubscribe(OnCoinCollected);
            EventBus<PlayerJumpedEvent>.Unsubscribe(OnPlayerJumped);
            EventBus<PlayerDiedEvent>.Unsubscribe(OnPlayerDied);
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null || musicSource == null) return;
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = audioConfig.DefaultMusicVolume;
            musicSource.Play();
        }

        public void PlaySFX(AudioClip clip, Vector3 position = default)
        {
            if (clip == null || sfxSource == null) return;
            sfxSource.PlayOneShot(clip, audioConfig.DefaultSFXVolume);
        }

        public void SetMasterVolume(float value)
        {
            AudioListener.volume = Mathf.Clamp01(value);
        }

        public void SetSFXVolume(float value)
        {
            if (sfxSource != null) sfxSource.volume = Mathf.Clamp01(value);
        }

        private void OnCoinCollected(CoinCollectedEvent e) => PlaySFX(audioConfig.CoinSFX, e.CollectionPosition);
        private void OnPlayerJumped(PlayerJumpedEvent e) => PlaySFX(audioConfig.JumpSFX);
        private void OnPlayerDied(PlayerDiedEvent e) => PlaySFX(audioConfig.DeathSFX);
    }
}