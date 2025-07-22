using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawnTimer;
        public float spawnInterval;
        public int enemiesPerWave;
        public int spawnedEnemyCount;
    }

    public List<Wave> waves;
    public int waveNumber;

    public Transform minPos;
    public Transform maxPos;

    [SerializeField] private Transform enemyParentTransform;

    void Update()
    {
        waves[waveNumber].spawnTimer += Time.deltaTime;
        if (waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval)
        {
            waves[waveNumber].spawnTimer = 0f; // Reset the timer
            SpawnEnemy();
            waves[waveNumber].spawnedEnemyCount++;
        }

        if (waves[waveNumber].spawnedEnemyCount >= waves[waveNumber].enemiesPerWave)
        {
            waves[waveNumber].spawnedEnemyCount = 0; // Reset the count for the next wave
            if (waves[waveNumber].spawnInterval > 0.15f)
            {
                waves[waveNumber].spawnInterval *= 0.8f; // Decrease the spawn interval by 10% for the next wave
            }
            waveNumber++;
        }

        if (waveNumber >= waves.Count)
        {
            waveNumber = 0; // Reset to the first wave if all waves are completed

        }
    }

    private void SpawnEnemy()
    {
        if (!PlayerController.Instance.gameObject.activeSelf) return;
        Instantiate(waves[waveNumber].enemyPrefab, RandomSpawner(), transform.rotation, enemyParentTransform);
    }
    
    private Vector2 RandomSpawner()
    {
        Vector2 spawnPoint;

        if(Random.Range(0f,1f) > 0.5)
        {
            spawnPoint.x = UnityEngine.Random.Range(minPos.position.x, maxPos.position.x);
            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.y = maxPos.position.y; // Spawn at the top edge
            }
            else
            {
                spawnPoint.y = minPos.position.y; // Spawn at the bottom edge
            }
        }
        else
        {
            spawnPoint.y = UnityEngine.Random.Range(minPos.position.y, maxPos.position.y);
            if (Random.Range(0f, 1f) > 0.5f)
            {
                spawnPoint.x = maxPos.position.x; // Spawn at the right edge
            }
            else
            {
                spawnPoint.x = minPos.position.x; // Spawn at the left edge
            }
        }

        return spawnPoint;
    }
}
