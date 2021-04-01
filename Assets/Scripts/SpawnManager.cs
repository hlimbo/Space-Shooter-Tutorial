using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPrefab;
    [SerializeField]
    private float spawnDelay = 5f;
    [SerializeField]
    private GameObject enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool stopSpawning = false;

    public void OnBeginSpawning(float secondsDelay)
    {
        // Stop All coroutines to prevent the same routine running more than once
        StopAllCoroutines();
        StartCoroutine(BeginSpawningRoutine(secondsDelay));
    }

    IEnumerator BeginSpawningRoutine(float secondsDelay)
    {
        yield return new WaitForSeconds(secondsDelay);
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(!stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(RandomXPosition(), 6.5f, 0f);
            GameObject enemy = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.SetParent(enemyContainer.transform);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while(!stopSpawning)
        {
            int randomDelay = Random.Range(3, 7);
            yield return new WaitForSeconds(randomDelay);
            Vector3 spawnPosition = new Vector3(RandomXPosition(), 6.5f, 0f);
            var randomPowerUp = powerups[(int)PowerUp.Extensions.GetRandomPowerUp()];
            Instantiate(randomPowerUp, spawnPosition, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }

    private float RandomXPosition()
    {
        return Random.Range(-9f, 9f);
    }
}
