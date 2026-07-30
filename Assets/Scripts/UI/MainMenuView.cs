using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Core;

namespace EndlessRunner.UI
{
    /// <summary>
    /// Handles main menu UI interactions.
    /// </summary>
    public class MainMenuView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button playButton;

        [Header("Dependencies")]
        [SerializeField] private GameManager gameManager;

        private void Awake()
        {
            if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            if (gameManager != null) gameManager.StartGame();
        }
    }
}