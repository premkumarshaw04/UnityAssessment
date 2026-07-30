using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using EndlessRunner.Architecture;
using EndlessRunner.Audio;
using EndlessRunner.Interfaces;
using EndlessRunner.Pooling;
using EndlessRunner.Save;

namespace EndlessRunner.Core
{
    /// <summary>
    /// Application entry point. Configures mobile target frame rates,
    /// instantiates and registers system services into ServiceLocator, and switches scenes cleanly.
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Target Configurations")]
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private string targetSceneName = "Gameplay";

        [Header("Scene Services")]
        [SerializeField] private PoolManager poolManagerPrefab;
        [SerializeField] private AudioManager audioManagerPrefab;

        private async void Start()
        {
            ConfigureMobilePerformance();
            await InitializeServicesAsync();

            // Proceed to main gameplay scene
            SceneManager.LoadSceneAsync(targetSceneName);
        }

        private void ConfigureMobilePerformance()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0; // Disable VSync to give manual frame rate control to mobile GPU
        }

        private async Task InitializeServicesAsync()
        {
            // 1. Initialize Save Service
            SaveManager saveManager = new SaveManager();
            await saveManager.InitializeAsync();
            ServiceLocator.RegisterService<ISaveService>(saveManager);

            // 2. Initialize Pool Manager
            if (poolManagerPrefab != null)
            {
                PoolManager poolManager = Instantiate(poolManagerPrefab);
                DontDestroyOnLoad(poolManager.gameObject);
                await poolManager.InitializeAsync();
                ServiceLocator.RegisterService<PoolManager>(poolManager);
            }

            // 3. Initialize Audio Manager
            if (audioManagerPrefab != null)
            {
                AudioManager audioManager = Instantiate(audioManagerPrefab);
                DontDestroyOnLoad(audioManager.gameObject);
                await audioManager.InitializeAsync();
                ServiceLocator.RegisterService<IAudioService>(audioManager);
            }

            Debug.Log("[Bootstrapper] Core services successfully registered and initialized.");
        }
    }
}