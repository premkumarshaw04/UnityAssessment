using UnityEngine;

namespace EndlessRunner.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "EndlessRunner/Configs/SpawnConfig")]
    public class SpawnConfigSO : ScriptableObject
    {
        [Header("Track Segment Settings")]
        [SerializeField] private GameObject[] groundTilePrefabs;
        [SerializeField, Range(10f, 50f)] private float tileLength = 20f;
        [SerializeField, Range(3, 10)] private int initialTileCount = 5;

        [Header("Obstacle & Coin Prefabs")]
        [SerializeField] private GameObject[] obstaclePrefabs;
        [SerializeField] private GameObject coinPrefab;

        [Header("Spawn Probabilities")]
        [SerializeField, Range(0f, 1f)] private float obstacleSpawnChance = 0.6f;
        [SerializeField, Range(0f, 1f)] private float coinSpawnChance = 0.4f;

        public GameObject[] GroundTilePrefabs => groundTilePrefabs;
        public float TileLength => tileLength;
        public int InitialTileCount => initialTileCount;
        public GameObject[] ObstaclePrefabs => obstaclePrefabs;
        public GameObject CoinPrefab => coinPrefab;
        public float ObstacleSpawnChance => obstacleSpawnChance;
        public float CoinSpawnChance => coinSpawnChance;
    }
}