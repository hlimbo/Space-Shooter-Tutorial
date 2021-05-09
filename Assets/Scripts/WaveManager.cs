using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private float spawnDelayInSeconds = 2f;

    private int waveIndex = 0;
    private int currentSpawnCount = 0;

    private void Start()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    private IEnumerator SpawnWaveRoutine()
    {
        if(waves.Length == 0)
        {
            yield break;
        }

        Wave currentWave = waves[waveIndex];

        // Spawn Paths
        Instantiate(currentWave.pathContainer);
        
        while(currentSpawnCount < currentWave.enemyPrefabs.Length)
        {
            var enemyPrefab = currentWave.enemyPrefabs[currentSpawnCount];
            Instantiate(enemyPrefab);
            ++currentSpawnCount;
            yield return new WaitForSeconds(spawnDelayInSeconds);
        }
    }
}
