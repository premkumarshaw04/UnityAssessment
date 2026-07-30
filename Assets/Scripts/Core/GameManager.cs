using System.Threading.Tasks;
using UnityEngine;
using EndlessRunner.Events;
using EndlessRunner.Interfaces;
using EndlessRunner.ScriptableObjects;

namespace EndlessRunner.Core
{
    /// <summary>
    /// Finite State Machine controlling overall game execution lifecycle states.
    /// Operates completely decoupled via EventBus broadcasts.
    /// </summary>
    public class GameManager : MonoBehaviour, IGameService
    {
        [Header("Configurations")]
        [SerializeField] private GameConfigSO gameConfig;

        private GameState _currentState = GameState.Booting;

        public GameState CurrentState => _currentState;

        public Task InitializeAsync()
        {
            SetState(GameState.MainMenu);
            Debug.Log("[GameManager] Initialized in MainMenu state.");
            return Task.CompletedTask;
        }

        public void Deinitialize() { }

        /// <summary>
        /// Transitions the game machine state and notifies subscribed listeners.
        /// </summary>
        public void SetState(GameState newState)
        {
            if (_currentState == newState) return;

            GameState previousState = _currentState;
            _currentState = newState;

            switch (_currentState)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;
            }

            EventBus<GameStateChangedEvent>.Raise(new GameStateChangedEvent(_currentState, previousState));
        }

        public void StartGame()
        {
            SetState(GameState.Playing);
        }

        public void PauseGame()
        {
            if (_currentState == GameState.Playing)
            {
                SetState(GameState.Paused);
            }
        }

        public void ResumeGame()
        {
            if (_currentState == GameState.Paused)
            {
                SetState(GameState.Playing);
            }
        }

        public void TriggerGameOver(string reason)
        {
            if (_currentState == GameState.Playing)
            {
                SetState(GameState.GameOver);
                EventBus<PlayerDiedEvent>.Raise(new PlayerDiedEvent(reason));
            }
        }
    }
}