using UnityEngine;
using TMPro;
using EndlessRunner.Events;

namespace EndlessRunner.UI
{
    /// <summary>
    /// In-game HUD controller updating score and coin UI elements dynamically.
    /// </summary>
    public class HUDView : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text coinText;

        private void OnEnable()
        {
            EventBus<ScoreUpdatedEvent>.Subscribe(OnScoreUpdated);
            EventBus<CoinCollectedEvent>.Subscribe(OnCoinCollected);
        }

        private void OnDisable()
        {
            EventBus<ScoreUpdatedEvent>.Unsubscribe(OnScoreUpdated);
            EventBus<CoinCollectedEvent>.Unsubscribe(OnCoinCollected);
        }

        private void OnScoreUpdated(ScoreUpdatedEvent e)
        {
            if (scoreText != null)
            {
                scoreText.text = $"SCORE: {e.CurrentScore}";
            }
        }

        private void OnCoinCollected(CoinCollectedEvent e)
        {
            // Coin visual update signals handled via event bus
        }
    }
}