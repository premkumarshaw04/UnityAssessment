using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Events;
using EndlessRunner.Pooling;
using EndlessRunner.ScriptableObjects;

namespace EndlessRunner.Gameplay.Environment
{
    /// <summary>
    /// Spawns and recycles track segments, obstacles, and coins continuously ahead of the player.
    /// Uses PoolManager for zero runtime memory allocations.
    /// </summary>
    public class TrackSpawner : MonoBehaviour
    {
        [Header("Configurations")]
        [SerializeField] private SpawnConfigSO spawnConfig;

        [Header("References")]
        [SerializeField] private Transform playerTransform;

        private PoolManager _poolManager;
        private readonly Queue<GameObject> _activeTiles = new Queue<GameObject>();
        private Vector3 _nextSpawnPoint = Vector3.zero;
        private bool _isSpawningActive;

        private void Start()
        {
            _poolManager = Architecture.ServiceLocator.GetService<PoolManager>();
            PrewarmPools();
        }

        private void OnEnable()
        {
            EventBus<GameStateChangedEvent>.Subscribe(OnGameStateChanged);
        }

        private void OnDisable()
        {
            EventBus<GameStateChangedEvent>.Unsubscribe(OnGameStateChanged);
        }

        private void Update()
        {
            if (!_isSpawningActive || playerTransform == null) return;

            // Spawn new tiles ahead as player moves forward
            if (playerTransform.position.z + (spawnConfig.InitialTileCount * spawnConfig.TileLength) > _nextSpawnPoint.z)
            {
                SpawnNextTile();
                RecycleOldestTile();
            }
        }

        private void PrewarmPools()
        {
            if (_poolManager == null) return;

            foreach (var tilePrefab in spawnConfig.GroundTilePrefabs)
            {
                _poolManager.CreatePool(tilePrefab, spawnConfig.InitialTileCount + 2);
            }
            foreach (var obstaclePrefab in spawnConfig.ObstaclePrefabs)
            {
                _poolManager.CreatePool(obstaclePrefab, 10);
            }
            if (spawnConfig.CoinPrefab != null)
            {
                _poolManager.CreatePool(spawnConfig.CoinPrefab, 20);
            }
        }

        private void InitialTrackGen()
        {
            ClearTrack();
            _nextSpawnPoint = Vector3.zero;

            for (int i = 0; i < spawnConfig.InitialTileCount; i++)
            {
                SpawnNextTile(i == 0); // Keep first tile clear of obstacles
            }
        }

        private void SpawnNextTile(bool isSafeTile = false)
        {
            if (_poolManager == null || spawnConfig.GroundTilePrefabs.Length == 0) return;

            GameObject selectedPrefab = spawnConfig.GroundTilePrefabs[Random.Range(0, spawnConfig.GroundTilePrefabs.Length)];
            GameObject tileInstance = _poolManager.Spawn(selectedPrefab, _nextSpawnPoint, Quaternion.identity);

            if (tileInstance.TryGetComponent(out GroundTile groundTile))
            {
                _nextSpawnPoint = groundTile.NextTilePosition;

                if (!isSafeTile)
                {
                    SpawnItemsOnTile(groundTile);
                }
            }
            else
            {
                _nextSpawnPoint += new Vector3(0, 0, spawnConfig.TileLength);
            }

            _activeTiles.Enqueue(tileInstance);
        }

        private void SpawnItemsOnTile(GroundTile tile)
        {
            // Spawn Obstacles
            if (tile.ObstacleSpawnPoints != null && tile.ObstacleSpawnPoints.Length > 0)
            {
                if (Random.value < spawnConfig.ObstacleSpawnChance)
                {
                    Transform spawnPoint = tile.ObstacleSpawnPoints[Random.Range(0, tile.ObstacleSpawnPoints.Length)];
                    GameObject obstaclePrefab = spawnConfig.ObstaclePrefabs[Random.Range(0, spawnConfig.ObstaclePrefabs.Length)];
                    _poolManager.Spawn(obstaclePrefab, spawnPoint.position, spawnPoint.rotation);
                }
            }

            // Spawn Coins
            if (tile.CoinSpawnPoints != null && tile.CoinSpawnPoints.Length > 0 && spawnConfig.CoinPrefab != null)
            {
                if (Random.value < spawnConfig.CoinSpawnChance)
                {
                    Transform spawnPoint = tile.CoinSpawnPoints[Random.Range(0, tile.CoinSpawnPoints.Length)];
                    _poolManager.Spawn(spawnConfig.CoinPrefab, spawnPoint.position, spawnPoint.rotation);
                }
            }
        }

        private void RecycleOldestTile()
        {
            if (_activeTiles.Count > spawnConfig.InitialTileCount + 2)
            {
                GameObject oldestTile = _activeTiles.Dequeue();
                _poolManager.Despawn(oldestTile);
            }
        }

        private void ClearTrack()
        {
            while (_activeTiles.Count > 0)
            {
                _poolManager.Despawn(_activeTiles.Dequeue());
            }
        }

        private void OnGameStateChanged(GameStateChangedEvent e)
        {
            _isSpawningActive = (e.CurrentState == GameState.Playing);

            if (e.CurrentState == GameState.Playing && e.PreviousState == GameState.MainMenu)
            {
                InitialTrackGen();
            }
        }
    }
}