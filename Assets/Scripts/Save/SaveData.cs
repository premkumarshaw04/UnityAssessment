using System;

namespace EndlessRunner.Save
{
    /// <summary>
    /// Serializable data contract container holding persistent game state records.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public int HighScore;
        public int TotalCoins;
        public float MusicVolume;
        public float SFXVolume;

        public SaveData()
        {
            HighScore = 0;
            TotalCoins = 0;
            MusicVolume = 0.7f;
            SFXVolume = 1.0f;
        }
    }
}