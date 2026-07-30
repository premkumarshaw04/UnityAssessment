using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Core;
using EndlessRunner.Events;

namespace EndlessRunner.UI
{
    /// <summary>
    /// Displays final run metrics and exposes restart/menu buttons.
    /// </summary>
    public class GameOverView : MonoBehaviour
    {
        [Header("UI Text Display")]
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text highScoreText;

        [Header("Action Buttons")]
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Dependencies")]
        [SerializeField] private GameManager gameManager;

        private void Awake()
        {
            if (restartButton != null) restartButton.onClick.AddListener(OnRestartClicked);
            if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void OnEnable()
        {
            EventBus<ScoreUpdatedEvent>.Subscribe(OnScoreUpdated);
        }

        private void OnDisable()
        {
            EventBus<ScoreUpdatedEvent>.Unsubscribe(OnScoreUpdated);
        }

        private void OnScoreUpdated(ScoreUpdatedEvent e)
        {
            if (finalScoreText != null) finalScoreText.text = $"FINAL SCORE: {e.CurrentScore}";
            if (highScoreText != null) highScoreText.text = $"BEST: {e.HighScore}";
        }

        private void OnRestartClicked()
        {
            if (gameManager != null) gameManager.StartGame();
        }

        private void OnMainMenuClicked()
        {
            if (gameManager != null) gameManager.SetState(GameState.MainMenu);
        }
    }
}