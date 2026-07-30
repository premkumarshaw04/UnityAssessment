using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using EndlessRunner.Interfaces;

namespace EndlessRunner.Save
{
    /// <summary>
    /// File-based JSON save service implementing ISaveService contract.
    /// Safely handles reading/writing to Application.persistentDataPath.
    /// </summary>
    public class SaveManager : ISaveService
    {
        private readonly string _saveFilePath;
        private SaveData _cachedSaveData;

        public SaveManager()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, "player_save.json");
        }

        public Task InitializeAsync()
        {
            LoadDataFromDisk();
            Debug.Log($"[SaveManager] Initialized. Save file path: {_saveFilePath}");
            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            SaveToDisk();
        }

        public void Save<T>(string key, T data)
        {
            switch (key)
            {
                case "HighScore":
                    if (data is int highScore) _cachedSaveData.HighScore = highScore;
                    break;
                case "TotalCoins":
                    if (data is int totalCoins) _cachedSaveData.TotalCoins = totalCoins;
                    break;
                case "MusicVolume":
                    if (data is float musicVol) _cachedSaveData.MusicVolume = musicVol;
                    break;
                case "SFXVolume":
                    if (data is float sfxVol) _cachedSaveData.SFXVolume = sfxVol;
                    break;
                default:
                    Debug.LogWarning($"[SaveManager] Key '{key}' not recognized in standard SaveData structure.");
                    break;
            }

            SaveToDisk();
        }

        public T Load<T>(string key, T defaultValue)
        {
            object value = key switch
            {
                "HighScore" => _cachedSaveData.HighScore,
                "TotalCoins" => _cachedSaveData.TotalCoins,
                "MusicVolume" => _cachedSaveData.MusicVolume,
                "SFXVolume" => _cachedSaveData.SFXVolume,
                _ => null
            };

            if (value is T typedValue)
            {
                return typedValue;
            }

            return defaultValue;
        }

        public bool HasKey(string key)
        {
            return key switch
            {
                "HighScore" => _cachedSaveData.HighScore > 0,
                "TotalCoins" => _cachedSaveData.TotalCoins > 0,
                "MusicVolume" => true,
                "SFXVolume" => true,
                _ => false
            };
        }

        public void SaveToDisk()
        {
            try
            {
                string json = JsonUtility.ToJson(_cachedSaveData, true);
                File.WriteAllText(_saveFilePath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Failed to write save file: {ex.Message}");
            }
        }

        private void LoadDataFromDisk()
        {
            if (File.Exists(_saveFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_saveFilePath);
                    _cachedSaveData = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[SaveManager] Failed to read save file, reverting to default: {ex.Message}");
                    _cachedSaveData = new SaveData();
                }
            }
            else
            {
                _cachedSaveData = new SaveData();
            }
        }
    }
}