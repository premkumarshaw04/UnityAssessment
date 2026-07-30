using System.Threading.Tasks;
using UnityEngine;
using EndlessRunner.Events;
using EndlessRunner.Interfaces;

namespace EndlessRunner.UI
{
    /// <summary>
    /// Main canvas navigation controller. Synchronizes UI panels with GameState changes.
    /// </summary>
    public class UIManager : MonoBehaviour, IGameService
    {
        [Header("Views")]
        [SerializeField] private GameObject mainMenuView;
        [SerializeField] private GameObject hudView;
        [SerializeField] private GameObject pauseView;
        [SerializeField] private GameObject gameOverView;

        public Task InitializeAsync()
        {
            EventBus<GameStateChangedEvent>.Subscribe(OnGameStateChanged);
            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            EventBus<GameStateChangedEvent>.Unsubscribe(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedEvent e)
        {
            HideAllViews();

            switch (e.CurrentState)
            {
                case GameState.MainMenu:
                    if (mainMenuView != null) mainMenuView.SetActive(true);
                    break;
                case GameState.Playing:
                    if (hudView != null) hudView.SetActive(true);
                    break;
                case GameState.Paused:
                    if (hudView != null) hudView.SetActive(true);
                    if (pauseView != null) pauseView.SetActive(true);
                    break;
                case GameState.GameOver:
                    if (gameOverView != null) gameOverView.SetActive(true);
                    break;
            }
        }

        private void HideAllViews()
        {
            if (mainMenuView != null) mainMenuView.SetActive(false);
            if (hudView != null) hudView.SetActive(false);
            if (pauseView != null) pauseView.SetActive(false);
            if (gameOverView != null) gameOverView.SetActive(false);
        }
    }
}