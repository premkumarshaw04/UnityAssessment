using UnityEngine;

namespace EndlessRunner.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "EndlessRunner/Configs/AudioConfig")]
    public class AudioConfigSO : ScriptableObject
    {
        [Header("Music Tracks")]
        [SerializeField] private AudioClip backgroundMusic;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip jumpSFX;
        [SerializeField] private AudioClip coinSFX;
        [SerializeField] private AudioClip deathSFX;
        [SerializeField] private AudioClip buttonClickSFX;

        [Header("Default Volume Settings")]
        [SerializeField, Range(0f, 1f)] private float defaultMusicVolume = 0.7f;
        [SerializeField, Range(0f, 1f)] private float defaultSFXVolume = 1.0f;

        public AudioClip BackgroundMusic => backgroundMusic;
        public AudioClip JumpSFX => jumpSFX;
        public AudioClip CoinSFX => coinSFX;
        public AudioClip DeathSFX => deathSFX;
        public AudioClip ButtonClickSFX => buttonClickSFX;
        public float DefaultMusicVolume => defaultMusicVolume;
        public float DefaultSFXVolume => defaultSFXVolume;
    }
}