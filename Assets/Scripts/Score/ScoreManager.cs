using System.Threading.Tasks;
using UnityEngine;
using EndlessRunner.Events;
using EndlessRunner.Interfaces;
using EndlessRunner.Save;
using EndlessRunner.ScriptableObjects;

namespace EndlessRunner.Score
{
    /// <summary>
    /// Tracks distance traveled, coins collected, and calculates live running score.
    /// Handles High Score persistence integration.
    /// </summary>
    public class ScoreManager : MonoBehaviour, IGameService
    {
        [Header("Configurations")]
        [SerializeField] private GameConfigSO gameConfig;

        private ISaveService _saveService;
        private float _distanceTraveled;
        private int _coinsCollected;
        private int _currentScore;
        private int _highScore;
        private bool _isTracking;

        public int CurrentScore => _currentScore;
        public int HighScore => _highScore;
        public int CoinsCollected => _coinsCollected;
        public float DistanceTraveled => _distanceTraveled;

        public Task InitializeAsync()
        {
            _saveService = Architecture.ServiceLocator.GetService<ISaveService>();
            _highScore = _saveService?.Load("HighScore", 0) ?? 0;

            EventBus<GameStateChangedEvent>.Subscribe(OnGameStateChanged);
            EventBus<CoinCollectedEvent>.Subscribe(OnCoinCollected);

            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            EventBus<GameStateChangedEvent>.Unsubscribe(OnGameStateChanged);
            EventBus<CoinCollectedEvent>.Unsubscribe(OnCoinCollected);
        }

        private void OnGameStateChanged(GameStateChangedEvent e)
        {
            if (e.CurrentState == GameState.Playing)
            {
                if (e.PreviousState == GameState.MainMenu || e.PreviousState == GameState.GameOver)
                {
                    ResetScore();
                }
                _isTracking = true;
            }
            else
            {
                _isTracking = false;
                if (e.CurrentState == GameState.GameOver)
                {
                    CheckAndSaveHighScore();
                }
            }
        }

        private void Update()
        {
            if (!_isTracking) return;

            // Calculate distance based score addition
            float distanceDelta = gameConfig.InitialSpeedMultiplier * 10f * Time.deltaTime;
            _distanceTraveled += distanceDelta;

            CalculateScore();
        }

        private void OnCoinCollected(CoinCollectedEvent e)
        {
            _coinsCollected += e.Value;
            CalculateScore();
        }

        private void CalculateScore()
        {
            _currentScore = Mathf.FloorToInt(_distanceTraveled * gameConfig.PointsPerMeter) + (_coinsCollected * gameConfig.PointsPerCoin);

            if (_currentScore > _highScore)
            {
                _highScore = _currentScore;
            }

            EventBus<ScoreUpdatedEvent>.Raise(new ScoreUpdatedEvent(_currentScore, _highScore, _distanceTraveled));
        }

        private void ResetScore()
        {
            _distanceTraveled = 0f;
            _coinsCollected = 0;
            _currentScore = 0;
            CalculateScore();
        }

        private void CheckAndSaveHighScore()
        {
            if (_saveService != null)
            {
                _saveService.Save("HighScore", _highScore);
                int savedCoins = _saveService.Load("TotalCoins", 0);
                _saveService.Save("TotalCoins", savedCoins + _coinsCollected);
                _saveService.SaveToDisk();
            }
        }
    }
}