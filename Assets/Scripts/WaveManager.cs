using System.Collections;
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

    private GameObject bossUi;

    private Player player;


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
        player = FindObjectOfType<Player>();
        bossUi = GameObject.Find("BossHp");
        bossUi.SetActive(false);
    }

    public void StartGame ()
    {
        StartCoroutine(StartWavesRoutine());
    }

    private void CalculateActiveEnemyCount (Wave wave)
    {
        activeEnemyCount = 0;
        for (int i = 0;i < wave.enemyPrefabs.Length; ++i)
        {
            if (wave.enemyPrefabs[i].GetComponent<BaseEnemyBrain>() != null)
            {
                ++activeEnemyCount;
            }
            else if(wave.enemyPrefabs[i].GetComponent<Swarm>() != null)
            {
                activeEnemyCount += wave.enemyPrefabs[i].GetComponent<Swarm>().SwarmCount;
            }
        }
    }

    private IEnumerator StartWavesRoutine()
    {
        while(waveIndex < waves.Length)
        {
            // if player is dead, don't start the next wave
            if (player == null)
            {
                yield break;
            }

            var currentWave = waves[waveIndex];
            CalculateActiveEnemyCount(currentWave);

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

            if (currentWave.name.Contains("Boss"))
            {
                bossUi.SetActive(true);
            }
            else
            {
                bossUi.SetActive(false);
            }

            // Spawn Paths
            GameObject possiblePaths = null;
            if(currentWave.pathContainer != null)
            {
                possiblePaths = Instantiate(currentWave.pathContainer);
            }

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

            if(possiblePaths != null)
            {
                Destroy(possiblePaths);
            }

            spawnIndex = 0;
        }
    }
}
