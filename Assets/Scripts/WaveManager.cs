using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private float spawnDelayInSeconds = 2f;

    [SerializeField]
    private float waveTransitionDelay = 5f;

    private int waveIndex = 0;
    private int spawnIndex = 0;

    private int activeEnemyCount = 0;
    public void DecrementEnemyCount ()
    {
        if(activeEnemyCount > 0)
        {
            --activeEnemyCount;
        }
    }

    private UiManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
    }

    private void Start()
    {
        StartCoroutine(StartWavesRoutine());
    }

    private IEnumerator StartWavesRoutine()
    {
        while(waveIndex < waves.Length)
        {
            var currentWave = waves[waveIndex];
            activeEnemyCount = currentWave.enemyPrefabs.Length;

            uiManager.ToggleWaveTextVisibility(true);
            uiManager.SetWaveText(currentWave.name);
            yield return new WaitForSeconds(waveTransitionDelay);
            uiManager.ToggleWaveTextVisibility(false);

            yield return SpawnWaveRoutine();
            ++waveIndex;
        }
    }

    private IEnumerator SpawnWaveRoutine()
    {
        if(waves.Length == 0)
        {
            yield break;
        }

        if(waveIndex < waves.Length)
        {
            Wave currentWave = waves[waveIndex];

            // Spawn Paths
            var possiblePaths = Instantiate(currentWave.pathContainer);

            while (spawnIndex < currentWave.enemyPrefabs.Length)
            {
                var enemyPrefab = currentWave.enemyPrefabs[spawnIndex];
                Instantiate(enemyPrefab);
                ++spawnIndex;
                yield return new WaitForSeconds(spawnDelayInSeconds);
            }

            // Wait until all enemies are destroyed
            while(activeEnemyCount > 0)
            {
                yield return null;
            }

            Destroy(possiblePaths);
            spawnIndex = 0;
        }
    }
}
