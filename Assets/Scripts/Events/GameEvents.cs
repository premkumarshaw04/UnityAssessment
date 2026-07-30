using UnityEngine;

namespace EndlessRunner.Events
{
    public enum GameState
    {
        Booting,
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    /// <summary> Broadcast when game loop state changes. </summary>
    public struct GameStateChangedEvent
    {
        public readonly GameState CurrentState;
        public readonly GameState PreviousState;

        public GameStateChangedEvent(GameState currentState, GameState previousState)
        {
            CurrentState = currentState;
            PreviousState = previousState;
        }
    }

    /// <summary> Broadcast when running score or high score is updated. </summary>
    public struct ScoreUpdatedEvent
    {
        public readonly int CurrentScore;
        public readonly int HighScore;
        public readonly float DistanceTraveled;

        public ScoreUpdatedEvent(int currentScore, int highScore, float distanceTraveled)
        {
            CurrentScore = currentScore;
            HighScore = highScore;
            DistanceTraveled = distanceTraveled;
        }
    }

    /// <summary> Broadcast when player picks up a coin. </summary>
    public struct CoinCollectedEvent
    {
        public readonly int Value;
        public readonly Vector3 CollectionPosition;

        public CoinCollectedEvent(int value, Vector3 collectionPosition)
        {
            Value = value;
            CollectionPosition = collectionPosition;
        }
    }

    /// <summary> Broadcast when player jumps. </summary>
    public struct PlayerJumpedEvent { }

    /// <summary> Broadcast when player collides with an obstacle. </summary>
    public struct PlayerDiedEvent
    {
        public readonly string CauseOfDeath;

        public PlayerDiedEvent(string causeOfDeath)
        {
            CauseOfDeath = causeOfDeath;
        }
    }

    /// <summary> Broadcast when game speed increases due to difficulty progression. </summary>
    public struct SpeedMultiplierChangedEvent
    {
        public readonly float Multiplier;

        public SpeedMultiplierChangedEvent(float multiplier)
        {
            Multiplier = multiplier;
        }
    }
}